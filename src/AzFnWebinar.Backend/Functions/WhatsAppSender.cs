using AzFnWebinar.Backend.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace AzFnWebinar.Backend
{
    public class WhatsAppSender
    {
        private readonly IConfiguration _configuration;

        public WhatsAppSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [FunctionName("WhatsAppSender")]
        [return: TwilioSms(AccountSidSetting = "TwilioAccountSid", AuthTokenSetting = "TwilioAuthToken")]
        public CreateMessageOptions Run([QueueTrigger("sms-queue", Connection = "WebinarStorage")] Message message)
        {
            return new CreateMessageOptions(new PhoneNumber($"whatsapp:{message.PhoneNumber}"))
            {
                From = new PhoneNumber(_configuration["From"]),
                Body = message.Body
            };
        }
    }
}
