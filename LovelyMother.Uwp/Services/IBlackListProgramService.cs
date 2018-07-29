using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Motherlibrary;

namespace LovelyMother.Uwp.Services
{
    public interface IBlackListProgramService
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
    }
}
