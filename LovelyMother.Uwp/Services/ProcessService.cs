using System;
using System.Collections.ObjectModel;
using System.Linq;
using LovelyMother.Uwp.Models;
using Windows.System.Diagnostics;
using Windows.System;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            var details = ProcessDiagnosticInfo.GetForProcesses().OrderByDescending(x => x.ExecutableFileName);

            foreach (var detail in details)
            {
                //UWP程序:将用Id加Name一起判断 -> 应用+进程显示的名称
                if(detail.IsPackaged == true)
                {
                    var temp = detail.GetAppDiagnosticInfos();
                    AppDiagnosticInfo diagnosticInfo = temp.FirstOrDefault();
                    if(diagnosticInfo != null)
                    {
                        var temp2 = new Process(diagnosticInfo.AppInfo.DisplayInfo.DisplayName,diagnosticInfo.AppInfo.AppUserModelId,4);
                        processes.Add(temp2);
                    }
                    continue;
                }

                //Win32程序，循例判断
                if (detail.Parent != null)
                {
                    if ((!detail.ExecutableFileName.Equals("winlogon.exe")) && (!detail.ExecutableFileName.Equals("System")) && (!detail.ExecutableFileName.Equals("svchost.exe"))  && (!detail.Parent.ExecutableFileName.Equals("wininit.exe")))
                    {
                        var temp2 = new Process(detail.ExecutableFileName, detail.ProcessId.ToString(), 3);
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
                    if ((processFirst[i].fileName.Equals(processSecond[j].fileName))&&(processFirst[i].type == processSecond[j].type))
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

        public bool IfBlackListProcessExist(List<BlackListProcess> appName,ObservableCollection<Process> processesNow)
        {
            bool judge = false;

            //如果相同 => 则是黑名单成员，返回真值
            for (int i = 0; i < processesNow.Count; i++)
            {
                for (int j = 0; j < appName.Count; j++)
                {
                    if ((processesNow[i].fileName.Equals(appName[j].fileName)) && (processesNow[i].type == appName[j].type))
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
