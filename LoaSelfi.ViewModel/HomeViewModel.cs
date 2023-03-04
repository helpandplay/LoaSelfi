using System;
using System.Collections.ObjectModel;
using System.Threading;
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

    private bool _IsLoadedImages = false;
    public bool IsLoadedImages
    {
        get
        {
            return _IsLoadedImages;
        }
        set
        {
            _IsLoadedImages = value;
            OnPropertyChanged(nameof(IsLoadedImages));
        }
    }

    private string _ProgressText = string.Empty;
    public string ProgressText
    {
        get
        {
            return _ProgressText;
        }
        set
        {
            _ProgressText = value;
            OnPropertyChanged(nameof(ProgressText));
        }
    }

    public Service.Finder Finder { get; }
    public ImageFileLoader FileLoader { get; }

    public HomeViewModel(Service.Finder finder, Service.ImageFileLoader fileLoader)
    {
        this.Finder = finder;
        this.FileLoader = fileLoader;

        LoadImages();
    }

    private async void LoadImages()
    {
        string folderPath = await Task.Run(() => Finder.GetFolderPathAllSerachDrives());

        await Task.Run(() =>
        {
            string[] files = ImageFileLoader.GetFiles(folderPath);
            string baseProgressText = $"{files.Length} Loading...";

            int cnt = 0;
            ProgressText = $"{cnt} / {baseProgressText}";

            foreach(var file in files)
            {
                Task<ImageInfo?> imageInfoTask = Task.Factory.StartNew(() => FileLoader.LoadImageAsync(file));
                var completedLoadImageTask = imageInfoTask.ContinueWith(imageInfo =>
                {
                    if(imageInfo.Result == null)
                    {
                        return;
                    }

                    Application.Current.Dispatcher.BeginInvoke(
                        System.Windows.Threading.DispatcherPriority.Background,
                        new Action(delegate
                        {
                            ImageInfos.Add(imageInfo.Result);
                            Interlocked.Increment(ref cnt);
                            ProgressText = $"{cnt} / {baseProgressText}";

                            if(cnt == files.Length)
                            {
                                IsLoadedImages = true;
                            }
                        })
                    );
                });
            }
        });
    }
}
