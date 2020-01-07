using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace frameworkApi
{
    public class LogHelper
    {
        private ILog _log4Net = null;
        private const string DEFAULT_LOGGER_NAME = "Logger";

        /// <summary>
        /// Prevents a default instance of the <see cref="LogWriter"/> class from being created.
        /// </summary>
        /// <param name="log4NetInstance">The log4net instance to be used.</param>
        private LogHelper(ILog log4NetInstance)
        {
            _log4Net = log4NetInstance;
        }

        /// <summary>
        /// 获取具有指定名称的配置器.
        /// </summary>
        /// <param name="configName">日志配置文件的名字</param>
        /// <returns>The logger obtained.</returns>
        /// <exception cref="System.Configuration.ConfigurationException">Thrown when no logger with the specified configuration name was found.</exception>
        public static LogHelper GetLogger(string configName)
        {
            var logger = LogManager.GetLogger(configName);
            if (logger == null)
            {
                throw new ArgumentException(string.Format("No logger configuration named '{0}' was found in the configuration.", configName), "configName");
            }
            return new LogHelper(logger);
        }

        /// <summary>
        /// 获取默认配置器.
        /// </summary>
        public static LogHelper Default
        {
            get
            {
                return GetLogger(DEFAULT_LOGGER_NAME);
            }
        }

        /// <summary>
        /// 写入信息级日志记录
        /// </summary>
        /// <param name="message">The message to be written.</param>
        public void WriteInfo(object message)
        {
            _log4Net.Info(message);
        }

        /// <summary>
        /// 写入警告级日志记录
        /// </summary>
        /// <param name="message">The message to be written.</param>
        public void WriteWarning(object message)
        {
            _log4Net.Warn(message);
        }

        /// <summary>
        /// 写入警告级日志记录及exception信息.
        /// </summary>
        /// <param name="message">The message to be written.</param>
        /// <param name="exception">The exception.</param>
        public void WriteWarning(object message, System.Exception exception)
        {
            _log4Net.Warn(message, exception);
        }

        /// <summary>
        /// 写入错误级日志信息
        /// </summary>
        /// <param name="message">The message to be written.</param>
        public void WriteError(object message)
        {
            _log4Net.Error(message);
        }

        /// <summary>
        /// 写入错误级日志信息及exception信息.
        /// </summary>
        /// <param name="message">The message to be written.</param>
        /// <param name="exception">The exception.</param>
        public void WriteError(object message, System.Exception exception)
        {
            _log4Net.Error(message, exception);
        }

        /// <summary>
        /// 写入致命级日志信息.
        /// </summary>
        /// <param name="message">The message to be written.</param>
        public void WriteFatal(object message)
        {
            _log4Net.Fatal(message);
        }

        /// <summary>
        /// 写入致命级日志信息及Exception信息
        /// </summary>
        /// <param name="message">The message to be written.</param>
        /// <param name="exception">The exception.</param>
        public void WriteFatal(object message, System.Exception exception)
        {
            _log4Net.Fatal(message, exception);
        }

        /// <summary>
        /// 删除日志（30天前的）
        /// </summary>
        public void DeleteLog()
        {
            string logDirPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Log");
            //如果获取不到返回
            if (!Directory.Exists(logDirPath))
                return;
            int days = 30;
            //循环删除日志
            foreach (string filePath in Directory.GetFiles(logDirPath))
            {
                DateTime dt;
                DateTime.TryParse(Path.GetFileNameWithoutExtension(filePath).Replace(@"Log\", "").Replace(".", "-"), out dt);
                if (dt.AddDays(days).CompareTo(DateTime.Now) < 0)
                {
                    File.Delete(filePath);
                }
            }
        }
    }
}