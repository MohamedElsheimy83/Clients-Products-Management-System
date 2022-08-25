using System;
using System.Collections;
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

namespace Mohamed_Projet_426
{
    /// <summary>
    /// Interaction logic for ListeClients.xaml
    /// </summary>
    public partial class ListeClients : Window
    {
        //constructeur
        public ListeClients(IEnumerable listeClients)
        {
            InitializeComponent();
            ListViewClients.ItemsSource = listeClients;
        }

        public ListeClients()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void lstClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
