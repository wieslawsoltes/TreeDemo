using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TreeDemo.ViewModels;

public partial class NodeViewModel : ViewModelBase
{
    [ObservableProperty] private string? _name;
    [ObservableProperty] private ObservableCollection<NodeViewModel>? _children;
    [ObservableProperty] private bool _isExpanded;

    public bool HasChildren => _children?.Count > 0;
}