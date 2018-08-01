using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LovelyMother.Uwp.Models.Messages
{
    public class AddProgressMessage
    {
        /// <summary>
        /// 1:第一次读取(删除) 2:第二次读取（更新） 3:添加操作（刷新）
        /// </summary>
        public int choice { get; set; }

        /// <summary>
        /// true : 添加服务 / false : 删除、更新、刷新
        /// </summary>
        public bool ifSelectToAdd { get; set; }

        /// <summary>
        /// true && 3 时传递的参数 - 选择的进程
        /// </summary>
        public Process parameter { get; set; }

        /// <summary>
        /// true && 3 时传递的参数 - 预设的名称
        /// </summary>
        public string newName { get; set; }

        /// <summary>
        /// false && 1 时传递的参数 - 删除的进程列
        /// </summary>
        public List<Motherlibrary.MyDatabaseContext.BlackListProgress> deleteList { get; set; }
    }
}
