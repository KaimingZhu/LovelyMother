
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LovelyMother.Uwp.Models;
using LovelyMother.Uwp.Models.Messages;
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

        private readonly IWebTaskService _webTaskService;

        /// <summary>
        /// 用户服务。
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        ///     根导航服务。
        /// </summary>
        private readonly IRootNavigationService _rootNavigationService;


       
        public ObservableCollection<WebTask> TaskCollection
        {
            get;
            private set;
        }

        private WebTask _selectTask;
        public  WebTask SelectTask
        {
            get => _selectTask;
            set => Set(nameof(SelectTask), ref _selectTask, value);
        }



        private string _inputDate;
        public string InputDate
        {
            get => _inputDate;
            set => Set(nameof(InputDate), ref _inputDate, value);
        }

        private string _inputBegin;
        public string InputBegin
        {
            get => _inputBegin;
            set => Set(nameof(InputBegin), ref _inputBegin, value);
        }





        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="identityService">身份服务。</param>
        /// <param name="rootNavigationService">根导航服务。</param>
        /// <param name="dialogService">对话框服务。</param>
        public WebTaskViewModel(IIdentityService identityService,
            IRootNavigationService rootNavigationService,
            IDialogService dialogService,
            IUserService userService,
            IWebTaskService webTaskService)
        {
            _identityService = identityService;
            _rootNavigationService = rootNavigationService;
            _dialogService = dialogService;
            _userService = userService;
            _webTaskService = webTaskService;
            TaskCollection = new ObservableCollection<WebTask>();
            SelectTask = new WebTask();

            refresh();

        }

        public async void refresh()
        {
            TaskCollection.Clear();
            var webTasks = await _webTaskService.ListWebTaskAsync();

            foreach (var thistask in webTasks)
            {
                TaskCollection.Add(thistask);
            }
        }


        /// <summary>
        ///     刷新命令。
        /// </summary>
        private RelayCommand _refreshTaskCommand;

        public RelayCommand RefreshTaskCommand =>
            _refreshTaskCommand ?? (_refreshTaskCommand = new RelayCommand(async () => {

                TaskCollection.Clear();
                var webTasks = await _webTaskService.ListWebTaskAsync();

                foreach (var thistask in webTasks)
                {
                    TaskCollection.Add(thistask);
                }

            }));


        private RelayCommand _addTaskCommand;

        public RelayCommand AddTaskCommand =>
            _addTaskCommand ?? (_addTaskCommand = new RelayCommand(
                async () =>
                {
                   await _webTaskService.NewWebTaskAsync(InputDate,InputBegin,60);
                    TaskCollection.Clear();
                    var webTasks = await _webTaskService.ListWebTaskAsync();

                    foreach (var thistask in webTasks)
                    {
                        TaskCollection.Add(thistask);
                    }
                }));


        private RelayCommand _deleteTaskCommand;

        public RelayCommand DeleteTaskCommand =>
            _deleteTaskCommand ?? (_deleteTaskCommand = new RelayCommand(
                async () =>
                {
                    await _webTaskService.DeleteWebTaskAsync(SelectTask.ID);
                    TaskCollection.Clear();
                    var webTasks = await _webTaskService.ListWebTaskAsync();

                    foreach (var thistask in webTasks)
                    {
                        TaskCollection.Add(thistask);
                    }
                }));





    }
}
