using System;
using System.Configuration;
using MailKit.Net.Smtp;
using MimeKit;

namespace crud.services
{
    internal class emailService
    {
        private string str_email;
        private string str_senha;

        public emailService()
        {
            str_email = ConfigurationManager.AppSettings["FD_SMTP_EMAIL"];
            str_senha = ConfigurationManager.AppSettings["FD_SMTP_SENHA"];
        }

        public void enviarEmail(string str_email_destino, MimeMessage str_message)
        {
            var client = new SmtpClient();
            using (client)
            {
                try
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate(this.str_email, this.str_senha);
                    client.Send(str_message);
                    client.Disconnect(true);
                    Console.WriteLine("E-mail enviado com sucesso!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
                }
            }
        }

        // Função para enviar e-mail de confirmação de cadastro
        public void enviarEmailConfirmacaoCadastro(string emailUsuario, string nomeUsuario, DateTime dataCadastro, int int_codigo_email)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Equipe de Cadastro", this.str_email)); // Remetente
            message.To.Add(new MailboxAddress(nomeUsuario, emailUsuario));  // Destinatário
            message.Subject = "Confirmação de Cadastro";

            // Corpo do e-mail com as informações
            string corpoEmail = $"Olá {nomeUsuario},\n\n";
            corpoEmail += "Obrigado por se cadastrar em nosso sistema! Estamos felizes em tê-lo conosco.\n\n";
            corpoEmail += $"Seu cadastro foi realizado com sucesso no dia {dataCadastro.ToString("dd/mm/yyyy")}. \n\n";
            corpoEmail += $"Resta agora apenas confirmar seu email, o código para confirmação é:  {int_codigo_email}. \n\n";
            corpoEmail += "Se você não realizou este cadastro, por favor, entre em contato conosco imediatamente.\n\n";
            corpoEmail += "Atenciosamente,\nEquipe da MinhasFinancas";

            var body = new TextPart("plain")
            {
                Text = corpoEmail
            };

            message.Body = body;

            // Enviar o e-mail de confirmação
            this.enviarEmail(emailUsuario, message);
        }
    }
}
