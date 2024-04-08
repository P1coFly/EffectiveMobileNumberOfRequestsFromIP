using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NumberOfRequestsFromIP
{
    internal class AppConfig
    {
        [Option("file-log", Required = false, HelpText = "Path to the log file. Environment variable 'LOG_FILE_PATH'")]
        public string? LogFilePath { get; private set; }

        [Option("file-output", Required = false, HelpText = "Path to the output file. Environment variable 'OUTPUT_FILE_PATH'")]
        public string? OutputFilePath { get; private set; }

        [Option("address-start", Required = false, HelpText = "Lower bound of the IP address range. Environment variable 'ADDRESS_START'")]
        public string? AddressStart { get; private set; }

        [Option("address-mask", Required = false, HelpText = "Subnet mask specifying the upper bound of the IP address range." +
            " The --address-mask parameter cannot be used without --address-start parameter. Environment variable 'ADDRESS_MASK'")]
        public string? AddressMask { get; private set; } 
         
        [Option("time-start", Required = false, HelpText = "Lower bound of the time interval (format: dd.MM.yyyy). Environment variable 'TIME_START'")]
        public string? TimeStart { get; private set; }

        [Option("time-end", Required = false, HelpText = "Upper bound of the time interval (format: dd.MM.yyyy). Environment variable 'TIME_END'")]
        public string? TimeEnd { get; private set; }

        // Метод для проверки правильности переданных параметров
        public void Validate()
        {
            //проверяем обязательные параметры на null
            RequireNotNull(LogFilePath, "--file-log", "LOG_FILE_PATH");
            RequireNotNull(OutputFilePath, "--file-output", "OUTPUT_FILE_PATH");
            RequireNotNull(TimeStart, "--time-start", "TIME_START");
            RequireNotNull(TimeEnd, "--time-end", "TIME_END");


            // Проверяем, был ли указан параметр address-mask без address-start
            if (string.IsNullOrEmpty(AddressStart) && !string.IsNullOrEmpty(AddressMask))
            {
                throw new ArgumentException("The --address-mask parameter cannot be used without --address-start parameter. Use --help for more information.");
            }

            // Присваиваем значения по умолчанию, если addressStart или addressMask равны null
            if (AddressStart == null)
                AddressStart = "0.0.0.0";

            if (AddressMask == null)
                AddressMask = "0.0.0.0";

            //проверяем на соответсвие IPv4
            IsIPv4(AddressStart, "--address-start");
            IsIPv4(AddressMask, "--address-mask");

            // Проверка формата даты для параметров time-start и time-end
            DateTime tempDateTime;
            if (!DateTime.TryParseExact(TimeStart, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out tempDateTime))
            {
                throw new ArgumentException("Invalid format for --time-start parameter. Date should be in the format dd.MM.yyyy. Use --help for more information.");
            }

            if (!DateTime.TryParseExact(TimeEnd, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out tempDateTime))
            {
                throw new ArgumentException("Invalid format for --time-end parameter. Date should be in the format dd.MM.yyyy. Use --help for more information.");
            }


        }

        private void RequireNotNull(string? value, string paramName, string envName)
        {
            if (value == null)
            {
                throw new ArgumentException($"The {paramName} parameter is required. Please provide a value either via command line or environment variable ({envName}). Use --help for more information.");
            }
        }
        private void IsIPv4(string ipAddress, string paramName)
        {
            if (IPAddress.TryParse(ipAddress, out IPAddress address))
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return;
                }
            }

            throw new ArgumentException($"The {paramName} parameter must be a valid IPv4 address. Use --help for more information.");

        }

        //Считываем переменные среды
        public void GetEnvVar()
        {
            if (LogFilePath == null) {
                LogFilePath = Environment.GetEnvironmentVariable("LOG_FILE_PATH");
            }

            if (OutputFilePath == null) {
                OutputFilePath = Environment.GetEnvironmentVariable("OUTPUT_FILE_PATH");
            }

            if (AddressStart == null) {
                AddressStart = Environment.GetEnvironmentVariable("ADDRESS_START");
            }

            if (AddressMask == null) {
                AddressMask = Environment.GetEnvironmentVariable("ADDRESS_MASK");
            }

            if (TimeStart == null)
            {
                TimeStart = Environment.GetEnvironmentVariable("TIME_START");
            }

            if (TimeEnd == null)
            {
                TimeEnd = Environment.GetEnvironmentVariable("TIME_END");
            }
        }

    }
}
