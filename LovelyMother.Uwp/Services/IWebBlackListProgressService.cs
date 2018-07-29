using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LovelyMother.Uwp.Models;

namespace LovelyMother.Uwp.Services
{
    public interface IWebBlackListProgressService
    {

        Task<List<BlackListProgress>> ListWebBlackListProgressesAsync();

        Task<bool> NewWebBlackListProgress(string uwp_Id,string fileName,string resetName,int type);

        Task<bool> DeleteWebBlackListProgressAsync(int id);

        Task<BlackListProgress> GetWebBlackListProgressAsync(int id);
    }
}
