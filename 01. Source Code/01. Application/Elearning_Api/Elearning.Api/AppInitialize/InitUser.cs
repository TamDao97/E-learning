using NTS.Common;
using NTS.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Models.Entities;

namespace Elearning.Api.AppInitialize
{
    public static class InitUser
    {
        public static void Init(ElearningContext _sqlContext, IPasswordUtilsService passwordUtilsService)
        {
            if (!_sqlContext.User.Any())
            {
                using (var trans = _sqlContext.Database.BeginTransaction())
                {
                    try
                    {
                        User user = new User()
                        {
                            Id = "1",
                            UserName = "admin",
                            IsDisable = false,
                            IsLogin = false,
                            SecurityStamp = passwordUtilsService.CreatePasswordHash(),                            
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            Type = Constants.User_UserType_Admin
                        };
                        user.PasswordHash = passwordUtilsService.ComputeHash("123456" + user.SecurityStamp);

                        _sqlContext.User.Add(user);
                        _sqlContext.SaveChanges();

                        //_sqlContext.UserInfos.Add(userInfo);
                        //_sqlContext.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}
