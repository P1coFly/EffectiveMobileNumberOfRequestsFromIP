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

            CommandLine.Parser.Default.ParseArguments<AppConfig>(args)
                .WithParsed(config =>
                {
                    config.Validate();

                    CalculateNumberOfIPRequests(config,dataProvider,counterIP,dataSender);
                });
        }

        static void CalculateNumberOfIPRequests(AppConfig config, IDataProvider dataProvider, ICounterIP counterIP, IDataSender dataSender)
        {
            var dataIP = dataProvider.GetData(config.LogFilePath);
            var processedData = counterIP.CountIP(dataIP, config.AddressStart, config.AddressMask, config.TimeStart, config.TimeEnd);
            dataSender.SendData(processedData, config.OutputFilePath);
        }
    }
}
