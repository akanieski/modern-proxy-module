using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace ModernProxyModule
{
    public static class Logger
    {
        private static readonly string _proxyLogEnvVariable = "HTTP_PROXY_LOG_PATH";
        private static readonly string _proxyLogPath = Environment.GetEnvironmentVariable(_proxyLogEnvVariable) ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static readonly string _logDirectory = File.Exists(_proxyLogPath) ? Path.GetDirectoryName(_proxyLogPath) : _proxyLogPath;
        private static readonly string _logFileName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
        private static readonly string _logFile = Path.Combine(_logDirectory, $"{_logFileName}.proxylog");
        private static readonly long _maxLogSize = 5 * 1024 * 1024; // 5MB

        public static void Log(string message)
        {
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }

            if (File.Exists(_logFile) && new FileInfo(_logFile).Length > _maxLogSize)
            {
                RollLog();
            }

            using (var writer = new StreamWriter(_logFile, true, Encoding.UTF8))
            {
                var m = $"[{DateTime.UtcNow.ToString("s")}] {message}";
                writer.WriteLine(m);
                Console.WriteLine(m);
            }
        }

        private static void RollLog()
        {
            var logFiles = Directory.GetFiles(_logDirectory, "*.proxylog");
            var lastLogFile = logFiles[logFiles.Length - 1];
            var lastLogFileNumber = int.Parse(lastLogFile.Substring(lastLogFile.LastIndexOf('.') + 1));
            var newLogFileNumber = lastLogFileNumber + 1;
            var newLogFile = Path.Combine(_logDirectory, $"{_logFileName}.{newLogFileNumber}.proxylog");

            File.Move(lastLogFile, newLogFile);
        }
    }
}
