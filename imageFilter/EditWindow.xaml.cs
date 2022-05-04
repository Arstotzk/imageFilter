using imageFilter.Filters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace imageFilter
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public Filter chosenFilter;
        ObservableCollection<PropertyValue> collection = null;
        public bool deleteFilter = false;
        public EditWindow(Filter _chosenFilter)
        {
            InitializeComponent();
            chosenFilter = _chosenFilter;
            FilterName.Text = chosenFilter.Name;
            FieldInfo[] fields = chosenFilter.GetType().GetFields();
            collection = new ObservableCollection<PropertyValue>();
            foreach (FieldInfo field in fields)
            {
                PropertyValue fieldVal = (PropertyValue)field.GetValue(chosenFilter);
                collection.Add(new PropertyValue() { Name = fieldVal.Name, Value = fieldVal.Value });
            }
            dgPropetys.ItemsSource = collection;
        }

        private void OnDelete(object sender, RoutedEventArgs e)
        {
            deleteFilter = true;
            DialogResult = true;
            Close();
        }
        public void OnSave(object sender, RoutedEventArgs e)
        {
            FieldInfo[] fields = chosenFilter.GetType().GetFields();
            int row = 0;
            foreach (FieldInfo field in fields)
            {
                PropertyValue fieldVal = (PropertyValue)field.GetValue(chosenFilter);
                fieldVal.Value = collection[row].Value;
                row++;
            }
            DialogResult = true;
            Close();
        }
    }
}
