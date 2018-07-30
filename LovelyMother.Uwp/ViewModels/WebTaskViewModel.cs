
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using LovelyMother.Uwp.Models;
using LovelyMother.Uwp.Services;

namespace LovelyMother.Uwp.ViewModels
{
    public class WebTaskViewModel:ViewModelBase
    {
        /// <summary>
        ///     对话框服务。
        /// </summary>
        private readonly IDialogService _dialogService;

        /// <summary>
        ///     身份服务。
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        /// 用户服务。
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        ///     根导航服务。
        /// </summary>
        private readonly IRootNavigationService _rootNavigationService;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="identityService">身份服务。</param>
        /// <param name="rootNavigationService">根导航服务。</param>
        /// <param name="dialogService">对话框服务。</param>
        public WebTaskViewModel(IIdentityService identityService,
            IRootNavigationService rootNavigationService,
            IDialogService dialogService,
            IUserService userService)
        {
            _identityService = identityService;
            _rootNavigationService = rootNavigationService;
            _dialogService = dialogService;
            _userService = userService;
        }

        //private List<WebTask> 







    }
}
