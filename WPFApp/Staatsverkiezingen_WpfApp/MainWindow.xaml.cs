using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Staatsverkiezingen_WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            //Slaat de data op wat is ingevuld
            string user = tbName.Text;
            string pass = pbWachtwoord.Password;

            //Slaat de data voor het inloggen op
            MySqlConnection conn = new MySqlConnection("Server=localhost;Database=projecten;Uid=root;Pwd=;");

            //Opent de connectie
            conn.Open();

            //Maakt een sql command aan
            MySqlCommand command = conn.CreateCommand();
            command.CommandText = $"SELECT * FROM inloggegevens WHERE gebruikersnaam = '{user}' AND wachtwoord = '{pass}' AND Admin = '1';";

            //Execute de command
            MySqlDataReader reader = command.ExecuteReader();

            //Leest de command
            DataTable dtData = new DataTable();
            dtData.Load(reader);

            try
            {
                //Kijkt of er een row uit komt of niet
                if (dtData.Rows[0][0].ToString() != "")
                {
                    //Voert het stuk loggedin uit
                    loggedin();
                }
                else
                {
                    //Voert het strukje loginfailed uit
                    loginfailed();
                }
            } catch (Exception)
            {
                //Voert het strukje loginfailed uit
                loginfailed();
            }

            //Sluit de connectie
            conn.Close();
        }

        private void loggedin()
        {
            //Maakt een niewe window window en sluit de actieve window
            window mwindow = new window();
            mwindow.Show();
            Close();
        }

        private void loginfailed()
        {
            //Geeft een error message dat er iets fout is gegaan.
            MessageBox.Show("Foutive inloggegevens of teweinig permissions", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
