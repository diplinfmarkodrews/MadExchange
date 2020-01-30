using MadXchange.Exchange.Services.HttpRequests;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace MadXchange.Connector.Services
{
    public interface IPollService : IHostedService
    {
        
    }
    public sealed class TimedPollService : BackgroundService, IPollService
    {
        private readonly System.Timers.Timer _timer;       
        private readonly ILogger _logger;
        private readonly IInstrumentRequestService _instrumentRequestService;
        private readonly Dictionary<int, CancellationToken> _cancellationDictionary;
        public TimedPollService(IInstrumentRequestService instrumentRequestService, ILogger<TimedPollService> logger) : base()
        {
            _cancellationDictionary = new Dictionary<int, CancellationToken>();
            _instrumentRequestService = instrumentRequestService;
            _logger = logger;
            _timer = new System.Timers.Timer();
            _timer.Interval = 1000.0;
            _timer.Elapsed += _timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Start();
             
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            ExecuteAsync(0);
        }

        public Task ExecuteAsync(int i)
        {
            var stoppingToken = new CancellationToken();
            _cancellationDictionary[i] = stoppingToken;
            return Task.FromResult(ExecuteAsync(stoppingToken).ConfigureAwait(false));
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var ins = await _instrumentRequestService.GetInstrumentAsync(Exchange.Domain.Models.Exchanges.ByBit, "BTCUSD");
            _logger.LogInformation("instrument Request returned", ins);
        }
    }
}
