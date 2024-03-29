using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp10.MagAsiaDataSetTableAdapters;
using Newtonsoft.Json;
using System.IO;

namespace WpfApp10
{
    public partial class ProductW : Window
    {
        private class ProductData
        {
            public string NameProduct { get; set; }
            public string DescriptionProduct { get; set; }
            public decimal Price { get; set; }
            public int CategoryID { get; set; }
        }

        private MagAsiaDataSet dataset = new MagAsiaDataSet();
        private ProductsTableAdapter productsAdapter = new ProductsTableAdapter();
        private CategoriesTableAdapter categoriesAdapter = new CategoriesTableAdapter();
        public ProductW()
        {
            InitializeComponent();
            AsianCuisineGrid.ItemsSource = dataset.Products.DefaultView;
            LoadData();
            NameTextBox.PreviewTextInput += NameTextBox_PreviewTextInput;
            PriceTextBox.PreviewTextInput += PriceTextBox_PreviewTextInput;

            // Добавляем обработчик события SelectionChanged для таблицы
            AsianCuisineGrid.SelectionChanged += AsianCuisineGrid_SelectionChanged;
        }
        private void AsianCuisineGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AsianCuisineGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)AsianCuisineGrid.SelectedItem;
                NameTextBox.Text = selectedRow["NameProduct"].ToString();
                DescriptionTextBox.Text = selectedRow["DescriptionProduct"].ToString();
                PriceTextBox.Text = selectedRow["Price"].ToString();
            }
        }
        private void LoadData()
        {
            try
            {
                productsAdapter.Fill(dataset.Products);
                categoriesAdapter.Fill(dataset.Categories);
                CategoryComboBox.ItemsSource = dataset.Categories.Select(c => c.NameCategory).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsNameValid(NameTextBox.Text) || string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ||
                !IsPriceValid(PriceTextBox.Text) || string.IsNullOrWhiteSpace(CategoryComboBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataRow newRow = dataset.Products.NewRow();
            newRow["NameProduct"] = NameTextBox.Text;
            newRow["DescriptionProduct"] = DescriptionTextBox.Text;
            newRow["Price"] = Convert.ToDecimal(PriceTextBox.Text);
            newRow["CategoryID"] = dataset.Categories.Where(c => c.NameCategory == CategoryComboBox.Text).FirstOrDefault()?.CategoryID;

            dataset.Products.Rows.Add(newRow);

            productsAdapter.Update(dataset.Products);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (AsianCuisineGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)AsianCuisineGrid.SelectedItem;
                selectedRow.BeginEdit();
                selectedRow["NameProduct"] = NameTextBox.Text;
                selectedRow["DescriptionProduct"] = DescriptionTextBox.Text;
                selectedRow["Price"] = Convert.ToDecimal(PriceTextBox.Text);
                selectedRow["CategoryID"] = dataset.Categories.Where(c => c.NameCategory == CategoryComboBox.Text).FirstOrDefault()?.CategoryID;
                selectedRow.EndEdit();

                productsAdapter.Update(dataset.Products);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (AsianCuisineGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)AsianCuisineGrid.SelectedItem;
                selectedRow.Delete();

                productsAdapter.Update(dataset.Products);
            }
        }

        

        private void PriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9]+(.[0-9]{0,2})?$"))
            {
                e.Handled = true;
            }
        }

        private bool IsNameValid(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.All(char.IsLetter);
        }

        private bool IsPriceValid(string price)
        {
            decimal result;
            return decimal.TryParse(price, out result);
        }

        private void NameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNameValid(e.Text))
            {
                e.Handled = true;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void ImportProducts()
        {
            try
            {
                string json = File.ReadAllText("products.json");
                List<ProductData> productData = JsonConvert.DeserializeObject<List<ProductData>>(json);

                // Очистка таблицы перед импортом
                dataset.Products.Clear();

                // Добавление импортированных данных в таблицу
                foreach (var item in productData)
                {
                    DataRow newRow = dataset.Products.NewRow();
                    newRow["NameProduct"] = item.NameProduct;
                    newRow["DescriptionProduct"] = item.DescriptionProduct;
                    newRow["Price"] = item.Price;
                    newRow["CategoryID"] = item.CategoryID;
                    dataset.Products.Rows.Add(newRow);
                }

                productsAdapter.Update(dataset.Products);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при импорте данных продуктов: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            ImportProducts();
            // Добавьте вызовы других методов импорта для остальных таблиц, если необходимо
            // Обновите пользовательский интерфейс после импорта данных
            LoadData();
        }
    }
}
