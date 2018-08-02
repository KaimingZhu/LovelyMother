using GalaSoft.MvvmLight.Messaging;
using LovelyMother.Uwp.Models.Messages;
using LovelyMother.Uwp.ViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace LovelyMother.Uwp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public  MainPage()
        {
            this.InitializeComponent();

            DataContext = ViewModelLocator.Instance.CountDownViewModel;

            //窗口初始化大小。
            ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default);
            ApplicationView.PreferredLaunchViewSize = new Size(500, 500);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            //DispatcherTimer timer = new DispatcherTimer() { Interval = new TimeSpan(0, 1, 0) };   
            ExtendAcrylicIntoTitleBar();

        }

        /// Extend acrylic into the title bar. 
        private void ExtendAcrylicIntoTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }


        /// <summary>
        /// 选择头像。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
      private async void ChoosePicture_Click(object sender, RoutedEventArgs e)
        {

            // 创建和自定义 FileOpenPicker
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail; //可通过使用图片缩略图创建丰富的视觉显示，以显示文件选取器中的文件
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".gif");

            //选取单个文件

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            //文件处理
            if (file != null)
            {
                var inputFile = SharedStorageAccessManager.AddFile(file);
                var destination = await ApplicationData.Current.LocalFolder.CreateFileAsync("Cropped.jpg", CreationCollisionOption.ReplaceExisting);//在应用文件夹中建立文件用来存储裁剪后的图像
                var destinationFile = SharedStorageAccessManager.AddFile(destination);
                var options = new LauncherOptions();
                options.TargetApplicationPackageFamilyName = "Microsoft.Windows.Photos_8wekyb3d8bbwe";
                //待会要传入的参数


                var parameters = new ValueSet();
                parameters.Add("InputToken", inputFile);                //输入文件
                parameters.Add("DestinationToken", destinationFile);    //输出文件            
                parameters.Add("ShowCamera", false);                    //它允许我们显示一个按钮，以允许用户采取当场图象
                parameters.Add("EllipticalCrop", true);                 //截图区域显示为圆（最后截出来还是方形）
                parameters.Add("CropWidthPixals", 300);
                parameters.Add("CropHeightPixals", 300);
                //调用系统自带截图并返回结果
                var result = await Launcher.LaunchUriForResultsAsync(new Uri("microsoft.windows.photos.crop:"), options, parameters);
                //按理说下面这个判断应该没问题呀，但是如果裁剪界面点了取消的话后面会出现异常，所以后面我加了try catch
                if (result.Status == LaunchUriStatus.Success && result.Result != null)
                {
                    //对裁剪后图像的下一步处理
                    try
                    {
                        // 载入已保存的裁剪后图片
                        var stream = await destination.OpenReadAsync();
                        var bitmap = new BitmapImage();
                        //此处生成结束，设置img控件的源，同时上传
                        await bitmap.SetSourceAsync(stream);
                        // 显示
                        img.ImageSource = bitmap;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message + ex.StackTrace);
                    }
                }
            }
        }

        /// <summary>
        /// /倒计时。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (CutTimer.Value == 0)
            {
                await new MessageDialog("请先设置时间！！").ShowAsync();//弹窗。
            }
            else
            {
                Frame root = Window.Current.Content as Frame;
                root.Navigate(typeof(CountDownPage),CutTimer.Value);
            }
        }

       /* private void Test_Click(object sender, RoutedEventArgs e)
        {
            Frame root = Window.Current.Content as Frame;
            Frame.Navigate(typeof(LoginPage));
        }*/

        private void ListTaskButton_Click(object sender, RoutedEventArgs e)
        {
            Frame root = Window.Current.Content as Frame;
            Frame.Navigate(typeof(ListTask));
        }

        private void AddProgress_Click(object sender, RoutedEventArgs e)
        {
            Frame root = Window.Current.Content as Frame;
            Frame.Navigate(typeof(ViewProgress));
        }


        private void AddFriend_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FriendPage));
        }

        private void RankList_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TopPage));
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {

            
            Frame.Navigate(typeof(UpdateUser));
        }



        /*public async Task LoadState()
        {
            var task = await StartupTask.GetAsync("AppAutoRun");
            this.tbState.Text = $"Status: {task.State}";
            switch (task.State)
            {
                case StartupTaskState.Disabled:
                    // 禁用状态
                    this.btnSetState.Content = "启用";
                    this.btnSetState.IsEnabled = true;
                    break;
                case StartupTaskState.DisabledByPolicy:
                    // 由管理员或组策略禁用
                    this.btnSetState.Content = "被系统策略禁用";
                    this.btnSetState.IsEnabled = false;
                    break;
                case StartupTaskState.DisabledByUser:
                    // 由用户手工禁用
                    this.btnSetState.Content = "被用户禁用";
                    this.btnSetState.IsEnabled = false;
                    break;
                case StartupTaskState.Enabled:
                    // 当前状态为已启用
                    this.btnSetState.Content = "已启用";
                    this.btnSetState.IsEnabled = false;
                    break;
            }
        }

        private async void Auto_OnClick (object sender, RoutedEventArgs e)
        {
            var task = await StartupTask.GetAsync("AppAutoRun");
            if (task.State == StartupTaskState.Disabled)
            {
                await task.RequestEnableAsync();
            }

            // 重新加载状态
            await LoadState();
        }*/

        private async Task<bool> GetRequest()
        {

            DiagnosticAccessStatus temp = await AppDiagnosticInfo.RequestAccessAsync();
            switch (temp)
            {
                case DiagnosticAccessStatus.Allowed:
                    {
                        GetProcessRequest.IsEnabled = false;
                        break;
                    }
                case DiagnosticAccessStatus.Limited:
                    {
                        AppDiagnosticInfo.RequestAccessAsync();
                        GetProcessRequest.IsEnabled = true;
                        return true;
                    }
            }
            return false;
        }

        private async void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            //await LoadState();
            await GetRequest();
        }

        private async void GetProcessRequest_Click(object sender, RoutedEventArgs e)
        {
            var judge = await GetRequest();
            if( judge == true )
            {
                AppDiagnosticInfo.RequestAccessAsync();
            }
        }
    }
}
