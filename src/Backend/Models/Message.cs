using Microsoft.Azure.WebJobs;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Backend
{

    public class Message
    {
        public string PhoneNumber { get; set; }

        public string Body { get; set; }
    }
}
