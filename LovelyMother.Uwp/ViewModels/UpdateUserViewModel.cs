using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LovelyMother.Uwp.Models;
using LovelyMother.Uwp.Services;

namespace LovelyMother.Uwp.ViewModels
{
    public class UpdateUserViewModel : ViewModelBase
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


        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set => Set(nameof(CurrentUser), ref _currentUser, value);
        }

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="identityService">身份服务。</param>
        /// <param name="rootNavigationService">根导航服务。</param>
        /// <param name="dialogService">对话框服务。</param>
        public UpdateUserViewModel(IIdentityService identityService,
            IRootNavigationService rootNavigationService,
            IDialogService dialogService,
            IUserService userService)
        {
            _identityService = identityService;
            _rootNavigationService = rootNavigationService;
            _dialogService = dialogService;
            _userService = userService;
            CurrentUser = new User();

            CurrentUser.ID = _identityService.GetCurrentUserAsync().ID;
            CurrentUser.UserName = _identityService.GetCurrentUserAsync().UserName;
            CurrentUser.TotalTime = _identityService.GetCurrentUserAsync().TotalTime;
            CurrentUser.ApplicationUserID = _identityService.GetCurrentUserAsync().ApplicationUserID;
            CurrentUser.Image = _identityService.GetCurrentUserAsync().Image;


        }


        /// <summary>
        ///     刷新命令。
        /// </summary>
        private RelayCommand _refreshCommand;

        public RelayCommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new RelayCommand(async () => {


                CurrentUser.ID = _identityService.GetCurrentUserAsync().ID;
                CurrentUser.UserName = _identityService.GetCurrentUserAsync().UserName;
                CurrentUser.TotalTime = _identityService.GetCurrentUserAsync().TotalTime;
                CurrentUser.ApplicationUserID = _identityService.GetCurrentUserAsync().ApplicationUserID;
                CurrentUser.Image = _identityService.GetCurrentUserAsync().Image;
            }));

        /// <summary>
        ///     更新用户命令。
        /// </summary>
        private RelayCommand _updateUserCommand;

        public RelayCommand UpdateUserCommand =>
            _updateUserCommand ?? (_updateUserCommand = new RelayCommand(async () => {

                var updateUser = new User{UserName = CurrentUser.UserName,TotalTime = CurrentUser.TotalTime,Image = CurrentUser.Image};
                _identityService.SetCurrentUserAsync(updateUser);
                await _userService.UpdateMeAsync(CurrentUser.UserName, CurrentUser.TotalTime, CurrentUser.WeekTotalTime,
                    CurrentUser.Image);
                CurrentUser.ID = _identityService.GetCurrentUserAsync().ID;
                CurrentUser.UserName = _identityService.GetCurrentUserAsync().UserName;
                CurrentUser.TotalTime = _identityService.GetCurrentUserAsync().TotalTime;
                CurrentUser.ApplicationUserID = _identityService.GetCurrentUserAsync().ApplicationUserID;
                CurrentUser.Image = _identityService.GetCurrentUserAsync().Image;

            }));


        /// <summary>
        ///     跳转命令。
        /// </summary>
        private RelayCommand _navigateToTaskCommand;

        public RelayCommand NavigateToTaskCommand =>
            _navigateToTaskCommand ?? (_navigateToTaskCommand = new RelayCommand(async () => {

                _rootNavigationService.Navigate(typeof(YuhaoTest3));

            }));

        /// <summary>
        ///     跳转命令。
        /// </summary>
        private RelayCommand _navigateToFriendCommand;

        public RelayCommand NavigateToFriendCommand =>
            _navigateToFriendCommand ?? (_navigateToFriendCommand = new RelayCommand(async () => {

                _rootNavigationService.Navigate(typeof(YuhaoTest4));

            }));

    }
}
