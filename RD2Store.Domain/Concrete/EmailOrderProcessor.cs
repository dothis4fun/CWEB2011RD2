using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using RD2Store.Domain.Abstract;
using RD2Store.Domain.Entities;

namespace RD2Store.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "orders@example.com";
        public string MailFromAddress = "games4allorders@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = @"c:\games_4_all_orders";
    }

    public class EmailOrderProcessor: IOrderProcessor
    {
        private EmailSettings emailSettings;
        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);
                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }
                StringBuilder body = new StringBuilder().AppendLine("A new order has been submitted").AppendLine("---").AppendLine("Items: ");
                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2:C})", line.Quantity, line.Product.Name, subtotal);
                    body.AppendLine();
                }
                body.AppendLine().AppendFormat("Total order value: {0:C}", cart.ComputeTotalCost())
                    .AppendLine()
                    .AppendLine("---")
                    .Append("Ship to: ").AppendLine(shippingDetails.Name)
                    .AppendLine(shippingDetails.Line1);
                if (shippingDetails.Line2 != null) // checks for non required fields being filled
                {
                    body.AppendLine(shippingDetails.Line2);
                }
                if (shippingDetails.Line3 != null) // checks for non required fields being filled
                {
                    body.AppendLine(shippingDetails.Line3);
                }
                body.AppendLine(shippingDetails.City)
                    .AppendLine(shippingDetails.State)
                    .AppendLine(shippingDetails.Country)
                    .AppendLine(shippingDetails.Zip)
                    .AppendLine("---")
                    .AppendFormat("Gift wrap: {0}", shippingDetails.Giftwrap ? "Yes" : "No");

                MailMessage mailMessage = new MailMessage(
                                                        emailSettings.MailFromAddress,
                                                        emailSettings.MailToAddress,
                                                        "New order submited!",
                                                        body.ToString());
                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                smtpClient.Send(mailMessage);
            }
        }
    }
}
