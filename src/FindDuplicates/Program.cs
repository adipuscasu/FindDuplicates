using FindDuplicates.Application;

var commandArgs = Environment.GetCommandLineArgs().Skip(1).ToArray();
var app = ServiceContainer.CreateApp();
app.Run(commandArgs);