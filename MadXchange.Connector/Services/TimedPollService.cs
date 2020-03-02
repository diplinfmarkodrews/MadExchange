using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Services.HttpRequests;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Text;

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
        private readonly IPositionRequestService _positionRequestService;
        private readonly Dictionary<int, CancellationToken> _cancellationDictionary;

        public TimedPollService(IInstrumentRequestService instrumentRequestService, IPositionRequestService positionRequestService, ILogger<TimedPollService> logger) : base()
        {
            _cancellationDictionary = new Dictionary<int, CancellationToken>();
            _positionRequestService = positionRequestService;
            _instrumentRequestService = instrumentRequestService;
            _logger = logger;
            _timer = new System.Timers.Timer();
            _timer.Interval = 5000000.0;
            _timer.Elapsed += _timer_Elapsed;
            _timer.AutoReset = true;
           
            //_timer.Start();
        }
        public void Start() => _timer.Start();
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
            var guid = Guid.Parse("3ecf42a8-e7a6-4870-8a8b-164cd2d6d508");
            var pos = await _positionRequestService.GetPositionsAsync(guid, Xchange.ByBit, stoppingToken);
            //var ins = await _instrumentRequestService.GetInstrumentAsync(Xchange.ByBit, "BTCUSD");
            //_logger.LogInformation($"instrument request returned" );
        }
    }
}