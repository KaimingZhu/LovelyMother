using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LovelyMother.Uwp.Models;

namespace LovelyMother.Uwp.Services
{
    public class UserService : IUserService
    {
        public Task<ServiceResult> BindAccountAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<User>> GetMeAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<User>> GetUserByUserNameAsync(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
