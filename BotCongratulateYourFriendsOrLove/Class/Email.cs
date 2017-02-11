using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Text;

namespace BotCongratulateYourFriendsOrLove.Class
{   [Serializable]
    public class Email
    {
        public static IForm<Email> BuildForm()
        {
            return new FormBuilder<Email>()
                .Message("Привет. Я тут чтобы тебе помочь написать поздравление по электронной почте!")
                .Field(nameof(To))
                .Field(nameof(Subject))
                .Field(nameof(Body))
                .AddRemainingFields()
                .Confirm($"Вы уверены, что хотите отправить сообщение?")
                .OnCompletion(sendEmail)
                .Build();
               
        }
        [Prompt("Введите кому отправить поздравление.")]
        public string To { get; set; }
        [Prompt("Введите тему поздравления.")]
        public string Subject { get; set; }
        [Prompt("Введите текст сообщения.")]
        public string Body { get; set; }

        private async static Task sendEmail(IDialogContext context, Email state)
        {
            await context.PostAsync(await state.BuildResult());
        }
        const string fromEmail = "email@email.com";
        const string password = "password";
        public async Task<string> BuildResult()
        {

            MailMessage mail = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential(fromEmail, password);
            mail.Subject = Subject.ToString();
            mail.BodyEncoding = UTF8Encoding.UTF8;
            mail.SubjectEncoding = UTF8Encoding.UTF8;
            mail.Body = Body.ToString();
            try
            {
                client.Send(mail);
                return "Ваше сообщение отправлено!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
      
    }
}