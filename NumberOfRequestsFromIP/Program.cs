using CommandLine;
using System.ComponentModel;

namespace NumberOfRequestsFromIP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataProviderFromFile dataProvider = new DataProviderFromFile();
            DataSenderToFile dataSender = new DataSenderToFile();
            CounterIP counterIP = new CounterIP();
            try
            {
                //Парсим параметры командной строки в экземпляр класса AppConfig
                CommandLine.Parser.Default.ParseArguments<AppConfig>(args)
                    .WithParsed(config =>
                    {
                        config.GetEnvVar();
                        config.Validate();

                        CalculateNumberOfIPRequests(config, dataProvider, counterIP, dataSender);

                        Console.WriteLine($"Success - File was created: {config.OutputFilePath}");
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

           
        }

        /* На вход подаётся конфиг, объект реализующий интерфейс IDataProvider (Получение ip-адресов),
           объект реализующий интерфейс ICounterIP (подсчёт ip-адресов) и
           объект реализующий интерфейс IDataSender (сохранение\отправка результата)*/
        static void CalculateNumberOfIPRequests(AppConfig config, IDataProvider dataProvider, ICounterIP counterIP, IDataSender dataSender)
        {
            //считываем входные данные
            var dataIP = dataProvider.GetData(config.LogFilePath);
            //считаем кол-во ip-адресов
            var processedData = counterIP.CountIP(dataIP, config.AddressStart, config.AddressMask, config.TimeStart, config.TimeEnd);
            //запись результата
            dataSender.SendData(processedData, config.OutputFilePath);
        }
    }
}
