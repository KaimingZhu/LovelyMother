using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LovelyMother.Uwp.Models
{
    public class Task : ObservableObject
    {
        /// <summary>
        /// Taskid
        /// </summary>
        private long _id;

        public long ID
        {
            get => _id;
            set => Set(nameof(ID), ref _id, value);
        }

        /// <summary>
        /// 日期
        /// </summary>
        private string _date;

        public string Date
        {
            get => _date;
            set => Set(nameof(Date),ref _date,value);
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        private string _begin;

        public string Begin
        {
            get => _date;
            set => Set(nameof(Begin), ref _begin, value);
        }

        /// <summary>
        /// 预设时间
        /// </summary>
        private int _defaultTime;

        public int DefaultTime
        {
            get => _defaultTime;
            set => Set(nameof(DefaultTime), ref _defaultTime, value);
        }

        /// <summary>
        /// 完成时间
        /// </summary>
        private int _finishTime;

        public int FinishTime
        {
            get => _finishTime;
            set => Set(nameof(FinishTime), ref _finishTime, value);
        }

        

        /// <summary>
        /// 完成状态 : -1(进行中) / 0 : 已完成 / 1 : 进行中
        /// </summary>
        private int _finishFlag;

        public int FinishFlag
        {
            get => _finishFlag;
            set => Set(nameof(FinishFlag), ref _finishFlag, value);
        }

        /// <summary>
        /// UserId : -1代表本地任务
        /// </summary>
        private string _userId;

        public string userId
        {
            get => _userId;
            set => Set(nameof(userId), ref _userId, value);
        }
    }
}
