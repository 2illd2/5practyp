using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Windows;
using WpfApp10.MagAsiaDataSetTableAdapters;

namespace WpfApp10
{
    public partial class OrdersW : Window

    {
        private class CustomerData
        {
            public int CustomerID { get; set; }
            // Другие свойства
        }

        private class PaymentData
        {
            public int PaymentID { get; set; }
            // Другие свойства
        }

        private class ReceiptData
        {
            public int ReceiptID { get; set; }
            // Другие свойства
        }
        private MagAsiaDataSet dataset = new MagAsiaDataSet();
        private OrdersTableAdapter ordersAdapter = new OrdersTableAdapter();
        private CustomersTableAdapter customersAdapter = new CustomersTableAdapter();
        private PaymentsTableAdapter paymentsAdapter = new PaymentsTableAdapter();
        private ReceiptsTableAdapter receiptsAdapter = new ReceiptsTableAdapter();

        public OrdersW()
        {
            InitializeComponent();
            OrdersGrid.ItemsSource = dataset.Orders.DefaultView;
            LoadData();

            // Привязываем обработчики событий PreviewTextInput для валидации ввода только цифр
            TotalAmountTextBox.PreviewTextInput += NumericTextBox_PreviewTextInput;
            QuantityTextBox.PreviewTextInput += NumericTextBox_PreviewTextInput;

            // Добавляем обработчик события MouseDoubleClick для таблицы
            OrdersGrid.MouseDoubleClick += OrdersGrid_MouseDoubleClick;
        }
        private void OrdersGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (OrdersGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)OrdersGrid.SelectedItem;
                CustomerComboBox.SelectedItem = selectedRow["CustomerID"];
                PaymentComboBox.SelectedItem = selectedRow["PaymentID"];
                ReceiptComboBox.SelectedItem = selectedRow["ReceiptID"];
                TotalAmountTextBox.Text = selectedRow["TotalAmount"].ToString();
                QuantityTextBox.Text = selectedRow["Quantity"].ToString();
            }
        }

        private void LoadData()
        {
            try
            {
                ordersAdapter.Fill(dataset.Orders);
                customersAdapter.Fill(dataset.Customers);
                paymentsAdapter.Fill(dataset.Payments);
                receiptsAdapter.Fill(dataset.Receipts);

                CustomerComboBox.ItemsSource = dataset.Customers.Select(c => c.CustomerID).ToList();
                PaymentComboBox.ItemsSource = dataset.Payments.Select(p => p.PaymentID).ToList();
                ReceiptComboBox.ItemsSource = dataset.Receipts.Select(r => r.ReceiptID).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerComboBox.SelectedItem == null || PaymentComboBox.SelectedItem == null || ReceiptComboBox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(TotalAmountTextBox.Text) || string.IsNullOrWhiteSpace(QuantityTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataRow newRow = dataset.Orders.NewRow();
            newRow["CustomerID"] = CustomerComboBox.SelectedItem;
            newRow["PaymentID"] = PaymentComboBox.SelectedItem;
            newRow["ReceiptID"] = ReceiptComboBox.SelectedItem;
            newRow["OrderDate"] = DateTime.Now;
            newRow["TotalAmount"] = Convert.ToDecimal(TotalAmountTextBox.Text);
            newRow["Quantity"] = QuantityTextBox.Text;

            dataset.Orders.Rows.Add(newRow);

            ordersAdapter.Update(dataset.Orders);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)OrdersGrid.SelectedItem;
                selectedRow.BeginEdit();
                selectedRow["CustomerID"] = CustomerComboBox.SelectedItem;
                selectedRow["PaymentID"] = PaymentComboBox.SelectedItem;
                selectedRow["ReceiptID"] = ReceiptComboBox.SelectedItem;
                selectedRow["TotalAmount"] = Convert.ToDecimal(TotalAmountTextBox.Text);
                selectedRow["Quantity"] = QuantityTextBox.Text;
                selectedRow.EndEdit();

                ordersAdapter.Update(dataset.Orders);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)OrdersGrid.SelectedItem;
                selectedRow.Delete();

                ordersAdapter.Update(dataset.Orders);
            }
        }

        private void OrdersGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (OrdersGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)OrdersGrid.SelectedItem;
                CustomerComboBox.SelectedItem = selectedRow["CustomerID"];
                PaymentComboBox.SelectedItem = selectedRow["PaymentID"];
                ReceiptComboBox.SelectedItem = selectedRow["ReceiptID"];
                TotalAmountTextBox.Text = selectedRow["TotalAmount"].ToString();
                QuantityTextBox.Text = selectedRow["Quantity"].ToString();
            }
        }

        // Обработчик события для валидации ввода только цифр
        private void NumericTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9]+$"))
            {
                e.Handled = true; // Отмена ввода, если символ не является цифрой
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Импорт данных из JSON файла для каждой таблицы
                ImportCustomers();
                ImportPayments();
                ImportReceipts();

                // После импорта обновите источники данных для соответствующих комбо-боксов или других элементов управления
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при импорте данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImportCustomers()
        {
            // Пример импорта данных из JSON файла для таблицы Customers
            string json = File.ReadAllText("customers.json");
            List<CustomerData> customerData = JsonConvert.DeserializeObject<List<CustomerData>>(json);

            // Очистка таблицы перед импортом
            dataset.Customers.Clear();

            // Добавление импортированных данных в таблицу
            foreach (var item in customerData)
            {
                DataRow newRow = dataset.Customers.NewRow();
                newRow["CustomerID"] = item.CustomerID;
                // Заполните другие поля, если они есть
                dataset.Customers.Rows.Add(newRow);
            }
        }

        private void ImportPayments()
        {
            // Пример импорта данных из JSON файла для таблицы Payments
            string json = File.ReadAllText("payments.json");
            List<PaymentData> paymentData = JsonConvert.DeserializeObject<List<PaymentData>>(json);

            // Очистка таблицы перед импортом
            dataset.Payments.Clear();

            // Добавление импортированных данных в таблицу
            foreach (var item in paymentData)
            {
                DataRow newRow = dataset.Payments.NewRow();
                newRow["PaymentID"] = item.PaymentID;
                // Добавьте другие поля, если они есть
                dataset.Payments.Rows.Add(newRow);
            }
        }

        private void ImportReceipts()
        {
            // Пример импорта данных из JSON файла для таблицы Receipts
            string json = File.ReadAllText("receipts.json");
            List<ReceiptData> receiptData = JsonConvert.DeserializeObject<List<ReceiptData>>(json);

            // Очистка таблицы перед импортом
            dataset.Receipts.Clear();

            // Добавление импортированных данных в таблицу
            foreach (var item in receiptData)
            {
                DataRow newRow = dataset.Receipts.NewRow();
                newRow["ReceiptID"] = item.ReceiptID;
                // Добавьте другие поля, если они есть
                dataset.Receipts.Rows.Add(newRow);
            }
        }
    }
}