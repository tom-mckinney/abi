using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "Abi",
    Author = "Tom McKinney, UpstreamCode",
    Website = "https://upstreamcode.com",
    Version = "0.0.1",
    Description = "Enables features for multivariate user testing using Abi (Analytics and Business Insights).",
    Category = "UpstreamCode",
    Dependencies = new string[] { "OrchardCore.Liquid" }
)]
