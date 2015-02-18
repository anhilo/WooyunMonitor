using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Class.Common
{
    /// <summary>
    /// 提醒功能相关
    /// </summary>
    public class E_Mail
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailto">收件</param>
        /// <param name="cc">抄送</param>
        /// <param name="title">主题</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public bool SendEmail(string[] mailto, string[] cc, string title, string content)
        {
            try
            {

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("nsftester@sohu.com");
                msg.Sender = new MailAddress("nsftester@sohu.com");
                foreach (string item in mailto)
                {
                    msg.To.Add(item);
                }
                if (cc != null)
                {
                    foreach (var item in cc)
                    {
                        msg.CC.Add(item);
                    }
                }

                msg.Subject = title;
                msg.Body = content;
                msg.BodyEncoding = System.Text.Encoding.UTF8;

                SmtpClient client = new SmtpClient("smtp.sohu.com");
                client.UseDefaultCredentials = false;
                System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential("nsftester@sohu.com", "!q@w#e$r%t^y");
                client.Credentials = basicAuthenticationInfo;
                client.EnableSsl = true;
                client.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}