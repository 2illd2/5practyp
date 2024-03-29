using System;
using System.Data;
using System.Windows;
using WpfApp10.MagAsiaDataSetTableAdapters;

namespace WpfApp10
{
    public partial class ProductIngredientsWindow : Window
    {
        private MagAsiaDataSet dataset = new MagAsiaDataSet();
        private ProductIngredientsTableAdapter productIngredientsAdapter = new ProductIngredientsTableAdapter();
        private ProductsTableAdapter productsAdapter = new ProductsTableAdapter();
        private IngredientsTableAdapter ingredientsAdapter = new IngredientsTableAdapter();

        public ProductIngredientsWindow()
        {
            InitializeComponent();
            ProductIngredientsGrid.ItemsSource = dataset.ProductIngredients.DefaultView;
            LoadData();
            LoadProductNames();
            LoadIngredientNames();
        }

        private void LoadData()
        {
            try
            {
                productIngredientsAdapter.Fill(dataset.ProductIngredients);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void LoadProductNames()
        {
            try
            {
                productsAdapter.Fill(dataset.Products);
                ProductComboBox.ItemsSource = dataset.Products.DefaultView;
                ProductComboBox.DisplayMemberPath = "NameProduct"; // Замените "ProductName" на имя столбца с названием продукта
                ProductComboBox.SelectedValuePath = "ProductID"; // Замените "ProductID" на имя столбца с идентификатором продукта
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке названий продуктов: " + ex.Message);
            }
        }

        private void LoadIngredientNames()
        {
            try
            {
                ingredientsAdapter.Fill(dataset.Ingredients);
                IngredientComboBox.ItemsSource = dataset.Ingredients.DefaultView;
                IngredientComboBox.DisplayMemberPath = "NameIngredient"; // Замените "NameIngredient" на имя столбца с названием ингредиента
                IngredientComboBox.SelectedValuePath = "IngredientID"; // Замените "IngredientID" на имя столбца с идентификатором ингредиента
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке названий ингредиентов: " + ex.Message);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductComboBox.SelectedItem == null || IngredientComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите как продукт, так и ингредиент.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataRow newRow = dataset.ProductIngredients.NewRow();
            newRow["ProductID"] = ProductComboBox.SelectedValue;
            newRow["IngredientID"] = IngredientComboBox.SelectedValue;

            dataset.ProductIngredients.Rows.Add(newRow);

            productIngredientsAdapter.Update(dataset.ProductIngredients);
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductIngredientsGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)ProductIngredientsGrid.SelectedItem;
                selectedRow.BeginEdit();
                selectedRow["ProductID"] = ProductComboBox.SelectedValue;
                selectedRow["IngredientID"] = IngredientComboBox.SelectedValue;
                selectedRow.EndEdit();

                productIngredientsAdapter.Update(dataset.ProductIngredients);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите запись для обновления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductIngredientsGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)ProductIngredientsGrid.SelectedItem;
                selectedRow.Delete();

                productIngredientsAdapter.Update(dataset.ProductIngredients);
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}