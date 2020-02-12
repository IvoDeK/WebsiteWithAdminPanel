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
using System.Windows.Shapes;

namespace Staatsverkiezingen_WpfApp
{
    /// <summary>
    /// Interaction logic for Partijverwijderen.xaml
    /// </summary>
    public partial class Partijverwijderen : Window
    {
        public Partijverwijderen()
        {
            InitializeComponent();
            //Voert het stuk addpartijen toe als de window wordt gelaunched
            addpartijen();
        }

        private void addpartijen()
        {
            //Alle partijen verwijderen zodat ze er niet dubbel in komen
            partijen.Items.Clear();

            //Slaat de data voor het inloggen op
            MySqlConnection conn = new MySqlConnection("Server=localhost;Database=projecten;Uid=root;Pwd=;");

            //Opent de connectie
            conn.Open();

            //Maakt een sql command aan
            MySqlCommand command = conn.CreateCommand();
            command.CommandText = "SELECT partijnaam FROM partijen;";

            //Execute de command
            MySqlDataReader reader = command.ExecuteReader();

            //Leest de command
            DataTable dtData = new DataTable();
            dtData.Load(reader);

            //Loopt voor elke partij en add een item voor de listbox
            foreach (DataRow row in dtData.Rows)
            {
                string pName = row["partijnaam"].ToString();
                partijen.Items.Add(pName);
            }

            //Sluit de connectie
            conn.Close();
        }

        private void Partijen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Enabled de knop nadat hij een partij is geselecteerd
            btnDel.IsEnabled = true;
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            //Kijkt welke partij is aan geklikt en dan omzetten naar een string
            string partijnaamselected = partijen.SelectedItem.ToString();

            //Vraagt of je de partij wel zeker wilt verwijderen.
            MessageBoxResult result = MessageBox.Show($"Weet u zeker dat je de partij {partijnaamselected} wilt verwijderen?", "Zeker?", MessageBoxButton.YesNo, MessageBoxImage.Hand);

            //Als ja wordt geselecteerd
            if (result == MessageBoxResult.Yes)
            {
                //Slaat de data voor het inloggen op
                MySqlConnection conn = new MySqlConnection("Server=localhost;Database=projecten;Uid=root;Pwd=;");

                //Opent de connectie
                conn.Open();

                //Maakt een sql command aan
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = $"DELETE FROM partijen WHERE partijnaam = '{partijnaamselected}';";

                //Execute de command
                MySqlDataReader reader = command.ExecuteReader();

                //Leest de command
                DataTable dtData = new DataTable();
                dtData.Load(reader);

                //Geeft aan dat de partij is verwijderd
                MessageBox.Show($"De partij {partijnaamselected} is verwijderd", "Verwijderd", MessageBoxButton.OK, MessageBoxImage.Information);

                //Voert het stukje addpartijen opniew uit
                addpartijen();

                //disabled de button
                btnDel.IsEnabled = false;
            }
            //Als nee wordt geselecteerd word geselecteerd
            else
            {
                //Zegt dat de partij niet verwijderd is
                MessageBox.Show($"De partij {partijnaamselected} is niet verwijderd", "Niet verwijderd", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
            

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            //Maakt een niewe window window en sluit de actieve window
            window mwindow = new window();
            mwindow.Show();
            Close();
        }
    }
}
