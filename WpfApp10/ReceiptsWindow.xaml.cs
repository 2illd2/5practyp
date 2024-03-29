using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp10.MagAsiaDataSetTableAdapters;
namespace WpfApp10
{
    public partial class ReceiptsWindow : Window
    {
        private MagAsiaDataSet dataset = new MagAsiaDataSet();
        private ReceiptsTableAdapter receiptsAdapter = new ReceiptsTableAdapter();
        private OrdersTableAdapter ordersAdapter = new OrdersTableAdapter();

        public ReceiptsWindow()
        {
            InitializeComponent();
            ReceiptsGrid.ItemsSource = dataset.Receipts.DefaultView;
            LoadData();
            TotalAmountTextBox.PreviewTextInput += NumericTextBox_PreviewTextInput;
            ChangeAmountTextBox.PreviewTextInput += NumericTextBox_PreviewTextInput;
            AmountCusTextBox.PreviewTextInput += NumericTextBox_PreviewTextInput;
            PaymentMethodTextBox.PreviewTextInput += TextOnlyTextBox_PreviewTextInput;
            ReceiptsGrid.SelectionChanged += ReceiptsGrid_SelectionChanged;
        }

        private void LoadData()
        {
            try
            {
                receiptsAdapter.Fill(dataset.Receipts);
                ordersAdapter.Fill(dataset.Orders);

                OrderComboBox.ItemsSource = dataset.Orders.Select(o => o.OrderID).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrderComboBox.SelectedItem == null || string.IsNullOrWhiteSpace(TotalAmountTextBox.Text) ||
                string.IsNullOrWhiteSpace(ChangeAmountTextBox.Text) || string.IsNullOrWhiteSpace(AmountCusTextBox.Text) ||
                string.IsNullOrWhiteSpace(PaymentMethodTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            

            DataRow newRow = dataset.Receipts.NewRow();
            newRow["OrderID"] = OrderComboBox.SelectedItem;
            newRow["ReceiptDate"] = DateTime.Now;
            newRow["TotalAmount"] = Convert.ToDecimal(TotalAmountTextBox.Text);
            newRow["ChangeAmount"] = Convert.ToDecimal(ChangeAmountTextBox.Text);
            newRow["AmountCus"] = Convert.ToDecimal(AmountCusTextBox.Text);
            newRow["PaymentMethod"] = PaymentMethodTextBox.Text;

            dataset.Receipts.Rows.Add(newRow);

            receiptsAdapter.Update(dataset.Receipts);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReceiptsGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)ReceiptsGrid.SelectedItem;
                selectedRow.BeginEdit();
                selectedRow["OrderID"] = OrderComboBox.SelectedItem;
                selectedRow["TotalAmount"] = Convert.ToDecimal(TotalAmountTextBox.Text);
                selectedRow["ChangeAmount"] = Convert.ToDecimal(ChangeAmountTextBox.Text);
                selectedRow["AmountCus"] = Convert.ToDecimal(AmountCusTextBox.Text);
                selectedRow["PaymentMethod"] = PaymentMethodTextBox.Text;
                selectedRow.EndEdit();

                receiptsAdapter.Update(dataset.Receipts);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReceiptsGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)ReceiptsGrid.SelectedItem;
                selectedRow.Delete();

                receiptsAdapter.Update(dataset.Receipts);
            }
        }

       
        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, что вводится только цифры или точка (для дробных чисел)
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9]+(.[0-9]{0,2})?$"))
            {
                e.Handled = true;
            }
        }
        private void ReceiptsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReceiptsGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)ReceiptsGrid.SelectedItem;
                OrderComboBox.SelectedItem = selectedRow["OrderID"];
                TotalAmountTextBox.Text = selectedRow["TotalAmount"].ToString();
                ChangeAmountTextBox.Text = selectedRow["ChangeAmount"].ToString();
                AmountCusTextBox.Text = selectedRow["AmountCus"].ToString();
                PaymentMethodTextBox.Text = selectedRow["PaymentMethod"].ToString();
            }
        }

        private void TextOnlyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, что вводятся только буквы
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Zа-яА-Я]+$"))
            {
                e.Handled = true;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
