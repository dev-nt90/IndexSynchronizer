using IndexSynchronizerServices.Models;
using IndexSynchronizerServices.Repositories;
using IndexSynchronizerServices.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexSynchronizerCli
{
    internal static class Runner
    {
        internal static void Usage(List<String> validationFailures)
        {
            Console.WriteLine("No work done!");
            Console.WriteLine();
            Console.WriteLine("To use this program, please specify either a \"sync\" or \"preview\" argument, and fill in the values in the appsettings.json file, which should live alongside this executable.");
            Console.WriteLine();
            Console.WriteLine("Note: in preview mode, only the Source settings need to be filled in, as the Target settings will be unused.");
            Console.WriteLine();

            foreach (var failure in validationFailures) 
            { 
                Console.WriteLine(failure);
                Console.WriteLine();
            }
        }

        internal static Boolean ValidateSettings(string operationMode, IConfiguration configuration)
        {
            var valid = true;
            var validationFailures = new List<String>();

            if(String.IsNullOrWhiteSpace(operationMode))
            {
                validationFailures.Add("Operation cannot be empty. Please specify an operation mode.");
                valid = false;
            }

            if(!operationMode.Equals("sync", StringComparison.InvariantCultureIgnoreCase) && 
                !operationMode.Equals("preview", StringComparison.InvariantCultureIgnoreCase))
            {
                validationFailures.Add($"Operation {operationMode} is not valid. Please a valid operation mode.");
                valid = false;
            }

            if (configuration.GetSection("SourceConnectionSettings") is null)
            {
                validationFailures.Add("Could not find SourceConnectionSettings in appsettings");
                valid = false;
            }

            if (String.IsNullOrWhiteSpace(configuration["SourceConnectionSettings:ServerName"]))
            {
                validationFailures.Add("Could not find SourceConnectionSettings:ServerName");
                valid = false;
            }

            if (String.IsNullOrWhiteSpace(configuration["SourceConnectionSettings:DatabaseName"]))
            {
                validationFailures.Add("Could not find SourceConnectionSettings:DatabaseName");
                valid = false;
            }

            if (String.IsNullOrWhiteSpace(configuration["SourceConnectionSettings:SchemaName"]))
            {
                validationFailures.Add("Could not find SourceConnectionSettings:SchemaName");
                valid = false;
            }

            if (String.IsNullOrWhiteSpace(configuration["SourceConnectionSettings:TableName"]))
            {
                validationFailures.Add("Could not find SourceConnectionSettings:TableName");
                valid = false;
            }

            if (String.IsNullOrWhiteSpace(configuration["SourceConnectionSettings:UserName"]))
            {
                validationFailures.Add("Could not find SourceConnectionSettings:UserName");
                valid = false;
            }

            if (String.IsNullOrWhiteSpace(configuration["SourceConnectionSettings:Password"]))
            {
                validationFailures.Add("Could not find SourceConnectionSettings:Password");
                valid = false;
            }

            // only run validations on sync, where both connections are required
            if (operationMode.Equals("sync", StringComparison.InvariantCultureIgnoreCase))
            {
                if (configuration.GetSection("TargetConnectionSettings") is null)
                {
                    validationFailures.Add("Could not find TargetConnectionSettings in appsettings");
                    valid = false;
                }

                if (String.IsNullOrWhiteSpace(configuration["TargetConnectionSettings:ServerName"]))
                {
                    validationFailures.Add("Could not find TargetConnectionSettings:ServerName");
                    valid = false;
                }

                if (String.IsNullOrWhiteSpace(configuration["TargetConnectionSettings:DatabaseName"]))
                {
                    validationFailures.Add("Could not find TargetConnectionSettings:DatabaseName");
                    valid = false;
                }

                if (String.IsNullOrWhiteSpace(configuration["TargetConnectionSettings:SchemaName"]))
                {
                    validationFailures.Add("Could not find TargetConnectionSettings:SchemaName");
                    valid = false;
                }

                if (String.IsNullOrWhiteSpace(configuration["TargetConnectionSettings:TableName"]))
                {
                    validationFailures.Add("Could not find TargetConnectionSettings:TableName");
                    valid = false;
                }

                if (String.IsNullOrWhiteSpace(configuration["TargetConnectionSettings:UserName"]))
                {
                    validationFailures.Add("Could not find TargetConnectionSettings:UserName");
                    valid = false;
                }

                if (String.IsNullOrWhiteSpace(configuration["TargetConnectionSettings:Password"]))
                {
                    validationFailures.Add("Could not find TargetConnectionSettings:Password");
                    valid = false;
                }
            }

            if (!valid)
            {
                Usage(validationFailures);
            }

            return valid;
        }

        internal static void Run(string operationMode, IConfiguration configuration)
        {
            operationMode = operationMode.Trim();
            if(!ValidateSettings(operationMode, configuration))
            {
                return;
            }

            var sourceConnection = new ConnectionDetails
            {
                ServerName = configuration["SourceConnectionSettings:ServerName"],
                DatabaseName = configuration["SourceConnectionSettings:DatabaseName"],
                SchemaName = configuration["SourceConnectionSettings:SchemaName"],
                TableName = configuration["SourceConnectionSettings:TableName"],
                Username = configuration["SourceConnectionSettings:UserName"],
                Password = configuration["SourceConnectionSettings:Password"]
            };

            var targetConnection = new ConnectionDetails
            {
                ServerName = configuration["TargetConnectionSettings:ServerName"],
                DatabaseName = configuration["TargetConnectionSettings:DatabaseName"],
                SchemaName = configuration["TargetConnectionSettings:SchemaName"],
                TableName = configuration["TargetConnectionSettings:TableName"],
                Username = configuration["TargetConnectionSettings:UserName"],
                Password = configuration["TargetConnectionSettings:Password"]
            };

            if(operationMode.Equals("sync", StringComparison.InvariantCultureIgnoreCase))
            {
                DoSync(sourceConnection, targetConnection);
            }

            if(operationMode.Equals("preview", StringComparison.InvariantCultureIgnoreCase))
            {
                DoPreview(sourceConnection);
            }
        }

        internal static void DoSync(IConnectionDetails source, IConnectionDetails target)
        {
            Console.WriteLine("Starting sync...");
            try
            {
                var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

                var indexSyncService = new IndexSynchronizerService(loggerFactory.CreateLogger<IndexSynchronizerService>(), new IndexDefinitionRepository(), new IndexSyncRepository());

                indexSyncService.StartAsync(source, target, Guid.NewGuid().ToString()).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Sync complete!");
        }

        internal static void DoPreview(IConnectionDetails source)
        {
            Console.WriteLine("Starting preview...");
            try
            {
                var indexDefinitionRepo = new IndexDefinitionRepository();
                var indexDefinitions = indexDefinitionRepo.GetIndexDefinitionsAsync(source).Result;

                foreach (var kvp in indexDefinitions)
                {
                    Console.WriteLine($"Index Name: {kvp.Key}");
                    Console.WriteLine($"Index Definition: {kvp.Value}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Preview complete!");
        }
    }
}
