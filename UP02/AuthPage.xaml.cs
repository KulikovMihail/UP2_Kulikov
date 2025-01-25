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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UP02
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {


        public AuthPage()
        {
            InitializeComponent();
        }

        public Users GetUser(string Login, string Password)
        {
            try
            {
                Users user = Entities.GetContext().Users.ToList().Where(x => x.Login == Login && x.Password == Password).ToList().First();
                return user;
            }
            catch
            {
                return null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (
                string.IsNullOrEmpty(tbLogin.Text) ||
                string.IsNullOrEmpty(pbPassword.Password)
                )
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }
            else
            {
                Users user = GetUser(tbLogin.Text, pbPassword.Password);
                if (user == null)
                {
                    MessageBox.Show("Пользователь с таким логином и паролем не найден");
                    pbPassword.Password = string.Empty;
                    return;
                }
                else
                {
                    NavigationService.Navigate(new MainPage((Users)user));
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
    }
}
