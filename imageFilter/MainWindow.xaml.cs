using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using imageFilter.Filters;
using Microsoft.Win32;

namespace imageFilter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Filter lastFilter = null;

        private ObservableCollection<Filter> _FilterList = new ObservableCollection<Filter>();

        public MainWindow()
        {
            InitializeComponent();
            FiltersList.ItemsSource = _FilterList;

            Style itemContainerStyle = new Style(typeof(ListBoxItem));
            itemContainerStyle.Setters.Add(new Setter(ListBoxItem.AllowDropProperty, true));
            itemContainerStyle.Setters.Add(new EventSetter(ListBoxItem.PreviewMouseRightButtonDownEvent, new MouseButtonEventHandler(ListBox_PreviewMouseRightButtonDown)));
            itemContainerStyle.Setters.Add(new EventSetter(ListBoxItem.DropEvent, new DragEventHandler(ListBox_Drop)));
            FiltersList.ItemContainerStyle = itemContainerStyle;
        }
        public void PathDialog(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofdPicture = new OpenFileDialog();
            ofdPicture.Filter =
                "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*";
            ofdPicture.FilterIndex = 1;

            if (ofdPicture.ShowDialog() == true)
            {
                imgWithoutFilters.Source = new BitmapImage(new Uri(ofdPicture.FileName));
                imgWithFilters.Source = new BitmapImage(new Uri(ofdPicture.FileName));
                BitmapImage bitmapImgToFilter = new BitmapImage(new Uri(ofdPicture.FileName));
                TestFilter tf = new TestFilter();
                imgWithFilters.Source = tf.Apply(bitmapImgToFilter);
            }
        }
        public void AddFilter(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow(lastFilter);
            bool? addRes = addWindow.ShowDialog();
            if (addRes.HasValue && addRes.Value) {_FilterList.Add((Filter)addWindow.ChosenFilter.SelectedItem); }
        }
        public void EditFilter(object sender, RoutedEventArgs e)
        {
            EditWindow editWindow = new EditWindow((Filter)FiltersList.SelectedItem);
            bool? editRes = editWindow.ShowDialog();
            if (editRes.HasValue && editRes.Value)
            {
                int index = _FilterList.IndexOf((Filter)FiltersList.SelectedItem);
                if (editWindow.deleteFilter == true) { _FilterList.Remove(_FilterList[index]); }
                else { _FilterList[index] = editWindow.chosenFilter; }
            }
        }
        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            IEnumerable<Type> subFilters = typeof(Filter).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Filter)));
            Filter droppedData = null;
            foreach (Type item in subFilters)
            {
                if (droppedData != null) break;
                droppedData = (Filter)e.Data.GetData(item);
            }
            Filter target = ((ListBoxItem)(sender)).DataContext as Filter;

            int removedIdx = FiltersList.Items.IndexOf(droppedData);
            int targetIdx = FiltersList.Items.IndexOf(target);

            if (removedIdx < targetIdx)
            {
                _FilterList.Insert(targetIdx + 1, droppedData);
                _FilterList.RemoveAt(removedIdx);
            }
            else
            {
                int remIdx = removedIdx + 1;
                if (_FilterList.Count + 1 > remIdx)
                {
                    _FilterList.Insert(targetIdx, droppedData);
                    _FilterList.RemoveAt(remIdx);
                }
            }

        }
        private void ListBox_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem)
            {
                ListBoxItem draggedItem = sender as ListBoxItem;
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                draggedItem.IsSelected = true;
            }
        }
    }
}
