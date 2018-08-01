using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace LovelyMother.Uwp.Models
{
    public class RankList:ObservableObject
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => Set(nameof(Name),ref _name,value);
        }

        private int _totalTime;

        public int TotalTime
        {
            get => _totalTime;
            set => Set(nameof(TotalTime), ref _totalTime, value);
        }

    }
}
