using IndexSynchronizerCli;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
var operationMode = args[0];

Runner.Run(operationMode, configuration);