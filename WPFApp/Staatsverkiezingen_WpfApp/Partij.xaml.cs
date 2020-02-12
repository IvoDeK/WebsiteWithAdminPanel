using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Drawing;

namespace Staatsverkiezingen_WpfApp
{
    /// <summary>
    /// Interaction logic for Partij.xaml
    /// </summary>
    public partial class Partij : Window
    {
        public Partij()
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
            try
            {
                //Kijkt welke partij is aan geklikt en dan omzetten naar een string
                string partijnaam = partijen.SelectedItem.ToString();

                //Voert het stukje reset uit
                reset();

                //Slaat de data voor het inloggen op
                MySqlConnection conn = new MySqlConnection("Server=localhost;Database=projecten;Uid=root;Pwd=;");

                //Opent de connectie
                conn.Open();

                //Maakt een sql command aan
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = $"SELECT * FROM partijen WHERE partijnaam = '{partijnaam}';";

                //Execute de command
                MySqlDataReader reader = command.ExecuteReader();

                //Leest de command
                DataTable dtData = new DataTable();
                dtData.Load(reader);

                //Loopt voor de geselecteerde partij en haalt alle data op
                foreach (DataRow row in dtData.Rows)
                {
                    tbPName.Text += row["partijnaam"].ToString();
                    tbPStandpunten.Text += row["standpunten"].ToString();
                    tbPInfo.Text += row["partijinfo"].ToString();
                    try
                    {
                        //Zet de eventuele blob om in een picture
                        byte[] data = (byte[])row["partijfoto"];
                        using (MemoryStream ms = new MemoryStream(data))
                        {
                            var imageSource = new BitmapImage();
                            imageSource.BeginInit();
                            imageSource.StreamSource = ms;
                            imageSource.CacheOption = BitmapCacheOption.OnLoad;
                            imageSource.EndInit();

                            iPfoto.Source = imageSource;
                        }
                    }
                    catch { }

                }

                //Sluit de connectie
                conn.Close();

                //Als er een partij is geselecteerd dan mogen de buttons worden aan geklikt
                btnEdit.IsEnabled = true;
                btnPfoto.IsEnabled = true;
            }
            catch (Exception) { }
            
        }

        private void reset()
        {
            //Reset alle TextBoxes
            tbPName.Text = "";
            tbPStandpunten.Text = "";
            tbPInfo.Text = "";
            iPfoto.Source = null;
            tbPLocation.Text = "";
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            //Maakt een niewe window window en sluit de actieve window
            window mwindow = new window();
            mwindow.Show();
            Close();
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

        public byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            //Zet de foto in de Image Box om in een byte[]
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        { 
            //Maakt van alle textblocken een string
            string partijnaam = tbPName.Text;
            string partijnaamselected = partijen.SelectedItem.ToString();
            string partijinfo = tbPInfo.Text;
            string standpunten = tbPStandpunten.Text;

            //Maakt een value byte[] aan voor de foto die wordt omgezet
            byte[] encode = getJPGFromImageControl(iPfoto.Source as BitmapImage);

            //Slaat de data voor het inloggen op
            MySqlConnection conn = new MySqlConnection("Server=localhost;Database=projecten;Uid=root;Pwd=;");

            //Opent de connectie
            conn.Open();

            //Maakt een sql command aan
            MySqlCommand command = conn.CreateCommand();
            command.CommandText = $"UPDATE partijen SET partijnaam = '{partijnaam}', partijinfo = '{partijinfo}', standpunten = '{standpunten}', partijfoto = @img WHERE partijnaam = '{partijnaamselected}';";

            //Zet de byte[] array om in een blob
            MySqlParameter blob = new MySqlParameter("@img", MySqlDbType.Blob, encode.Length);

            blob.Value = encode;
            command.Parameters.Add(blob);

            //Execute de command
            command.ExecuteNonQuery();

            //Geeft weer dat de partij is veranderd
            MessageBox.Show($"De partij {partijnaamselected} is veranderd", "Veranderd!", MessageBoxButton.OK, MessageBoxImage.Information);

            //Runt addpartijen en reset opniew
            addpartijen();
            reset();

            //Disabled de buttons
            btnEdit.IsEnabled = false;
            btnPfoto.IsEnabled = false;
        }
    }
}
