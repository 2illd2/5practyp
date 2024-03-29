using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class REGCUS2 : Window
    {
        private MagAsiaDataSet dataset = new MagAsiaDataSet();
        public ObservableCollection<MagAsiaDataSet.CustomersRow> CustomersData { get; set; }
        private CustomersTableAdapter customersTableAdapter = new CustomersTableAdapter();
        public REGCUS2()
        {
            InitializeComponent();
            InitializeValidation();
        }
        private void InitializeValidation()
        {
            // для валидации
            FirstNameTextBoxR.PreviewTextInput += TextBoxName_PreviewTextInput;
            LastNameTextBoxR.PreviewTextInput += TextBoxName_PreviewTextInput;
            MiddleNameTextBoxR.PreviewTextInput += TextBoxName_PreviewTextInput;
            PhoneTextBoxR.PreviewTextInput += TextBoxPhone_PreviewTextInput;
            EmailTextBoxR.PreviewTextInput += EmailTextBoxE_PreviewTextInput;
        }
        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBoxR.Text;
            string lastName = LastNameTextBoxR.Text;
            string middleName = MiddleNameTextBoxR.Text;
            string email = EmailTextBoxR.Text;
            string phone = PhoneTextBoxR.Text;
            string username = UsernameTextBoxR.Text;
            string password = PasswordTextBoxR.Password;

            //чтобы поля не были пустыми
            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName) &&
                !string.IsNullOrWhiteSpace(middleName) && !string.IsNullOrWhiteSpace(email) &&
                !string.IsNullOrWhiteSpace(phone) && !string.IsNullOrWhiteSpace(username) &&
                !string.IsNullOrWhiteSpace(password))
            {
                //на существование пользователя с таким логином
                if (IsUsernameExists(username))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует.");
                    return;
                }

                try
                {
                    // Хэшируем пароль перед сохранением в базу данных
                    string hashedPassword = PasswordHasher.HashPassword(password);

                    // Создаем экземпляр таблицы Customers и добавляем новую запись
                    MagAsiaDataSet.CustomersRow newCustomer = dataset.Customers.NewCustomersRow();
                    newCustomer.FirstNameC = firstName;
                    newCustomer.LastNameC = lastName;
                    newCustomer.MiddleNameC = middleName;
                    newCustomer.Email = email;
                    newCustomer.Phone = phone;
                    newCustomer.loginC = username;
                    newCustomer.PasswordC = hashedPassword; // Сохраняем хэшированный пароль
                    dataset.Customers.AddCustomersRow(newCustomer);

                    // Сохраняем изменения в базе данных
                    customersTableAdapter.Update(dataset.Customers);
                    MessageBox.Show("Регистрация прошла успешно! Теперь вы можете войти в систему.");

                    // Очищаем поля для ввода
                    FirstNameTextBoxR.Text = "";
                    LastNameTextBoxR.Text = "";
                    MiddleNameTextBoxR.Text = "";
                    EmailTextBoxR.Text = "";
                    PhoneTextBoxR.Text = "";
                    UsernameTextBoxR.Text = "";
                    PasswordTextBoxR.Password = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при регистрации: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля для регистрации.");
            }
            if (!Regex.IsMatch(firstName, "^[A-Za-z]+$") ||
                !Regex.IsMatch(lastName, "^[A-Za-z]+$") ||
                !Regex.IsMatch(middleName, "^[A-Za-z]+$"))
            {
                
                return;
            }

            // Валидация телефона (только цифры и максимум 11 цифр)
            if (!Regex.IsMatch(phone, "^[0-9]{1,11}$"))
            {
                
                return;
            }
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
        private void TextBoxName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                // Проверяем, что вводятся только буквы
                if (!Regex.IsMatch(e.Text, @"^[a-zA-Zа-яА-Я]+$"))
                {
                    e.Handled = true;
                }
            }
        }
        private void EmailTextBoxE_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                // Проверяем, что вводятся только буквы, цифры, символ @ и знак подчеркивания
                if (!Regex.IsMatch(e.Text, @"^[a-zA-Z0-9_@]+$"))
                {
                    e.Handled = true;
                    return;
                }

                // Добавляем "@" в начало строки, если его там нет
                if (!textBox.Text.Contains("@"))
                {
                    textBox.Text = "@" + textBox.Text;
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }
        private void TextBoxPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, что вводятся только цифры и не более 11 символов
            TextBox textBox = sender as TextBox;
            if (!Regex.IsMatch(e.Text, @"^[0-9]+$") || textBox.Text.Length >= 11)
            {
                e.Handled = true;
                return;
            }

            // Добавляем "+" в начало строки, если его там нет
            if (textBox.Text.Length == 0 && e.Text != "+")
            {
                textBox.Text = "+";
                textBox.CaretIndex = 1; // Перемещаем каретку в конец текста
            }
        }
    }
}
