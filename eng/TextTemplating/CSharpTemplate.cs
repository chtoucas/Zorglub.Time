﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Zorglub.TextTemplating;

using System;
using System.IO;
using System.Runtime.Remoting.Messaging;

using Microsoft.VisualStudio.TextTemplating;

/// <summary>Provides a base class for generated C# transformations hosted inside Visual Studio.</summary>
[CLSCompliant(false)]
public abstract class CSharpTemplate : TextTransformation
{
    private readonly Lazy<ITextTemplatingEngineHost> _host;

    /// <summary>Initializes a new instance of the <see cref="CSharpTemplate"/> class.</summary>
    protected CSharpTemplate()
    {
        _host = new Lazy<ITextTemplatingEngineHost>(() => GetHostProperty(this));
    }

    /// <summary>Initializes a new instance of the <see cref="CSharpTemplate"/> class.</summary>
    /// <param name="parent">The parent text transformation.</param>
    protected CSharpTemplate(TextTransformation parent)
    {
        if (parent is null) throw new ArgumentNullException(nameof(parent));

        _host = new Lazy<ITextTemplatingEngineHost>(() => GetHostProperty(parent));
    }

    private string? _name;
    /// <summary>Gets or sets the template name.
    /// <para>If none was specified, the name is inferred from the template filename.</para></summary>
    public string Name
    {
        get => _name ??= InferName();

        set
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentException("The name can not be null or blank.", nameof(value));

            _name = value;
        }
    }

    private string? _namespace;
    /// <summary>Gets or sets the template namespace.
    /// <para>If none was specified, the namespace inferred from the template location.</para></summary>
    public string Namespace
    {
        get => _namespace ??= InferNamespace();

        set
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentException("The namespace can not be null or blank.", nameof(value));

            _namespace = value;
        }
    }

    /// <summary>Gets the templating engine host.</summary>
    protected ITextTemplatingEngineHost VSHost => _host.Value;

    /// <summary>Initializes the templating class then generates the output text of the
    /// transformation.</summary>
    /// <returns>The output text of the transformation.</returns>
    public string Execute()
    {
        Initialize();
        return TransformText();
    }

    public sealed override string TransformText()
    {
        WriteLine(
"""
//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behaviour and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
""");
        WriteContent();
        return GenerationEnvironment.ToString();
    }

    //
    // Writers
    //

    protected abstract void WriteContent();

    /// <summary>Write the T4 compiler attribute into the generated output.</summary>
    protected void WriteCompilerAttribute() =>
        WriteLine("""[global::System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.TextTemplating")""");

    /// <summary>Increases the indent by the default indent.</summary>
    protected void PushIndent() => PushIndent("    ");

    /// <summary>Writes a new (empty) line directly into the generated output.</summary>
    protected void WriteLine() => WriteLine(String.Empty);

    //
    // Private Helpers
    //

    private static ITextTemplatingEngineHost GetHostProperty(TextTransformation transformation)
    {
        var host = transformation.GetType().GetProperty("Host");

        if (host is null)
            throw new NotSupportedException(
                "Unable to access the templating engine host. "
                + "Please make sure your template includes hostspecific=\"true\" "
                + "attribute in the <#@ template #> directive.");

        return (ITextTemplatingEngineHost)host.GetValue(transformation, null);
    }

    private string InferNamespace() =>
        VSHost.ResolveParameterValue("directiveId", "namespaceDirectiveProcessor", "namespaceHint")
        ?? CallContext.LogicalGetData("NamespaceHint")?.ToString()
        ?? throw new NotSupportedException("Unable to infer the current namespace.");

    private string InferName() => Path.GetFileNameWithoutExtension(VSHost.TemplateFile);
}