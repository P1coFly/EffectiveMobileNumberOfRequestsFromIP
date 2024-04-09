using CommandLine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NumberOfRequestsFromIP
{
    internal class AppConfig
    {
        [Option("file-log", Required = false, HelpText = "Path to the log file. Environment variable 'FILE_LOG'")]
        public string? LogFilePath { get; private set; }

        [Option("file-output", Required = false, HelpText = "Path to the output file. Environment variable 'FILE_OUTPUT'")]
        public string? OutputFilePath { get; private set; }

        [Option("address-start", Required = false, HelpText = "Lower bound of the IP address range. parameter must be a valid IPv4. Environment variable 'ADDRESS_START'")]
        public string? AddressStart { get; private set; }

        [Option("address-mask", Required = false, HelpText = "Subnet mask specifying the upper bound of the IP address range. Parameter must be a valid address mask IPv4 or prefix address.." +
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
            RequireNotNull(LogFilePath, "--file-log", "FILE_LOG");
            RequireNotNull(OutputFilePath, "--file-output", "FILE_OUTPUT");
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
            AddressMaskValidate();

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

        private void AddressMaskValidate()
        {
            // Проверяем, является ли входной параметр префиксом
            if (int.TryParse(AddressMask, out int prefixLength))
            {
                if (prefixLength < 0 || prefixLength > 32)
                {
                    throw new ArgumentException("Prefix length must be in the range of 0 to 32.");
                }

                // Преобразуем префикс в маску подсети
                uint mask = 0;
                if (prefixLength != 0)
                {
                    mask = uint.MaxValue << (32 - prefixLength);
                }

                byte[] bytes = new byte[4];
                bytes[0] = (byte)(mask >> 24);
                bytes[1] = (byte)(mask >> 16);
                bytes[2] = (byte)(mask >> 8);
                bytes[3] = (byte)(mask);

                // Преобразуем маску в строковое представление
                AddressMask = "";
                for (int i = 0; i < 4; i++)
                {
                    int tmp = (int)bytes[i];
                    if (i == 3){
                        AddressMask += tmp.ToString();
                        break;
                    }
                    AddressMask += tmp.ToString() + ".";
                }
                
                byte[] ipBytes = AddressMask.Split('.').Select(byte.Parse).ToArray();
            }
            else
            {
                // Если входной параметр не является префиксом, проверяем его как IP-адрес
                IsIPv4(AddressMask, "--address-mask");

                // Проверяем валидна ли маска
                string[] octets = AddressMask.Split('.');

                uint[] maskOctets = new uint[4];
                for (int i = 0; i < 4; i++)
                {
                    if (!uint.TryParse(octets[i], out maskOctets[i]))
                    {
                        throw new ArgumentException($"The --address-mask parameter must be a valid address mask IPv4 or prefix address.. Use --help for more information.");
                    }
                }

                // получаем маску
                uint mask = 0;
                for (int i=0; i<4; i++)
                {
                    mask = mask << 8;
                    mask += maskOctets[i];
                }

                // если маска = 0, то выходим
                if (mask == 0)
                {
                    return;
                }

                //идём по битам, ищем нулевой бит и генерируем эталонную маску
                int count = 0;
                var tmp = mask;
                for (uint j = 0; j < 1U<<31; j++)
                {
                    
                    if ( (tmp & (0b1U<<31)) == 0){  
                        break;
                    }
                    count++;
                    tmp = tmp << 1;
                }


                // проверяем с эталоном
                if (mask != uint.MaxValue << (32 - count))
                {
                    throw new ArgumentException($"The --address-mask parameter must be a valid address mask IPv4 or prefix address. Use --help for more information.");
                }


            }
        }


        //Считываем переменные среды
        public void GetEnvVar()
        {
            if (LogFilePath == null) {
                LogFilePath = Environment.GetEnvironmentVariable("FILE_LOG");
            }

            if (OutputFilePath == null) {
                OutputFilePath = Environment.GetEnvironmentVariable("FILE_OUTPUT");
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
