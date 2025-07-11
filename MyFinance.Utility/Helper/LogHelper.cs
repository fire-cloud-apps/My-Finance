using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Utility.Helper;
public static class LogHelper
{
    public static long LogId { get; set; } = 1;
    public static void LogMessage(string message, LogLevel level = LogLevel.Information, string? source = null, string? method = null, string? user = null, Exception? ex = null)
    {
        
#if DEBUG
        var logMessage = new StringBuilder();
        //logMessage.Append($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] ");
        logMessage.Append($"[{DateTime.Now.ToString("yyyy-MM-dd")} - Id: {LogId}] ");

        switch (level)
        {
            case LogLevel.Critical:
                logMessage.Append("[Critical] ");
                logMessage.Append(ex.ToString());
                break;
            case LogLevel.Error:
                logMessage.Append("[Error] ");
                logMessage.Append(ex.ToString());
                break;
            case LogLevel.Warning:
                logMessage.Append("[Warning] ");
                logMessage.Append(message);
                break;
            case LogLevel.Information:
                logMessage.Append("[Information] ");
                
                logMessage.Append(message);
                break;
            case LogLevel.Debug:
                logMessage.Append("[Debug] ");
                logMessage.Append(message);
                break;
            case LogLevel.Trace:
                logMessage.Append("[Trace] ");
                logMessage.Append(message);
                break;
            default:
                logMessage.Append("[Unknown] ");
                logMessage.Append(message);
                break;
        }

        if (!string.IsNullOrEmpty(source))
        {
            logMessage.Append($"[{source}] ");
        }
        if (!string.IsNullOrEmpty(method))
        {
            logMessage.Append($"[{method}] ");
        }
        if (!string.IsNullOrEmpty(user))
        {
            logMessage.Append($"[User: {user}] ");
        }

        switch(level)
        {
            case LogLevel.Critical:
            case LogLevel.Error:
                Console.Error.WriteLine(logMessage.ToString());
                break;
            default:
                Console.WriteLine(logMessage.ToString());
                break;
        }
        
        LogId++;
        if (LogId > 1000000)
        {
            LogId = 1; // Reset LogId if it exceeds a million
        }
#endif

    }
}

