using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberOfRequestsFromIP
{
    internal class DataSenderToFile:IDataSender
    {
        public void SendData(Dictionary<string, int> data, string outputFilePath)
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                // Проходим по всем записям в словаре и записываем их в файл
                foreach (var entry in data)
                {
                    // Форматируем строку в виде "IP адрес - количество запросов"
                    string line = $"{entry.Key} {entry.Value}";

                    // Записываем строку в файл
                    writer.WriteLine(line);
                }
            }
        }
    }
}
