using LovelyMother.Uwp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Motherlibrary;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;
using LovelyMother.Uwp.Models.Messages;
using LovelyMother.Uwp.Models;
using Windows.UI.Popups;

namespace LovelyMother.Uwp.ViewModels
{
    public class AddProgressViewModel
    {

        //第一次读取的集合
        private ObservableCollection<Process> _firstCollection;

        //第二次读取的集合
        private ObservableCollection<Process> _secondCollection;

        //AddProgress页绑定的进程列
        public ObservableCollection<Process> addProgressCollection
        {
            get;
            private set;
        }

        //viewProgress页绑定的进程列
        public ObservableCollection<MyDatabaseContext.BlackListProgress> viewProgressCollection
        {
            get;
            private set;
        }

        //本地黑名单服务
        private ILocalBlackListProgressService _localBlackListProgressService;

        //服务器预设黑名单服务

        //进程服务
        private IProcessService _processService;

        public AddProgressViewModel(IProcessService processService, ILocalBlackListProgressService localBlackListProgressService)
        {
            //变量初始化
            _secondCollection = new ObservableCollection<Process>();
            _firstCollection = new ObservableCollection<Process>();
            addProgressCollection = new ObservableCollection<Process>();
            viewProgressCollection = new ObservableCollection<MyDatabaseContext.BlackListProgress>();
            //服务初始化
            _localBlackListProgressService = localBlackListProgressService;
            _processService = processService;

            Messenger.Default.Register<AddProgressMessage>(this, async (message) =>
            {
                //选择添加
                if (message.ifSelectToAdd == true)
                {
                    switch (message.choice)
                    {
                        case 1:
                            {
                                //清空数组
                                _firstCollection.Clear();
                                _secondCollection.Clear();

                                //第一次读取
                                _firstCollection = _processService.GetProcessNow();

                                break;
                            }
                        case 2:
                            {
                                //第二次读取
                                _secondCollection = _processService.GetProcessNow();

                                //获得不同的进程
                                var temp = _processService.GetProcessDifferent(_firstCollection, _secondCollection);

                                //清空数组，加入
                                addProgressCollection.Clear();
                                if (temp != null)
                                {
                                    foreach (var template in temp)
                                    {
                                        addProgressCollection.Add(template);
                                    }
                                }
                                break;
                            }
                        case 3: {

                                if (message.parameter == null) {
                                    await new MessageDialog("添加失败").ShowAsync();
                                }

                                //传入参数，写入数据库
                                //UWP进程
                                Motherlibrary.MyDatabaseContext.BlackListProgress temp;
                                temp = _localBlackListProgressService.GetBlackListProgress(message.parameter.ID, message.parameter.FileName, message.newName, message.parameter.Type);

                                bool judge = await _localBlackListProgressService.AddBlackListProgressAsync(temp);
                                if (judge == false)
                                {
                                    await new MessageDialog("添加失败！").ShowAsync();
                                }
                                else
                                {
                                    await new MessageDialog("添加成功！").ShowAsync();
                                }
                                break; }
                    }
                }
                else
                {
                    //删除、更新、刷新
                    switch (message.choice)
                    {

                        case 1 : {
                                //删除

                                        _localBlackListProgressService.DeleteBlackListProgressAsync(message.deleteList);       

                                    //刷新
                                    await RefreshTheCollection();
                                    break;
                            }

                        case 2 : {
                                    //更新
                                    await RefreshTheCollection();
                                    break;
                            }

                        case 3 : {
                                    //刷新
                                    await RefreshTheCollection();
                                    break;
                            }
                    }
                }
            });
        }

        private async Task RefreshTheCollection()
        {
            viewProgressCollection.Clear();
            foreach(var temp in (await _localBlackListProgressService.ListBlackListProgressAsync()))
            {
                viewProgressCollection.Add(temp);
            }
        }
    }
}
