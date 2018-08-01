
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using LovelyMother.Uwp.Models;
using LovelyMother.Uwp.Models.Messages;
using LovelyMother.Uwp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Security.Cryptography;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LovelyMother.Uwp.ViewModels
{
    public class CountDownViewModel : ViewModelBase
    {
        //任务创建变量：是否登陆
        private bool ifLogin;

        //监听算法变量 : 黑名单列表
        List<Motherlibrary.MyDatabaseContext.BlackListProgress> blackListProgresses;

        //监听算法变量 : 音乐uri
        private static string[] musicLocation = { "ms-appx:///Assets/Music/1.mp3", "ms-appx:///Assets/Music/2.mp3",
                                           "ms-appx:///Assets/Music/3.mp3", "ms-appx:///Assets/Music/4.mp3",
                                           "ms-appx:///Assets/Music/5.mp3", "ms-appx:///Assets/Music/6.mp3",
                                           "ms-appx:///Assets/Music/7.mp3" };

        //监听算法变量 : 音乐播放器
        private MediaPlayer mediaPlayer;

        //监听算法辅助标识符 : 'true' - 音乐处于播放状态，'false' - 音乐处于关闭状态
        private bool _ifMusicPlaying { get; set; }

        //监听算法辅助标识符 : 为1时取消监听并重置为0
        private bool _listenFlag { get; set; }

        //服务器与数据库读写所需的临时变量writingTask
        private Motherlibrary.MyDatabaseContext.Task writingTask;

        /// <summary>
        /// 服务调用
        /// </summary>

        private readonly IProcessService _processService;

        private readonly IIdentityService _identityService;

        private readonly IRootNavigationService _rootNavigationService;

        private readonly IDialogService _dialogService;

        private readonly ILocalTaskService _localTaskService;

        private readonly ILocalBlackListProgressService _localBlackListProgressService;

        private readonly IWebBlackListProgressService _webBlackListService;

        private readonly IWebTaskService _webTaskService;

        /// <summary>
        /// 绑定属性。
        /// </summary>

        private TimeSpan _defaultBegin;
        public TimeSpan DefaultBegin
        {
            get => _defaultBegin;
            set => Set(nameof(DefaultBegin), ref _defaultBegin, value);
        }

        private TimeSpan _defaultend;
        public TimeSpan DefaultEnd
        {
            get => _defaultend;
            set => Set(nameof(DefaultEnd), ref _defaultend, value);
        }

        public int Date;

        public int DefaultTime;

        public string Begin;

        //public User CurrentUser;

        //public Task CurrentTask


        /// <summary>
        /// Command类
        /// </summary>

        //_updateLocalTaskCommand

        //_updateWebTaskCommand

        //_InitLocalTaskCommand

        //_InitWebTaskCommand


        /// <summary>
        /// Message类
        /// </summary>
        
        //开始监听进程
        private async Task BeginListenAsync()
        {

            blackListProgresses.Clear();

            //添加本地黑名单
            var localblackList = await _localBlackListProgressService.ListBlackListProgressAsync();
            foreach( var progress in localblackList )
            {
                blackListProgresses.Add(progress);
            }

            //若已登陆：添加服务器黑名单
            if (ifLogin == true)
            {
                var webblacklist = await _webBlackListService.ListWebBlackListProgressesAsync();
                foreach (var progress in webblacklist)
                {
                    blackListProgresses.Add(_localBlackListProgressService.WebProcessToLocal(progress));
                }
            }
            
            //开始监听
            if (_listenFlag == false)
            {
                //避免多进程运行造成的不必要CPU与内存占用
                _listenFlag = true;

                //打开黑名单: i = 1 => Delay(10000) / 不打开 : i = 0 => delay(2000)
                do
                {
                    var temp = _processService.GetProcessNow();
                    if(temp == null)
                    {
                        Task.Delay(5000).Wait();
                        continue;
                    }
                    var NewProcess = _processService.IfBlackListProcessExist(blackListProgresses, temp);

                    if (NewProcess == false)
                    {
                        if (_ifMusicPlaying == true)
                        {
                            Messenger.Default.Send<StopPlayingMusic>(new StopPlayingMusic());
                        }
                        Task.Delay(10000).Wait();
                    }
                    else
                    {

                        //弹出新窗口
                        //TODO : How to Solve
                        DispatcherHelper.CheckBeginInvokeOnUI( async () =>
                        {
                            CoreApplicationView newView = CoreApplication.CreateNewView();
                            int newViewId = 0;
                            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                Frame frame = new Frame();
                                frame.Navigate(typeof(PunishPage), null);
                                Window.Current.Content = frame;
                                // You have to activate the window in order to show it later.
                                Window.Current.Activate();

                                newViewId = ApplicationView.GetForCurrentView().Id;
                            });
                            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
                        });

                        //设置音量50
                        VolumeControl.ChangeVolumeTotheLevel(0.5);

                        //播放音乐
                        if (_ifMusicPlaying == false)
                        {
                            Messenger.Default.Send<BeginPlayingMusic>(new BeginPlayingMusic());
                        }

                        Task.Delay(5000).Wait();
                    }

                    if (_listenFlag == false)
                    {
                        break;
                    }
                }
                while (true);
            }
        }
        
        

        //取消进程监听
        private void StopListen()
        {
            _listenFlag = false;
        }

        public CountDownViewModel(IProcessService processService, IRootNavigationService rootNavigationService,IIdentityService identityService, 
            IDialogService dialogService, ILocalTaskService localTaskService,ILocalBlackListProgressService localBlackListProgressService,
            IWebBlackListProgressService webBlackListProgressService, IWebTaskService webTaskService)
        {

            ifLogin = false;
            writingTask = new Motherlibrary.MyDatabaseContext.Task();
            //进程服务所需变量初始化
            mediaPlayer = new MediaPlayer();
            _listenFlag = false;
            _ifMusicPlaying = false;
            //进程服务所需service初始化
            _localTaskService = localTaskService;
            _processService = processService;
            _identityService = identityService;
            _dialogService = dialogService;
            _rootNavigationService = rootNavigationService;
            _localBlackListProgressService = localBlackListProgressService;
            _webBlackListService = webBlackListProgressService;
            //TODO : 网易云音乐测试

            blackListProgresses = new List<Motherlibrary.MyDatabaseContext.BlackListProgress>();

            MessageRegister();

        }

        private void MessageRegister()
        {
            //开始监听Message注册
            Messenger.Default.Register<BeginListenMessage>(this, async (message) =>
            {
                //原bug可能性：不由UI线程创建，访问时出错
                //我们来尝试一下，这个线程由UI线程初始化，使用异步机制来进行
                Task t1 = Task.Factory.StartNew(delegate { BeginListenAsync(); });

                
            });

            //取消监听Message注册
            Messenger.Default.Register<StopListenMessage>(this, (message) =>
            {
                StopListen();
            });

            Messenger.Default.Register<BeginPlayingMusic>(this, (message) =>
            {
                BeginPlaying();
            });

            Messenger.Default.Register<StopPlayingMusic>(this, (message) =>
            {
                StopPlaying();
            });

            Messenger.Default.Register<AddTask>(this, async (message) =>
            {
                if (message.message.Equals("Init"))
                {
                    //更新临时变量
                    writingTask = _localTaskService.getTaskWithNowTime();
                    writingTask.DefaultTime = message.parameter;
                    writingTask.FinishTime = 0;
                    writingTask.FinishFlag = -1;

                    //判断服务器或数据库
                    AppUser userNow = _identityService.GetCurrentUserAsync();
                    if(userNow.ID == 0)
                    {
                        //未登陆：数据库读写
                        ifLogin = false;
                        writingTask.UserID = -1;
                        await _localTaskService.AddTaskAsync(writingTask);
                    }
                    else
                    {
                        //创建新任务，保存ID
                        ifLogin = true;
                        var template = await _webTaskService.NewWebTaskAsync(writingTask.Date,writingTask.Begin,writingTask.DefaultTime);
                        writingTask.ID = template.ID;
                    }
                }
                else if (message.message.Equals("Refresh"))
                {
                    writingTask.FinishTime++;
                    if(ifLogin == false)
                    {
                        await _localTaskService.UpdateTaskAsync(writingTask);
                    }
                }
                else if (message.message.Equals("Finish"))
                {
                    //判断
                    writingTask.FinishTime++;
                    writingTask.FinishFlag = 0;

                    StopListen();
                    StopPlaying();

                    if (ifLogin == false)
                    {
                        await _localTaskService.UpdateTaskAsync(writingTask);
                    }
                    else
                    {
                        await _webTaskService.UpdateWebTaskAsync(writingTask.ID, writingTask.FinishFlag, writingTask.FinishTime, writingTask.Introduction);
                    }
                }
                else if (message.message.Equals("Fail"))
                {
                    //判断
                    writingTask.FinishFlag = 1;

                    StopListen();
                    StopPlaying();

                    if (ifLogin == false)
                    {
                        await _localTaskService.UpdateTaskAsync(writingTask);
                    }
                    else
                    {
                        await _webTaskService.UpdateWebTaskAsync(writingTask.ID, writingTask.FinishFlag, writingTask.FinishTime, writingTask.Introduction);
                    }
                }
                else if (message.message.Equals("ForeFinish"))
                {
                    //判断
                    writingTask.FinishFlag = 0;

                    StopListen();
                    StopPlaying();

                    if (ifLogin == false)
                    {
                        await _localTaskService.UpdateTaskAsync(writingTask);
                    }
                    else
                    {
                        await _webTaskService.UpdateWebTaskAsync(writingTask.ID, writingTask.FinishFlag, writingTask.FinishTime, writingTask.Introduction);
                    }
                }
            });
        }

        private void BeginPlaying()
        {
            //随机歌曲
            int random = (int)(CryptographicBuffer.GenerateRandomNumber() % 7);
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(musicLocation[random]));
            mediaPlayer.IsLoopingEnabled = true;
            mediaPlayer.Play();
            _ifMusicPlaying = true;
        }

        private void StopPlaying()
        {
            //Dispose() : 释放对象
            mediaPlayer.Pause();
            _ifMusicPlaying = false;
        }


        private bool _navigate;
        public bool Navigate
        {
            get => _navigate;
            set => Set(nameof(Navigate), ref _navigate, value);
        }

        private bool _logoutFlag;
        public bool LogoutFlag
        {
            get => _logoutFlag;
            set => Set(nameof(LogoutFlag), ref _logoutFlag, value);
        }


        /// <summary>
        ///     跳转命令。
        /// </summary>
        /// 
        private RelayCommand _navigateToLoginCommand;

        public RelayCommand NavigateToLoginCommand =>
            _navigateToLoginCommand ?? (_navigateToLoginCommand = new RelayCommand(() => {

                if (_identityService.GetCurrentUserAsync().ID == 0)
                {
                    Navigate = true;
                    _navigateToLoginCommand.RaiseCanExecuteChanged();
                    _rootNavigationService.Navigate(typeof(LoginPage));
                }
                else
                {
                    Navigate = false;
                    _navigateToLoginCommand.RaiseCanExecuteChanged();
                }
            }, ()=>!Navigate));



        private RelayCommand _logoutCommand;

        public RelayCommand LogoutCommand =>
            _logoutCommand ?? (_logoutCommand = new RelayCommand(async () => {

                if (_identityService.GetCurrentUserAsync().ID == 0)
                {

                    await _dialogService.ShowAsync("请先登录！");
                    _rootNavigationService.Navigate(typeof(LoginPage));
                }
                else
                {
                   
                    _identityService.SignOut();
                    await _dialogService.ShowAsync("登出成功！");
                    Navigate = true;

                }
            }, () => Navigate));

    }
}
