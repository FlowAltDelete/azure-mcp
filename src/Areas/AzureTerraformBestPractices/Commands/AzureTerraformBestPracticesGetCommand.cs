﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Reflection;
using AzureMcp.Commands;
using AzureMcp.Helpers;
using Microsoft.Extensions.Logging;

namespace AzureMcp.Areas.AzureTerraformBestPractices.Commands;

public sealed class AzureTerraformBestPracticesGetCommand(ILogger<AzureTerraformBestPracticesGetCommand> logger) : BaseCommand()
{
    private const string CommandTitle = "Get Terraform Best Practices for Azure";
    private readonly ILogger<AzureTerraformBestPracticesGetCommand> _logger = logger;
    private static readonly string s_bestPracticesText = LoadBestPracticesText();

    private static string GetBestPracticesText() => s_bestPracticesText;

    private static string LoadBestPracticesText()
    {
        Assembly assembly = typeof(AzureTerraformBestPracticesGetCommand).Assembly;
        string resourceName = EmbeddedResourceHelper.FindEmbeddedResource(assembly, "terraform-best-practices-for-azure.txt");
        return EmbeddedResourceHelper.ReadEmbeddedResource(assembly, resourceName);
    }

    public override string Name => "get";

    public override string Description =>
        "Returns Terraform best practices for Azure. " +
        "Call this command and follow its guidance before generating or suggesting any Terraform code specific to Azure. " +
        "This command returns the content of the markdown file as a string array.";

    public override string Title => CommandTitle;

    [McpServerTool(Destructive = false, ReadOnly = true, Title = CommandTitle)]
    public override Task<CommandResponse> ExecuteAsync(CommandContext context, ParseResult parseResult)
    {
        var bestPractices = GetBestPracticesText();
        context.Response.Status = 200;
        context.Response.Results = ResponseResult.Create(new List<string> { bestPractices }, JsonSourceGenerationContext.Default.ListString);
        context.Response.Message = string.Empty;
        return Task.FromResult(context.Response);
    }
}
