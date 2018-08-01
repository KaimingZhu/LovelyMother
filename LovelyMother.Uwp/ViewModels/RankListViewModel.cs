using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LovelyMother.Uwp.Models;
using LovelyMother.Uwp.Services;

namespace LovelyMother.Uwp.ViewModels
{
    public class RankListViewModel:ViewModelBase
    {
        /// <summary>
        ///     对话框服务。
        /// </summary>
        private readonly IDialogService _dialogService;

        /// <summary>
        ///     身份服务。
        /// </summary>
        private readonly IIdentityService _identityService;

        private readonly IFriendService _friendService;

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
        public RankListViewModel(IIdentityService identityService,
            IRootNavigationService rootNavigationService,
            IDialogService dialogService,
            IUserService userService,
            IFriendService friendService)
        {
            _identityService = identityService;
            _rootNavigationService = rootNavigationService;
            _dialogService = dialogService;
            _userService = userService;
            _friendService = friendService;
            RankListCollection = new ObservableCollection<RankList>();
            refresh();

        }
        public async void refresh()
        {


            RankListCollection.Clear();
            var thisuser = _identityService.GetCurrentUserAsync();
            if (thisuser.ApplicationUserID != null)
            {
                var rankList = await _friendService.GetRankList(thisuser.ApplicationUserID);


                foreach (var thisRankList in rankList)
                {
                    RankListCollection.Add(thisRankList);
                }
            }

             

        }

        public ObservableCollection<RankList> RankListCollection
        {
            get;
            private set;
        }


        private RelayCommand _getRankListCommand;
        public RelayCommand GetRankListCommand =>
            _getRankListCommand ?? (_getRankListCommand = new RelayCommand(async () =>
            {

                RankListCollection.Clear();
                var thisuser = _identityService.GetCurrentUserAsync();
                if (thisuser.ApplicationUserID != null)
                {
                    var rankList = await _friendService.GetRankList(thisuser.ApplicationUserID);


                    foreach (var thisRankList in rankList)
                    {
                        RankListCollection.Add(thisRankList);
                    }
                }

            }));



    }
}
