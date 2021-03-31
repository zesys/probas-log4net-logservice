using Microsoft.Extensions.Logging;
using System;

namespace ConsoleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new LoggerFactory().AddLog4Net().CreateLogger(nameof(Program));

            logger.LogInformation("这是一条普通日志");
            logger.LogDebug("这是一条调试日志");
            logger.LogWarning("这是一条警告日志");
            logger.LogError("这是一条错误日志");

            try
            {
                try
                {
                    try
                    {
                        throw new DivideByZeroException();
                    }
                    catch (Exception ex1)
                    {
                        throw new Exception("1", ex1);
                    }
                }
                catch (Exception ex2)
                {
                    throw new Exception("2", ex2);
                }
            }
            catch (Exception ex)
            {
                logger.LogTrace(ex, "这是一条异常日志");

                logger.LogInformation(ex, "这是一条普通日志");
                logger.LogDebug(ex, "这是一条调试日志");
                logger.LogWarning(ex, "这是一条警告日志");
                logger.LogError(ex, "这是一条错误日志");
            }

            Console.ReadKey();
        }
    }
}
