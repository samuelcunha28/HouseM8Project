using HouseM8API.Models;
using System;
using System.Net.Mail;
using System.Security.Claims;
using System.Web.Helpers;

namespace HouseM8API
{
    public static class PasswordOperations
    {
        /// <summary>
        /// Método para encriptar a password
        /// </summary>
        /// <param name="pass">Password</param>
        /// <returns>Salt e Hash</returns>
        public static Tuple<string, string> Encrypt(string pass)
        {
            string salt = Crypto.GenerateSalt();
            string password = pass + salt;
            string hash = Crypto.HashPassword(password);

            return new Tuple<string, string>(salt, hash);
        }

        /// <summary>
        /// Método para comparar as passwords
        /// </summary>
        /// <param name="pass">Password</param>
        /// <param name="hash">Hash da Password</param>
        /// <param name="salt">Salt da Password</param>
        /// <returns>Bool com o resultado se as passwords são iguais</returns>
        public static bool VerifyHash(string pass, string hash, string salt)
        {
            pass = pass + salt;
            var hashVerify = Crypto.VerifyHashedPassword(hash, pass);

            return hashVerify;
        }

        /// <summary>
        /// Método para realizar o pedido de uma nova passowrd
        /// </summary>
        /// <param name="emailToSend">Endereço Email</param>
        /// <param name="tokenString">Token</param>
        public static void NewPasswordRequest(string emailToSend, string tokenString)
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

            mail.To.Add(emailToSend);
            mail.From = new MailAddress("no.reply.housem8@gmail.com");
            mail.Subject = "Recuperação de Password - HouseM8";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = "Aqui está o token para recuperação de password. <br />\r\n"
                        + "Continua com os passos na sua aplicação para a recuperação da password. <br />" + "\r\n" + "<br>"
                        + "Token: <b>" + tokenString + "</b>\r\n <br />" + "<br>"
                        + "Se não pediu alguma alteração de palavra passe, por favor ignore este e-mail!";
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient();

            client.Credentials = new System.Net.NetworkCredential("no.reply.housem8@gmail.com", "ldshousem8");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;

            client.Send(mail);
        }
    }
}
