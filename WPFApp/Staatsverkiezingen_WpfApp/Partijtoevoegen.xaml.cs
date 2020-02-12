using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for Partijtoevoegen.xaml
    /// </summary>
    public partial class Partijtoevoegen : Window
    {
        public Partijtoevoegen()
        {
            InitializeComponent();
        }

        private void BtnPfoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Opent file explorer en zorgt er voor dat je alleen foto's kan uploaden
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = "Selecteer een partijfoto";
                dialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                                "Portable Network Graphic (*.png)|*.png";

                if (dialog.ShowDialog() == true)
                {
                    //Zet de foto in de Image box
                    iPfoto.Source = new BitmapImage(new Uri(dialog.FileName));
                    tbPLocation.Text = dialog.FileName;
                }
            }
            catch (Exception)
            {
                //Geeft een error als je OpenFileDialog niet klopt
                MessageBox.Show("Er is een error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            //Maakt een niewe window window en sluit de actieve window
            window mwindow = new window();
            mwindow.Show();
            Close();
        }

        public byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            //Zet de foto in de Image Box om in een byte[]
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //Maakt van alle textblocken een string
            string partijnaam = tbPName.Text;
            string partijinfo = tbPInfo.Text;
            string standpunten = tbPStandpunten.Text;

            //Checkt of er een foto is geselecteerd
            if (tbPLocation.Text != "")
            {
                //Maakt een value byte[] aan voor de foto die wordt omgezet
                byte[] encode = getJPGFromImageControl(iPfoto.Source as BitmapImage);

                //Als de vakken niet leeg zijn dan voeg je de partij toe
                if (partijnaam != "" && partijinfo != "" && standpunten != "")
                {
                    //Slaat de data voor het inloggen op
                    MySqlConnection conn = new MySqlConnection("Server=localhost;Database=projecten;Uid=root;Pwd=;");

                    //Opent de connectie
                    conn.Open();

                    //Maakt een sql command aan
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = $"INSERT INTO partijen (partijnaam, partijinfo, standpunten, partijfoto) VALUES ('{partijnaam}', '{partijinfo}', '{standpunten}', @img);";

                    //Zet de byte[] array om in een blob
                    MySqlParameter blob = new MySqlParameter("@img", MySqlDbType.Blob, encode.Length);

                    blob.Value = encode;
                    command.Parameters.Add(blob);

                    //Execute de command
                    command.ExecuteNonQuery();

                    //Geeft weer dat de partij is toegevoedg
                    MessageBox.Show($"De partij {partijnaam} is toegevoegd", "Toegevoegd", MessageBoxButton.OK, MessageBoxImage.Information);

                    //Gaat terug naar de window window
                    window mwindow = new window();
                    mwindow.Show();
                    Close();
                }
                else
                {
                    //Geeft de error code dat er data mist
                    MessageBox.Show("Er mist data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                //Geeft aan dat er geen foto is geselecteerd
                MessageBox.Show("Er is geen image geselecteerd", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
        }
    }
}
