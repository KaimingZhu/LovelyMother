using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using LovelyMother.Uwp.Services;

namespace LovelyMother.Uwp.ViewModels
{
    /// <summary>
    ///     ViewModel定位器。
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        ///     ViewModel定位器单件。
        /// </summary>
        public static readonly ViewModelLocator Instance =
            new ViewModelLocator();

        /// <summary>
        ///     构造函数。
        /// </summary>
       /* private ViewModelLocator()
        {
            SimpleIoc.Default
                .Register<IRootNavigationService, RootNavigationService>();
            SimpleIoc.Default.Register<IIdentityService, IdentityService>();
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<IProcessService, ProcessService>();
            SimpleIoc.Default.Register<IUserService, UserService>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<UpdateUserViewModel>();



        }*/
    }
}
