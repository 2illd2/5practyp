using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp10.MagAsiaDataSetTableAdapters;

namespace WpfApp10
{
    public partial class ShippingAddressesWindow : Window
    {
        private MagAsiaDataSet dataset = new MagAsiaDataSet();
        private ShippingAddressesTableAdapter shippingAddressesAdapter = new ShippingAddressesTableAdapter();
        private CustomersTableAdapter customersAdapter = new CustomersTableAdapter();

        public ShippingAddressesWindow()
        {
            InitializeComponent();
            ShippingAddressesGrid.ItemsSource = dataset.ShippingAddresses.DefaultView;
            LoadData();

            // Привязываем обработчики событий PreviewTextInput к текстовым полям для валидации
            CityTextBox.PreviewTextInput += TextBox_PreviewTextInput;
            StateShippingTextBox.PreviewTextInput += TextBox_PreviewTextInput;
            CountryTextBox.PreviewTextInput += TextBox_PreviewTextInput;
            PostalCodeTextBox.PreviewTextInput += NumericTextBox_PreviewTextInput;
            ShippingAddressesGrid.SelectionChanged += ShippingAddressesGrid_SelectionChanged;
        }

        private void LoadData()
        {
            try
            {
                shippingAddressesAdapter.Fill(dataset.ShippingAddresses);
                customersAdapter.Fill(dataset.Customers);

                CustomerComboBox.ItemsSource = dataset.Customers.Select(c => c.CustomerID).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerComboBox.SelectedItem == null || string.IsNullOrWhiteSpace(AddressLine1TextBox.Text) ||
                string.IsNullOrWhiteSpace(CityTextBox.Text) || string.IsNullOrWhiteSpace(PostalCodeTextBox.Text) ||
                string.IsNullOrWhiteSpace(CountryTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataRow newRow = dataset.ShippingAddresses.NewRow();
            newRow["CustomerID"] = CustomerComboBox.SelectedItem;
            newRow["AddressLine1"] = AddressLine1TextBox.Text;
            newRow["City"] = CityTextBox.Text;
            newRow["StateShipping"] = StateShippingTextBox.Text;
            newRow["PostalCode"] = PostalCodeTextBox.Text;
            newRow["Country"] = CountryTextBox.Text;

            dataset.ShippingAddresses.Rows.Add(newRow);

            shippingAddressesAdapter.Update(dataset.ShippingAddresses);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShippingAddressesGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)ShippingAddressesGrid.SelectedItem;
                selectedRow.BeginEdit();
                selectedRow["CustomerID"] = CustomerComboBox.SelectedItem;
                selectedRow["AddressLine1"] = AddressLine1TextBox.Text;
                selectedRow["City"] = CityTextBox.Text;
                selectedRow["StateShipping"] = StateShippingTextBox.Text;
                selectedRow["PostalCode"] = PostalCodeTextBox.Text;
                selectedRow["Country"] = CountryTextBox.Text;
                selectedRow.EndEdit();

                shippingAddressesAdapter.Update(dataset.ShippingAddresses);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShippingAddressesGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)ShippingAddressesGrid.SelectedItem;
                selectedRow.Delete();

                shippingAddressesAdapter.Update(dataset.ShippingAddresses);
            }
        }

        private void ShippingAddressesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ShippingAddressesGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)ShippingAddressesGrid.SelectedItem;
                CustomerComboBox.SelectedValue = selectedRow["CustomerID"];
                AddressLine1TextBox.Text = selectedRow["AddressLine1"].ToString();
                CityTextBox.Text = selectedRow["City"].ToString();
                StateShippingTextBox.Text = selectedRow["StateShipping"].ToString();
                PostalCodeTextBox.Text = selectedRow["PostalCode"].ToString();
                CountryTextBox.Text = selectedRow["Country"].ToString();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем вводимые символы для текстовых полей (только буквы)
            e.Handled = !IsLetter(e.Text);
        }

        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем вводимые символы для числового поля (только цифры)
            e.Handled = !IsDigit(e.Text);
        }

        private bool IsLetter(string text)
        {
            // Проверяем, что вводимые символы являются буквами
            return text.All(char.IsLetter);
        }

        private bool IsDigit(string text)
        {
            // Проверяем, что вводимые символы являются цифрами
            return text.All(char.IsDigit);
        }
    }
}
