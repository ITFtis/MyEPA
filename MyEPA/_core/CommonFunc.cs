using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Policy;
using System.Web;
using System.Web.Services.Description;

namespace MyEPA
{
    public class CommonFunc
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 測試目的端是否正常連線
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool TestUrl(string url)
        {
            bool result = false;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                HttpStatusCode statusCode = response.StatusCode;

                result = statusCode == HttpStatusCode.OK;
                if (!result)
                {
                    logger.Error("HttpStatusCode回應錯誤代碼：" + statusCode);
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("url 無法正常連線：{0}", url));
                logger.Error("錯誤：" + ex.Message);
                logger.Error(ex.StackTrace);

                return false;
            }

            return result;
        }

        /// <summary>
        /// 測試目的端是否正常連線
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool TestMailServerSmtpIp(string ip, int port)
        {
            bool result = false;

            System.Net.Sockets.TcpClient client = new TcpClient();
            try
            {
                client.Connect(ip, port);
                result = true;
            }
            catch (SocketException ex)
            {
                logger.Error(string.Format("SMTP 無法正常連線：{0}, {1}", ip, port.ToString()));
                logger.Error("錯誤：" + ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }
    }
}