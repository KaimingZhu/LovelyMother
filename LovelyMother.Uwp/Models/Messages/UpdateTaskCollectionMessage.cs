using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LovelyMother.Uwp.Models.Messages
{
    public class UpdateTaskCollectionMessage
    {
        /// <summary>
        /// 1 : 增加 / 2 : 删除 / 3 : 更改 / 4 : 刷新 
        /// </summary>
        public int selection { get; set; }
        public List<Motherlibrary.MyDatabaseContext.Task> taskList { get; set; }
    }
}
