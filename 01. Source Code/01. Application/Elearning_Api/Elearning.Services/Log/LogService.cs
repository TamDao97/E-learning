using Elearning.Model.Entities;
using Elearning.Model.Models.UserHistory;
using Elearning.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Wangkanai.Detection.Services;


namespace Elearning.Services.Log
{
    public static class LogService
    {


        /// <summary>
        /// Log đang nhập
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userHistory"></param>
        public static void Login(UserHistoryModel userHistory, IDetectionService _detection)
        {
            ElearningContext db = new ElearningContext();

            UserHistories model = new UserHistories();
            model.UserId = userHistory.UserId;
            model.CreateDate = DateTime.Now;
            model.Content = "Đăng nhập hệ thống";
            model.Type = userHistory.Type;
            model.OS = userHistory.OS;
            model.Device = _detection.Device.Type.ToString();
            model.ClientIP = userHistory.ClientIP;
            model.BrowserName = _detection.Browser.Name.ToString();
            model.BrowserVersion = _detection.Browser.Version.ToString();
            db.UserHistories.Add(model);
            db.SaveChanges();
        }

        /// <summary>
        /// Log đăng xuất 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userHistory"></param>
        public static void Logout(UserHistoryModel userHistory, IDetectionService _detection)
        {
            ElearningContext db = new ElearningContext();

            UserHistories model = new UserHistories();
            model.UserId = userHistory.UserId;
            model.CreateDate = DateTime.Now;
            model.Content = "Đăng xuất hệ thống";
            model.Type = userHistory.Type;
            model.OS = userHistory.OS;
            model.Device = _detection.Device.Type.ToString();
            model.ClientIP = userHistory.ClientIP;
            model.BrowserName = _detection.Browser.Name.ToString();
            model.BrowserVersion = _detection.Browser.Version.ToString();
            db.UserHistories.Add(model);
            db.SaveChanges();
        }

        /// <summary>
        /// Log thao tác dữ liệu
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userHistory"></param>

        public static void Event(UserHistoryModel userHistory, IDetectionService _detection)
        {
            try
            {
                ElearningContext db = new ElearningContext();

                UserHistories model = new UserHistories();
                model.UserId = userHistory.UserId;
                model.CreateDate = DateTime.Now;
                model.Content = userHistory.Content;
                model.Type = userHistory.Type;
                model.OS = userHistory.OS;
                model.Device = _detection.Device.Type.ToString();
                model.ClientIP = userHistory.ClientIP;
                model.BrowserName = _detection.Browser.Name.ToString();
                model.BrowserVersion = _detection.Browser.Version.ToString();
                db.UserHistories.Add(model);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
