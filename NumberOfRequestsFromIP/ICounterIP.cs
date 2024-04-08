using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberOfRequestsFromIP
{
    //подсчёт кол-во ip-адресов
    internal interface ICounterIP
    {
        Dictionary<string, int> CountIP(string[] data, string AddressStart, string AddressMask, string TimeStart, string TimeEnd );
    }
}
