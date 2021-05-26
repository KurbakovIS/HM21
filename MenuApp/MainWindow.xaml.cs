using MenuApp.Entity.Models;
using MenuApp.Entity.ModelsDto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MenuApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Api context = new Api();

            btnGet.Click += delegate { listView.ItemsSource = context.GetFoods().ToObservableCollection(); };
            btnAdd.Click += delegate
            {
                context.AddFood(new FoodForCreationDto()
                {
                    Name = txtName.Text,
                    Description = txtDescription.Text,
                    Price = txtPrice.Text,
                });
            };

            btnDel.Click += delegate
            {
                //context.DeleteFood();
            };
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9,]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
