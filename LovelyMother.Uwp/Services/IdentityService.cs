using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using LovelyMother.Uwp.Helpers;
using LovelyMother.Uwp.Models;
using Newtonsoft.Json;

namespace LovelyMother.Uwp.Services
{
    /// <summary>
    ///     身份服务。
    /// </summary>
    public class IdentityService : IIdentityService
    {
        /// <summary>
        ///     默认用户名。
        /// </summary>
        private const string DefaultUsername =
            App.QualifiedAppName + ".defaultusername";

        /// <summary>
        ///     RefreshToken资源名。
        /// </summary>
        private const string RefreshTokenResource =
            App.QualifiedAppName + ".RefreshToken";

        /// <summary>
        ///     AccessToken资源名。
        /// </summary>
        private const string AccessTokenResource =
            App.QualifiedAppName + ".AccessToken";


        /// <summary>
        /// 当前用户。
        /// </summary>
        private AppUser CurrentUser;


        /// <summary>
        ///     根导航服务。
        /// </summary>
        private readonly IRootNavigationService _rootNavigationService;

        /// <summary>
        ///     Token锁。
        /// </summary>
        private readonly object _tokenLock = new object();

        /// <summary>
        ///     AccessToken。
        /// </summary>
        private string _accessToken = "";

        /// <summary>
        ///     RefreshToken。
        /// </summary>
        private string _refreshToken = "";



        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="rootNavigationService">根导航服务。</param>

        public IdentityService(IRootNavigationService rootNavigationService)
        {
            _rootNavigationService = rootNavigationService;
            CurrentUser = new AppUser(){
                ID = 0,
                UserName = ""
            };

            var passwordVault = new PasswordVault();

            PasswordCredential refreshTokenCredential = null;
            try
            {
                refreshTokenCredential =
                    passwordVault.Retrieve(RefreshTokenResource,
                        DefaultUsername);
            }
            catch
            {
                // ignored
            }

            if (refreshTokenCredential != null)
            {
                refreshTokenCredential.RetrievePassword();
                _refreshToken = refreshTokenCredential.Password;
            }

            PasswordCredential accessTokenCredential = null;
            try
            {
                accessTokenCredential =
                    passwordVault.Retrieve(AccessTokenResource,
                        DefaultUsername);
            }
            catch
            {
                // ignored
            }

            if (accessTokenCredential != null)
            {
                accessTokenCredential.RetrievePassword();
                _accessToken = accessTokenCredential.Password;
            }
        }


        /// <summary>
        ///     获得带有身份的HttpMessageHandler。
        /// </summary>
        /// <returns>带有身份的HttpMessageHandler</returns>
        public IdentifiedHttpMessageHandler GetIdentifiedHttpMessageHandler()
        {
    
            var oidcClientOptions = CreateOidcClientOptions();
            var tokenClient =
                new TokenClient(App.ServerEndpoint + "/connect/token",
                        oidcClientOptions.ClientId,
                        oidcClientOptions.BackchannelHandler)
                { Timeout = oidcClientOptions.BackchannelTimeout };

            var identifiedHttpMessageHandler = new IdentifiedHttpMessageHandler(
                tokenClient, _refreshToken, _accessToken,
                new HttpClientHandler(), _rootNavigationService,
                RefreshTokenHandler_TokenRefreshed);

            return identifiedHttpMessageHandler;
        }

        /// <summary>
        ///     创建OidcClientOptions。
        /// </summary>
        private OidcClientOptions CreateOidcClientOptions()
        {
            return new OidcClientOptions
            {
                Authority = App.ServerEndpoint,
                ClientId = "native.hybrid",
                Scope = "openid profile api offline_access",
                RedirectUri = App.QualifiedAppName + "://callback",
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect,
                //todo
                Browser = new SystemBrowser()
            };
        }

        /// <summary>
        ///     RefreshTokenHandler TokenRefreshed事件处理函数。
        /// </summary>
        private void RefreshTokenHandler_TokenRefreshed(object sender,
            TokenRefreshedEventArgs e)
        {
            lock (_tokenLock)
            {
                _refreshToken = e.RefreshToken;
                _accessToken = e.AccessToken;
            }
        }


        /// <summary>
        ///     登录。
        /// </summary>
        /// <returns>服务结果。</returns>
        public async Task<ServiceResult> LoginAsync()
        {
            var oidcClient = new OidcClient(CreateOidcClientOptions());

            LoginResult loginResult = null;
            try
            {
                loginResult = await oidcClient.LoginAsync(new LoginRequest());
            }
            catch (Exception e)
            {
                return new ServiceResult
                {
                    Status = ServiceResultStatus.Exception,
                    Message = e.Message
                };
            }

            if (!string.IsNullOrEmpty(loginResult.Error))
                return new ServiceResult
                {
                    Status =
                        ServiceResultStatusHelper.FromHttpStatusCode(
                            HttpStatusCode.BadRequest),
                    Message = loginResult.Error
                };

            var refreshTokenHandler =
                (RefreshTokenHandler)loginResult.RefreshTokenHandler;
            _refreshToken = refreshTokenHandler.RefreshToken;
            _accessToken = refreshTokenHandler.AccessToken;



            var identifiedHttpMessageHandler = GetIdentifiedHttpMessageHandler();
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler))
            {

                HttpResponseMessage response;
               
                    response =
                        await httpClient.GetAsync(
                            App.ServerEndpoint + "/api/Users");
                
                    var json = await response.Content.ReadAsStringAsync();
                    var thisUser =
                        JsonConvert.DeserializeObject<AppUser>(json);


                CurrentUser.ID = thisUser.ID;
                CurrentUser.UserName = thisUser.UserName;
                CurrentUser.TotalTime = thisUser.TotalTime;
                CurrentUser.ApplicationUserID = thisUser.ApplicationUserID;
                CurrentUser.Image = thisUser.Image;
   
            }
            return new ServiceResult { Status = ServiceResultStatus.OK};

        }


        /// <summary>
        ///     保存。
        /// </summary>
        public void Save()
        {
            var passwordVault = new PasswordVault();
            passwordVault.Add(new PasswordCredential(RefreshTokenResource,
                DefaultUsername, _refreshToken));
            passwordVault.Add(new PasswordCredential(AccessTokenResource,
                DefaultUsername, _accessToken));
        }




        /// <summary>
        ///     注销。
        /// </summary>
        public void SignOut()
        {
            _refreshToken = "empty";
            _accessToken = "empty";
            Save();
            CurrentUser.ID = 0;
            CurrentUser.UserName = null;
            CurrentUser.ApplicationUserID = null;
            CurrentUser.Image = null;
            CurrentUser.TotalTime = 0;
            CurrentUser.WeekTotalTime = 0;
            _rootNavigationService.Navigate(typeof(MainPage), null,
                NavigationTransition.EntranceNavigationTransition);
        }


        public AppUser GetCurrentUserAsync()
        {
            return CurrentUser;
        }

        public AppUser SetCurrentUserAsync(string updateUserName, int updateUserTotalTime,string updateUserImage)
        {
           
            CurrentUser.UserName = updateUserName;
            CurrentUser.TotalTime = updateUserTotalTime;       
            CurrentUser.Image =  updateUserImage;

            return CurrentUser;
        }


    }
}
