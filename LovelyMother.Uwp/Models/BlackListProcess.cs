using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LovelyMother.Uwp.Models
{
    public class BlackListProcess : ObservableObject
    {
        /// <summary>
        /// 存储Id
        /// </summary>
        public int id { get; private set; }

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
        /// 预设名字
        /// </summary>
        private string _resetName;

        public string resetName
        {
            get => _resetName;
            set => Set(nameof(resetName), ref _resetName, value);
        }

        /// <summary>
        /// 种类：0为服务器UWP黑名单 / 1为服务器win32黑名单
        /// / 2为用户UWP黑名单 / 3为用户win32黑名单
        /// </summary>
        private int _type;

        public int type
        {
            get => _type;
            set => Set(nameof(type), ref _type, value);
        }

        public BlackListProcess(Process process,string resetName)
        {
            fileName = process.fileName;
            this.resetName = resetName;
            type = process.type;
        }
    }
}
