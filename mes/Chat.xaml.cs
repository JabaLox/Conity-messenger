using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media.Animation;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Input;
using System.Windows.Shapes;
using Microsoft.Office.Interop.Word;
using Word = Microsoft.Office.Interop.Word;

namespace mes
{
    /// <summary>
    /// Логика взаимодействия для Chat.xaml
    /// </summary>
    public partial class Chat :System.Windows.Window
    {
        MySqlConnection con;
        string Connect = "server=localhost;port=3306;user=root;database=conity_messenger;password=kolyan28;";
        //string Connect = "server=87.236.19.248;port=3306;user=ramzilyo_conity;database=ramzilyo_conity;password=(Ramzil11);";
        string query;

        public Chat()
        {
            InitializeComponent();
            query = $"select distinct id_user, Name, login, password, photo from dialog join users on (sender_id = id_user or recipient_id=id_user) where id_dialog in (select id_dialog from dialog where (recipient_id<>'{Global.UsreId}' and sender_id='{Global.UsreId}') or (recipient_id='{Global.UsreId}' and sender_id<>'{Global.UsreId}')) and id_user<>'{Global.UsreId}'; ";
            UserList(query);
            border.Visibility = Visibility.Collapsed;
            refrash();
            //MessageBox.Show(path);

            // RunPeriodicSave();

            //con = new MySqlConnection(Connect);
            //DataTable dt = new DataTable();
            //MySqlDataAdapter adapter = new MySqlDataAdapter("select id_message from message", con);
            //con.Open();
            //adapter.Fill(dt);
            //id_mess = (dt.Rows.Count + 1).ToString();
            //con.Close();


        }
        string path = "";
        string filname=Global.Avatar;
        List<User> users = new List<User>();
        List<Message> messages = new List<Message>();
        bool flag_setings = false;
        int size_dialog = 0;
        async System.Threading.Tasks.Task RunPeriodicSave()
        {
            while (true)
            {
                //await Task.Delay(5000);
                await System.Threading.Tasks.Task.Delay(100);
                if (id_dialog != "")
                {
                    
                   // con = new MySqlConnection(Connect);
                    //DataTable dt = new DataTable();
                   // MySqlDataAdapter adapter = new MySqlDataAdapter($"select id_dialog from dialog where (sender_id={Convert.ToInt32(id_rec)} and recipient_id={Convert.ToInt32(Global.UsreId)}) or (recipient_id={Convert.ToInt32(id_rec)} and sender_id={Convert.ToInt32(Global.UsreId)})", con);
                   // con.Open();
                    //adapter.Fill(dt);
                   // if (dt.Rows.Count > size_dialog)
                   // {
                       // messagesList();
                   // }
                    //con.Close();

                }
                UserList(query);
            }
        }
        void refrash()
        {
            path = Directory.GetCurrentDirectory();
            int id = path.IndexOf("bin");
            path = path.Remove(id, 9);
            nick_user.Content = Global.Name;
            name_settings.Text = Global.Name;
            password_settings.Text = Global.Paswword;
            avatar_user.ImageSource = new BitmapImage(new Uri(path+ "image\\" + Global.Avatar, UriKind.Relative));
        }
        //string id_mess = "";
        public void UserList(string query)
        {

            users.Clear();
            con = new MySqlConnection(Connect);
           System.Data.DataTable dt = new System.Data.DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
            con.Open();
            adapter.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                string count = "";
                string last = "";
                string time = "";
                string color = "";
                System.Data.DataTable dm = new System.Data.DataTable();
                MySqlDataAdapter adapterm = new MySqlDataAdapter($"select id_dialog from dialog where (sender_id={dt.Rows[i][0]} and recipient_id={Convert.ToInt32(Global.UsreId)}) or (recipient_id={dt.Rows[i][0]} and sender_id={Convert.ToInt32(Global.UsreId)})", con);

                adapterm.Fill(dm);
                //MessageBox.Show(dt.Rows.Count.ToString());
                if (dm.Rows.Count > 0)
                {
                    System.Data.DataTable dt2 = new System.Data.DataTable();
                    MySqlDataAdapter adapter2 = new MySqlDataAdapter($"SELECT content,`read`,time(`time`),author_id from message where id_dialog={Convert.ToInt32(dm.Rows[0][0])} and id_message=(select max(id_message) from message where id_dialog={Convert.ToInt32(dm.Rows[0][0])})", con);

                    adapter2.Fill(dt2);

                    System.Data.DataTable dt3 = new System.Data.DataTable();
                    MySqlDataAdapter adapter3 = new MySqlDataAdapter($"SELECT `read`,author_id from message where id_dialog={Convert.ToInt32(dm.Rows[0][0])} and `read`=0 and author_id<>{Global.UsreId}" , con);

                    adapter3.Fill(dt3);

                    if (dt2.Rows.Count > 0 && dt2.Rows[0][3].ToString() == Global.UsreId)
                    {
                        last = "Вы: " + dt2.Rows[0][0].ToString();
                      
                            if (Convert.ToByte(dt2.Rows[0][1]) == 0)
                            {
                                color = "";
                                count = "•";
                            }
                       
                        
                       
                        time = dt2.Rows[0][2].ToString().Remove(5, 3);
                    }
                    else if (dt2.Rows.Count > 0 && dt2.Rows[0][3].ToString() != Global.UsreId)
                    {
                        last = dt2.Rows[0][0].ToString();
                        if (dt3.Rows.Count != 0)
                        {

                            count = dt3.Rows.Count.ToString();
                            color = "Aqua";
                        }
                        
                        time = dt2.Rows[0][2].ToString().Remove(5, 3);
                    }

                }
                string ima = "";
                if (dt.Rows[i][4].ToString() == "")
                {
                     ima= "image\\picture.png";
                }
                else
                {
                    ima="image\\"+dt.Rows[i][4].ToString();
                }
                users.Add(new User()
                {
                    Name_user = dt.Rows[i][1].ToString(),
                    login_user = dt.Rows[i][2].ToString(),
                    photo = ima,
                    id_user = dt.Rows[i][0].ToString(),
                    password = dt.Rows[i][3].ToString(),
                    last_message = last,
                    last_message_time = time,
                    NoRead = count,
                    color = color,
                });

            }
            con.Close();
            listUsers.ItemsSource = users;
            ICollectionView view = CollectionViewSource.GetDefaultView(users);
            view.Refresh();
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("dfdf");
        }
        double PrecentUser = 0;
        double PrecentRecipient = 0;
        int CountMessageUser = 0;
        int CountMessageRecipient = 0;

        public void messagesList()
        {
            int index = 0;
            messages.Clear();
            con = new MySqlConnection(Connect);
            System.Data.DataTable dt = new System.Data.DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter($"select id_message, content, author_id, `read`, time(`time`), Day(`time`), MONTHNAME(`time`), year(`time`), `time` from message where id_dialog={Convert.ToInt32(id_dialog)}", con);
            con.Open();
            adapter.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                size_dialog = dt.Rows.Count;
                messages.Add(new Message()
                {
                    date = dt.Rows[0][5].ToString() + " " + dt.Rows[0][6].ToString(),
                });
                //DateTime date_mess1 = Convert.ToDateTime(dt.Rows[0][8]);
                //MessageBox.Show(messages[index].date);
                for (int i = 0; i < dt.Rows.Count; ++i)
                {

                    if (messages[index].date != null)
                    {
                        if (messages[index].date != (dt.Rows[i][5].ToString() + " " + dt.Rows[i][6].ToString()))
                        {
                            messages.Add(new Message()
                            {
                                date = dt.Rows[i][5].ToString() + " " + dt.Rows[i][6].ToString(),
                            });
                            index = i + 1;
                        }
                    }
                    string ms_content_rec = "";
                    string ti_rec = "";
                    string ti_se = dt.Rows[i][4].ToString().Remove(5, 3);
                    string read_se = "";
                    if (dt.Rows[i][2].ToString() != Global.UsreId)
                    {
                        ms_content_rec = dt.Rows[i][1].ToString();
                        dt.Rows[i][1] = "";
                        ti_rec = dt.Rows[i][4].ToString().Remove(5, 3);
                        ti_se = "";
                        CountMessageRecipient++;
                    }
                    else
                    {
                        CountMessageUser++;
                    }
                    if (Convert.ToByte(dt.Rows[i][3]) == 0 && dt.Rows[i][2].ToString()==Global.UsreId)
                    {
                        read_se = "Aquamarine";
                    }
                    messages.Add(new Message()
                    {
                        id_message = dt.Rows[i][0].ToString(),
                        Message_content_send = dt.Rows[i][1].ToString(),
                        Message_content_rec = ms_content_rec,
                        id_auth = dt.Rows[i][2].ToString(),
                        read = dt.Rows[i][3].ToString(),
                        time_send = ti_se,
                        time_rec = ti_rec,
                        status_read = read_se,
                    });
                }
                MessageList.ItemsSource = messages;
                ICollectionView view = CollectionViewSource.GetDefaultView(messages);
                view.Refresh();
            }
            con.Close();
        }
       
        string id_dialog = "";
        string id_rec = "";
        private void listUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var bc = new BrushConverter();
                text_sender.Foreground = (Brush)bc.ConvertFrom("#FF777777");
                size_dialog = 0;
                PrecentUser = 0;
                PrecentRecipient = 0;
                CountMessageUser = 0;
                CountMessageRecipient = 0;
                id_dialog = "";
                mes_top.Opacity = 1;
                mes_bottom.Opacity = 1;
                con = new MySqlConnection(Connect);
               System.Data.DataTable dt = new System.Data.DataTable();
                var item = (sender as ListView).SelectedItem;
                if (item != null)
                {
                    id_rec = ((User)listUsers.SelectedItem).id_user;
                    //MessageBox.Show(path);

                    avatar_rec.ImageSource = new BitmapImage(new Uri(path + ((User)listUsers.SelectedItem).photo, UriKind.Relative));
                    nick_recipient.Content = ((User)listUsers.SelectedItem).Name_user;
                }
                MySqlDataAdapter adapter = new MySqlDataAdapter($"select dialog.id_dialog, author_id from dialog join message where (sender_id={Convert.ToInt32(id_rec)} and recipient_id={Convert.ToInt32(Global.UsreId)}) or (recipient_id={Convert.ToInt32(id_rec)} and sender_id={Convert.ToInt32(Global.UsreId)})", con);
                con.Open();
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    size_dialog = dt.Rows.Count;
                    id_dialog = dt.Rows[0][0].ToString();
                     MySqlDataAdapter adapterm = new MySqlDataAdapter($"UPDATE message SET `read` = 1 WHERE (id_dialog = {id_dialog} and author_id<>{Global.UsreId} and `read`=0);", con);
                     adapterm.Fill(dt);
                    messagesList();

                    UserList(query);

                }
                else
                {
                    messages.Clear();
                    ICollectionView view = CollectionViewSource.GetDefaultView(messages);
                    view.Refresh();
                    plug.Visibility = Visibility;
                }
                con.Close();
                ICollectionView view2 = CollectionViewSource.GetDefaultView(messages);
                view2.Refresh();
            name_search.Text = "Поиск";
        }

            public void SendMessageInDB()
        {
            con = new MySqlConnection(Connect);
            System.Data.DataTable dt = new System.Data.DataTable();
            // MessageBox.Show(((User)listUsers.SelectedItem).id_user+" "+ Convert.ToInt32(Global.UsreId));
            MySqlDataAdapter adapter = new MySqlDataAdapter($"select id_dialog from dialog where (sender_id={id_rec} and recipient_id={Convert.ToInt32(Global.UsreId)}) or (recipient_id={id_rec} and sender_id={Convert.ToInt32(Global.UsreId)})", con);
            con.Open();
            adapter.Fill(dt);
            //MessageBox.Show(dt.Rows.Count.ToString());
            if (dt.Rows.Count > 0)
            {
                id_dialog = dt.Rows[0][0].ToString();
            }
            else
            {
                id_dialog = (dt.Rows.Count + 1).ToString();
               System.Data.DataTable dt2 = new System.Data.DataTable();
                MySqlDataAdapter adapter2 = new MySqlDataAdapter($"insert into dialog (sender_id,recipient_id) values ({Convert.ToInt32(Global.UsreId)},{Convert.ToInt32(id_rec)})", con);
                adapter2.Fill(dt2);

                System.Data.DataTable dt3 = new System.Data.DataTable();
                MySqlDataAdapter adapter3 = new MySqlDataAdapter($" select id_dialog from dialog where (sender_id = {Convert.ToInt32(Global.UsreId)} and recipient_id={Convert.ToInt32(id_rec)}) or (recipient_id = {Convert.ToInt32(Global.UsreId)} and sender_id={Convert.ToInt32(id_rec)})", con);
                adapter3.Fill(dt3);
                id_dialog = dt3.Rows[0][0].ToString();
            }
            DateTime date = DateTime.UtcNow;
            string time = date.Year.ToString() + "-" + date.Month.ToString() + "-" + date.Day.ToString() + " " + date.ToLocalTime().ToShortTimeString();
            //MessageBox.Show(time);
            plug.Opacity = 0;
            System.Data.DataTable dm = new System.Data.DataTable();
            MySqlDataAdapter adapterm = new MySqlDataAdapter($"insert into message (id_dialog,content,author_id,`read`,time) value ({Convert.ToInt32(id_dialog)},'{text_sender.Text}',{Global.UsreId},0,'{time}')", con);
            adapterm.Fill(dm);
            con.Close();
            messagesList();
            UserList(query);
            text_sender.Text = "";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (text_sender.Text != "")
            {
                SendMessageInDB();
            }
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    DoubleAnimation buttonAnimation = new DoubleAnimation();
        //    buttonAnimation.From = helloButton.ActualWidth;
        //    buttonAnimation.To = 150;
        //    buttonAnimation.Duration = TimeSpan.FromSeconds(3);
        //    helloButton.BeginAnimation(Button.WidthProperty, buttonAnimation);
        //}

        private void Window_Activated(object sender, EventArgs e)
        {
            ////MessageBox.Show("ds");
            //if (id_dialog != "")
            //{
            //    messagesList();
            //}
            //UserList(query);
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            //if (id_dialog != "")
            //{
            //    messagesList();
            //}
            //UserList(query);
            
        }

        private bool IsUserVisible(FrameworkElement element, FrameworkElement container)
        {
            if (!element.IsVisible)
                return false;
            Rect bounds = element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
            Rect rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
            return rect.Contains(bounds.TopLeft) || rect.Contains(bounds.BottomRight); 
        }
        private void MessageList_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            IsUserVisible(MessageList, this);
        }
        public void ExitDataWord()
        {
            PrecentRecipient = CountMessageRecipient * 100 / size_dialog;
            PrecentUser = CountMessageUser * 100 / size_dialog;
            Word.Application app = new Word.Application();
            Word.Document doc = app.Documents.Add();
            doc.Paragraphs[1].Range.Text = $"Кол-во сообщений в диалоге: " + size_dialog.ToString() +
                "\n" + "Кол-во в дилоге процент сообщений от " + Global.Name + " является: " + PrecentUser.ToString() + "%" + " кол-во сообщений " + CountMessageUser +
                "\n" + "Кол-во в дилоге процент сообщений от " + nick_recipient.Content + " является: " + PrecentRecipient.ToString() + "%" + " кол-во сообщений " + CountMessageRecipient;
            app.Visible = true;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (name_search.Text != "" && name_search.Text!="Поиск" )
            {
                query = $"select distinct id_user, `Name`, login, `password`, photo from users where id_user<>{Global.UsreId} and `Name` like '%{name_search.Text}%';";
                UserList(query);
            }
            else
            {
                query = $"select distinct id_user, Name, login, password, photo from dialog join users on (sender_id = id_user or recipient_id=id_user) where id_dialog in (select id_dialog from dialog where (recipient_id<>{Global.UsreId} and sender_id={Global.UsreId}) or (recipient_id={Global.UsreId} and sender_id<>{Global.UsreId})) and id_user<>{Global.UsreId}";
                UserList(query);
            }
        }

        private void name_search_GotFocus(object sender, RoutedEventArgs e)
        {
            name_search.Text = "";
            name_search.Foreground = Brushes.Black;
        }

        private void name_search_LostFocus(object sender, RoutedEventArgs e)
        {
            
            name_search.Foreground = Brushes.DarkSlateBlue;
        }

        private void ButtonAnimation_Completed_opas(object sender, EventArgs e)
        {
            DoubleAnimation buttonAnimation = new DoubleAnimation();
            buttonAnimation.From = 1200;
            buttonAnimation.To = 0;
            buttonAnimation.Duration = TimeSpan.FromSeconds(0.8);
            buttonAnimation.SpeedRatio = 50;
            settings.BeginAnimation(Button.WidthProperty, buttonAnimation);
        }
        private void ButtonAnimation_Completed(object sender, EventArgs e)
        {
            DoubleAnimation buttonAnimation = new DoubleAnimation();
            buttonAnimation.From = 0;
            buttonAnimation.To = 0.5;
            buttonAnimation.Duration = TimeSpan.FromSeconds(1);
            buttonAnimation.SpeedRatio = 9;
            test.BeginAnimation(Button.OpacityProperty, buttonAnimation);

        }
        private void ButtonAnimation_Completed2(object sender, EventArgs e)
        {
            border.Visibility = Visibility.Collapsed;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            set_img(Global.Avatar);
                DoubleAnimation buttonAnimation = new DoubleAnimation();
                buttonAnimation.From = 0;
                buttonAnimation.To = 1200;
                buttonAnimation.Duration = TimeSpan.FromSeconds(0.8);
                buttonAnimation.SpeedRatio = 5;
            buttonAnimation.Completed += ButtonAnimation_Completed;
            settings.BeginAnimation(Button.WidthProperty, buttonAnimation);
        }

        private void test_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            DoubleAnimation buttonAnimation = new DoubleAnimation();
            buttonAnimation.From = 0.5;
            buttonAnimation.To = 0;
            buttonAnimation.Duration = TimeSpan.FromSeconds(1);
            buttonAnimation.SpeedRatio = 9;
            buttonAnimation.Completed += ButtonAnimation_Completed_opas;
            test.BeginAnimation(Button.OpacityProperty, buttonAnimation);
        }

        private void settings_photo_Click(object sender, RoutedEventArgs e)
        {
            border.Visibility = Visibility.Visible;
            border.Opacity = 0;
            DoubleAnimation buttonAnimation = new DoubleAnimation();
            buttonAnimation.From = 0;
            buttonAnimation.To = 1;
            buttonAnimation.SpeedRatio = 4;
            border.BeginAnimation(Button.OpacityProperty, buttonAnimation);
        }

        private void img2_MouseEnter(object sender, MouseEventArgs e)
        {
            img2.BeginAnimation(Button.MarginProperty, enter());
        }
        private void img2_MouseLeave(object sender, MouseEventArgs e)
        {
            img2.BeginAnimation(Button.MarginProperty, leave());
        }
        private void img1_MouseEnter(object sender, MouseEventArgs e)
        {
            img1.BeginAnimation(Button.MarginProperty, enter());
        }
        private void img1_MouseLeave(object sender, MouseEventArgs e)
        {
            img1.BeginAnimation(Button.MarginProperty, leave());
        }
        private void img3_MouseEnter(object sender, MouseEventArgs e)
        {
            img3.BeginAnimation(Button.MarginProperty, enter());
        }
        private void img3_MouseLeave(object sender, MouseEventArgs e)
        {
            img3.BeginAnimation(Button.MarginProperty, leave());
        }
        private ThicknessAnimation leave()
        {
            ThicknessAnimation buttonAnimation = new ThicknessAnimation();
            buttonAnimation.From = new Thickness(0, 0, 0, 10);
            buttonAnimation.To = new Thickness(0);
            buttonAnimation.SpeedRatio = 4;
            return buttonAnimation;
        }
        private ThicknessAnimation enter()
        {
            ThicknessAnimation buttonAnimation = new ThicknessAnimation();
            buttonAnimation.From = new Thickness(0);
            buttonAnimation.To = new Thickness(0, 0, 0, 10);
            buttonAnimation.SpeedRatio = 4;
            return buttonAnimation;
        }
        void close_border()
        {
            border.Visibility = Visibility.Visible;
            DoubleAnimation buttonAnimation = new DoubleAnimation();
            buttonAnimation.From = 1;
            buttonAnimation.To = 0;
            buttonAnimation.SpeedRatio = 4;
            buttonAnimation.Duration = TimeSpan.FromSeconds(1);
            buttonAnimation.Completed += ButtonAnimation_Completed2;
            border.BeginAnimation(Button.OpacityProperty, buttonAnimation);
        }
        void set_img(String img)
        {
            path = Directory.GetCurrentDirectory();
            int id = path.IndexOf("bin");
            path = path.Remove(id, 9);
            var brush = new ImageBrush();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(path+ "\\image\\" + img, UriKind.Relative);
            brush.ImageSource = bi;
            settings_photo.Background = brush;
            bi.EndInit();
        }

        private void img1_Click(object sender, RoutedEventArgs e)
        {
            filname = "profil3.jpg";
            close_border();
            set_img(filname);
        }

        private void img2_Click(object sender, RoutedEventArgs e)
        {
            filname = "profil.jpg";
            close_border();
            set_img(filname);
        }
        private void img3_Click(object sender, RoutedEventArgs e)
        {
            close_border();
            filname = "profil4.jpg";
            set_img(filname);
        }

        private void save_settings_Click(object sender, RoutedEventArgs e)
        {
            con = new MySqlConnection(Connect);
            con.Open();
            MySqlCommand command = new MySqlCommand($"UPDATE users SET photo = '{filname}',Name = '{name_settings.Text}',password = '{password_settings.Text}'  where id_user= '{Global.UsreId}'", con);
            command.ExecuteNonQuery();
            con.Close();
            Global.Name = name_settings.Text;
            Global.Paswword = password_settings.Text;
            Global.Avatar = filname;
            refrash();
            MessageBox.Show("Успешно сохранено!");
        }

        private void text_sender_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && text_sender.Text != "")
            {
                SendMessageInDB();
            }
        }

        private void text_sender_GotFocus(object sender, RoutedEventArgs e)
        {
            if(text_sender.Text== "Написать сообщение...")
            {
                text_sender.Text = "";
                text_sender.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void text_sender_LostFocus(object sender, RoutedEventArgs e)
        {
            if (text_sender.Text == "")
            {
                text_sender.Text = "Написать сообщение...";
                var bc = new BrushConverter();
                text_sender.Foreground = (Brush)bc.ConvertFrom("#FF777777");
            }
        }

        private void mes_img_Click(object sender, RoutedEventArgs e)
        {
            ExitDataWord();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
    public class User
    {
        public string Name_user { get; set; }
        public string id_user { get; set; }
        public string login_user { get; set; }
        public string photo { get; set; }
        public string password { get; set; }
        public string color { get; set; }
        public string last_message { get; set; }
        public string last_message_time { get; set; }
        public string NoRead { get; set; }
    }
    public class Message
    {
        public string Message_content_send { get; set; }
        public string Message_content_rec { get; set; }
        public string time_send { get; set; }
        public string time_rec { get; set; }
        public string read { get; set; }
        public string id_auth { get; set; }
        public string id_dialog { get; set; }
        public string id_message { get; set; }
        public string date { get; set; }
        public string status_read { get; set; }

    }
}
