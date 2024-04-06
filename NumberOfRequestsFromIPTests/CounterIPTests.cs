using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberOfRequestsFromIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberOfRequestsFromIP.Tests
{
    [TestClass()]
    public class CounterIPTests
    {
        [TestMethod]
        public void CountIPTest_AllAddressesInRange()
        {
            // Arrange
            CounterIP counterIP = new CounterIP();
            string[] data = { "192.168.1.1:2024-04-01 10:00:00", "192.168.1.2:2024-04-01 10:00:00" };
            string addressStart = "0.0.0.0";
            string addressMask = "0.0.0.0";
            string timeStart = "01.04.2024";
            string timeEnd = "01.04.2024";

            Dictionary<string, int> expected = new Dictionary<string, int>
            {
                { "192.168.1.1", 1 },
                { "192.168.1.2", 1 }
            };

            // Act
            Dictionary<string, int> result = counterIP.CountIP(data, addressStart, addressMask, timeStart, timeEnd);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CountIPTest_WithoutAdressStartAndMask()
        {
            // Arrange
            CounterIP counterIP = new CounterIP();
            string[] data = { "192.168.1.1:2024-04-01 10:00:00", "192.168.1.2:2024-04-01 10:00:00" };
            string? addressStart = null;
            string? addressMask = null;
            string timeStart = "01.04.2024";
            string timeEnd = "01.04.2024";

            Dictionary<string, int> expected = new Dictionary<string, int>
            {
                { "192.168.1.1", 1 },
                { "192.168.1.2", 1 }
            };

            // Act
            Dictionary<string, int> result = counterIP.CountIP(data, addressStart, addressMask, timeStart, timeEnd);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CountIPTest_AddressOutOfRange()
        {
            // Arrange
            CounterIP counterIP = new CounterIP();
            string[] data = { "192.168.1.1:2024-04-01 10:00:00" };
            string addressStart = "10.0.0.0";
            string addressMask = "255.255.255.0";
            string timeStart = "01.04.2024";
            string timeEnd = "01.04.2024";

            Dictionary<string, int> expected = new Dictionary<string, int>();

            // Act
            Dictionary<string, int> result = counterIP.CountIP(data, addressStart, addressMask, timeStart, timeEnd);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }
        [TestMethod]
        public void CountIPTest_NoData()
        {
            // Arrange
            CounterIP counterIP = new CounterIP();
            string[] data = { };
            string addressStart = "0.0.0.0";
            string addressMask = "255.255.255.255";
            string timeStart = "01.04.2024";
            string timeEnd = "01.04.2024";

            Dictionary<string, int> expected = new Dictionary<string, int>();

            // Act
            Dictionary<string, int> result = counterIP.CountIP(data, addressStart, addressMask, timeStart, timeEnd);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CountIPTest_TimeOutOfRange()
        {
            // Arrange
            CounterIP counterIP = new CounterIP();
            string[] data = { "192.168.1.1:2024-04-01 10:00:00" };
            string addressStart = "0.0.0.0";
            string addressMask = "255.255.255.255";
            string timeStart = "02.04.2024";
            string timeEnd = "02.04.2024";

            Dictionary<string, int> expected = new Dictionary<string, int>();

            // Act
            Dictionary<string, int> result = counterIP.CountIP(data, addressStart, addressMask, timeStart, timeEnd);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CountIPTest_MultipleRequestsFromSameAddressWithoutAddressMask()
        {
            // Arrange
            CounterIP counterIP = new CounterIP();
            string[] data = { "192.168.1.1:2024-04-01 10:00:00", "192.168.1.1:2024-04-01 11:00:00" };
            string addressStart = "0.0.0.0";
            string? addressMask = null;
            string timeStart = "01.04.2024";
            string timeEnd = "01.04.2024";

            Dictionary<string, int> expected = new Dictionary<string, int>
            {
                { "192.168.1.1", 2 }
            };

            // Act
            Dictionary<string, int> result = counterIP.CountIP(data, addressStart, addressMask, timeStart, timeEnd);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }
    }
}