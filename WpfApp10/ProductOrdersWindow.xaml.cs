using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WpfApp10.MagAsiaDataSetTableAdapters;

namespace WpfApp10
{
    public partial class ProductOrdersWindow : Window
    {
        private MagAsiaDataSet dataset = new MagAsiaDataSet();
        private ProductOrdersTableAdapter productOrdersAdapter = new ProductOrdersTableAdapter();
        private ProductsTableAdapter productsAdapter = new ProductsTableAdapter();
        private OrdersTableAdapter ordersAdapter = new OrdersTableAdapter();

        public ProductOrdersWindow()
        {
            InitializeComponent();
            ProductOrdersGrid.ItemsSource = dataset.ProductOrders.DefaultView;
            LoadData();
            QuantityTextBox.PreviewTextInput += QuantityTextBox_PreviewTextInput;
        }

        private void LoadData()
        {
            try
            {
                productOrdersAdapter.Fill(dataset.ProductOrders);
                productsAdapter.Fill(dataset.Products);
                ordersAdapter.Fill(dataset.Orders);

                // Устанавливаем отображаемое свойство и свойство выбранного значения для продуктов
                ProductComboBox.DisplayMemberPath = "NameProduct"; // Замените "ProductName" на имя столбца с названием продукта
                ProductComboBox.SelectedValuePath = "ProductID"; // Замените "ProductID" на имя столбца с идентификатором продукта
                ProductComboBox.ItemsSource = dataset.Products.DefaultView;

                // Устанавливаем отображаемое свойство и свойство выбранного значения для заказов
                OrderComboBox.DisplayMemberPath = "OrderID"; // Замените "OrderID" на имя столбца с идентификатором заказа
                OrderComboBox.SelectedValuePath = "OrderID"; // Замените "OrderID" на имя столбца с идентификатором заказа
                OrderComboBox.ItemsSource = dataset.Orders.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductComboBox.SelectedItem == null || OrderComboBox.SelectedItem == null || string.IsNullOrWhiteSpace(QuantityTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Получаем фактические значения выбранных элементов ComboBox
            int productId = (int)((DataRowView)ProductComboBox.SelectedItem)["ProductID"];
            int orderId = (int)((DataRowView)OrderComboBox.SelectedItem)["OrderID"];
            int quantity = Convert.ToInt32(QuantityTextBox.Text);

            DataRow newRow = dataset.ProductOrders.NewRow();
            newRow["ProductID"] = productId;
            newRow["OrderID"] = orderId;
            newRow["Quantity"] = quantity;

            dataset.ProductOrders.Rows.Add(newRow);

            productOrdersAdapter.Update(dataset.ProductOrders);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductOrdersGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)ProductOrdersGrid.SelectedItem;

                // Получаем фактические значения выбранных элементов ComboBox
                int productId = (int)((DataRowView)ProductComboBox.SelectedItem)["ProductID"];
                int orderId = (int)((DataRowView)OrderComboBox.SelectedItem)["OrderID"];
                int quantity = Convert.ToInt32(QuantityTextBox.Text);

                selectedRow.BeginEdit();
                selectedRow["ProductID"] = productId;
                selectedRow["OrderID"] = orderId;
                selectedRow["Quantity"] = quantity;
                selectedRow.EndEdit();

                productOrdersAdapter.Update(dataset.ProductOrders);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductOrdersGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)ProductOrdersGrid.SelectedItem;
                selectedRow.Delete();

                productOrdersAdapter.Update(dataset.ProductOrders);
            }
        }

        private void ProductOrdersGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ProductOrdersGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)ProductOrdersGrid.SelectedItem;
                ProductComboBox.SelectedItem = selectedRow["ProductID"];
                OrderComboBox.SelectedItem = selectedRow["OrderID"];
                QuantityTextBox.Text = selectedRow["Quantity"].ToString();
            }
        }

        private void QuantityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем вводимые символы для поля с количеством
            if (!IsQuantityValid(e.Text))
            {
                e.Handled = true;
            }
        }

        private bool IsQuantityValid(string text)
        {
            // Проверяем, что вводимые символы являются цифрами
            return text.All(char.IsDigit);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}