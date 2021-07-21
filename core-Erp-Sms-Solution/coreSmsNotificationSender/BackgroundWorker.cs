using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using coreSmsNotificationData.Abstractions;
using coreSmsNotificationSender.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace coreSmsNotificationSender
{
    public class BackgroundWorker : BackgroundService
    {
        private readonly ILogger<BackgroundWorker> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISmsHttpHelper _smsHttp;
        private readonly ISmsDataRepository _dataHelper;
        private readonly AppOptions _appOptions;
        public BackgroundWorker(ILogger<BackgroundWorker> logger,
            IConfiguration configuration,
            ILoggerFactory loggerFactory,
            ISmsHttpHelper smsHttp,
            ISmsDataRepository dataHelper)
        {
            _logger = logger;
            _configuration = configuration;
            _smsHttp = smsHttp;
            _dataHelper = dataHelper;
            _appOptions = new AppOptions();
            _configuration.Bind("AppConfigs", _appOptions);

            var logLocation = _configuration.GetValue<string>("LOG_DIRECTORY");
            var errorDir = Path.Combine(logLocation, "Error");
            var infoDir = Path.Combine(logLocation, "Info");
            var today = DateTime.Now.Date.ToString("dd_MMM_yyyy_hh");
            loggerFactory.AddFile($"{errorDir}\\{today}_errors.log", LogLevel.Error);
            loggerFactory.AddFile($"{infoDir}\\{today}_info.log", LogLevel.Information);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now.ToString("dd-MMM-yyyy-hh-mm-ss-tt"));
                await Task.Delay(10000, stoppingToken);

                await SendQueuedMessage();

                await Task.Delay(10000, stoppingToken);
            }
        }


        private async Task<bool> SendQueuedMessage()
        {
            try
            {
                var unSentMsgs = await _dataHelper.GetAllQueuedUnsentSmsEvents();
                if (unSentMsgs != null && unSentMsgs?.Count > 0)
                {
                    _logger.LogInformation("Worker running at: {Time} with a total of {Data} message(s)",
                        DateTimeOffset.Now.ToString("dd-MMM-yyyy-hh-mm-ss-tt"), unSentMsgs.Count);

                    foreach (var msg in unSentMsgs)
                    {
                        if (!string.IsNullOrWhiteSpace(msg.phoneNumber))
                        {
                            var msgSent = await _smsHttp.SendSmsByGetMethodAsync(msg.phoneNumber, msg.messageBody, msg.sender,
                                _appOptions.KiloMessageConfigId);

                            //Update sms
                            if (msgSent != null && msgSent?.IsSent == true)
                            {
                                var updated = _dataHelper.MarkSmsEventAsFinished(msg.messageEventID);
                            }
                        }


                    }
                }
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred executing {MethodName} on {ClassName}", nameof(SendQueuedMessage), nameof(BackgroundWorker));
                return false;
            }
        }

    }
}
