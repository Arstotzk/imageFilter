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
        private BitmapImage bitmapImgToFilter = null;
        private ImageWithFilters iwf = null;
        private bool FilterApplyComplete = false;

        private ObservableCollection<Filter> filterList = new ObservableCollection<Filter>();

        public MainWindow()
        {
            InitializeComponent();
            FiltersList.ItemsSource = filterList;

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
                bitmapImgToFilter = new BitmapImage(new Uri(ofdPicture.FileName));
            }
        }
        public void ApplyFilters(object sender, RoutedEventArgs e) 
        {
            if (FilterApplyComplete == true) { return; }
            iwf = new ImageWithFilters(bitmapImgToFilter, filterList);
            iwf.ApllyFilters();
            imgWithFilters.Source = iwf.GetBitmapSource();
            FilterApplyComplete = true;
        }
        public void AddFilter(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow(lastFilter);
            bool? addRes = addWindow.ShowDialog();
            if (addRes.HasValue && addRes.Value) {filterList.Add((Filter)addWindow.ChosenFilter.SelectedItem); }
            FilterApplyComplete = false;
        }
        public void EditFilter(object sender, RoutedEventArgs e)
        {
            //Сделать как диалог
            EditWindow editWindow = new EditWindow((Filter)FiltersList.SelectedItem);
            bool? editRes = editWindow.ShowDialog();
            if (editRes.HasValue && editRes.Value)
            {
                int index = filterList.IndexOf((Filter)FiltersList.SelectedItem);
                if (editWindow.deleteFilter == true) { filterList.Remove(filterList[index]); }
                else { filterList[index] = editWindow.chosenFilter; }
                FilterApplyComplete = false;
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
                filterList.Insert(targetIdx + 1, droppedData);
                filterList.RemoveAt(removedIdx);
            }
            else
            {
                int remIdx = removedIdx + 1;
                if (filterList.Count + 1 > remIdx)
                {
                    filterList.Insert(targetIdx, droppedData);
                    filterList.RemoveAt(remIdx);
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

        private void SaveImage(object sender, RoutedEventArgs e)
        {
            if (iwf == null && FilterApplyComplete == true)
            {
                MessageBox.Show("Сохранение невозможно. Необходимо применить фильтры.");
                return;
            }
            SaveFileDialog sfDlg = new SaveFileDialog();
            sfDlg.FileName = "ImageAfterFilters";
            sfDlg.Filter = "JPeg Image|*.jpg|Png Image|*.png";
            if (sfDlg.ShowDialog() == true)
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(iwf.GetBitmapSource()));
                using (var fileStream = sfDlg.OpenFile())
                {
                    encoder.Save(fileStream);
                }
            }
        }
    }
}
