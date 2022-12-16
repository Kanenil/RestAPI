using ClientWPF.ViewModels;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClientWPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(LoginViewModel model)
        {
            InitializeComponent();
            this.DataContext= model;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                Close();
        }

        private void exit_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as PackIconMaterial).Foreground = Brushes.White;
        }

        private void exit_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as PackIconMaterial).Foreground = Brushes.Black;
        }
    }
}
