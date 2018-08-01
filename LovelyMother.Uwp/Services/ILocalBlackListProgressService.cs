using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LovelyMother.Uwp.Models;
using Motherlibrary;

namespace LovelyMother.Uwp.Services
{
    public interface ILocalBlackListProgressService
    {
        //列出所有的黑名单
        Task<ObservableCollection<MyDatabaseContext.BlackListProgress>> ListBlackListProgressAsync();

        //新建一项任务
        Task<bool> AddBlackListProgressAsync(MyDatabaseContext.BlackListProgress newBLProgress);

        //删除一项任务
        Task<bool> DeleteBlackListProgressAsync(MyDatabaseContext.BlackListProgress deleteBLProgress);

        //转换为Task类型
        MyDatabaseContext.BlackListProgress GetBlackListProgress(string Uwp_ID, string FileName,
             string ResetName, int type);

        //OverRide : 删除多项任务
        Task<bool> DeleteBlackListProgressAsync(List<MyDatabaseContext.BlackListProgress> deleteList);

        //Web类型转换，返回一个本地型变量
        MyDatabaseContext.BlackListProgress WebProcessToLocal(BlackListProgress webBlackListProgress);
    }
}
