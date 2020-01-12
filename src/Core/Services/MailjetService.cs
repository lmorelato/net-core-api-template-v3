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
    public sealed class MailjetService : IMailjetService
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

        public async Task Send(EmailOptions options, bool throwIfError = false)
        {
            var message = this.CreateMessage(options);
            if (options.HasTemplate)
            {
                this.FormatTemplate(options, message);
            }
            else
            {
                this.FormatBody(options, message);
            }

            var response = await this.SendMessage(message);
            this.HandleResponse(options, throwIfError, response);
        }

        private async Task<MailjetResponse> SendMessage(JObject message)
        {
            var request = new MailjetRequest { Resource = Mailjet.Client.Resources.Send.Resource };
            request.Property(Mailjet.Client.Resources.Send.Messages, new JArray { message });

            var client = this.CreateMailjetClient();
            var response = await client.PostAsync(request);
            return response;
        }

        private void FormatBody(EmailOptions options, JObject message)
        {
            message.Add(Constants.Mailjet.HtmlPart, options.Body);
            message.Add(Constants.Mailjet.TextPart, Regex.Replace(options.Body, "<.*?>", string.Empty));
        }

        private void FormatTemplate(EmailOptions options, JObject message)
        {
            message.Add(Constants.Mailjet.TemplateId, Convert.ToInt64(options.TemplateId));
            message.Add(Constants.Mailjet.TemplateLanguage, true);

            var variables = new JObject();
            foreach (var (key, value) in options.Variables)
            {
                variables.Add(key, value);
            }

            message.Add(Constants.Mailjet.Variables, variables);
        }

        private void HandleResponse(EmailOptions options, bool throwIfError, MailjetResponse response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var errorText = this.localizer.GetAndApplyValues("MailjetError", options.Subject, options.ToEmail);
            var emailSenderException = new MailjetException(errorText);
            this.logger.LogError(emailSenderException, response.GetData().ToString());

            if (throwIfError)
            {
                throw emailSenderException;
            }
        }

        private JObject CreateMessage(EmailOptions options)
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
                    { Constants.Mailjet.Email, options.ToEmail },
                    { Constants.Mailjet.Name, options.ToName }
                }
            };

            var message = new JObject
            {
                { Constants.Mailjet.From, from },
                { Constants.Mailjet.To, to },
                { Constants.Mailjet.Subject, options.Subject }
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