using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
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
using WpfApp10.MagAsiaDataSetTableAdapters;

namespace WpfApp10
{
    public class OrderItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderItemWithTotal : OrderItem
    {
        public decimal TotalPrice => Price * Quantity;
    }

    public partial class CustomerMainWindow : Window
    {
        private readonly int _customerId;
        private readonly MagAsiaDataSet _dataset = new MagAsiaDataSet();
        private readonly ProductsTableAdapter _productsTableAdapter = new ProductsTableAdapter();
        private readonly OrdersTableAdapter _ordersTableAdapter = new OrdersTableAdapter();
        private readonly ProductOrdersTableAdapter _productOrdersTableAdapter = new ProductOrdersTableAdapter();
        private decimal totalAmount;
        private int currentOrderID;
        private MagAsiaDataSet.ProductsRow selectedProduct;
        public CustomerMainWindow(int customerId)
        {
            InitializeComponent();
            _customerId = customerId;
            LoadProducts();
            LoadOrderItems();
            DataContext = this;
            this.totalAmount = totalAmount;
        }

        private void LoadProducts()
        {
            try
            {
                _productsTableAdapter.Fill(_dataset.Products);
                productsDataGrid.ItemsSource = new ObservableCollection<MagAsiaDataSet.ProductsRow>(_dataset.Products);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки продуктов: " + ex.Message);
            }
        }

        private void addToOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (productsDataGrid.SelectedItem != null)
            {
                selectedProduct = (MagAsiaDataSet.ProductsRow)productsDataGrid.SelectedItem; // Установка значения selectedProduct
                try
                {
                    MagAsiaDataSet.OrdersRow newOrder = _dataset.Orders.NewOrdersRow();
                    newOrder.CustomerID = _customerId;
                    newOrder.PaymentID = 1; //  PaymentID 1 - это значение по умолчанию
                    newOrder.ReceiptID = 1; //ReceiptID 1 - это значение по умолчанию
                    newOrder.OrderDate = DateTime.Now; // Установка даты и времени заказа на текущие
                    newOrder.TotalAmount = selectedProduct.Price; 
                    newOrder.Quantity = 1.ToString();
                    _dataset.Orders.AddOrdersRow(newOrder);
                    _ordersTableAdapter.Update(_dataset.Orders);
                    // Сохраняем ID только что созданного заказа
                    currentOrderID = newOrder.OrderID;
                    MagAsiaDataSet.ProductOrdersRow productOrder = _dataset.ProductOrders.NewProductOrdersRow();
                    productOrder.OrderID = currentOrderID; // Используем сохраненный ID заказа
                    productOrder.ProductID = selectedProduct.ProductID;
                    productOrder.Quantity = 1; 
                    _dataset.ProductOrders.AddProductOrdersRow(productOrder);
                    _productOrdersTableAdapter.Update(_dataset.ProductOrders);
                    LoadOrderItems();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении продукта в заказ: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите продукт для добавления в заказ.");
            }
        }
        private void LoadOrderItems()
        {
            try
            {
                var orderItemsWithTotal = _dataset.ProductOrders
                    .Where(po => po.OrdersRow.CustomerID == _customerId)
                    .Select(po => new OrderItemWithTotal
                    {
                        Name = po.ProductsRow.NameProduct,
                        Price = po.ProductsRow.Price,
                        Quantity = po.Quantity
                    })
                    .ToList();
                orderItemsDataGrid.ItemsSource = orderItemsWithTotal;
                ShowTotalPrice();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке элементов заказа: " + ex.Message);
            }
        }
        private void ShowTotalPrice()
        {
            try
            {
                decimal totalPrice = CalculateTotalAmount();
                TotalPriceTextBlock.Text = $"Общая цена: {totalPrice:C2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отображении общей цены: {ex.Message}");
            }
        }


        private decimal CalculateTotalAmount()
        {
            decimal totalAmount = 0;

            try
            {
                foreach (var productOrder in _dataset.ProductOrders)
                {
                    MagAsiaDataSet.ProductsRow product = _dataset.Products.FirstOrDefault(p => p.ProductID == productOrder.ProductID);

                    if (product != null)
                    {
                        totalAmount += product.Price * productOrder.Quantity;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при вычислении общей суммы: {ex.Message}");
            }

            return totalAmount;
        }


        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            OrderItemWithTotal item = (OrderItemWithTotal)button.DataContext;
            item.Quantity++;
            MagAsiaDataSet.ProductOrdersRow productOrderRow = _dataset.ProductOrders
                .FirstOrDefault(po => po.OrdersRow.CustomerID == _customerId && po.ProductsRow.NameProduct == item.Name);
            if (productOrderRow != null)
            {
                productOrderRow.Quantity = item.Quantity;

                try
                {
                    _productOrdersTableAdapter.Update(_dataset.ProductOrders);
                    ShowTotalPrice();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении количества: " + ex.Message);
                }
            }
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            OrderItemWithTotal item = (OrderItemWithTotal)button.DataContext;
            if (item.Quantity > 0)
                item.Quantity--;
            MagAsiaDataSet.ProductOrdersRow productOrderRow = _dataset.ProductOrders
                .FirstOrDefault(po => po.OrdersRow.CustomerID == _customerId && po.ProductsRow.NameProduct == item.Name);
            if (productOrderRow != null)
            {
                productOrderRow.Quantity = item.Quantity;
                try
                {
                    _productOrdersTableAdapter.Update(_dataset.ProductOrders);
                    ShowTotalPrice();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении количества: " + ex.Message);
                }
            }
        }
        public decimal TotalPrice
        {
            get { return CalculateTotalAmount(); }
        }

        private void PayOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentOrderID != 0 && selectedProduct != null) 
            {
                DeliveryAddressWindow deliveryAddressWindow = new DeliveryAddressWindow(_customerId, currentOrderID, selectedProduct);
                deliveryAddressWindow.Show();
            }
            else
            {
                MessageBox.Show("текущий заказ не найден или не выбран продукт."); 
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}