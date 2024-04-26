# DropBear.Codex.AppLogger
## Description
`DropBear.Codex.AppLogger` is a flexible and configurable logging library designed to wrap around existing logging frameworks. It provides a fluent API for configuring logging behavior, including support for console and file outputs, custom log formats, and more. The library aims to make logging in .NET applications straightforward and adaptable.

## Features
 - Fluent API for easy configuration
 - Supports console and file logging
 - Integrates with ZLogger
 - Provides extension methods for easy setup with dependency injection containers
 - Dynamic logger configuration through runtime methods

## Getting Started
To use `DropBear.Codex.AppLogger` in your project, start by configuring the logger factory according to your needs:

## Basic Configuration
 ```csharp
 var loggerFactory = new LoggerConfigurationBuilder()
     .SetLogLevel(LogLevel.Information)
     .EnableConsoleOutput(true)
     .Build();
 var logger = loggerFactory.CreateLogger<MyClass>();
 logger.LogInformation("Hello, World!");
 ```
## Advanced Configuration with File Logging
 ```csharp
 var loggerFactory = new LoggerConfigurationBuilder()
     .SetLogLevel(LogLevel.Debug)
     .EnableConsoleOutput(true)
     .UseJsonFormatter()
     .ConfigureRollingFileAsync("logs/", 1024).GetAwaiter().GetResult()
     .Build();
 var logger = loggerFactory.CreateLogger<MyClass>();
 logger.LogDebug("Debugging information.");
 ```
## Using LoggerManager
 `LoggerManager` is a singleton class that provides a centralized way to manage and retrieve loggers throughout your application. It ensures that only one instance of each logger is created and provides a mechanism for reconfiguring loggers at runtime.

## Retrieving a Logger
 ```csharp
 var logger = LoggerManager.Instance.GetLogger<MyClass>();
 logger.LogInformation("Managed logger instance ready to use.");
 ```
## Reconfiguring Loggers at Runtime
 `LoggerManager` allows you to dynamically change logger configurations, such as log levels or outputs. This can be especially useful in scenarios where logging behaviors need to be adjusted without restarting the application.
 ```csharp
 LoggerManager.Instance.ConfigureLogger(builder =>
 {
     builder.SetLogLevel(LogLevel.Warning)
            .EnableConsoleOutput(false)
            .UseJsonFormatter(true)
            .ConfigureRollingFile("logs/", 2048);
 });
 var logger = LoggerManager.Instance.GetLogger<MyClass>();
 logger.LogWarning("Logger configuration updated.");
 ```
This functionality supports dynamic environments and helps maintain flexibility in how applications log their activities.

## Integration with ASP.NET Core
 `DropBear.Codex.AppLogger` can be easily integrated into ASP.NET Core applications using the provided extension method:
 ```csharp
 public void ConfigureServices(IServiceCollection services)
 {
     services.AddAppLogger(builder =>
     {
         builder.SetLogLevel(LogLevel.Information)
                .EnableConsoleOutput(true)
                .UseJsonFormatter()
                .ConfigureRollingFile("logs/", 1024);
     });
 }
 ```
## Warning
 This code is under active pre-development and is subject to change. It may or may not work as expected and should be used with caution in production environments.

## Contributing
 We welcome contributions and suggestions! Please submit issues and pull requests on our GitHub repository for any features or bug fixes.

## License
 This project is licensed under the LGPLv3 License - see the https://www.gnu.org/licenses/lgpl-3.0.en.html for details.
