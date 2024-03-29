using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
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
    public partial class DeliveryAddressWindow : Window
    {
        private readonly int _customerId;
        private readonly MagAsiaDataSet _dataset = new MagAsiaDataSet();
        private readonly OrdersTableAdapter _ordersTableAdapter = new OrdersTableAdapter();
        private readonly PaymentsTableAdapter paymentsTableAdapter = new PaymentsTableAdapter();
        private int currentOrderID;
        private readonly MagAsiaDataSet.ProductsRow _selectedProduct;
         private bool isAddressSaved = false;

        public DeliveryAddressWindow(int customerId, int orderId, MagAsiaDataSet.ProductsRow selectedProduct)
        {
            InitializeComponent();
            _customerId = customerId;
            currentOrderID = orderId;
            _selectedProduct = selectedProduct; 
            txtCity.PreviewTextInput += TextBoxLettersOnly_PreviewTextInput;
            txtState.PreviewTextInput += TextBoxLettersOnly_PreviewTextInput;
            txtCountry.PreviewTextInput += TextBoxLettersOnly_PreviewTextInput;
            txtPostalCode.PreviewTextInput += TextBoxPostalCode_PreviewTextInput;
        }
       
      

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddressLine1.Text) ||
                string.IsNullOrWhiteSpace(txtCity.Text) ||
                string.IsNullOrWhiteSpace(txtPostalCode.Text) ||
                string.IsNullOrWhiteSpace(txtCountry.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var shippingAddressesAdapter = new ShippingAddressesTableAdapter();
                shippingAddressesAdapter.Insert(_customerId, txtAddressLine1.Text, txtCity.Text, txtState.Text, txtPostalCode.Text, txtCountry.Text);
                MessageBox.Show("Адрес успешно сохранен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                isAddressSaved = true;
                EnableButtons(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void EnableButtons()
        {
            btnCardPayment.IsEnabled = true;
            btnCashPayment.IsEnabled = true;
        }
        private void DisableButtons()
        {
            btnCardPayment.IsEnabled = false;
            btnCashPayment.IsEnabled = false;
        }
        private void UpdatePaymentMethodInOrder(int paymentId)
        {
            try
            {
                MagAsiaDataSet.OrdersRow currentOrder = _dataset.Orders.Single(order => order.CustomerID == _customerId);
                currentOrder.PaymentID = paymentId;
                _ordersTableAdapter.Update(currentOrder);
                MessageBox.Show("Способ оплаты успешно обновлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при обновлении способа оплаты: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CardPayment_Click(object sender, RoutedEventArgs e)
        {
            if (!isAddressSaved)
            {
                MessageBox.Show("Пожалуйста, сначала сохраните адрес доставки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            RecordPayment("Кредитная карта");
            CreditCardPaymentWindow creditCardPaymentWindow = new CreditCardPaymentWindow();
            creditCardPaymentWindow.Show();
        }

        private void CashPayment_Click(object sender, RoutedEventArgs e)
        {
            if (!isAddressSaved)
            {
                MessageBox.Show("Пожалуйста, сначала сохраните адрес доставки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            RecordPayment("Наличные");
            MessageBox.Show("Ожидайте доставки заказа");
            var result = MessageBox.Show("Хотите оставить отзыв?", "Отзыв", MessageBoxButton.YesNo);

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



        private void RecordPayment(string paymentMethod)
        {
            try
            {
                paymentsTableAdapter.Fill(_dataset.Payments);
                int paymentID = GetPaymentID(paymentMethod);
                int receiptID = 1;  
                decimal totalAmount = _selectedProduct.Price; 
                string quantity = "1"; 
                _ordersTableAdapter.Insert(_customerId, paymentID, receiptID, DateTime.Now, totalAmount, quantity);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private int GetPaymentID(string paymentMethod)
        {
            foreach (var row in _dataset.Payments)
            {
                if (row.PaymentMethod == paymentMethod)
                {
                    return row.PaymentID;
                }
            }
            throw new ArgumentException("Способ оплаты не найден.");
        }
        private void TextBoxLettersOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (!Regex.IsMatch(e.Text, @"^[a-zA-Zа-яА-Я]+$"))
                {
                    e.Handled = true;
                }
            }
        }
        private void TextBoxPostalCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                // Проверяем, что вводятся только цифры и что длина строки не превышает 6
                if (!Regex.IsMatch(e.Text, @"^[0-9]+$") || textBox.Text.Length >= 6)
                {
                    e.Handled = true;
                }
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}