using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mailjet.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

using Template.Core.Exceptions;
using Template.Core.Models;
using Template.Core.Services.Interfaces;
using Template.Core.Settings;
using Template.Localization;
using Template.Shared;

namespace Template.Core.Services
{
    public class MailjetService : IMailjetService
    {
        private readonly ILogger<MailjetService> logger;
        private readonly ISharedResources localizer;
        private readonly MailjetApiSettings mailjetApiSettings;

        public MailjetService(
            IOptions<MailjetApiSettings> mailjetApiSettings,
            ILogger<MailjetService> logger,
            ISharedResources localizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.mailjetApiSettings = mailjetApiSettings.Value;
        }

        public async Task Send(EmailSettings settings, bool throwIfError = false)
        {
            var message = this.CreateMessage(settings);
            if (settings.HasTemplate)
            {
                this.FormatTemplate(settings, message);
            }
            else
            {
                this.FormatBody(settings, message);
            }

            var response = await this.SendMessage(message);
            this.HandleResponse(settings, throwIfError, response);
        }

        private async Task<MailjetResponse> SendMessage(JObject message)
        {
            var request = new MailjetRequest { Resource = Mailjet.Client.Resources.Send.Resource };
            request.Property(Mailjet.Client.Resources.Send.Messages, new JArray { message });

            var client = this.CreateMailjetClient();
            var response = await client.PostAsync(request);
            return response;
        }

        private void FormatBody(EmailSettings settings, JObject message)
        {
            message.Add(Constants.Mailjet.HTMLPart, settings.Body);
            message.Add(Constants.Mailjet.TextPart, Regex.Replace(settings.Body, "<.*?>", string.Empty));
        }

        private void FormatTemplate(EmailSettings settings, JObject message)
        {
            message.Add(Constants.Mailjet.TemplateID, Convert.ToInt64(settings.TemplateId));
            message.Add(Constants.Mailjet.TemplateLanguage, true);

            var variables = new JObject();
            foreach (var (key, value) in settings.Variables)
            {
                variables.Add(key, value);
            }

            message.Add(Constants.Mailjet.Variables, variables);
        }

        private void HandleResponse(EmailSettings settings, bool throwIfError, MailjetResponse response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var errorText = this.localizer.GetAndApplyValues("MailjetError", settings.Subject, settings.ToEmail);
            var emailSenderException = new MailjetException(errorText);
            this.logger.LogError(emailSenderException, response.GetData().ToString());

            if (throwIfError)
            {
                throw emailSenderException;
            }
        }

        private JObject CreateMessage(EmailSettings settings)
        {
            var from = new JObject
            {
                { Constants.Mailjet.Email, this.mailjetApiSettings.Sender },
                { Constants.Mailjet.Name, this.mailjetApiSettings.SenderName }
            };

            var to = new JArray
            {
                new JObject
                {
                    { Constants.Mailjet.Email, settings.ToEmail },
                    { Constants.Mailjet.Name, settings.ToName }
                }
            };

            var message = new JObject
            {
                { Constants.Mailjet.From, from },
                { Constants.Mailjet.To, to },
                { Constants.Mailjet.Subject, settings.Subject }
            };

            return message;
        }

        private MailjetClient CreateMailjetClient()
        {
            var client = new MailjetClient(
                             this.mailjetApiSettings.ApiKey,
                             this.mailjetApiSettings.ApiSecret)
            { Version = ApiVersion.V3_1 };

            return client;
        }
    }
}