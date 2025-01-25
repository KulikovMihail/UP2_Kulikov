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
using System.Data.Entity;



namespace UP02
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private List<Ads> _ads;
        private List<Cities> _cities;
        private List<Categories> _categories;
        private List<Types> _adTypes;
        private List<AdStatuses> _adStatus;
        private List<Users> _users;

        Users CurrentUser = null; 

        public MainPage(Users user = null)
        {
            InitializeComponent();
            _ads = Entities.GetContext().Ads.ToList();
            _cities = Entities.GetContext().Cities.ToList();
            _categories = Entities.GetContext().Categories.ToList();
            _adTypes = Entities.GetContext().Types.ToList();
            _adStatus = Entities.GetContext().AdStatuses.ToList();
            _users=Entities.GetContext().Users.ToList();

            
            CityFilter.ItemsSource = _cities;
            CityFilter.DisplayMemberPath = "CityName";

            TypeFilter.ItemsSource = _adTypes;
            TypeFilter.DisplayMemberPath = "TypeName";

            CategoryFilter.ItemsSource = _categories;
            CategoryFilter.DisplayMemberPath = "CategoryName";

            StatusFilter.ItemsSource = _adStatus;
            StatusFilter.DisplayMemberPath = "StatusName";

            UpdateList();

            
            if (user != null)
            {
                CurrentUser = user;
                profile.Text = user.Login;
                btnMyAdds.IsEnabled = true;
            }

          

        }

        private void UpdateList()
        {
            var filteredAds = _ads;

            
            if (CityFilter.SelectedItem is Cities selectedCity)
                filteredAds = filteredAds.Where(ad => ad.Cities.CityID == selectedCity.CityID).ToList();

            
            if (TypeFilter.SelectedItem is Types selectedType)
                filteredAds = filteredAds.Where(ad => ad.Types.TypeID == selectedType.TypeID).ToList();

            
            if (CategoryFilter.SelectedItem is Categories selectedCategory)
                filteredAds = filteredAds.Where(ad => ad.Categories.CategoryID == selectedCategory.CategoryID).ToList();

            
            if (!string.IsNullOrWhiteSpace(TitleSearch.Text))
                filteredAds = filteredAds.Where(ad => ad.Title != null && ad.Title.ToLower().Contains(TitleSearch.Text.ToLower())).ToList();

            if (StatusFilter.SelectedItem is AdStatuses SelectedStatus)
                filteredAds = filteredAds.Where(ad => ad.AdStatuses.StatusID == SelectedStatus.StatusID).ToList();

            ListUser.ItemsSource = filteredAds;
        }

        private void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateList();
        }

        private void TitleSearch_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            UpdateList();
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            CityFilter.SelectedItem = null;
            TypeFilter.SelectedItem = null;
            CategoryFilter.SelectedItem = null;
            StatusFilter.SelectedItem = null;
            TitleSearch.Text = string.Empty;
            UpdateList();
        }

        private void TypeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateList();
        }

        private void CategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateList();
        }

        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateList();
        }
        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());
        }

        private void btnMyAdds_Click(object sender, RoutedEventArgs e)
        {
                NavigationService.Navigate(new UserAddPage(CurrentUser));
        }
    }
}
