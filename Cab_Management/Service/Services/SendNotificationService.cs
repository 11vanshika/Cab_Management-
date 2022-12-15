using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Models;
using Persistence;
using Service.Inteface;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Service.Services
{
    public class SendNotificationService : ISendNotification
    {
        private readonly DbCabServicesContext _dbCabServicesContext;
        public SendNotificationService(DbCabServicesContext dbcontext)
        {
            _dbCabServicesContext = dbcontext;
        }
        public bool SendWhatsAppMessage(SendingNotification sendingNotification)
        {
            if(sendingNotification != null) { 
                var accountSid = "AC65cf2916278f8df92873f0da47b6f03e";
                var authToken = "803a292834f8d63b6476622de1e4ec57";
                TwilioClient.Init(accountSid, authToken);
                var messageOptions = new CreateMessageOptions(
                new PhoneNumber("Whatsapp:91"+sendingNotification.MobileNumber));
                messageOptions.From = new PhoneNumber("whatsapp:+1(415)523-8886");
                messageOptions.Body = sendingNotification.Message;
                 var message = MessageResource.Create(messageOptions);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SendSMSMessage(SendingNotification sendingNotification)
        {
           if (sendingNotification != null) {
                var accountSid = "AC65cf2916278f8df92873f0da47b6f03e";
                var authToken = "803a292834f8d63b6476622de1e4ec57";
                TwilioClient.Init(accountSid, authToken);
                var messageOptions = new CreateMessageOptions(
                new PhoneNumber("91" + sendingNotification.MobileNumber));
                messageOptions.MessagingServiceSid = "MG8221dbe8e727556d0189ac7baa924d86";
                messageOptions.Body = sendingNotification.Message;
                var message = MessageResource.Create(messageOptions);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
