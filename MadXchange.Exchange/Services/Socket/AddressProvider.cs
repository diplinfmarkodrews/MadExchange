using MadXchange.Exchange.Domain.Types;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Timers;

namespace MadXchange.Exchange.Services.Socket
{
    //public interface IAddressProvider 
    //{
    //    public IPAddress GetIPAddress(Xchange exchange);
    //}

    public class AddressProvider //:IAddressProvider
    {
        private Timer _updateTimer = new Timer();
        private Dictionary<Xchange, string> _xchangeUrls = new Dictionary<Xchange, string>();
        private ConcurrentDictionary<Xchange, IPAddress[]> _addresses = new ConcurrentDictionary<Xchange, IPAddress[]>();
        private readonly ILogger _logger;
        public AddressProvider() { }
        public AddressProvider(Dictionary<Xchange, string> xchangeUrls, ILogger<AddressProvider> logger)
        {
            _logger = logger;
            _xchangeUrls = xchangeUrls;
            _updateTimer = new Timer(1000 * 360);
            _updateTimer.Elapsed += UpdateTimer_Elapsed;
            Task.Run(() => ScanForAddresses()).ConfigureAwait(false);
            
        }

        private void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Task.Run(() => ScanForAddresses()).ConfigureAwait(false);
        }

        
        public IPAddress GetIPAddress(Xchange exchange) => _addresses[exchange]?.FirstOrDefault();

        private async Task ScanForAddresses() 
        {
            foreach (var url in _xchangeUrls)
                _addresses[url.Key] = await Dns.GetHostAddressesAsync(url.Value).ConfigureAwait(false);

            _logger.LogInformation("IP-Address Dns lookup result", _addresses);
        }
    }
}
