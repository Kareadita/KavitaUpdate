﻿using System.Threading.Tasks;
using KavitaUpdate.Release;
using KavitaUpdate.Release.Azure;
using KavitaUpdate.Release.Github;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KavitaUpdate.Controllers
{
    [Route("v1/[controller]")]
    public class WebhookController
    {
        private readonly Config _config;
        private readonly ReleaseService _releaseService;

        public WebhookController(ReleaseService releaseService, IOptions<Config> optionsConfig)
        {
            _releaseService = releaseService;
            _config = optionsConfig.Value;
        }

        [Route("refresh")]
        [HttpGet]
        [HttpPost]
        public async Task<string> Refresh([FromQuery] string source, [FromQuery(Name = "api_key")] string apiKey)
        {
            if (!_config.ApiKey.Equals(apiKey))
            {
                return "No, thank you.";
            }

            var type = source.ToLower() switch
            {
                "azure" => typeof(AzureReleaseSource),
                "github" => typeof(GithubReleaseSource),
                _ => null
            };

            if (type == null)
            {
                return $"Unknown source {source}";
            }

            await _releaseService.UpdateReleasesAsync(type);

            return "Thank you.";
        }
    }
}
