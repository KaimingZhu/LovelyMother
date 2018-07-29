using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using LovelyMother.Uwp.Models.Messages;
using LovelyMother.Uwp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        //监听算法变量 : 音乐uri
        private static string[] musicLocation = { "ms-appx:///Assets/Musics/1.mp3", "ms-appx:///Assets/Musics/2.mp3",
                                           "ms-appx:///Assets/Musics/3.mp3", "ms-appx:///Assets/Musics/4.mp3",
                                           "ms-appx:///Assets/Musics/5.mp3", "ms-appx:///Assets/Musics/6.mp3",
                                           "ms-appx:///Assets/Musics/7.mp3" };

        //监听算法变量 : 音乐播放器
        private MediaPlayer mediaPlayer;

        //监听算法辅助标识符 : 'true' - 音乐处于播放状态，'false' - 音乐处于关闭状态
        private bool _ifMusicPlaying { get; set; }

        //监听算法辅助标识符 : 为1时取消监听并重置为0
        private bool _listenFlag { get; set; }

        /// <summary>
        /// 服务调用
        /// </summary>

        private IProcessService _processService;

        private IRootNavigationService _rootNavigationService;

        //调用一个读取数据库和服务器黑名单的Service - private变量
        List<Motherlibrary.MyDatabaseContext.BlackListProgress> blackListProgresses;


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
        private async void BeginListen()
        {
            if (_listenFlag == false)
            {
                //避免多进程运行造成的不必要CPU与内存占用
                _listenFlag = true;

                //打开黑名单: i = 1 => Delay(10000) / 不打开 : i = 0 => delay(2000)
                do
                {
                    var NewProcess = _processService.IfBlackListProcessExist(blackListProgresses, _processService.GetProcessNow());

                    if (NewProcess == false)
                    {

                        if (_ifMusicPlaying == true)
                        {
                            //Dispose() : 释放对象
                            mediaPlayer.Pause();
                            _ifMusicPlaying = false;
                        }

                        await Task.Delay(10000);

                    }
                    else
                    {

                        //弹出新窗口
                        PunishWindow();

                        //播放音乐
                        if (_ifMusicPlaying == false)
                        {
                            //设置音量50
                            VolumeControl.ChangeVolumeTotheLevel(0.5);

                            //播放音乐
                            int random = (int)(CryptographicBuffer.GenerateRandomNumber() % 7);
                            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(musicLocation[random]));
                            mediaPlayer.Play();
                            _ifMusicPlaying = true;

                        }
                        await Task.Delay(2000);
                    }

                    if (_listenFlag == false)
                    {
                        break;
                    }
                }
                while (true);
            }
        }

        /// <summary>
        /// 弹出骚扰窗口
        /// </summary>
        private async void PunishWindow()
        {
            var currentAV = ApplicationView.GetForCurrentView();
            var newAV = CoreApplication.CreateNewView();
            await newAV.Dispatcher.RunAsync(
                            CoreDispatcherPriority.Normal,
                            async () =>
                            {
                                var newWindow = Window.Current;
                                var newAppView = ApplicationView.GetForCurrentView();
                                newAppView.Title = "你怎么回事弟弟？";

                                var frame = new Frame();
                                frame.Navigate(typeof(PunishPage), null);
                                newWindow.Content = frame;
                                newWindow.Activate();

                                await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
                                newAppView.Id,
                                ViewSizePreference.UseMinimum,
                                currentAV.Id,
                                ViewSizePreference.UseMinimum);
                            });
        }

        //取消进程监听
        private void StopListen()
        {
            //如何跳转
            _listenFlag = false;
        }

        public CountDownViewModel(IProcessService processService, IRootNavigationService rootNavigationService)
        {
            //进程服务所需变量初始化
            mediaPlayer = new MediaPlayer();
            _listenFlag = false;
            _ifMusicPlaying = false;
            //进程服务所需service初始化
            _processService = processService;
            _rootNavigationService = rootNavigationService;
            //TODO : 网易云音乐测试
            blackListProgresses = new List<Motherlibrary.MyDatabaseContext.BlackListProgress>();
            blackListProgresses.Add(new Motherlibrary.MyDatabaseContext.BlackListProgress()
            {
                FileName = "cloudmusic.exe",
                Type = 3
            });

            //开始监听Message注册
            Messenger.Default.Register<BeginListenMessage>(this, (message) =>
            {
                BeginListen();
            });

            //取消监听Message注册
            Messenger.Default.Register<StopListenMessage>(this, (message) =>
            {
                StopListen();
            });
        }

        public CountDownViewModel() : this(new ProcessService(),new RootNavigationService())
        {

        }
    }
}
