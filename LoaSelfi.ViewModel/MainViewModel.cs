using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using LoaSelfi.Service;

namespace LoaSelfi.ViewModel;
[INotifyPropertyChanged]
public partial class MainViewModel : ViewModelBase
{
    private IList<ViewModelBase> ViewModels { get; set; }

    private ViewModelBase? _CurrentView;
    public ViewModelBase? CurrentView
    {
        get => _CurrentView;
        private set
        {
            if(_CurrentView != value)
            {
                _CurrentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
    }

    public MainViewModel()
    {
        var finder = Finder.Instance;
        var fileLoader = new Service.ImageFileLoader();

        ViewModels = new List<ViewModelBase>()
        {
            new HomeViewModel(finder, fileLoader)
        };

        CurrentView = ViewModels.First(item => item is HomeViewModel);
    }
}
