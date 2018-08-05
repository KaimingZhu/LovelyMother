using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LovelyMother.Uwp.Models
{
    public class TaskBindingModel
    {
        public Motherlibrary.MyDatabaseContext.Task theTask { get; set; }
        public string condition { get; set; }
        public string uri { get; set; }
    }
}
