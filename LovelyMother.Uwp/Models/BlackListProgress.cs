using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace LovelyMother.Uwp.Models
{
    public class BlackListProgress : ObservableObject
    {


        private string _uwp_ID;
        public string Uwp_ID
        {
            get => _uwp_ID;
            set => Set(nameof(Uwp_ID), ref _uwp_ID, value);
        }

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
        /// 进程名称。
        /// </summary>
        private string _fileName;

        public string FileName
        {
            get => _fileName;
            set => Set(nameof(FileName), ref _fileName, value);
        }

        /// <summary>
        /// 用户定义名称。
        /// </summary>
        private string _resetName;

        public string ResetName
        {
            get => _resetName;
            set => Set(nameof(ResetName), ref _resetName, value);
        }

        /// <summary>
        /// 种类。
        /// </summary>
        private int _type;

        public int Type
        {
            get => _type;
            set => Set(nameof(Type), ref _type, value);
        }

    }
}
