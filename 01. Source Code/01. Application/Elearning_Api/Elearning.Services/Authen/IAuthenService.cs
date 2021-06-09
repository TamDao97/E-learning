using NTS.Common.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.Authen
{
    public interface IAuthenService
    {
        Task<NtsUserTokenModel> LoginAsync(NtsLogInModel loginModel);

        Task<NtsUserTokenModel> LoginUserAsync(NtsLogInModel loginModel);

        Task<bool> LogOutAsync(string userId);
        Task<NtsUserTokenModel> GetUserInfoAsync(string accessToken);
        bool IsTokenAlive(string userId);
    }
}
