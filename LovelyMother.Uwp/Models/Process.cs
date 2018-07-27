﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LovelyMother.Uwp.Models
{
    public class Process : ObservableObject
    {
        /// <summary>
        /// 可执行文件名
        /// </summary>
        private string _fileName;

        public string fileName
        {
            get => _fileName;
            set => Set(nameof(fileName), ref _fileName, value);
        }

        /// <summary>
        /// 进程id
        /// </summary>
        private string _id;

        public string id
        {
            get => _id;
            set => Set(nameof(id), ref _id, value);
        }

        /// <summary>
        /// 种类：2为UWP进程 / 3为win32进程
        /// 同时放置type => 方便函数构造
        /// </summary>
        private int _type;

        public int type
        {
            get => _type;
            set => Set(nameof(type), ref _type, value);
        }

        public Process(string fileName, string id, int type)
        {
            this.fileName = fileName;
            this.id = id;
            this.type = type;
        }
    }
}
