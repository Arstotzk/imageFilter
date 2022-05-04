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
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        ObservableCollection<PropertyValue> collection = null;
        public AddWindow(Filter _chosenFilter)
        {
            InitializeComponent();
            ChosenFilter.ItemsSource = new List<Filter> 
            { new TestFilter(),
              new Dilatation()};
            ChosenFilter.SelectionChanged += (s, e) => { ChosenFilterChanged(s, e); };
            dgPropetys.ItemsSource = collection;
        }
        private void ChosenFilterChanged(object sender, RoutedEventArgs e)
        {
            FieldInfo[] fields = ChosenFilter.SelectedItem.GetType().GetFields();
            collection = new ObservableCollection<PropertyValue>();
            foreach (FieldInfo field in fields) 
            {
                PropertyValue fieldVal = (PropertyValue)field.GetValue(ChosenFilter.SelectedItem);
                collection.Add(new PropertyValue() {Name = fieldVal.Name, Value = fieldVal.Value });
            }
            dgPropetys.ItemsSource = collection;
        }
        public void OnSave(object sender, RoutedEventArgs e) 
        {
            FieldInfo[] fields = ChosenFilter.SelectedItem.GetType().GetFields();
            int row = 0;
            foreach (FieldInfo field in fields)
            {
                PropertyValue fieldVal = (PropertyValue)field.GetValue(ChosenFilter.SelectedItem);
                fieldVal.Value = collection[row].Value;
                row++;
            }
            DialogResult = true;
            Close();
        }
    }
}
