using System;
using GalaSoft.MvvmLight;

namespace LovelyMother.Uwp.Models
{
    public class WebTask : ObservableObject
    {

        /// <summary>
        /// 主键。
        /// </summary>
        private int _id;

        public int ID
        {
            get => _id;
            set => Set(nameof(ID), ref _id, value);
        }


        /// <summary>
        /// 创建日期。
        /// </summary>
        private String _date;
        public String Date
        {
            get => _date;
            set => Set(nameof(Date), ref _date, value);
        }

        /// <summary>
        /// 开始时间。
        /// </summary>
        private String _begin;
        public String Begin
        {
            get => _begin;
            set => Set(nameof(Begin), ref _begin, value);
        }

        /// <summary>
        /// 任务总时间。
        /// </summary>
        private int _defaultTime;

        public int DefaultTime
        {
            get => _defaultTime;
            set => Set(nameof(DefaultTime), ref _defaultTime, value);
        }


        /// <summary>
        /// 任务当前完成时间。
        /// </summary>
        private int _finishTime;

        public int FinishTime
        {
            get => _finishTime;
            set => Set(nameof(FinishTime), ref _finishTime, value);
        }

        /// <summary>
        /// 任务说明。
        /// </summary>
        private String _introduction;
        public String Introduction
        {
            get => _introduction;
            set => Set(nameof(Introduction), ref _introduction, value);
        }

        /// <summary>
        /// 是否完成任务。
        /// </summary>
        private int _finishFlag;

        public int FinishFlag
        {
            get => _finishFlag;
            set => Set(nameof(FinishFlag), ref _finishFlag, value);
        }


        /// <summary>
        /// 所属用户ID。
        /// </summary>
        private int _userID;

        public int UserID
        {
            get => _userID;
            set => Set(nameof(UserID), ref _userID, value);
        }

        public User User { get; set; }
    }
}
