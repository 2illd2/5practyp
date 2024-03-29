using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
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
namespace WpfApp10
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Преобразуем пароль в массив байтов
                byte[] bytes = Encoding.UTF8.GetBytes(password);

                // Вычисляем хэш
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Преобразуем хэш в строку HEX
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
    public partial class REGEMPL : Window
    {
        private MagAsiaDataSet dataset = new MagAsiaDataSet();
        public ObservableCollection<MagAsiaDataSet.EmployeesRow> EmployeesData { get; set; }
        private EmployeesTableAdapter employeesTableAdapter = new EmployeesTableAdapter();

        public REGEMPL()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                employeesTableAdapter.Fill(dataset.Employees);
                EmployeesData = new ObservableCollection<MagAsiaDataSet.EmployeesRow>(dataset.Employees);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private bool CheckEmployeeLogin(string login, string hashedPassword)
        {
            // Проверяем наличие пользователя с указанным логином и хэшированным паролем
            return dataset.Employees.Any(employee => employee.loginE == login && employee.PasswordE == hashedPassword);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;

            // Хэшируем введенный пользователем пароль
            string hashedPassword = PasswordHasher.HashPassword(password);

            if (CheckEmployeeLogin(login, hashedPassword))
            {
                // Открыть окно, если вход выполнен успешно
                GLWEMPL gLWEMPL = new GLWEMPL();
                gLWEMPL.Show();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
        }
        private void EmployeeButton_Click(object sender,RoutedEventArgs e)
        {
            REDEMPL2 rEDEMPL2 = new REDEMPL2();
            rEDEMPL2.Show();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
