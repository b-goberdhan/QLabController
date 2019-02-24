using QLabOSCInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalPropClient.Services
{
    
    public class QLabService
    {
        private QLabOSCClient _client;
        private string _ipAddress;
        public QLabService()
        {
            _ipAddress = "192.168.0.108";
            _client = new QLabOSCClient(_ipAddress);
        }
        public void Reset(string ipAddress = "")
        {
            if (ipAddress != "")
            {
                _ipAddress = ipAddress;
            }
            _client.Dispose();
            _client = new QLabOSCClient(_ipAddress);
        }
        public QLabOSCClient Client
        {
            get { return _client; }
        }
    }
}
