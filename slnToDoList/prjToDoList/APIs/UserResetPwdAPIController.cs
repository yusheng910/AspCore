using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using prjToDoList.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace prjToDoList.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserResetPwdAPIController : ControllerBase
    {
        private readonly demoDBContext _db;
        public UserResetPwdAPIController(demoDBContext db)
        {
            _db = db;
        }

        // POST api/<UserResetPwdAPIController>
        [HttpPost]
        public IActionResult Post([FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            if (resetPasswordRequest == null || string.IsNullOrEmpty(resetPasswordRequest.registeredMail))
            {
                return BadRequest(new { status = "null or empty" });
            }
            else
            {
                string recipient = resetPasswordRequest.registeredMail;
                // check if user is in the db
                tUser? user = _db.tUsers.FirstOrDefault(u => u.fEmail == recipient);
                if (user == null)
                {
                    return BadRequest(new { status = "User doesn't exist" });
                }
                var recipientUserName = user.fUserName;

                var smtpConfig = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build().GetSection("Smtp");

                var host = smtpConfig["Host"];
                var smtpUserAccount = smtpConfig["SmtpUserAccount"];
                var smtpUserName = smtpConfig["SmtpUserName"];
                var password = smtpConfig["Password"];

                int port;
                if (!string.IsNullOrEmpty(smtpConfig["Port"]) && int.TryParse(smtpConfig["Port"], out port))
                {
                    try
                    {
                        string newPwd = GenerateRandomPassword(8);

                        var email = new MimeMessage();
                        email.From.Add(new MailboxAddress(smtpUserName, smtpUserAccount));
                        email.To.Add(new MailboxAddress("", recipient));
                        email.Subject = "ToDo Service Reset Password Notification";
                        email.Body = new TextPart("plain") 
                        { Text = $"Dear {recipientUserName} " +
                        $"\r\n\r\n Please find your new password below: " +
                        $"\r\n {newPwd} " +
                        $"\r\n Remember to change the password after your login as soon as possible." +
                        $"\r\n\r\n Sincerely," +
                        $"\r\n {smtpUserName}" };

                        using (var client = new SmtpClient())
                        {
                            client.Connect(host, port, SecureSocketOptions.StartTls);
                            client.Authenticate(smtpUserAccount, password);
                            client.Send(email);
                            client.Disconnect(true);
                        }
                        
                        user.fPassword = CommonFn.ComputeSHA256Hash(newPwd);
                        _db.SaveChanges();
                        return Ok(new { status = "Mail sent to: " + resetPasswordRequest.registeredMail });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error sending email: " + ex.Message);
                        return StatusCode(500, "Error sending email");
                    }
                }
                else {
                    return BadRequest(new { status = "Check port setting in appsettings.json" });
                }
            }
        }

        private static readonly string charactersToUse = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#%*-+=~{}<>?";

        private static string GenerateRandomPassword(int pwLength)
        {
            Random random = new Random();
            char[] password = new char[pwLength];

            for (int i = 0; i < pwLength; i++)
            {
                int randomIndex = random.Next(charactersToUse.Length);
                password[i] = charactersToUse[randomIndex];
            }

            return new string(password);
        }
    }

    public class ResetPasswordRequest
    {
        public string? registeredMail { get; set; }
    }

}
