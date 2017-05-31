using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using MessagingService.Contracts;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Net.Mail;

namespace MessagingService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class MessagingService : StatelessService, IMessagingService
    {
        private readonly IRepository<Message> _messageRepo;
        private readonly IRepository<Contact> _contactRepo;
        private const string twilioPhoneNumber = "+15873234407";
        const string accountSid = "yourtwilioaccont";
        //please don't steal my api keys
        const string authToken = "yourtwilioauth";

        static MessagingService()
        {
            TwilioClient.Init(accountSid, authToken);
        }


        public MessagingService(StatelessServiceContext context, IRepository<Contact> contactRepo, IRepository<Message> messageRepo)
            : base(context)
        {
            this._contactRepo = contactRepo;
            this._messageRepo = messageRepo;

        }

        public Task<bool> SendMessage(Contact recipient, Message message)
        {
            //MessageResource.Create(
            //   from: new PhoneNumber(twilioPhoneNumber), // From number, must be an SMS-enabled Twilio number
            //   to: new PhoneNumber(recipient.PhoneNumber), // To number, if using Sandbox see note above
            //   body: $"{message.Subject}: {message.MessageContent}"
            //);

            MailMessage mail = new MailMessage("breeves.dev@gmail.com", recipient.EmailAddress);
             System.Net.NetworkCredential credentials = 
                new System.Net.NetworkCredential("breeves.dev@gmail.com", "youraccoutpassword");
            SmtpClient client = new SmtpClient()
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = credentials,
                EnableSsl = true,
                Host = "smtp.gmail.com"
            };
            mail.Subject = message.Subject;
            mail.Body =message.MessageContent;
            client.Send(mail);

            //archive
            _messageRepo.Save(message);
            return Task.FromResult(true);
        }

        public Task<bool> SendMessage(string recipientName, Message message)
        {
            var contact = _contactRepo.FindOne(new Contact { Name = recipientName });
            if (contact != default(Contact))
            {
                return SendMessage(contact, message);
            }
            else
                return Task.FromResult(false);
        }

        /// <summary>
        /// Creates service instance listeners to use the transfer protocol of your choice. In this example, we are using
        /// the RPC protocol for simplicity. But you can easily add HTTP, WCF, Websocket
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new List<ServiceInstanceListener>() {
                new ServiceInstanceListener((context) =>
               this.CreateServiceRemotingListener(context)),
                //new ServiceInstanceListener(context => new HttpCommunicationListener(context))
            };
        }

    }
}
