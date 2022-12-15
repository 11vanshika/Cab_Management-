using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Models;

namespace Service.Inteface
{
    public interface ISendNotification
    {
        public bool SendWhatsAppMessage(SendingNotification sendingNotification);
        public bool SendSMSMessage(SendingNotification sendingNotification);
    }
}
