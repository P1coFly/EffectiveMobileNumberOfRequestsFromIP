using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberOfRequestsFromIP
{
    internal class AppConfig
    {
        [Option("file-log", Required = true, HelpText = "Path to the log file.")]
        public string LogFilePath { get; private set; }

        [Option("file-output", Required = true, HelpText = "Path to the output file.")]
        public string OutputFilePath { get; private set; }

        [Option("address-start", Required = false, HelpText = "Lower bound of the IP address range.")]
        public string? AddressStart { get; private set; }

        [Option("address-mask", Required = false, HelpText = "Subnet mask specifying the upper bound of the IP address range.")]
        public string? AddressMask { get; private set; }

        [Option("time-start", Required = true, HelpText = "Lower bound of the time interval (format: dd.MM.yyyy).")]
        public string TimeStart { get; private set; }

        [Option("time-end", Required = true, HelpText = "Upper bound of the time interval (format: dd.MM.yyyy).")]
        public string TimeEnd { get; private set; }

        // Метод для проверки правильности переданных параметров
        public void Validate()
        {
            // Проверка формата даты для параметров time-start и time-end
            DateTime tempDateTime;
            if (!DateTime.TryParseExact(TimeStart, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out tempDateTime))
            {
                throw new ArgumentException("Invalid format for --time-start parameter. Date should be in the format dd.MM.yyyy.");
            }

            if (!DateTime.TryParseExact(TimeEnd, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out tempDateTime))
            {
                throw new ArgumentException("Invalid format for --time-end parameter. Date should be in the format dd.MM.yyyy.");
            }

            // Проверяем, был ли указан параметр address-mask без address-start
            if (string.IsNullOrEmpty(AddressStart) && !string.IsNullOrEmpty(AddressMask))
            {
                throw new ArgumentException("The --address-mask parameter cannot be used without --address-start parameter.");
            }
        }

    }
}
