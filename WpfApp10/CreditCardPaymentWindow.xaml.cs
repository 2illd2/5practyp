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
    public partial class CreditCardPaymentWindow : Window
    {

        private readonly int _customerId;
        public CreditCardPaymentWindow()
        {
            InitializeComponent();
        }
        private void CardNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Форматирование номера карты: добавление пробелов после каждых 4 цифр
            TextBox textBox = (TextBox)sender;
            string cardNumber = textBox.Text.Replace(" ", ""); // Удаляем пробелы из текста

            // Убеждаемся, что не превышено общее количество цифр
            if (cardNumber.Length > 16)
            {
                cardNumber = cardNumber.Substring(0, 16);
            }

            // Форматируем текст, добавляя пробелы после каждых 4 цифр
            StringBuilder formattedNumber = new StringBuilder();
            for (int i = 0; i < cardNumber.Length; i++)
            {
                if (i > 0 && i % 4 == 0)
                {
                    formattedNumber.Append(" "); // Добавляем пробел после каждых 4 цифр
                }
                formattedNumber.Append(cardNumber[i]);
            }

            // Обновляем текст в поле номера карты
            textBox.Text = formattedNumber.ToString();
            textBox.SelectionStart = textBox.Text.Length;
        }

        private void CardNumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Разрешаем ввод только цифр и добавляем автоматические пробелы после каждых 4 цифр
            TextBox textBox = (TextBox)sender;
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
            else if (textBox.Text.Replace(" ", "").Length == 16) // Ограничение на количество цифр
            {
                e.Handled = true;
            }
        }


        private void ExpiryDateTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Разрешаем ввод только цифр и автоматически вставляем разделитель "/"
            TextBox textBox = (TextBox)sender;
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
            else if (textBox.Text.Length == 5) // Ограничение на количество символов
            {
                e.Handled = true;
            }
            else if (textBox.Text.Length == 2)
            {
                textBox.Text += "/";
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private void CVVTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Разрешаем ввод только цифр
            e.Handled = !char.IsDigit(e.Text, 0);
            if ((sender as TextBox).Text.Length == 3) // Ограничение на количество символов
            {
                e.Handled = true;
            }
        }
        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Заказ успешно оплачен.\nХотите оставить отзыв?",
                                                       "Подтверждение оплаты",
                                                       MessageBoxButton.YesNo,
                                                       MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                ReviewWindow ReviewWindow = new ReviewWindow(_customerId);
                ReviewWindow.Show();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
