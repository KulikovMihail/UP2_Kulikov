using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
    
    public partial class EditAddPage : Page
    {
        Users CurrentUser;
        Ads CurrentAdd = null;

        public EditAddPage(Users User, Ads Add = null)
        {
            InitializeComponent();
            CurrentUser = User;
            if (Add == null)
            {
                btnSave.Content = "Создать";
                btnDelete.Visibility = Visibility.Hidden;
            }
            else
            {
                CurrentAdd = Add;
                btnDelete.Visibility = Visibility.Visible;
                btnSave.Content = "Обновить";

                cbStatus.SelectedValue = CurrentAdd.AdStatuses;
                cbCity.SelectedValue = CurrentAdd.Cities;
                cbCategory.SelectedValue = CurrentAdd.Categories;
                cbType.SelectedValue = CurrentAdd.Types;
                tbName.Text = CurrentAdd.Title;
                tbDescription.Text = CurrentAdd.Description;
                tbPrice.Text = CurrentAdd.Price.ToString();
                tbPhoto.Text = CurrentAdd.Photo;
            }

            cbCategory.ItemsSource = Entities.GetContext().Categories.ToList() ;
            cbCity.ItemsSource = Entities.GetContext().Cities.ToList();
            cbType.ItemsSource = Entities.GetContext().Types.ToList();
            cbStatus.ItemsSource = Entities.GetContext().AdStatuses.ToList();
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {


            try
            {
                Entities.GetContext().Ads.Remove(CurrentAdd);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Объявление успешно удалено");
                NavigationService.Navigate(new UserAddPage(CurrentUser));
            }
            catch
            {
                MessageBox.Show("Произошла ошибка удаления");
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (
                cbCity.SelectedIndex == -1 &&
                cbCategory.SelectedIndex == -1 &&
                cbType.SelectedIndex == -1 &&
                string.IsNullOrEmpty(tbName.Text) &&
                string.IsNullOrEmpty(tbDescription.Text) &&
                string.IsNullOrEmpty(tbPrice.Text)
                )
            {
                MessageBox.Show("Не все поля заполнены");

                return;
            }
            if (
                !decimal.TryParse(tbPrice.Text, out _)
                )
            {
                MessageBox.Show("Цена должна быть указана числом!!!");
                return;
            }

            if (CurrentAdd == null) CurrentAdd = new Ads();

            CurrentAdd.PublicationDate = DateTime.Now;
            CurrentAdd.UserID = CurrentUser.UserID;
            CurrentAdd.CityID = ((Cities)cbCity.SelectedValue).CityID;
            CurrentAdd.CategoryID = ((Categories)cbCategory.SelectedValue).CategoryID;
            CurrentAdd.TypeID = ((Types)cbType.SelectedValue).TypeID;
            CurrentAdd.StatusID = ((AdStatuses)cbStatus.SelectedValue).StatusID;
            CurrentAdd.Title = tbName.Text;
            CurrentAdd.Description = tbDescription.Text;
            CurrentAdd.Price = int.Parse(tbPrice.Text);
            CurrentAdd.Photo = tbPhoto.Text == "" ? null : tbPhoto.Text;

            try
            {
                Entities.GetContext().Ads.AddOrUpdate(CurrentAdd);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Объявление успешно добавлено/обновлено");
                NavigationService.Navigate(new UserAddPage(CurrentUser));
            }
            catch
            {
                MessageBox.Show("Произошла ошибка добавления/обновления");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserAddPage(CurrentUser));
        }

        private void cbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CurrentAdd != null && CurrentAdd.StatusID == 1 && ((AdStatuses)cbStatus.SelectedValue).StatusID == 2)
            {
                MessageBox.Show("Введите в поле \"Цена\" полученную сумму");
            }

        }

    }
}
