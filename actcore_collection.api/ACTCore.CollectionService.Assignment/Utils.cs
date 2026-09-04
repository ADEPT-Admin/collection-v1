using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTCore.CollectionService.Assignment
{
    public static class Utils
    {
        // แปลงวันที่เป็นรูปแบบ yyyy-MM-dd
        public static string FormatDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        // ตรวจสอบว่า string ว่างหรือไม่
        public static bool IsNullOrEmpty(string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Appends a message with a timestamp to the log file.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void LogMessage(string message)
        {
            if (string.IsNullOrEmpty(GlobalState.Instance._logFilePath)) return;

            try
            {
                // Get the directory of the log file
                string? logDirectory = Path.GetDirectoryName(GlobalState.Instance._logFilePath);

                // Check if the directory exists and create it if not
                if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
                
                string logEntry = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)} - {message}";
                File.AppendAllText(GlobalState.Instance._logFilePath, logEntry + Environment.NewLine);
                Console.WriteLine(logEntry); // Also print to console for real-time feedback
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write to log file: {ex.Message}");
            }
        }

        public static string GetCurrentFormattedDateTime()
        {
            // Use DateTime.Now to get the current local date and time.
            // The format string "yyyyMMddHHmmss" ensures the string is
            // easily sortable and contains no illegal characters for filenames.
            // Using CultureInfo.InvariantCulture ensures the format is always consistent,
            // regardless of the server's regional settings.
            return DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
        }


    }
}
