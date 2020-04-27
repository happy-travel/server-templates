﻿using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

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


        public static bool IsLocal(this IWebHostEnvironment hostingEnvironment) 
            => hostingEnvironment.IsEnvironment(LocalEnvironment);    
        
        
        private const string LocalEnvironment = "Local";
    }
}
