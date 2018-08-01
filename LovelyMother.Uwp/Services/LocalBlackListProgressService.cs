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
    

    public class LocalBlackListProgressService : ILocalBlackListProgressService
    {
        /// <summary>
        /// 新增黑名单进程
        /// </summary>
        /// <param name="newBLProgress"></param>
        /// <returns></returns>
        public async Task<bool> AddBlackListProgressAsync(MyDatabaseContext.BlackListProgress newBLProgress)
        {
            using (var db = new MyDatabaseContext())
            {

                MyDatabaseContext.BlackListProgress Announcement;

                if (newBLProgress.Type == 2)
                {
                    Announcement = await db.BlackListProgresses.SingleOrDefaultAsync(m => m.Uwp_ID.Equals(newBLProgress.ID));
                }
                else
                {
                    Announcement = await db.BlackListProgresses.SingleOrDefaultAsync(m => m.FileName == newBLProgress.FileName && m.Type == newBLProgress.Type);
                }
                if (Announcement == null)
                {
                    db.BlackListProgresses.Add(newBLProgress);
                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    await new DialogService().ShowAsync("程序已存在!");
                    return false;
                }

            }
        }

        /// <summary>
        /// 按照type,uwp_id与FileName来删除对应黑名单进程
        /// </summary>
        /// <param name="deleteBLProgress"></param>
        /// <returns></returns>
        public async Task<bool> DeleteBlackListProgressAsync(MyDatabaseContext.BlackListProgress deleteBLProgress)
        {
            using(var db = new MyDatabaseContext())
            {
                //UWP黑名单
                if (deleteBLProgress.Type == 2)
                {
                    var temp = await db.BlackListProgresses.FirstOrDefaultAsync(m => m.Uwp_ID == deleteBLProgress.Uwp_ID);
                    if (temp != null)
                    {
                        db.BlackListProgresses.Remove(temp);
                        await db.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    var temp = await db.BlackListProgresses.FirstOrDefaultAsync(m => m.FileName == deleteBLProgress.FileName);
                    if(temp != null)
                    {
                        db.BlackListProgresses.Remove(temp);
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

        /// <summary>
        /// 按照type,uwp_id与FileName来删除对应黑名单进程
        /// </summary>
        /// <param name="deleteBLProgress"></param>
        /// <returns></returns>
        public async Task<bool> DeleteBlackListProgressAsync(List<MyDatabaseContext.BlackListProgress> deleteBLProgressList)
        {
            using (var db = new MyDatabaseContext())
            {
                foreach (var template in deleteBLProgressList)
                {
                    //UWP黑名单
                    if (template.Type == 2)
                    {
                        var temp = await db.BlackListProgresses.FirstOrDefaultAsync(m => m.Uwp_ID == template.Uwp_ID);
                        if (temp != null)
                        {
                            db.BlackListProgresses.Remove(temp);
                        }
                    }
                    else
                    {
                        var temp = await db.BlackListProgresses.FirstOrDefaultAsync(m => m.FileName == template.FileName);
                        if (temp != null)
                        {
                            db.BlackListProgresses.Remove(temp);
                        }
                    }
                }
                await db.SaveChangesAsync();
            }
            return true;
        }

        /// <summary>
        /// 返回一个BlackListProgress对象(无id)
        /// </summary>
        /// <param name="Uwp_ID"></param>
        /// <param name="FileName"></param>
        /// <param name="ResetName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public MyDatabaseContext.BlackListProgress GetBlackListProgress(string Uwp_ID, string FileName, string ResetName, int type)
        {

            var temp = new MyDatabaseContext.BlackListProgress()
            {
                Uwp_ID = Uwp_ID ,
                FileName = FileName ,
                ResetName = ResetName ,
                Type = type
            };

            return temp;
        }

        /// <summary>
        /// 读取自设黑名单列表
        /// </summary>
        /// <returns>ObservableCollection</returns>
        public async Task<ObservableCollection<MyDatabaseContext.BlackListProgress>> ListBlackListProgressAsync()
        {

            ObservableCollection<MyDatabaseContext.BlackListProgress> result = new ObservableCollection<MyDatabaseContext.BlackListProgress>();

            using (var db = new MyDatabaseContext())
            {
                var temp = await db.BlackListProgresses.ToListAsync();

                foreach(MyDatabaseContext.BlackListProgress template in temp)
                {
                    result.Add(template);
                }
            }

            return result;
        }

        public MyDatabaseContext.BlackListProgress WebProcessToLocal(BlackListProgress webBlackListProgress)
        {
            return new MyDatabaseContext.BlackListProgress()
            {
                FileName = webBlackListProgress.FileName,
                Type = webBlackListProgress.Type,
                Uwp_ID = webBlackListProgress.Uwp_ID,
                ID = webBlackListProgress.ID
            };
        }
    }

    
}
