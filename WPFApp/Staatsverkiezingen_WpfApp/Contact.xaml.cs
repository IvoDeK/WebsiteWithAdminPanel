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
    /// Interaction logic for Contact.xaml
    /// </summary>
    public partial class Contact : Window
    {
        public Contact()
        {
            InitializeComponent();
            //Voert het stukje addcontacts uit
            addcontacts();
        }

        public void addcontacts()
        {
            //Leegt de items in de listbox zodat het er niet dubbel inkomt
            contactlist.Items.Clear();

            //Slaat de data voor het inloggen op
            MySqlConnection conn = new MySqlConnection("Server=localhost;Database=projecten;Uid=root;Pwd=;");

            //Opent de connectie
            conn.Open();

            //Maakt een sql command aan
            MySqlCommand command = conn.CreateCommand();
            command.CommandText = "SELECT contactnaam FROM contact;";

            //Execute de command
            MySqlDataReader reader = command.ExecuteReader();

            //Leest de command
            DataTable dtData = new DataTable();
            dtData.Load(reader);

            //Loopt voor elke contactnemende en add een item voor de listbox
            foreach (DataRow row in dtData.Rows)
            {
                string cName = row["contactnaam"].ToString();
                contactlist.Items.Add(cName);
            }

            //Sluit de connectie
            conn.Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            //Maakt een niewe window window en sluit de actieve window
            window mwindow = new window();
            mwindow.Show();
            Close();
        }

        private void Contactlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //Kijkt welke contactnaam is geselecteerd en zet het om in een string
                string contactnaam = contactlist.SelectedItem.ToString();

                //Voert het stuk boxreset uit
                boxreset();

                //Slaat de data voor het inloggen op
                MySqlConnection conn = new MySqlConnection("Server=localhost;Database=projecten;Uid=root;Pwd=;");

                //Opent de connectie
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = $"SELECT contact.contactnaam, contact.contactmail, contact.contactnummer, contact.contactvraag, partijen.partijnaam FROM contact LEFT JOIN partijen ON contact.partijid = partijen.partijid WHERE contactnaam = '{contactnaam}';";

                //Execute de command
                MySqlDataReader reader = command.ExecuteReader();

                //Leest de command
                DataTable dtData = new DataTable();
                dtData.Load(reader);

                //Loopt voor de geselecteerde persoon en haalt de data op uit de database
                foreach (DataRow row in dtData.Rows)
                {
                    cname.Text += row["contactnaam"].ToString();
                    ctel.Text += row["contactnummer"].ToString();
                    cmail.Text += row["contactmail"].ToString();
                    cpartij.Text += row["partijnaam"].ToString();
                    cquestion.Text = row["contactvraag"].ToString();
                }

                //Sluit de connectie
                conn.Close();

                //Zet de button active
                btnDel.IsEnabled = true;
            }
            catch (Exception) { }
            
        }

        private void boxreset()
        {
            //Reset alle textblocken
            cname.Text = "Naam: ";
            ctel.Text = "Telefoonnummer: ";
            cmail.Text = "E-mail: ";
            cpartij.Text = "Partij: ";
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            //Kijkt welke contactnemnede is aan geklikt en zet het dan om naar een string
            string nameselected = contactlist.SelectedItem.ToString();

            //Vraagt of je zeker weet of je de contactnemende wilt verwijderen
            MessageBoxResult result = MessageBox.Show($"Weet u zeker dat je de contactnemende {nameselected} wilt verwijderen?", "Zeker?", MessageBoxButton.YesNo, MessageBoxImage.Hand);

            if (result == MessageBoxResult.Yes)
            {
                //Slaat de data voor het inloggen op
                MySqlConnection conn = new MySqlConnection("Server=localhost;Database=projecten;Uid=root;Pwd=;");

                //Opent de connectie
                conn.Open();

                //Maakt een sql command aan
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = $"DELETE FROM contact WHERE contactnaam = '{nameselected}';";

                //Execute de command
                MySqlDataReader reader = command.ExecuteReader();

                //Leest de command
                DataTable dtData = new DataTable();
                dtData.Load(reader);

                //Geeft aan dat de contactnemende is verwijderd
                MessageBox.Show($"De contactnemende {nameselected} is verwijderd", "Verwijderd", MessageBoxButton.OK, MessageBoxImage.Information);

                //Voert het stukje addcontacts en boxreset opniew uit
                addcontacts();
                boxreset();

                //disabled de button
                btnDel.IsEnabled = false;
            }
            //Als nee wordt geselecteerd word geselecteerd
            else
            {
                //Zegt dat de contactnemende niet verwijderd is
                MessageBox.Show($"De partij {nameselected} is niet verwijderd", "Niet verwijderd", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
