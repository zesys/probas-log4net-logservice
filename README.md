# Probas.log4net.LogService

#### 介绍
log4net 基于阿里云日志服务组件

#### 软件架构
软件架构说明

#### 安装Nuget包
```bash
dotnet add package Probas.log4net.LogService
```

#### 使用说明(配置)

1.  在 `Program.cs` 进行如下配置
```cs
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, builder) =>
                {
                    // 添加 Log4Net 日志扩展
                    builder.AddLog4Net();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
```
2.  添加配置文件 `log4net.config`
```xml
<?xml version="1.0" encoding="utf-8" ?>
<log4net>
  <appender name="AliSlsApiAppender" type="Probas.log4net.LogService.AliSlsApiAppender, Probas.log4net.LogService">
    <Settings>
      <endpoint value="*.log.aliyuncs.com" /><!--API访问地址-->
      <topic value="*" /><!--日志主题，用户自定义字段，用于区分不同特征的日志数据-->
      <accesskey value="*" /><!--AccessKey ID-->
      <secretkey value="*" /><!--AccessKey Secret-->
      <project value="*" /><!--配置为日志服务Project名称-->
      <logstore value="*" /><!--配置为日志服务Logstore名称-->
    </Settings>
    <layout type="Probas.log4net.LogService.LogServiceLayout, Probas.log4net.LogService" >
      <appkey value="CoreAppSamlpeTests" />
    </layout>
  </appender>
  <appender name="AliSlsKafkaAppender" type="Probas.log4net.LogService.AliSlsKafkaAppender, Probas.log4net.LogService">
    <Settings>
      <endpoint value="*.*.log.aliyuncs.com:10012" /><!--Kafka访问地址-->
      <accesskey value="*" /><!--AccessKey ID-->
      <secretkey value="*" /><!--AccessKey Secret-->
      <project value="*" /><!--配置为日志服务Project名称-->
      <logstore value="*" /><!--配置为日志服务Logstore名称-->
    </Settings>
    <layout type="Probas.log4net.LogService.LogServiceLayout, Probas.log4net.LogService" >
      <appkey value="CoreAppSamlpeTests" />
    </layout>
  </appender>
  <root>
    <level value="ALL"/>
    <!--使用基于阿里日志服务API的实现-->
    <appender-ref ref="AliSlsApiAppender" />
    <!--使用基于阿里日志服务Kafka协议的实现-->
    <appender-ref ref="AliSlsKafkaAppender" />
  </root>
</log4net>

```
3.  使用，参考Sample
 ```cs
        // 自动注入直接使用
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
 ```
