
Preview Features
----------------

See https://github.com/dotnet/designs/blob/main/accepted/2021/preview-features/preview-features.md

D.B.targets

We do not want System.Runtime.Experimental to be used by the normal packages.lock.json.
Does not work with a simple "Condition" but it's OK w/ "<When>" -and- $(Configuration) not
$(EnablePreviewFeatures).
Main drawback: we cannot use different configurations at the same time, for instance
Debug w/ the command line and Release w/ VS; "dotnet restore" depends on the configuration.
https://github.com/NuGet/Home/issues/5895
https://docs.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files#lock-file-extensibility

```
<Choose>
  <When Condition=" '$(Configuration)' == 'Debug' ">
    <PropertyGroup>
      <NuGetLockFilePath>packages.SRE.lock.json</NuGetLockFilePath>
    </PropertyGroup>
  </When>
</Choose>
<ItemGroup Condition=" '$(EnablePreviewFeatures)' == 'true' ">
  <PackageReference Include="System.Runtime.Experimental" Version="6.0.2" />
</ItemGroup>
```

```
<PropertyGroup Condition=" '$(EnablePreviewFeatures)' == 'true' ">
  <GenerateRequiresPreviewFeaturesAttribute>false</GenerateRequiresPreviewFeaturesAttribute>
</PropertyGroup>

<ItemGroup Condition=" '$(EnablePreviewFeatures)' == 'true' ">
  <Using Include="System.Runtime.Versioning" />
</ItemGroup>

<ItemGroup Condition=" '$(EnablePreviewFeatures)' == 'true' ">
  <PackageReference Include="System.Runtime.Experimental" Version="6.0.2" />
</ItemGroup>
```
