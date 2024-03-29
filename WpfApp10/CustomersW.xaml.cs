using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp10.MagAsiaDataSetTableAdapters;

namespace WpfApp10
{
    public partial class CustomersW : Window
    {
        private MagAsiaDataSet dataset = new MagAsiaDataSet();
        private CustomersTableAdapter customersAdapter = new CustomersTableAdapter();

        public CustomersW()
        {
            InitializeComponent();
            CustomersGrid.ItemsSource = dataset.Customers.DefaultView;
            LoadData();

            // Привязываем обработчики событий PreviewTextInput к текстовым полям для валидации
            FirstNameTextBox.PreviewTextInput += TextBoxName_PreviewTextInput;
            LastNameTextBox.PreviewTextInput += TextBoxName_PreviewTextInput;
            MiddleNameTextBox.PreviewTextInput += TextBoxName_PreviewTextInput;
            PhoneTextBox.PreviewTextInput += TextBoxPhone_PreviewTextInput;
            EmailTextBox.PreviewTextInput += EmailTextBoxE_PreviewTextInput;
            CustomersGrid.MouseLeftButtonUp += CustomersGrid_MouseLeftButtonUp;
        }
        private void CustomersGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (CustomersGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)CustomersGrid.SelectedItem;
                FirstNameTextBox.Text = selectedRow["FirstNameC"].ToString();
                LastNameTextBox.Text = selectedRow["LastNameC"].ToString();
                MiddleNameTextBox.Text = selectedRow["MiddleNameC"].ToString();
                EmailTextBox.Text = selectedRow["Email"].ToString();
                PhoneTextBox.Text = selectedRow["Phone"].ToString();
                LoginTextBox.Text = selectedRow["loginC"].ToString();
                PasswordTextBox.Password = selectedRow["PasswordC"].ToString();
            }
        }
        private void LoadData()
        {
            try
            {
                customersAdapter.Fill(dataset.Customers);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на заполнение всех полей
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) || string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(MiddleNameTextBox.Text) || string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneTextBox.Text) || string.IsNullOrWhiteSpace(LoginTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordTextBox.Password))
            {
                MessageBox.Show("Пожалуйста, заполните все текстовые поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataRow newRow = dataset.Customers.NewRow();
            newRow["FirstNameC"] = FirstNameTextBox.Text;
            newRow["LastNameC"] = LastNameTextBox.Text;
            newRow["MiddleNameC"] = MiddleNameTextBox.Text;
            newRow["Email"] = EmailTextBox.Text;
            newRow["Phone"] = PhoneTextBox.Text;
            newRow["loginC"] = LoginTextBox.Text;
            newRow["PasswordC"] = HashPassword(PasswordTextBox.Password); // Хэшируем пароль

            dataset.Customers.Rows.Add(newRow);

            customersAdapter.Update(dataset.Customers);
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

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)CustomersGrid.SelectedItem;
                selectedRow.BeginEdit();
                selectedRow["FirstNameC"] = FirstNameTextBox.Text;
                selectedRow["LastNameC"] = LastNameTextBox.Text;
                selectedRow["MiddleNameC"] = MiddleNameTextBox.Text;
                selectedRow["Email"] = EmailTextBox.Text;
                selectedRow["Phone"] = PhoneTextBox.Text;
                selectedRow["loginC"] = LoginTextBox.Text;
                selectedRow["PasswordC"] = PasswordTextBox.Password;
                selectedRow.EndEdit();

                customersAdapter.Update(dataset.Customers);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)CustomersGrid.SelectedItem;
                selectedRow.Delete();

                customersAdapter.Update(dataset.Customers);
            }
        }

        private void CustomersGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CustomersGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)CustomersGrid.SelectedItem;
                FirstNameTextBox.Text = selectedRow["FirstNameC"].ToString();
                LastNameTextBox.Text = selectedRow["LastNameC"].ToString();
                MiddleNameTextBox.Text = selectedRow["MiddleNameC"].ToString();
                EmailTextBox.Text = selectedRow["Email"].ToString();
                PhoneTextBox.Text = selectedRow["Phone"].ToString();
                LoginTextBox.Text = selectedRow["loginC"].ToString();
                PasswordTextBox.Password = selectedRow["PasswordC"].ToString();
            }
        }

        // Обработчик события для валидации ввода только букв
        private void TextBoxName_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        // Обработчик события для валидации ввода только цифр для поля телефона
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
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}