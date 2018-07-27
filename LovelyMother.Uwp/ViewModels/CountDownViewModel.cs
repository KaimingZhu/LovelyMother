using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using LovelyMother.Uwp.Models.Messages;
using LovelyMother.Uwp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LovelyMother.Uwp.ViewModels
{
    public class CountDownViewModel : ViewModelBase
    {
        //监听算法辅助标识符 : 为1时取消监听并重置为0
        public int flag { get; private set; }

        /// <summary>
        /// 服务调用
        /// </summary>

        private IProcessService _processService; 

        //调用一个数据库读写的Service - private变量



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
        private void BeginListen()
        {

        }

        //取消进程监听
        private void StopListen()
        {

        }

        public CountDownViewModel(IProcessService processService)
        {

            flag = 0;

            _processService = processService;

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

        public CountDownViewModel() : this(new ProcessService())
        {

        }
    }
}
