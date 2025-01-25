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
    /// Логика взаимодействия для UserAddPage.xaml
    /// </summary>
    public partial class UserAddPage : Page
    {
        Users CurrentUser;
        bool EndedClicked = false;
        private Users currentUser;

        public UserAddPage(Users user)
        {
            CurrentUser = user;
            InitializeComponent();
            var ads = Entities.GetContext().Ads.ToList();
            if (user != null)
            {
                ads = ads.Where(x => x.UserID == user.UserID).ToList();
            }

            ListUser.ItemsSource = ads;
            ListUser.MouseDoubleClick += lviDoubleClick;

        }

        private void btnAddAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditAddPage(CurrentUser));
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage(CurrentUser));
        }

        public void lviDoubleClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditAddPage(CurrentUser, (Ads)ListUser.SelectedValue));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.GetContext().Ads.Remove((Ads)ListUser.SelectedValue);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Объявление успешно удалено");
                NavigationService.Navigate(new UserAddPage(CurrentUser));
            }
            catch
            {
                MessageBox.Show("Произошла ошибка удаления");
            }
        }

        public double CountProfit(Users Owner)
        {
            double res = (double)Entities.GetContext().Ads.ToList().Where(x => x.StatusID == 2 && x.UserID == Owner.UserID).ToList().Select(x => x.Price).Sum();
            return res;
        }

        private void btnEnded_Click(object sender, RoutedEventArgs e)
        {
            if (!EndedClicked)
            {
                var ads = Entities.GetContext().Ads.ToList();
                ads = ads.Where(x => x.UserID == CurrentUser.UserID && x.StatusID == 2).ToList();
                ListUser.ItemsSource = ads;
                lblProfit.Content = $"Общая полученная прибыль: {CountProfit(CurrentUser)}";
            }
            else
            {
                ListUser.ItemsSource = Entities.GetContext().Ads.ToList();
                lblProfit.Content = string.Empty;
            }
        }
    }
}
