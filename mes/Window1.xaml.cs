using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;


namespace mes
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public String login = "";
        public bool flag = false;
        public MySqlConnection con;
        public string filename;
        public string Connect = "server=87.236.19.248;port=3306;user=ramzilyo_conity;database=ramzilyo_conity;password=(Ramzil11)";
        public Window1()
        {
            InitializeComponent();
            border.Opacity = 0;
            border.Visibility = Visibility.Collapsed;
            register.Visibility = Visibility.Collapsed;
            image.Visibility = Visibility.Collapsed;
            Login_text.FontSize = 20;
            Login_text.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(156, 156, 156));
            Login_text.Text = "логин";
            password_text.FontSize = 20;
            password_text.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(156, 156, 156));
            password_text.Text = "пароль";
        }
        private void resume_Click(object sender, RoutedEventArgs e)
        {
            register.Visibility = Visibility.Visible;
            password_text.Visibility = Visibility.Collapsed;
            image.Visibility = Visibility.Visible;
            label.Content = "Добавьте имя и фотографию";
            login = Login_text.Text;
            Login_text.Text = "";
            Login_text.FontSize = 20;
            Login_text.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(156, 156, 156));
            Login_text.Text = "имя";
            flag = true;
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            String query = "";
            if (filename == null)
            {
                query = $"INSERT INTO users (Name,login,password,photo) values ('{Login_text.Text}','{login}','{password_text.Text}','profil2.png')";
            }
            else
            {
                query = $"INSERT INTO users (Name,login,password,photo) values ('{Login_text.Text}','{login}','{password_text.Text}','{filename}')";
            }
           try {
                con = new MySqlConnection(Connect);
                con.Open();
                MySqlCommand command = new MySqlCommand(query, con);
                command.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Вы успешно зарегестрировались!");
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Пользователь с таким логином уже существует!");
            }
        }

        private void image_Click(object sender, RoutedEventArgs e)
        {
            border.Visibility = Visibility.Visible;
            border.Opacity = 0;
            DoubleAnimation buttonAnimation = new DoubleAnimation();
            buttonAnimation.From = 0;
            buttonAnimation.To = 1;
            buttonAnimation.SpeedRatio = 4;
            border.BeginAnimation(Button.OpacityProperty, buttonAnimation);
        }

        private void break_Click(object sender, RoutedEventArgs e)
        {
            if (flag == false)
            {
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                Window1 window1 = new Window1();
                window1.Show();
                this.Close();
            }
        }
        private void Login_text_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Login_text.Text == "логин" || Login_text.Text == "имя")
            {
                Login_text.Text = null;
                Login_text.FontSize = 30;
                Login_text.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            }
        }

        private void password_text_GotFocus(object sender, RoutedEventArgs e)
        {
            if (password_text.Text == "пароль")
            {
                password_text.Text = null;
                password_text.FontSize = 30;
                password_text.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            }
        }
        private void ButtonAnimation_Completed(object sender, EventArgs e)
        {
            border.Visibility = Visibility.Collapsed;
        }
        private void img2_MouseEnter(object sender, MouseEventArgs e)
        {
            ThicknessAnimation buttonAnimation = new ThicknessAnimation();
            buttonAnimation.From = new Thickness(0);
            buttonAnimation.To = new Thickness(0, 0, 0, 10);
            buttonAnimation.SpeedRatio = 4;
            img2.BeginAnimation(Button.MarginProperty, buttonAnimation);
        }


        private void img2_MouseLeave(object sender, MouseEventArgs e)
        {
            ThicknessAnimation buttonAnimation = new ThicknessAnimation();
            buttonAnimation.From = new Thickness(0, 0, 0, 10);
            buttonAnimation.To = new Thickness(0);
            buttonAnimation.SpeedRatio = 4;
            img2.BeginAnimation(Button.MarginProperty, buttonAnimation);
        }

        private void img1_MouseEnter(object sender, MouseEventArgs e)
        {
            ThicknessAnimation buttonAnimation = new ThicknessAnimation();
            buttonAnimation.From = new Thickness(0);
            buttonAnimation.To = new Thickness(0, 0, 0, 10);
            buttonAnimation.SpeedRatio = 4;
            img1.BeginAnimation(Button.MarginProperty, buttonAnimation);

        }

        private void img1_MouseLeave(object sender, MouseEventArgs e)
        {

            ThicknessAnimation buttonAnimation = new ThicknessAnimation();
            buttonAnimation.From = new Thickness(0, 0, 0, 10);
            buttonAnimation.To = new Thickness(0);
            buttonAnimation.SpeedRatio = 4;
            img1.BeginAnimation(Button.MarginProperty, buttonAnimation);
        }

        private void img3_MouseEnter(object sender, MouseEventArgs e)
        {
            ThicknessAnimation buttonAnimation = new ThicknessAnimation();
            buttonAnimation.From = new Thickness(0);
            buttonAnimation.To = new Thickness(0, 0, 0, 10);
            buttonAnimation.SpeedRatio = 4;
            img3.BeginAnimation(Button.MarginProperty, buttonAnimation);
        }

        private void img3_MouseLeave(object sender, MouseEventArgs e)
        {

            ThicknessAnimation buttonAnimation = new ThicknessAnimation();
            buttonAnimation.From = new Thickness(0, 0, 0, 10);
            buttonAnimation.To = new Thickness(0);
            buttonAnimation.SpeedRatio = 4;
            img3.BeginAnimation(Button.MarginProperty, buttonAnimation);
        }
        void close_border()
        {
            border.Visibility = Visibility.Visible;
            DoubleAnimation buttonAnimation = new DoubleAnimation();
            buttonAnimation.From = 1;
            buttonAnimation.To = 0;
            buttonAnimation.SpeedRatio = 4;
            buttonAnimation.Duration = TimeSpan.FromSeconds(1);
            buttonAnimation.Completed += ButtonAnimation_Completed;
            border.BeginAnimation(Button.OpacityProperty, buttonAnimation);
        }
        void set_img(String img)
        {
            var brush = new ImageBrush();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(img, UriKind.Relative);
            brush.ImageSource = bi;
            image.Background = brush;
            bi.EndInit();
        }

        private void img1_Click(object sender, RoutedEventArgs e)
        {
            close_border();
            filename = "profil3.jpg";
            path = CorrectPath() + "\\image\\" + filename;
            set_img(path);///!!!!!!!!!!!!
            
        }

        private void img2_Click(object sender, RoutedEventArgs e)
        {
            close_border();
            filename = "profil.jpg";
            path = CorrectPath() + "\\image\\" + filename;
            set_img(path);//!!!!!!!!!!!!!!!!!
            
        }
        string path = "";
        public string CorrectPath()
        {
            path = Directory.GetCurrentDirectory();
            int id = path.IndexOf("bin");
            path = path.Remove(id, 9);
            return path;
        }
        private void img3_Click(object sender, RoutedEventArgs e)
        {
            close_border();
            filename = "profil4.jpg";
            path = CorrectPath() + "\\image\\" + filename;
            set_img(path);//!!!!!
           
        }

        private void Login_text_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Login_text.Text == "")
            {
                Login_text.FontSize = 20;
                Login_text.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(156, 156, 156));
                if (flag == true)
                {
                    Login_text.Text = "имя";
                }
                else
                {
                    Login_text.Text = "логин";
                }
            }
        }

        private void password_text_LostFocus(object sender, RoutedEventArgs e)
        {
            if (password_text.Text == "")
            {
                password_text.FontSize = 20;
                password_text.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(156, 156, 156));
                password_text.Text = "пароль";
            }
        }
    }
}
