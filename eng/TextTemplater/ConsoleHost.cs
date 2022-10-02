// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

// Adapted from
// https://learn.microsoft.com/en-us/visualstudio/modeling/processing-text-templates-by-using-a-custom-host
// See also
// https://github.com/mono/t4/blob/main/Mono.TextTemplating/Mono.TextTemplating/TemplateGenerator.cs
// https://github.com/unoplatform/uno/tree/master/src/T4Generator

namespace TextTemplater;

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TextTemplating;

internal sealed class ConsoleHost : MarshalByRefObject, ITextTemplatingEngineHost
{
    //private static readonly Dictionary<string, string> s_KnownAssemblies = new(StringComparer.OrdinalIgnoreCase)
    //{
    //    { "System.Core.dll", typeof(System.Linq.Enumerable).Assembly.Location },
    //    { "System.Data.dll", typeof(System.Data.DataTable).Assembly.Location },
    //    { "System.Linq.dll", typeof(System.Linq.Enumerable).Assembly.Location },
    //    { "System.Xml.dll", typeof(System.Xml.XmlAttribute).Assembly.Location },
    //    { "System.Xml.Linq.dll", typeof(System.Xml.Linq.XDocument).Assembly.Location },

    //    { "System.Core", typeof(System.Linq.Enumerable).Assembly.Location },
    //    { "System.Data", typeof(System.Data.DataTable).Assembly.Location },
    //    { "System.Linq", typeof(System.Linq.Enumerable).Assembly.Location },
    //    { "System.Xml", typeof(System.Xml.XmlAttribute).Assembly.Location },
    //    { "System.Xml.Linq", typeof(System.Xml.Linq.XDocument).Assembly.Location }
    //};

    private static readonly string[] s_StandardAssemblyReferences = new string[]
    {
        typeof(Uri).Assembly.Location,
        typeof(System.Linq.Enumerable).Assembly.Location,
    };

    private static readonly string[] s_StandardImports = new string[] { "System" };

    public ConsoleHost(string templateFile)
    {
        TemplateFile = templateFile ?? throw new ArgumentNullException(nameof(templateFile));
    }

    private string _fileExtension = ".txt";
    public string FileExtension
    {
        get => _fileExtension;
        set => _fileExtension = value ?? throw new ArgumentNullException(nameof(value));
    }

    private Encoding _outputEncoding = Encoding.UTF8;
    public Encoding OutputEncoding
    {
        get => _outputEncoding;
        set => _outputEncoding = value ?? throw new ArgumentNullException(nameof(value));
    }

    public CompilerErrorCollection Errors { get; private set; } = new();

    public string TemplateFile { get; }

    public IList<string> StandardAssemblyReferences => s_StandardAssemblyReferences;

    public IList<string> StandardImports => s_StandardImports;

    public bool LoadIncludeText(string requestFileName, out string content, out string location)
    {
        content = String.Empty;
        location = String.Empty;

        if (Path.IsPathRooted(requestFileName) == false)
        {
            requestFileName = Path.Combine(Path.GetDirectoryName(TemplateFile), requestFileName);
        }

        if (File.Exists(requestFileName))
        {
            content = File.ReadAllText(requestFileName);
            return true;
        }

        return false;
    }

    public object? GetHostOption(string optionName) =>
        optionName switch
        {
            "CacheAssemblies" => true,
            _ => null,
        };

    public string? ResolveAssemblyReference(string assemblyReference)
    {
        if (assemblyReference is null) throw new ArgumentNullException(nameof(assemblyReference));

        if (File.Exists(assemblyReference)) return assemblyReference;

        string candidate = Path.Combine(Path.GetDirectoryName(TemplateFile), assemblyReference);
        return File.Exists(candidate) ? candidate : null;
    }

    public Type ResolveDirectiveProcessor(string processorName) => throw new NotSupportedException();

    public string ResolvePath(string fileName)
    {
        if (fileName is null) throw new ArgumentNullException(nameof(fileName));

        if (File.Exists(fileName)) return fileName;

        string candidate = Path.Combine(Path.GetDirectoryName(TemplateFile), fileName);
        return File.Exists(candidate) ? candidate : fileName;
    }

    public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
    {
        if (directiveId is null) throw new ArgumentNullException(nameof(directiveId));
        if (processorName is null) throw new ArgumentNullException(nameof(processorName));
        if (parameterName is null) throw new ArgumentNullException(nameof(parameterName));

        return String.Empty;
    }

    public void SetFileExtension(string extension) => FileExtension = extension;

    public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective) => OutputEncoding = encoding;

    public void LogErrors(CompilerErrorCollection errors) => Errors = errors;

    public AppDomain ProvideTemplatingAppDomain(string content) =>
        AppDomain.CreateDomain("T4 console AppDomain");
}
