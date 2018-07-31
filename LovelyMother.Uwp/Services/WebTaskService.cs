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
    public class WebTaskService : IWebTaskService
    {

        /// <summary>
        ///     身份服务。
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="identityService">身份服务。</param>
        public WebTaskService(IIdentityService identityService)
        {
            _identityService = identityService;
        }




        /// <summary>
        /// 获取历史任务。
        /// </summary>
        /// <returns></returns>
        public async Task<List<WebTask>> ListWebTaskAsync()
        {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();

            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler))
            {
                HttpResponseMessage response;

                    response =
                        await httpClient.GetAsync(
                            App.ServerEndpoint + "/api/Tasks");
                
                

                var json = await response.Content.ReadAsStringAsync();
                var webTasks =
                    JsonConvert.DeserializeObject<List<WebTask>>(json);

                return webTasks;
            }

 
        }//ListWebTaskAsync

        

        public async Task<bool> NewWebTaskAsync(string date,string begin,int defaultTime)
        {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            var postWebTask = new WebTask{ Date = date,Begin = begin,DefaultTime = defaultTime,UserID = 1};
            var json = JsonConvert.SerializeObject(postWebTask);
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler))
            {
                HttpResponseMessage response;
                response = await httpClient.PostAsync(App.ServerEndpoint + "/api/Tasks",new StringContent(json,Encoding.UTF8,"application/json"));
                return true;

            }

        }


        public async Task<bool> DeleteWebTaskAsync(int id)
        {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();

            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler))
            {
                HttpResponseMessage response;
                response = await httpClient.DeleteAsync(App.ServerEndpoint + "/api/Tasks/" + id.ToString());
                return true;

            }

        }


        public async Task<bool> UpdateWebTaskAsync(int taskId,int finishFlag, int finishTime, string introduction)
        {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            var updateWebTask = new WebTask { FinishFlag = finishFlag, FinishTime = finishTime,Introduction = introduction};
            var json = JsonConvert.SerializeObject(updateWebTask);
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler))
            {
                HttpResponseMessage response;
                response = await httpClient.PutAsync(App.ServerEndpoint + "/api/Tasks?id=" + taskId.ToString(),new StringContent(json,Encoding.UTF8,"application/json"));
                return true;

            }

        }

        public async Task<WebTask> GetWebTaskAsync(int id)
        {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler))
            {
                HttpResponseMessage response;
                response = await httpClient.GetAsync(App.ServerEndpoint + "/api/Tasks/" + id.ToString());

                var json = await response.Content.ReadAsStringAsync();
                var webTask =
                    JsonConvert.DeserializeObject<WebTask>(json);

                return webTask;


            }
        }

        /// <summary>
        /// LocalTask转换为WebTask : 返回一个UserID为-1的WebTask(若UserID为-1)
        /// </summary>
        /// <param name="localtask"></param>
        /// <returns></returns>
        public WebTask LocalTaskToWeb_NoneUser(Motherlibrary.MyDatabaseContext.Task localtask)
        {
            return new WebTask()
            {
                ID = localtask.ID,
                UserID = localtask.UserID,
                Introduction = localtask.Introduction,
                Date = localtask.Date,
                Begin = localtask.Begin,
                DefaultTime = localtask.DefaultTime,
                FinishTime = localtask.FinishTime,
                FinishFlag = localtask.FinishFlag
            };
        }

      
    }
}
