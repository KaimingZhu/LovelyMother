using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LovelyMother.Uwp.Models
{
    public class WebTask
    {

        /// <summary>
        /// 主键。
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 创建日期。
        /// </summary>
        public String Date { get; set; }

        /// <summary>
        /// 开始时间。
        /// </summary>
        public String Begin { get; set; }

        /// <summary>
        /// 任务总时间。
        /// </summary>
        public int DefaultTime { get; set; }

        /// <summary>
        /// 任务当前完成时间。
        /// </summary>
        public int FinishTime { get; set; }

        /// <summary>
        /// 任务说明。
        /// </summary>
        public String Introduction { get; set; }

        /// <summary>
        /// 是否完成任务。
        /// </summary>
        public int FinishFlag { get; set; }

        /// <summary>
        /// 所属用户ID。
        /// </summary>
        public int UserID { get; set; }

        public User User { get; set; }
    }
}
