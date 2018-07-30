using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LovelyMother.Uwp.Models;

namespace LovelyMother.Uwp.Services
{
    public interface IFriendService
    {

        Task<bool> AddMyFriend(string friendUserName);

        Task<List<FriendList>> GetMyFriend(string applicationUserID);

        Task<bool> DeleteMyFriend(string friendUserName);

        Task<List<RankList>> GetRankList(string applicationUserID);
    }
}
