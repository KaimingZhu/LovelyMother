using LovelyMother.Uwp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace LovelyMother.Uwp.Services
{
    public class WindowKeepTopService
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// 判断当前窗体是否置顶
        /// </summary>
        /// <returns></returns>
        public bool IfKeepTop()
        {
            return ( ApplicationView.GetForCurrentView().ViewMode.Equals(ApplicationViewMode.Default) == true );
        }

        /// <summary>
        /// 置顶与取消
        /// </summary>
        /// <param name="parameter"></param>
        public async void Execute(object parameter)
        {
            if (ApplicationView.GetForCurrentView().ViewMode.Equals(ApplicationViewMode.Default))
            {
                //置顶
                await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay);
                OverallConfigManager.Instence.WindowMode = ApplicationViewMode.CompactOverlay;

            }
            else{
                //取消
                await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default);
                OverallConfigManager.Instence.WindowMode = ApplicationViewMode.Default;
            }
        }
    }
}
