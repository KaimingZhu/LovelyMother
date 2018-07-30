using LovelyMother.Uwp.Services;
using Motherlibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace CountDownTask
{
    public sealed class CountDownTask : IBackgroundTask
    {
        private ProcessService _processService;
        private List<MyDatabaseContext.BlackListProgress> blackListProgresses;
        private bool _ifMusicPlaying;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            //Async : deferral
            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();

                _ifMusicPlaying = false;
                _processService = new ProcessService();

                //打开黑名单: i = 1 => Delay(10000) / 不打开 : i = 0 => delay(2000)
                do
                {
                    var NewProcess = _processService.IfBlackListProcessExist(blackListProgresses, _processService.GetProcessNow());

                    if (NewProcess == false)
                    {
                        if (_ifMusicPlaying == true)
                        {
                            Messenger.Default.Send<StopListenMessage>(new StopListenMessage());
                        }
                        Thread.Sleep(10000);
                    }
                    else
                    {

                        //弹出新窗口
                        PunishWindow();

                        //设置音量50
                        VolumeControl.ChangeVolumeTotheLevel(0.5);

                        //播放音乐
                        if (_ifMusicPlaying == false)
                        {
                            Messenger.Default.Send<BeginListenMessage>(new BeginListenMessage());
                        }

                        Thread.Sleep(2000);
                    }

                    if (_listenFlag == false)
                    {
                        break;
                    }
                }
                while (true);
            }

            _deferral.Complete();
        }

        public static BackgroundTaskRegistration RegisterBackgroundTask(string taskEntryPoint, string name, IBackgroundTrigger trigger, IBackgroundCondition condition)
        {

            // We’ll add code to this function in subsequent steps.

            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {

                if (cur.Value.Name == name)
                {
                    // 
                    // The task is already registered.
                    // 
                    

                    return (BackgroundTaskRegistration)(cur.Value);
                }

                //The Task has not been registered

                var builder = new BackgroundTaskBuilder();

                builder.Name = name;
                builder.TaskEntryPoint = taskEntryPoint;
                builder.SetTrigger(trigger);

                if (condition != null)
                {
                    //registered in the BackgroundTaskRegistration
                    builder.AddCondition(condition);
                }

                BackgroundTaskRegistration task = builder.Register();

                return task;

            }

        }

    }
}
