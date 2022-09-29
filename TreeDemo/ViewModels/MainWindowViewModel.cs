using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TreeDemo.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<NodeViewModel> _nodes;
    [ObservableProperty] private NodeViewModel? _selectedNode;

    public MainWindowViewModel()
    {
        _nodes = new ObservableCollection<NodeViewModel>();

        for (var i = 0; i < 10_000; i++)
        {
            var root = new NodeViewModel()
            {
                Name = $"Root{i}",
                Children = new ObservableCollection<NodeViewModel>(),
                IsExpanded = false
            };

            for (var j = 0; j < 100; j++)
            {
                var child = new NodeViewModel()
                {
                    Name = $"Child_{i}_{j}"
                };
                root.Children.Add(child);
            }
            _nodes.Add(root);
        }
        
        NodesSource = CreateNodesSource();
    }
    
    public HierarchicalTreeDataGridSource<NodeViewModel> NodesSource { get; }

    private HierarchicalTreeDataGridSource<NodeViewModel> CreateNodesSource()
    {
        var toolboxSource = new HierarchicalTreeDataGridSource<NodeViewModel>(_nodes)
        {
            Columns =
            {
                new HierarchicalExpanderColumn<NodeViewModel>(
                    inner: new TextColumn<NodeViewModel, string>(
                        "Nodes",
                        m => m.Name, 
                        (m, v) => m.Name = v,
                        new GridLength(1, GridUnitType.Star),
                        new TextColumnOptions<NodeViewModel>()
                        {
                            CanUserSortColumn = true,
                            CanUserResizeColumn = true
                        }),
                    childSelector: x => x.Children,
                    hasChildrenSelector: x => x.HasChildren,
                    isExpandedSelector: x => x.IsExpanded)
            }
        };

        toolboxSource.RowSelection!.SingleSelect = true;

        toolboxSource.RowSelection.SelectionChanged += (_, args) =>
        {
            SelectedNode = args.SelectedItems.FirstOrDefault();
        };

        return toolboxSource;
    }
}
