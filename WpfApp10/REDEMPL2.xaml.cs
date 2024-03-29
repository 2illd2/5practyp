using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// Логика взаимодействия для REDEMPL2.xaml
    /// </summary>
    public partial class REDEMPL2 : Window
    {
        private MagAsiaDataSet dataset = new MagAsiaDataSet();
        public ObservableCollection<MagAsiaDataSet.EmployeesRow> EmployeesData { get; set; }
        private EmployeesTableAdapter employeesTableAdapter = new EmployeesTableAdapter();
        public REDEMPL2()
        {
            InitializeComponent();
            InitializeValidation();
        }
        private void InitializeValidation()
        {
            // Привязываем обработчики событий PreviewTextInput к текстовым полям для валидации
            FirstNameTextBoxE.PreviewTextInput += TextBoxName_PreviewTextInput;
            LastNameTextBoxE.PreviewTextInput += TextBoxName_PreviewTextInput;
            MiddleNameTextBoxE.PreviewTextInput += TextBoxName_PreviewTextInput;
            PhoneTextBoxE.PreviewTextInput += TextBoxPhone_PreviewTextInput;
            EmailTextBoxE.PreviewTextInput += EmailTextBoxE_PreviewTextInput;
        }
       
        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            string adminCode = Microsoft.VisualBasic.Interaction.InputBox("Введите код администратора:", "Код администратора", "");
            if (adminCode != "143")
            {
                MessageBox.Show("Неверный код администратора.");
                return;
            }

            string firstName = FirstNameTextBoxE.Text;
            string lastName = LastNameTextBoxE.Text;
            string middleName = MiddleNameTextBoxE.Text;
            string email = EmailTextBoxE.Text;
            string phone = PhoneTextBoxE.Text;
            string username = UsernameTextBoxE.Text;
            string password = PasswordTextBoxE.Password;

            // Проверяем, чтобы поля не были пустыми
            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName) &&
                !string.IsNullOrWhiteSpace(middleName) && !string.IsNullOrWhiteSpace(email) &&
                !string.IsNullOrWhiteSpace(phone) && !string.IsNullOrWhiteSpace(username) &&
                !string.IsNullOrWhiteSpace(password))
            {
                // Проверяем существование пользователя с таким логином
                if (IsUsernameExists(username))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует.");
                    return;
                }

                try
                {
                    // Хэшируем пароль перед сохранением в базу данных
                    string hashedPassword = PasswordHasher.HashPassword(password);

                    // Создаем экземпляр таблицы Employees и добавляем новую запись
                    MagAsiaDataSet.EmployeesRow newEmployee = dataset.Employees.NewEmployeesRow();
                    newEmployee.FirstNameE = firstName;
                    newEmployee.LastNameE = lastName;
                    newEmployee.MiddleNameE = middleName;
                    newEmployee.Email = email;
                    newEmployee.Phone = phone;
                    newEmployee.loginE = username;
                    newEmployee.PasswordE = hashedPassword; // Сохраняем хэшированный пароль
                    dataset.Employees.AddEmployeesRow(newEmployee);

                    // Сохраняем изменения в базе данных
                    employeesTableAdapter.Update(dataset.Employees);

                    // Выводим сообщение об успешной регистрации
                    MessageBox.Show("Регистрация прошла успешно! Теперь вы можете войти в систему.");

                    // Очищаем поля для ввода
                    FirstNameTextBoxE.Text = "";
                    LastNameTextBoxE.Text = "";
                    MiddleNameTextBoxE.Text = "";
                    EmailTextBoxE.Text = "";
                    PhoneTextBoxE.Text = "";
                    UsernameTextBoxE.Text = "";
                    PasswordTextBoxE.Password = "";
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
            if (!Regex.IsMatch(phone, "^[0-9]{11,11}$"))
            {

                return;
            }
        }


        private bool IsUsernameExists(string username)
        {
            var existingUser = dataset.Employees.FirstOrDefault(employee => employee.loginE == username);
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
   