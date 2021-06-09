using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Users
{
    public interface INtsUserService
    {
        /// <summary>
        /// Auth login Jwt
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        Task<NtsUserTokenModel> NtsJwtLogin(NtsUserLoginModel loginModel);
    }
}
