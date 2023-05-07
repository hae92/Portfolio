using System.Runtime.CompilerServices;
using Serilog;
using Serilog.Events;
using System;

namespace BMS.Core.Logger
{
    public class Logger
    {

        private Serilog.Core.Logger _logger;

        public Logger()
        {
            String logPath = @"C:\Project\BMS.Server\Log";
            String logName = "BMS_Log";

            _logger = new LoggerConfiguration().MinimumLevel.Debug().
                WriteTo.Console(LogEventLevel.Debug, outputTemplate: "[{Timestamp:yyyy/MM/dd HH:mm:ss.fff}][{Level:u4}]{Message}{NewLine}{Exception}").
                //WriteTo.Console(outputTemplate: "[{Timestamp:yyyy/MM/dd HH:mm:ss.fff}][{Level:u3}]{Message}{NewLine}{Exception}").
                //WriteTo.Console(outputTemplate: "[{Timestamp:yyyy/MM/dd HH:mm:ss.fff}][{Level}]{Message}{NewLine}{Exception}").
                WriteTo.File(string.Format(@"{0}\{1}_.txt", logPath, logName), rollingInterval: RollingInterval.Hour, rollOnFileSizeLimit: true).CreateLogger();
        }

        public void LogWrite(Object obj, String text, [CallerLineNumber] int line = 0)
        {
            try
            {
                var _obj = obj as System.Reflection.MethodBase;
                var logWrite = true;
                if (_obj != null)
                {
                    //var _text = string.Format("[{0}][{1}][{2}] {3}", _obj.ReflectedType.FullName.PadRight(30, ' '), _obj.Name.PadRight(16, ' '), line.ToString().PadLeft(4, '0'), (text.Length > 80) ? "\r\n" + text : text);
                    var _text = string.Format("[{0}][{1}][{2}] {3}", _obj.ReflectedType.FullName, _obj.Name.PadRight(16, ' '), line.ToString().PadLeft(4, '0'), (text.Length > 80) ? "\r\n" + text : text);
                    _logger.Write(Serilog.Events.LogEventLevel.Debug, _text);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}