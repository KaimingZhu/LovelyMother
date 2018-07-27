using LovelyMother.Uwp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LovelyMother.Uwp.Services
{
    public interface IProcessService
    {
        /// <summary>
        /// 读取所有进程 / 包括win32 与 UWP
        /// </summary>
        /// <returns>所有进程</returns>
        ObservableCollection<Process> GetProcessNow();

        /// <summary>
        /// 判断是否有黑名单进程出现
        /// </summary>
        /// <param name="appName"></param>
        /// <returns>true : Yes / False : No</returns>
        bool IfBlackListProcessExist(List<BlackListProcess> appName, ObservableCollection<Process> processesNow);

        /// <summary>
        /// 读取相异的进程 : 第一次比第二次多出来的项
        /// </summary>
        /// <param name="processFirst"></param>
        /// <param name="processSecond"></param>
        /// <returns>相异项或null</returns>
        ObservableCollection<Process> GetProcessDifferent(ObservableCollection<Process> processFirst, ObservableCollection<Process> processSecond);
    }
}
