using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LovelyMother.Uwp.Models;
using Newtonsoft.Json;

namespace LovelyMother.Uwp.Services
{
    public class UserService : IUserService
    {
        /// <summary>
        ///     身份服务。
        /// </summary>
        private readonly IIdentityService _identityService;



        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="identityService">身份服务。</param>
        public UserService(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// 修改用户名和头像。
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public async Task<ServiceResult> UpdateMeAsync(string userName, string image)
        {

            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler))
            {
                HttpResponseMessage response;
                var updateUser = new User {UserName = userName, Image = image};
                var json = JsonConvert.SerializeObject(updateUser);


                try
                {
                    response = await httpClient.PutAsync(
                        App.ServerEndpoint + "/api/Users", new StringContent(json, Encoding.UTF8,
                            "application/json"));
                    // "Student?studentId=" + HttpUtility.UrlEncode(updateUser),new StringContent(""));
                }
                catch (Exception e)
                {
                    return new ServiceResult
                    {
                        Status = ServiceResultStatus.Exception,
                        Message = e.Message
                    };
                }
                var serviceResult = new ServiceResult
                {
                    Status =
                        ServiceResultStatusHelper.FromHttpStatusCode(
                            response.StatusCode)
                };
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        break;
                    case HttpStatusCode.NoContent:
                        break;
                    case HttpStatusCode.BadRequest:
                        serviceResult.Message =
                            await response.Content.ReadAsStringAsync();
                        break;
                    default:
                        serviceResult.Message = response.ReasonPhrase;
                        break;
                }

                return serviceResult;

            }
        }

        public async Task<ServiceResult<User>> GetMeAsync()
        {

            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler))
            {

                HttpResponseMessage response;
                try
                {
                    response =
                        await httpClient.GetAsync(
                            App.ServerEndpoint + "/api/Users");
                }
                catch (Exception e)
                {
                    return new ServiceResult<User>
                    {
                        Status = ServiceResultStatus.Exception,
                        Message = e.Message
                    };
                }

                var serviceResult = new ServiceResult<User>
                {
                    Status =
                        ServiceResultStatusHelper.FromHttpStatusCode(
                            response.StatusCode)
                };

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                        break;
                    case HttpStatusCode.OK:
                        var json = await response.Content.ReadAsStringAsync();
                        serviceResult.Result =
                            JsonConvert.DeserializeObject<User>(json);
                        break;
                    default:
                        serviceResult.Message = response.ReasonPhrase;
                        break;
                }

                return serviceResult;
            }
            
        }

       



        /*
        public Task<ServiceResult<User>> GetUserByUserNameAsync(string userName)
        {
            throw new NotImplementedException();
        }
        */
    }
}
