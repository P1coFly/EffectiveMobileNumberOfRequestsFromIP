using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberOfRequestsFromIP
{
    public class CounterIP: ICounterIP
    {
        public Dictionary<string, int> CountIP(string[] data, string? addressStart, string? addressMask, string timeStart, string timeEnd)
        {
            // Присваиваем значения по умолчанию, если addressStart или addressMask равны null
            if (addressStart == null)
                addressStart = "0.0.0.0";

            if (addressMask == null)
                addressMask = "0.0.0.0";

            // Создаем словарь для хранения количества обращений к каждому IP-адресу
            Dictionary<string, int> ipCounts = new Dictionary<string, int>();

            // Преобразуем строковые представления времени в объекты DateTime
            DateTime startTime = DateTime.ParseExact(timeStart, "dd.MM.yyyy", null);
            DateTime endTime = DateTime.ParseExact(timeEnd, "dd.MM.yyyy", null);

            // Проходим по всем строкам данных
            foreach (var line in data)
            {
                // Разделяем строку на IP-адрес и время обращения
                string[] parts = line.Split(':');
                string ipAddress = parts[0];
                DateTime accessTime = DateTime.Parse(parts[1].Split(" ")[0]);

                // Проверяем, подходит ли IP-адрес по фильтрам
                if (IsIPInRange(ipAddress, addressStart, addressMask) && IsTimeInRange(accessTime, startTime, endTime))
                {
                    // Если IP-адрес подходит, увеличиваем счетчик обращений
                    if (ipCounts.ContainsKey(ipAddress))
                    {
                        ipCounts[ipAddress]++;
                    }
                    else
                    {
                        ipCounts[ipAddress] = 1;
                    }
                }
            }

            return ipCounts;
        }

        private bool IsIPInRange(string ipAddress, string addressStart, string addressMask)
        {
            if (addressMask == "0.0.0.0")
                return true;

            // Разбиваем IP-адрес и маску на отдельные части
            byte[] ipBytes = ipAddress.Split('.').Select(byte.Parse).ToArray();
            byte[] startBytes = addressStart.Split('.').Select(byte.Parse).ToArray();
            byte[] maskBytes = addressMask.Split('.').Select(byte.Parse).ToArray();

            // Применяем маску к IP-адресу и к начальному адресу
            byte[] maskedIP = new byte[4];
            byte[] maskedStart = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                maskedIP[i] = (byte)(ipBytes[i] & maskBytes[i]);
                maskedStart[i] = (byte)(startBytes[i] & maskBytes[i]);
            }

            // Сравниваем маскированный IP-адрес с маскированным начальным адресом
            return maskedIP.SequenceEqual(maskedStart);
        }


        private bool IsTimeInRange(DateTime accessTime, DateTime startTime, DateTime endTime)
        {
            // Проверяем, находится ли время обращения в заданном интервале
            return accessTime >= startTime && accessTime <= endTime;
        }
    }
}
