using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<bool> DeleteTaskAsync(MyDatabaseContext.Task deleteTask)
        {
            using (var db = new MyDatabaseContext())
            {
                var task = await db.Tasks.SingleOrDefaultAsync(m => m.Date == deleteTask.Date && m.Begin == deleteTask.Begin);
                if (task != null)
                {
                    db.Tasks.Remove(task);
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

                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }



    
}
