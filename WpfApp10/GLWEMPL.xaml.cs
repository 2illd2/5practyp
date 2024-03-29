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
using System.Windows.Shapes;

namespace WpfApp10
{
    /// <summary>
    /// Логика взаимодействия для GLWEMPL.xaml
    /// </summary>
    public partial class GLWEMPL : Window
    {
        public GLWEMPL()
        {
            InitializeComponent();
        }
        private void OpenCustomersOrdersWindow(object sender, RoutedEventArgs e)
        {
            var window = new CustomersW();
            window.Owner = this;
            window.ShowDialog();
        }
        private void OpenOrdersWindow(object sender, RoutedEventArgs e)
        {
            var window = new OrdersW();
            window.Owner = this;
            window.ShowDialog();
        }

        private void OpenReceiptsWindow(object sender, RoutedEventArgs e)
        {
            var window = new ReceiptsWindow();
            window.Owner = this;
            window.ShowDialog();
        }

        private void OpenProductsWindow(object sender, RoutedEventArgs e)
        {
            var window = new ProductW();
            window.Owner = this;
            window.ShowDialog();
        }

        private void OpenCategoriesWindow(object sender, RoutedEventArgs e)
        {
            var window = new CategoriesWindow();
            window.Owner = this;
            window.ShowDialog();
        }

        // Добавьте методы для остальных окон здесь

        // Методы для открытия остальных окон

        private void OpenProductOrdersWindow(object sender, RoutedEventArgs e)
        {
            var window = new ProductOrdersWindow();
            window.Owner = this;
            window.ShowDialog();
        }

        private void OpenShippingAddressesWindow(object sender, RoutedEventArgs e)
        {
            var window = new ShippingAddressesWindow();
            window.Owner = this;
            window.ShowDialog();
        }

        private void OpenPaymentsWindow(object sender, RoutedEventArgs e)
        {
            var window = new PaymentsWindow();
            window.Owner = this;
            window.ShowDialog();
        }

        private void OpenReviewsWindow(object sender, RoutedEventArgs e)
        {
            var window = new ReviewsWindow();
            window.Owner = this;
            window.ShowDialog();
        }

        private void OpenEmployeesWindow(object sender, RoutedEventArgs e)
        {
            var window = new EmployeesWindow();
            window.Owner = this;
            window.ShowDialog();
        }

        private void OpenEmployeeOrdersWindow(object sender, RoutedEventArgs e)
        {
            var window = new EmployeeOrdersWindow();
            window.Owner = this;
            window.ShowDialog();
        }

        private void OpenIngredientsWindow(object sender, RoutedEventArgs e)
        {
            var window = new IngredientsWindow();
            window.Owner = this;
            window.ShowDialog();
        }

        private void OpenProductIngredientsWindow(object sender, RoutedEventArgs e)
        {
            var window = new ProductIngredientsWindow();
            window.Owner = this;
            window.ShowDialog();
        }
    }
}
