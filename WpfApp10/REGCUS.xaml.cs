using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Security.Cryptography;
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
using WpfApp10.MagAsiaDataSetTableAdapters;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace WpfApp10
{
   
    public partial class REGCUS : Window
    {
        private MagAsiaDataSet dataset = new MagAsiaDataSet();
        public ObservableCollection<MagAsiaDataSet.CustomersRow> CustomersData { get; set; }
        private CustomersTableAdapter customersTableAdapter = new CustomersTableAdapter();
        public REGCUS()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                customersTableAdapter.Fill(dataset.Customers);
                CustomersData = new ObservableCollection<MagAsiaDataSet.CustomersRow>(dataset.Customers);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private bool CheckCustomerLogin(string login, string password)
        {
            var query = dataset.Customers
                              .Where(customer => customer.loginC == login && customer.PasswordC == password)
                              .ToList();

            return query.Count == 1;
        }
        private void EmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            REGCUS2 rEGCUS2 = new REGCUS2();
            rEGCUS2.Show();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;

            // Хэшируем введенный пользователем пароль
            string hashedPassword = PasswordHasher.HashPassword(password);

            if (CheckCustomerLogin(login, hashedPassword))
            {
                int customerId = GetCustomerId(login, hashedPassword);
                CustomerMainWindow customerWindow = new CustomerMainWindow(customerId);
                customerWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
            
        }
        
        private int GetCustomerId(string login, string password)
        {
            // Получение ID текущего пользователя из базы данных по логину и паролю
            var query = dataset.Customers
                              .Where(customer => customer.loginC == login && customer.PasswordC == password)
                              .Select(customer => customer.CustomerID)
                              .FirstOrDefault();

            return query;
        }
        private bool IsUsernameExists(string username)
        {
            var existingUser = dataset.Customers.FirstOrDefault(customer => customer.loginC == username);
            return existingUser != null;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}