using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using LovelyMother.Uwp.Models;

namespace LovelyMother.Uwp.Services
{
    public interface IWebTaskService
    {

        /// <summary>
        /// 列出所有历史任务。
        /// </summary>
        /// <returns></returns>
        Task<List<WebTask>> ListWebTaskAsync();

        /// <summary>
        /// 根据任务Id获取任务。
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task<WebTask> GetWebTaskAsync(int id);

        /// <summary>
        /// 新建任务。
        /// </summary>
        /// <param name="date"></param>
        /// <param name="begin"></param>
        /// <param name="defaultTime"></param>
        /// <returns></returns>
        Task<WebTask> NewWebTaskAsync(string date, string begin, int defaultTime);

        /// <summary>
        /// 删除历史任务。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteWebTaskAsync(int id);

        /// <summary>
        /// 更新任务信息。
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="finish"></param>
        /// <param name="finishTime"></param>
        /// <param name="introduction"></param>
        /// <returns></returns>
        Task<bool> UpdateWebTaskAsync(int taskId,int finishFlag, int finishTime, string introduction);

        /// <summary>
        /// LocalTask转换为WebTask : 返回一个UserID为-1的WebTask(若UserID为-1)
        /// </summary>
        /// <param name="localtask"></param>
        /// <returns></returns>
         WebTask LocalTaskToWeb_NoneUser(Motherlibrary.MyDatabaseContext.Task localtask);

    }
}
