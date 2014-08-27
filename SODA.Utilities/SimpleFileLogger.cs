using System;
using System.IO;

namespace SODA.Utilities
{
    /// <summary>
    /// A helper class for logging messages to files and the Console.
    /// </summary>
    public class SimpleFileLogger : IDisposable
    {
        static readonly string timestamp = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// The default filepath used for logging.
        /// </summary>
        public static readonly string DefaultLogFile = "log.txt";

        /// <summary>
        /// The default maximum size a log file can grow to before it is rolled over. The default is 1MB.
        /// </summary>
        public static readonly int DefaultMaximumLogBytes = 1048576;

        /// <summary>
        /// Handle to the underlying log file.
        /// </summary>
        private readonly TextWriter logger = null;

        /// <summary>
        /// The complete filepath to this SimpleFileLogger's log file.
        /// </summary>
        public string LogFilePath { get; private set; }
        
        /// <summary>
        /// Initialize a new SimpleFileLogger using the default log file and maximum log size.
        /// </summary>
        public SimpleFileLogger()
            : this(DefaultLogFile, DefaultMaximumLogBytes) { }

        /// <summary>
        /// Initialize a new SimpleFileLogger object using the specified log file path and the default maximum log size.
        /// </summary>
        /// <param name="logFilePath">The path to a readable and writeable text file.</param>
        public SimpleFileLogger(string logFilePath)
            : this(logFilePath, DefaultMaximumLogBytes) { }

        /// <summary>
        /// Initialize a new SimpleFileLogger object using the specified maximum log size (in bytes) and the default log file.
        /// </summary>
        /// <param name="maxLogBytes">A positive number of bytes that the log file size should not exceed.</param>
        public SimpleFileLogger(int maxLogBytes)
            : this(DefaultLogFile, maxLogBytes) { }

        /// <summary>
        /// Initialize a new SimpleFileLogger object using the specified file and maximum log size (in bytes).
        /// </summary>
        /// <param name="logFilePath">The path to a readable and writeable text file.</param>
        /// <param name="maxLogBytes">A positive number of bytes that the log file size should not exceed.</param>
        public SimpleFileLogger(string logFilePath, int maxLogBytes)
        {
            if (String.IsNullOrEmpty(logFilePath))
                throw new ArgumentNullException("logFilePath");

            if (maxLogBytes < 1)
                throw new ArgumentOutOfRangeException("maxLogBytes", "Maximum log size must be a positive number (in bytes).");

            LogFilePath = logFilePath;

            if (!File.Exists(logFilePath))
                File.WriteAllLines(logFilePath, new[] { String.Format("[{0}] Log Start", DateTime.Now.ToString(timestamp)) });

            var fi = new FileInfo(logFilePath);

            if (fi.Length > maxLogBytes)
                File.WriteAllLines(logFilePath, new[] { String.Format("[{0}] Log Rollover (max size: {1} bytes)", DateTime.Now.ToString(timestamp), maxLogBytes) });

            logger = fi.AppendText();
        }

        /// <summary>
        /// Implementing IDisposable to clean up the log file handler.
        /// </summary>
        public void Dispose()
        {
            if (logger != null)
            {
                logger.Flush();
                logger.Dispose();
            }
        }
        
        /// <summary>
        /// Log a blank line.
        /// </summary>
        public void WriteLine()
        {
            if (logger != null)
                logger.WriteLine();
        }

        /// <summary>
        /// Log the specified message on its own line.
        /// </summary>
        /// <param name="message">The text of the message to log.</param>
        public void WriteLine(string message)
        {
            string line = String.Format("[{0}] {1}", DateTime.Now.ToString(timestamp), message);

            if (logger != null)
                logger.WriteLine(line);

            Console.WriteLine(line);
        }

        /// <summary>
        /// Log a message, using the specified format and arguments, on its own line.
        /// </summary>
        /// <param name="format">The message template.</param>
        /// <param name="args">The data to insert into the message template.</param>
        public void WriteLine(string format, params object[] args)
        {
            WriteLine(String.Format(format, args));
        }

        /// <summary>
        /// Log the specified message and data from the specified exception.
        /// </summary>
        /// <param name="message">The text of the message to log.</param>
        /// <param name="exception">An exception whose data will be logged.</param>
        public void Write(string message, Exception exception)
        {
            WriteLine("{0}{4}{3}{1}{4}{3}{3}{2}", message, exception.Message, exception.StackTrace, "\t", Environment.NewLine);

            if (exception.InnerException != null)
                Write(exception.InnerException);
        }

        /// <summary>
        /// Log data from the specified exception.
        /// </summary>
        /// <param name="exception">An exception whose data will be logged.</param>
        public void Write(Exception exception)
        {
            Write(String.Empty, exception);
        }
    }
}
