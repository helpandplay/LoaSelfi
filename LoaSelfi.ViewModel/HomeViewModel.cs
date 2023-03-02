using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using LoaSelfi.Model;
using LoaSelfi.Service;

namespace LoaSelfi.ViewModel;
[INotifyPropertyChanged]
public partial class HomeViewModel : ViewModelBase
{
    private string _Test = "Test";
    public string Test
    {
        get
        {
            return _Test;
        }
        set
        {
            _Test = value;
            OnPropertyChanged(nameof(Test));
        }
    }

    private ObservableCollection<ImageInfo> _ImageInfos = new ObservableCollection<ImageInfo>();
    public ObservableCollection<ImageInfo> ImageInfos
    {
        get
        {
            return _ImageInfos;
        }
        set
        {
            _ImageInfos = value;
            OnPropertyChanged(nameof(ImageInfos));
        }
    }

    public Service.Finder Finder { get; }
    public FileLoader FileLoader { get; }

    public HomeViewModel(Service.Finder finder, Service.FileLoader fileLoader)
    {
        this.Finder = finder;
        this.FileLoader = fileLoader;

        LoadImages();
    }

    private void LoadImages()
    {
        var task = Finder.GetFolderPathTask();

        task.Start();

        task.ContinueWith(folderPath =>
        {
            var files = FileLoader.GetFiles(folderPath.Result);

            foreach(var file in files)
            {
                Task<ImageInfo?> imageInfoTask = Task.Factory.StartNew(() => FileLoader.LoadImageAsync(file));
                imageInfoTask.ContinueWith(imageInfo =>
                {
                    if(imageInfo.Result != null)
                    {
                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Background,
                            new Action(delegate
                            {
                                ImageInfos.Add(imageInfo.Result);
                            })
                        );
                    }
                });
            }
        });
    }
}
