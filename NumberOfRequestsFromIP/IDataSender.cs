using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberOfRequestsFromIP
{
    internal interface IDataSender
    {
        void SendData(Dictionary<string, int> data, string outputFilePath);
    }
}
