﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LovelyMother.Uwp.Services
{
    public interface ITaskService
    {
        //列出所有的任务
        Task<List<Motherlibrary.MyDatabaseContext.Task>> ListTaskAsync();

        //新建一项任务
        Task<bool> NewTaskAsync(Motherlibrary.MyDatabaseContext.Task newTask);

        //修改一项任务
        Task<bool> UpdateTaskAsync(Motherlibrary.MyDatabaseContext.Task updateTask);

        //删除一项任务
        Task<bool> DeleteTaskAsync(Motherlibrary.MyDatabaseContext.Task deleteTask);

        //转换为Task类型
        Motherlibrary.MyDatabaseContext.Task GetANewTask(int ID, string Date, string Begin,
            int DefaultTime, int FinishTime, string Introduction, int FinishFlag, int UserID);
    }
}
