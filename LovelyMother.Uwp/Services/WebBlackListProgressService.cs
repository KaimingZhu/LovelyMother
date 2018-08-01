using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LovelyMother.Uwp.Models;
using Newtonsoft.Json;

namespace LovelyMother.Uwp.Services
{
    public class WebBlackListProgressService : IWebBlackListProgressService
    {

        /// <summary>
        ///     身份服务。
        /// </summary>
        private readonly IIdentityService _identityService;
        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="identityService">身份服务。</param>
        public WebBlackListProgressService(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<List<BlackListProgress>> ListWebBlackListProgressesAsync()
        {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();

            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler))
            {

                HttpResponseMessage response;
                response =
                    await httpClient.GetAsync(
                        App.ServerEndpoint + "/api/BlackListProgresses");

                var json = await response.Content.ReadAsStringAsync();
                var webBlackListProgresses =
                    JsonConvert.DeserializeObject<BlackListProgress[]>(json).ToList();
                return webBlackListProgresses;
            }
        }



        public async Task<bool> NewWebBlackListProgress(string uwp_Id, string fileName, string resetName, int type)
        {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            var postWebBlackListProgress = new BlackListProgress{ Uwp_ID = uwp_Id,FileName = fileName,ResetName = resetName,Type = type};
            var json = JsonConvert.SerializeObject(postWebBlackListProgress);
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler))
            {
                HttpResponseMessage response;
                response = await httpClient.PostAsync(App.ServerEndpoint + "/api/BlackListProgresses", new StringContent(json, Encoding.UTF8, "application/json"));
                return true;
            }


        }


        public async Task<bool> DeleteWebBlackListProgressAsync(int id)
        {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();

            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler))
            {
                HttpResponseMessage response;
                response = await httpClient.DeleteAsync(App.ServerEndpoint + "/api/BlackListProgresses?id=" + id.ToString());
                return true;
            }
        }

        public async Task<BlackListProgress> GetWebBlackListProgressAsync(int id)
        {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler))
            {
                HttpResponseMessage response;
                response = await httpClient.GetAsync(App.ServerEndpoint + "/api/BlackListProgresses/" + id.ToString());

                var json = await response.Content.ReadAsStringAsync();
                var webBlackListProgress =
                    JsonConvert.DeserializeObject<BlackListProgress>(json);

                return webBlackListProgress;


            }
        }
    }
}
