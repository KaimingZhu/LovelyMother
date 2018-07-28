using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LovelyMother.Uwp.Models
{
    public class User
    {
        /// <summary>
        /// 主键。
        /// </summary>

        public int ID { get; set; }

        /// <summary>
        /// 自定义用户名。
        /// </summary>

        public string UserName { get; set; }

        /// <summary>
        /// 用户任务完成总时间。
        /// </summary>

        public int TotalTime { get; set; }


        /// <summary>
        ///     用户。
        /// </summary>
        public string ApplicationUserID { get; set; }

        /// <summary>
        /// 头像。
        /// </summary>

        public string Image { get; set; }

        /// <summary>
        /// 好友列表。
        /// </summary>
        public List<FriendShip> FriendShips { get; set; }


    }
}
