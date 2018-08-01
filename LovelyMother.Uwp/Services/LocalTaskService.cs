using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LovelyMother.Uwp.Models;
using Microsoft.EntityFrameworkCore;
using Motherlibrary;

namespace LovelyMother.Uwp.Services
{
    public class LocalTaskService : ILocalTaskService
    {
        /// <summary>
        /// 根据Date与Begin进行删除
        /// </summary>
        /// <param name="deleteTask"></param>
        /// <returns></returns>
        public async Task<bool> DeleteTaskAsync(List<MyDatabaseContext.Task> deleteTask)
        {
            if (deleteTask != null)
            {
                using (var db = new MyDatabaseContext())
                {
                    foreach (var task in deleteTask)
                    {
                        var temp = db.Tasks.Where(m => m.Date == task.Date && m.Begin == task.Begin).FirstOrDefault();
                        if (temp != null)
                        {
                            db.Tasks.Remove(temp);
                        }
                    }
                    await db.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 返回一个task类型对象(无id)
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="Begin"></param>
        /// <param name="DefaultTime"></param>
        /// <param name="FinishTime"></param>
        /// <param name="Introduction"></param>
        /// <param name="FinishFlag"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public MyDatabaseContext.Task GetTask(string Date, string Begin, int DefaultTime, int FinishTime, string Introduction, int FinishFlag, int UserID)
        {
            var temp = new MyDatabaseContext.Task();

            temp.Date = Date;
            temp.Begin = Begin;
            temp.DefaultTime = DefaultTime;
            temp.FinishTime = FinishTime;
            temp.FinishFlag = FinishFlag;
            temp.UserID = UserID;
            temp.Introduction = Introduction;

            return temp;
        }

        /// <summary>
        /// 列出所有的Tasks
        /// </summary>
        /// <returns>ObeservableCollection</returns>
        public async Task<ObservableCollection<MyDatabaseContext.Task>> ListTaskAsync()
        {
            ObservableCollection<MyDatabaseContext.Task> result = new ObservableCollection<MyDatabaseContext.Task>();
            using (var db = new MyDatabaseContext())
            {
                List<MyDatabaseContext.Task> temp = await db.Tasks.ToListAsync();
                foreach(MyDatabaseContext.Task template in temp)
                {
                    result.Add(template);
                }
            }
            return result;
        }

        /// <summary>
        /// 新增一个Task到数据库中
        /// </summary>
        /// <param name="newTask"></param>
        /// <returns></returns>
        public async Task<bool> AddTaskAsync(MyDatabaseContext.Task newTask)
        {
            using (var db = new MyDatabaseContext())
            {
                var Announcement = await db.Tasks.FirstOrDefaultAsync(m => m.Date == newTask.Date && m.Begin == newTask.Begin);
                if (Announcement == null)
                {
                    db.Tasks.Add(newTask);
                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        
        /// <summary>
        /// 更改某个id相同的Task的信息
        /// </summary>
        /// <param name="updateTask"></param>
        /// <returns></returns>
        public async Task<bool> UpdateTaskAsync(MyDatabaseContext.Task updateTask)
        {
            using (var db = new MyDatabaseContext())
            {
                var task = await db.Tasks.SingleOrDefaultAsync(m =>  m.ID == updateTask.ID);
                if(task != null)
                {
                    task.Date = updateTask.Date;
                    task.Begin = updateTask.Begin;
                    task.DefaultTime = updateTask.DefaultTime;
                    task.FinishTime = updateTask.FinishTime;
                    task.FinishFlag = updateTask.FinishFlag;
                    task.UserID = updateTask.UserID;
                    task.Introduction = updateTask.Introduction;

                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public MyDatabaseContext.Task WebTypeToLocalType(WebTask theWebTask)
        {
            return new MyDatabaseContext.Task()
            {
                Date = theWebTask.Date,
                Begin = theWebTask.Begin,
                DefaultTime = theWebTask.DefaultTime,
                FinishTime = theWebTask.FinishTime,
                FinishFlag = theWebTask.FinishFlag,
                Introduction = theWebTask.Introduction,

            };
        }
    }

        private string NormalLize(string temp, int time)
        {
            if (time > 0)
            {
                string template = "0";
                for (int i = time - 1; i > 0; i++)
                {
                    template += "0";
                }
                template += temp;
                return template;
            }
            return temp;
        }

        /// <summary>
        /// 获得当前时间的Task
        /// </summary>
        /// <returns></returns>
        public Motherlibrary.MyDatabaseContext.Task getTaskWithNowTime()
        {
            DateTime dateTime = DateTime.Now;

            string year = dateTime.Year.ToString();

            string month = dateTime.Month.ToString();
            month = NormalLize(month, 2 - month.Count());

            string day = dateTime.Day.ToString();
            day = NormalLize(day, 2 - day.Count());

            string hour = dateTime.Hour.ToString();
            hour = NormalLize(hour, 2 - hour.Count());

            string minute = dateTime.Minute.ToString();
            minute = NormalLize(minute, 2 - minute.Count());

            string second = dateTime.Second.ToString();
            second = NormalLize(second, 2 - second.Count());

            return GetTask(year + month + day, hour + minute + second, 5, 5, "学习", 0, -1);
        }

        public MyDatabaseContext.Task WebTaskToLocal(WebTask webtask)
        {
            return new MyDatabaseContext.Task()
            {
                ID = webtask.ID,
                Date = webtask.Date,
                Begin = webtask.Begin,
                DefaultTime = webtask.DefaultTime,
                FinishTime = webtask.FinishTime,
                FinishFlag = webtask.FinishFlag,
                Introduction = webtask.Introduction,
                UserID = webtask.UserID
            };
        }
    }
}
