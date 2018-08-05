using LovelyMother.Uwp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LovelyMother.Uwp.Services
{
    public interface ILocalTaskService
    {
        //列出所有的任务
        Task<ObservableCollection<Motherlibrary.MyDatabaseContext.Task>> ListTaskAsync();

        //新建一项任务
        Task<bool> AddTaskAsync(Motherlibrary.MyDatabaseContext.Task newTask);

        //修改一项任务
        Task<bool> UpdateTaskAsync(Motherlibrary.MyDatabaseContext.Task updateTask);

        //修改任务的Introduction属性
        Task<bool> UpdateTaskIntroductionAsync(Motherlibrary.MyDatabaseContext.Task updateTask);

        //删除一项任务
        Task<bool> DeleteTaskAsync(List<Motherlibrary.MyDatabaseContext.Task> deleteTask);

        //转换为Task类型
        Motherlibrary.MyDatabaseContext.Task GetTask(string Date, string Begin,
            int DefaultTime, int FinishTime, string Introduction, int FinishFlag, int UserID);

        //获得当前带有当前时间的Task
        Motherlibrary.MyDatabaseContext.Task getTaskWithNowTime();

        //将webTask类型转换为localTask类型
        Motherlibrary.MyDatabaseContext.Task WebTaskToLocal(WebTask webtask);
    }
}
