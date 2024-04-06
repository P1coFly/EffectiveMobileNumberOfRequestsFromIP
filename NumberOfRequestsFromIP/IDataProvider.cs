using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberOfRequestsFromIP
{
    internal interface IDataProvider
    {
        string[] GetData(string logFilePath);
    }
}
