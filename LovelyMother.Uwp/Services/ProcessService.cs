using System;
using System.Collections.ObjectModel;
using System.Linq;
using LovelyMother.Uwp.Models;
using Windows.System.Diagnostics;
using Windows.System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Motherlibrary;
using Windows.UI.Popups;

namespace LovelyMother.Uwp.Services
{
    public class ProcessService : IProcessService
    {
        /// <summary>
        /// 获取所有进程(接口实现) : 包括Win32 与 UWP
        /// </summary>
        /// <returns>ObservableCollection<Process></returns>
        public ObservableCollection<Process> GetProcessNow()
        {
            ObservableCollection<Process> processes = new ObservableCollection<Process>();

            //取出所有进程
            IReadOnlyList<ProcessDiagnosticInfo> details = ProcessDiagnosticInfo.GetForProcesses();

            foreach (ProcessDiagnosticInfo detail in details)
            {
                if (detail.Parent != null)
                {
                    if ((!detail.ExecutableFileName.Equals("winlogon.exe")) && (!detail.ExecutableFileName.Equals("svchost.exe")) && (!detail.Parent.ExecutableFileName.Equals("wininit.exe")))
                    {
                        var temp2 = new Process(detail.ExecutableFileName, "2333", 3);
                        processes.Add(temp2);
                    }
                }
            }
            return processes;
        }

        public ObservableCollection<Process> GetProcessDifferent(ObservableCollection<Process> processFirst, ObservableCollection<Process> processSecond)
        {
            int i;
            int j;
            bool judge;
            var temp = new ObservableCollection<Process>();
            
            //逐一比对，若有相同，则非新进程
            for (i = 0; i < processFirst.Count; i++)
            {
                for (j = 0, judge = true; j < processSecond.Count; j++)
                {
                    if ((processFirst[i].FileName.Equals(processSecond[j].FileName))&&(processFirst[i].Type == processSecond[j].Type))
                    {
                        judge = false;
                        break;
                    }
                }
                if (judge == true)
                {
                    temp.Add(processFirst[i]);
                }
             }
            return temp;
        }

        public bool IfBlackListProcessExist(List<MyDatabaseContext.BlackListProgress> appName,ObservableCollection<Process> processesNow)
        {
            bool judge = false;

            //如果相同 => 则是黑名单成员，返回真值
            for (int i = 0; i < processesNow.Count; i++)
            {
                for (int j = 0; j < appName.Count; j++)
                {
                    if (processesNow[i].FileName.Equals(appName[j].FileName))
                    {
                        judge = true;
                        break;
                    }
                }
                if (judge == true)
                {
                    break;
                }
            }

            return judge;
        }
    }
}
