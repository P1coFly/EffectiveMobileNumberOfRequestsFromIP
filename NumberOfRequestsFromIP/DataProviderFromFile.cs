using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberOfRequestsFromIP
{
    //Реализовывает интерфейс IDataProvider. Считывает данные из файла
    internal class DataProviderFromFile: IDataProvider
    {
        public string[] GetData(string logFilePath)
        {
            // Проверяем существование файла
            if (!File.Exists(logFilePath))
            {
                throw new FileNotFoundException("Log file not found.", logFilePath);
            }

            // Читаем все строки из файла и возвращаем их
            return File.ReadAllLines(logFilePath);
        }
    }
}
