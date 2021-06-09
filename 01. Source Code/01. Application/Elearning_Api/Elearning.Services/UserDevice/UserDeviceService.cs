using Elearning.Model.Models.UserHistory;
using Elearning.Models.Users;
using NTS.Common.Users;
using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Elearning.Services.UserDevice
{
    public static class UserDeviceService
    {
        public static UserHistoryModel GetUserHistory(Microsoft.AspNetCore.Http.HttpRequest request, object userId)
        {

            UserHistoryModel model = new UserHistoryModel();

            NtsUserTokenModel objuser = (NtsUserTokenModel)(userId);
            //var a = request.Headers["User-Agent"].ToString();
          
            //HttpBrowserCapabilities bc = Request.Browser;

            model.ClientIP = request.HttpContext.Connection.RemoteIpAddress.ToString();
            model.OS = GetUserData(request);
            model.UserId = objuser.UserId;
            return model;
        }

        public static UserHistoryModel GetUserLogHistory(Microsoft.AspNetCore.Http.HttpRequest request, string userId)
        {

            UserHistoryModel model = new UserHistoryModel();

  

            model.ClientIP = request.HttpContext.Connection.RemoteIpAddress.ToString();
            model.OS = GetUserData(request);
            model.UserId = userId;
            return model;
        }

        public static string GetUserData(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            var platform =  GetUserPlatform(request);
            return platform;
            //return string.Format("{0} {1} / {2}", browser.Browser, browser.Version, platform);
        }

        public static string GetUserPlatform(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            var ua = request.Headers["User-Agent"];

            if (ua[0].Contains("Android"))
                return string.Format("Android {0}", GetMobileVersion(ua[0], "Android"));

            if (ua[0].Contains("iPad"))
                return string.Format("iPad OS {0}", GetMobileVersion(ua[0], "OS"));

            if (ua[0].Contains("iPhone"))
                return string.Format("iPhone OS {0}", GetMobileVersion(ua[0], "OS"));

            if (ua[0].Contains("Linux") && ua[0].Contains("KFAPWI"))
                return "Kindle Fire";

            if (ua[0].Contains("RIM Tablet") || (ua[0].Contains("BB") && ua[0].Contains("Mobile")))
                return "Black Berry";

            if (ua[0].Contains("Windows Phone"))
                return string.Format("Windows Phone {0}", GetMobileVersion(ua[0], "Windows Phone"));

            if (ua[0].Contains("Mac OS"))
                return "Mac OS";

            if (ua[0].Contains("Windows NT 5.1") || ua[0].Contains("Windows NT 5.2"))
                return "Windows XP";

            if (ua[0].Contains("Windows NT 6.0"))
                return "Windows Vista";

            if (ua[0].Contains("Windows NT 6.1"))
                return "Windows 7";

            if (ua[0].Contains("Windows NT 6.2"))
                return "Windows 8";

            if (ua[0].Contains("Windows NT 6.3"))
                return "Windows 8.1";

            if (ua[0].Contains("Windows NT 10"))
                return "Windows 10";

            //fallback to basic platform:
            return null;
        }

        public static string GetMobileVersion(string userAgent, string device)
        {
            var temp = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
            var version = string.Empty;

            foreach (var character in temp)
            {
                var validCharacter = false;
                int test = 0;

                if (Int32.TryParse(character.ToString(), out test))
                {
                    version += character;
                    validCharacter = true;
                }

                if (character == '.' || character == '_')
                {
                    version += '.';
                    validCharacter = true;
                }

                if (validCharacter == false)
                    break;
            }

            return version;
        }

    }
}
