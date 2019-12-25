using System;
using Microsoft.Extensions.Configuration;

namespace HappyTravel.ServiceTemplate.Infrastructure.Environments
{
    public static class EnvironmentVariableHelper
    {
        public static string Get(string key, IConfiguration configuration)
        {
            var environmentVariable = configuration[key];
            if (environmentVariable is null)
                throw new Exception($"Couldn't obtain the value for '{key}' configuration key.");

            return Environment.GetEnvironmentVariable(environmentVariable);
        }
    }
}
