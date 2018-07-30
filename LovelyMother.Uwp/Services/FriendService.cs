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
    public class FriendService : IFriendService
    {

        /// <summary>
        ///     身份服务。
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="identityService">身份服务。</param>
        public FriendService(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<bool> AddMyFriend(string friendUserName)
        {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            
            var myFriend = new User{ UserName = friendUserName};
            var json = JsonConvert.SerializeObject(myFriend);
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler))
            {
                HttpResponseMessage response;

                try
                {
                    response = await httpClient.PostAsync(
                        App.ServerEndpoint + "/api/FriendShips" ,new StringContent(json, Encoding.UTF8,
                            "application/json"));
                    // "Student?studentId=" + HttpUtility.UrlEncode(updateUser),new StringContent(""));
                }
                catch (Exception e)
                {
                    return false;
                }

                return true;

            }


        }

        public async Task<bool> DeleteMyFriend(string friendUserName)
        {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler))
            {
                HttpResponseMessage response;

                try
                {
                    response = await httpClient.DeleteAsync(
                        App.ServerEndpoint + "/api/FriendLists/" + friendUserName);
                    // "Student?studentId=" + HttpUtility.UrlEncode(updateUser),new StringContent(""));
                }
                catch (Exception e)
                {
                    return false;
                }

            }

            return true;

        }

        public async Task<List<FriendList>> GetMyFriend(string applicationUserID)
        {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler))
            {
                HttpResponseMessage response;
                try
                {
                    response = await httpClient.GetAsync(
                        App.ServerEndpoint + "/api/FriendLists/" + applicationUserID);
                }
                catch (Exception e)
                {
                    return null;
                }
                var json = await response.Content.ReadAsStringAsync();
                var friends =  JsonConvert.DeserializeObject<FriendList[]>(json).ToList();
                return friends;
            }



        }

        public async Task<List<RankList>> GetRankList(string applicationUserID)
        {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler))
            {
                HttpResponseMessage response;
                try
                {
                    response = await httpClient.GetAsync(
                        App.ServerEndpoint + "/api/FriendShips?applicationUserID=" + applicationUserID);
                }
                catch (Exception e)
                {
                    return null;
                }
                var json = await response.Content.ReadAsStringAsync();
                var rankLists = JsonConvert.DeserializeObject<RankList[]>(json).ToList();

                return rankLists;
            }



        }
    }
}
