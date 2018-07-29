using System;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Core;

using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Windows.UI;
using Windows.Storage;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Media.Imaging;
using System.Diagnostics;


// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace LovelyMother.Uwp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            
            ApplicationView.PreferredLaunchViewSize = new Size(500, 500);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;//窗口初始化大小。
            this.InitializeComponent();
            //DispatcherTimer timer = new DispatcherTimer() { Interval = new TimeSpan(0, 1, 0) };
            
            var view = ApplicationView.GetForCurrentView();
            view.TitleBar.ButtonBackgroundColor = Colors.Transparent; //将标题栏的三个键背景设为透明
            view.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent; //失去焦点时，将三个键背景设为透明
            view.TitleBar.ButtonInactiveForegroundColor = Colors.White; //失去焦点时，将三个键前景色设为白色

           
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
        private async void StratButton_Click(object sender, RoutedEventArgs e)
        {
            if (CutTimer.Value == 0)
            {
                await new MessageDialog("请先设置时间！！").ShowAsync();//弹窗。
            }
            else
            {
                DispatcherTimer timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
                timer.Tick += new EventHandler<object>(async (sende, ei) =>
                {

                    //double a = CutTimer.Value * 60;

                    await Dispatcher.TryRunAsync
                        (CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                        {
                            CutTimer.Value = CutTimer.Value - 1;

                            CutTimer.Unit = (((int)CutTimer.Value) / 3600).ToString("00") + ":"//文本显示。
                                + ((((int)CutTimer.Value) % 3600) / 60).ToString("00") + ":"
                                + ((((int)CutTimer.Value) % 3600) % 60).ToString("00");
                            if (CutTimer.Value == CutTimer.Minimum)
                            {
                                timer.Stop();
                            }
                        }));

                });
                timer.Start();
            }
        }
        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {

          
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(YuHaoTest1));
        }

        private void AddProgress_Click(object sender, RoutedEventArgs e)
        {

            
            Frame.Navigate(typeof(ViewProgress));
        }

        private void ListTaskButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ListTask));
        }





       


    }
}
