# Probas.log4net.LogService

#### Description
log4net 基于阿里云日志服务组件

#### Software Architecture
Software architecture description

#### Install Nuget Package
```bash
dotnet add package Probas.log4net.LogService
```

#### Instructions(Usage)

1.  Configure in `Program.cs`
```cs
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, builder) =>
                {
                    // Add log4net log extension
                    builder.AddLog4Net();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
```

2.  Add configuration file `log4net.config`
```xml
<?xml version="1.0" encoding="utf-8" ?>
<log4net>
  <appender name="AliSlsApiAppender" type="Probas.log4net.LogService.AliSlsApiAppender, Probas.log4net.LogService">
    <Settings>
      <endpoint value="*.log.aliyuncs.com" /><!--Api access address-->
      <topic value="*" /><!--Log topic, user-defined field, used to distinguish log data with different characteristics-->
      <accesskey value="*" /><!--AccessKey ID-->
      <secretkey value="*" /><!--AccessKey Secret-->
      <project value="*" /><!--Configure as log service project name-->
      <logstore value="*" /><!--Configure as log service logstore name-->
    </Settings>
    <layout type="Probas.log4net.LogService.LogServiceLayout, Probas.log4net.LogService" >
      <appkey value="CoreAppSamlpeTests" />
    </layout>
  </appender>
  <appender name="AliSlsKafkaAppender" type="Probas.log4net.LogService.AliSlsKafkaAppender, Probas.log4net.LogService">
    <Settings>
      <endpoint value="*.*.log.aliyuncs.com:10012" /><!--Kafka access address-->
      <accesskey value="*" /><!--AccessKey ID-->
      <secretkey value="*" /><!--AccessKey Secret-->
      <project value="*" /><!--Configure as log service project name-->
      <logstore value="*" /><!--Configure as log service logstore name-->
    </Settings>
    <layout type="Probas.log4net.LogService.LogServiceLayout, Probas.log4net.LogService" >
      <appkey value="CoreAppSamlpeTests" />
    </layout>
  </appender>
  <root>
    <level value="ALL"/>
    <!--Implementation of using API based on Ali log service-->
    <appender-ref ref="AliSlsApiAppender" />
    <!--Implementation of Kafka protocol based on Ali log service-->
    <appender-ref ref="AliSlsKafkaAppender" />
  </root>
</log4net>

```

3.  Use, refer to sample
 ```cs
        // auto injection direct use
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
 ```
