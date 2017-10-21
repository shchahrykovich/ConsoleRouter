# ConsoleRouter

[![Build status](https://ci.appveyor.com/api/projects/status/v5l08rovnse9m34d?svg=true)](https://ci.appveyor.com/project/shchahrykovich/consolerouter)

This is a console router, which helps you parse command line arguments.


## Usage
1. Install-Package ConsoleRouter
2. Create {Action}Controller.cs in Controllers folder
3. Add `AppHost` to your Program.cs
```c#
var builder = new AppHostBuilder()
  .WithRoute("{controller=help} {action=help}")
  .WithHelp("This is help string");
var host = builder.Build();
host.Run(args);
```

## Example
See [TestApp](https://github.com/shchahrykovich/ConsoleRouter/tree/master/Src/TestApp/Program.cs)

## Route format
"{controller=help}" - expects parameter 'controller' with default value 'help'

"task(controller=work,action=do)" - expands 'task' argument from the command line to 'controller=work' and 'action=do'

"-n{name}" - defines alias '-n' for argument 'name'

## Route examples
"task(controller=work,action=do)" - calls `WorkController.Do` method

"long-task(controller=Worker,action=task) -o{output}" - calls `WorkerController.Task` and passes argument `{output}`

## Built-in services
'CancellationToken' - fires on Ctrl-C 

'TextWriter' - `Console.Out`
