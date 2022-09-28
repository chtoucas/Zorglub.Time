// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

//#define ENABLE_DTE

namespace Zorglub.TextTemplates;

using System;
using System.IO;
using System.Runtime.Remoting.Messaging;

#if ENABLE_DTE
using EnvDTE;
#endif

using Microsoft.VisualStudio.TextTemplating;

// Provides a base class for generated text transformations hosted inside Visual Studio.
public abstract class VSTemplate : TextTransformation
{
    // Default indentation level.
    private const string Indent = "    ";

#if ENABLE_DTE
    // Lazy factory for the DTE.
    private readonly Lazy<DTE> _dte;
#endif
    // Lazy factory for the templating engine host.
    private readonly Lazy<ITextTemplatingEngineHost> _host;

    // Default template's name.
    private string _name;
    // Default namespace.
    private string _namespace;

    protected VSTemplate()
    {
        _host = new Lazy<ITextTemplatingEngineHost>(() => HostFactory(this));
#if ENABLE_DTE
        _dte = new Lazy<DTE>(DteFactory);
#endif
    }

    // parent: The parent text transformation.
    protected VSTemplate(TextTransformation parent)
    {
        if (parent is null) throw new ArgumentNullException(nameof(parent));

        _host = new Lazy<ITextTemplatingEngineHost>(() => HostFactory(parent));
#if ENABLE_DTE
        _dte = new Lazy<DTE>(DteFactory);
#endif
    }

#if ENABLE_DTE
    // Gets the DTE (Development Tools Environment) service.
    protected DTE Dte => _dte.Value;
#endif

    // Gets the templating engine host.
    protected ITextTemplatingEngineHost VSHost => _host.Value;

    // Gets or sets the name of the template.
    // If none was specified, it is inferred from the template's filename.
    protected string Name
    {
        get => _name ??= InferName();

        set
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentException("The name can not be null or blank.", nameof(value));

            _name = value;
        }
    }

    // Gets or sets the name of the namespace.
    // If none was specified, it is inferred from the template location.
    protected string Namespace
    {
        get => _namespace ??= InferNamespace();

        set
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentException("The namespace can not be null or blank.", nameof(value));

            _namespace = value;
        }
    }

    // Initializes the templating class then generates the output text of the transformation.
    public string Execute()
    {
        Initialize();
        return TransformText();
    }

    public override string TransformText()
    {
        WriteContent();
        return GenerationEnvironment.ToString();
    }

    protected virtual void WriteContent() { }

    // Increases the indent by the default indent.
    protected void PushIndent() => PushIndent(Indent);

    // Writes a new (empty) line directly into the generated output.
    protected void WriteLine() => WriteLine(String.Empty);

    //
    // Private Helpers
    //

    private static ITextTemplatingEngineHost HostFactory(TextTransformation transformation)
    {
        var transformationType = transformation.GetType();
        var hostProperty = transformationType.GetProperty("Host");

        if (hostProperty is null)
            throw new NotSupportedException(
                "Unable to access the templating engine host. "
                + "Please make sure your template includes hostspecific=\"true\" "
                + "attribute in the <#@ template #> directive.");

        return (ITextTemplatingEngineHost)hostProperty.GetValue(transformation, null);
    }

#if ENABLE_DTE
    private DTE DteFactory()
    {
        var serviceProvider = (IServiceProvider)VSHost;
        if (serviceProvider is null)
            throw new NotSupportedException("Host property is null.");

        var dte = serviceProvider.GetService(typeof(DTE)) as DTE;
        if (dte is null)
            throw new NotSupportedException("Unable to retrieve the DTE (Development Tools Environment) service.");

        return dte;
    }
#endif

    private static string InferNamespace() => CallContext.LogicalGetData("NamespaceHint").ToString();

    private string InferName() => Path.GetFileNameWithoutExtension(VSHost.TemplateFile);
}
