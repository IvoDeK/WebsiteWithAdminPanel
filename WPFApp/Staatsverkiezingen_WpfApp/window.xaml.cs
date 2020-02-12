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

namespace Staatsverkiezingen_WpfApp
{
    /// <summary>
    /// Interaction logic for window.xaml
    /// </summary>
    public partial class window : Window
    {
        public window()
        {
            InitializeComponent();
        }

        private void PBewerken_Click(object sender, RoutedEventArgs e)
        {
            //Maakt een window voor bewerken aan en sluit de actieve window
            Partij pBewerken = new Partij();
            pBewerken.Show();
            Close();
        }

        private void PToevoegen_Click(object sender, RoutedEventArgs e)
        {
            //Maakt een window voor toevoegen aan en sluit de actieve window
            Partijtoevoegen pToevoegen = new Partijtoevoegen();
            pToevoegen.Show();
            Close();
        }

        private void PVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            //Maakt een window voor Verwijderen aan en sluit de actieve window
            Partijverwijderen pVerwijderen = new Partijverwijderen();
            pVerwijderen.Show();
            Close();
        }

        private void Contact_Click(object sender, RoutedEventArgs e)
        {
            //Maakt een window voor contacten aan en sluit de actieve window
            Contact contact2 = new Contact();
            contact2.Show();
            Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            //Gaat terug naar de inlog scherm en sluit de actieve window
            MainWindow login = new MainWindow();
            login.Show();
            Close();
        }
    }
}
