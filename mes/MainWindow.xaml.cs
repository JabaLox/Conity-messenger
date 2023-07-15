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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using Org.BouncyCastle.Utilities;
using static System.Net.Mime.MediaTypeNames;
using System.Data.SqlClient;

namespace mes
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MySqlConnection con;
        string Connect = "server=localhost;port=3306;user=root;database=conity_messenger;password=kolyan28;";
        //public string Connect = "server=87.236.19.248;port=3306;user=ramzilyo_conity;database=ramzilyo_conity;password=(Ramzil11)";
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Window1 window = new Window1();
            window.Show();
            this.Close();
            
        }
        private void voiti_Click(object sender, RoutedEventArgs e)
        {
            con = new MySqlConnection(Connect);
            con.Open();
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter($"SELECT id_user,Name,password,photo from users where password='{Password_box.Password}' and login='{Login_text.Text}'", con);
            try
            {
                adapter.Fill(dt);
                Global.UsreId = dt.Rows[0][0].ToString();
                Global.Paswword = dt.Rows[0][2].ToString();
                Global.Name = dt.Rows[0][1].ToString();
                if (dt.Rows[0][3].ToString() != "")
                {
                    Global.Avatar = dt.Rows[0][3].ToString();
                }
                else
                {
                    Global.Avatar =  "picture.png";
                }
                Chat chat = new Chat();
                chat.Show();
                this.Close();
            }
            catch
            {
                System.Windows.MessageBox.Show($"Не верный логин или пароль!");
            }

        }
    }
}
