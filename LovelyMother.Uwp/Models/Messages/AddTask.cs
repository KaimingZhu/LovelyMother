using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LovelyMother.Uwp.Models.Messages
{
    public class AddTask
    {
        /// <summary>
        /// 参数 : 创建(Init) / 刷新(Refresh) / 完成(Finish) / 失败(Fail) / 提前完成(ForeFinish)
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 参数 : 预设时间(Init)
        /// </summary>
        public int parameter { get; set; }
    }
}
