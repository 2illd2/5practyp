using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Security.Cryptography;
using System.Text;
using WpfApp10.MagAsiaDataSetTableAdapters;
using System.Text.RegularExpressions;

namespace WpfApp10
{
    public partial class EmployeesWindow : Window
    {
        private MagAsiaDataSet dataset = new MagAsiaDataSet();
        private EmployeesTableAdapter employeesAdapter = new EmployeesTableAdapter();

        public EmployeesWindow()
        {
            InitializeComponent();
            EmployeesGrid.ItemsSource = dataset.Employees.DefaultView;
            LoadData();

            // Привязываем обработчики событий PreviewTextInput к текстовым полям для валидации
            FirstNameTextBox.PreviewTextInput += TextBoxName_PreviewTextInput;
            LastNameTextBox.PreviewTextInput += TextBoxName_PreviewTextInput;
            MiddleNameTextBox.PreviewTextInput += TextBoxName_PreviewTextInput;
            PhoneTextBox.PreviewTextInput += TextBoxPhone_PreviewTextInput;
            EmailTextBox.PreviewTextInput += EmailTextBoxE_PreviewTextInput; 
            EmployeesGrid.SelectionChanged += EmployeesGrid_SelectionChanged;
        }

        private void EmployeesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeesGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)EmployeesGrid.SelectedItem;
                FirstNameTextBox.Text = selectedRow["FirstNameE"].ToString();
                LastNameTextBox.Text = selectedRow["LastNameE"].ToString();
                MiddleNameTextBox.Text = selectedRow["MiddleNameE"].ToString();
                EmailTextBox.Text = selectedRow["Email"].ToString();
                PhoneTextBox.Text = selectedRow["Phone"].ToString();
                LoginTextBox.Text = selectedRow["loginE"].ToString();
                PasswordTextBox.Password = selectedRow["PasswordE"].ToString();
            }
        }

        private void LoadData()
        {
            try
            {
                employeesAdapter.Fill(dataset.Employees);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) || string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(MiddleNameTextBox.Text) || string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneTextBox.Text) || string.IsNullOrWhiteSpace(LoginTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordTextBox.Password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataRow newRow = dataset.Employees.NewRow();
            newRow["FirstNameE"] = FirstNameTextBox.Text;
            newRow["LastNameE"] = LastNameTextBox.Text;
            newRow["MiddleNameE"] = MiddleNameTextBox.Text;
            newRow["Email"] = EmailTextBox.Text;
            newRow["Phone"] = PhoneTextBox.Text;
            newRow["loginE"] = LoginTextBox.Text;
            newRow["PasswordE"] = HashPassword(PasswordTextBox.Password); // Хэшируем пароль

            dataset.Employees.Rows.Add(newRow);

            employeesAdapter.Update(dataset.Employees);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)EmployeesGrid.SelectedItem;
                selectedRow.BeginEdit();
                selectedRow["FirstNameE"] = FirstNameTextBox.Text;
                selectedRow["LastNameE"] = LastNameTextBox.Text;
                selectedRow["MiddleNameE"] = MiddleNameTextBox.Text;
                selectedRow["Email"] = EmailTextBox.Text;
                selectedRow["Phone"] = PhoneTextBox.Text;
                selectedRow["loginE"] = LoginTextBox.Text;
                selectedRow["PasswordE"] = HashPassword(PasswordTextBox.Password); // Хэшируем пароль
                selectedRow.EndEdit();

                employeesAdapter.Update(dataset.Employees);
            }
        }

        private string HashPassword(string password)
        {
            // Создаем объект для вычисления хэша SHA256
            using (SHA256 sha256 = SHA256Managed.Create())
            {
                // Преобразуем строку пароля в массив байт
                byte[] bytes = Encoding.UTF8.GetBytes(password);

                // Вычисляем хэш пароля
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Преобразуем хэш в строку шестнадцатеричного представления
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)EmployeesGrid.SelectedItem;
                selectedRow.Delete();

                employeesAdapter.Update(dataset.Employees);
            }
        }

        

        // Обработчик события для валидации ввода только букв
        private void TextBoxName_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                // Проверяем, что вводятся только буквы
                if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"^[a-zA-Zа-яА-Я]+$"))
                {
                    e.Handled = true; // Отмена ввода, если символ не является буквой
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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}