using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WpfApp10.MagAsiaDataSetTableAdapters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace WpfApp10
{
    public partial class CategoriesWindow : Window
    {
        private MagAsiaDataSet dataset = new MagAsiaDataSet();
        private CategoriesTableAdapter categoriesAdapter = new CategoriesTableAdapter();

        public CategoriesWindow()
        {
            InitializeComponent();
            CategoriesGrid.ItemsSource = dataset.Categories.DefaultView;
            LoadData();
            NameCategoryTextBox.PreviewTextInput += NameCategoryTextBox_PreviewTextInput;
            CategoriesGrid.MouseLeftButtonUp += CategoriesGrid_MouseLeftButtonUp;
        }
        private void CategoriesGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (CategoriesGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)CategoriesGrid.SelectedItem;
                NameCategoryTextBox.Text = selectedRow["NameCategory"].ToString();
            }
        }
        private void LoadData()
        {
            try
            {
                categoriesAdapter.Fill(dataset.Categories);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameCategoryTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните название категории.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataRow newRow = dataset.Categories.NewRow();
            newRow["NameCategory"] = NameCategoryTextBox.Text;

            dataset.Categories.Rows.Add(newRow);

            categoriesAdapter.Update(dataset.Categories);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)CategoriesGrid.SelectedItem;
                selectedRow.BeginEdit();
                selectedRow["NameCategory"] = NameCategoryTextBox.Text;
                selectedRow.EndEdit();

                categoriesAdapter.Update(dataset.Categories); // Предполагается, что у вас есть categoriesAdapter
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)CategoriesGrid.SelectedItem;
                selectedRow.Delete();

                categoriesAdapter.Update(dataset.Categories);
            }
        }

        
        private void NameCategoryTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"^[a-zA-Zа-яА-Я]+$"))
            {
                e.Handled = true;
            }
        }

        private void ImportCategories()
        {
            try
            {
                string json = File.ReadAllText("categories.json");
                List<CategoryData> categoryData = JsonConvert.DeserializeObject<List<CategoryData>>(json);

                dataset.Categories.Clear();

                foreach (var item in categoryData)
                {
                    DataRow newRow = dataset.Categories.NewRow();
                    newRow["NameCategory"] = item.NameCategory;
                    dataset.Categories.Rows.Add(newRow);
                }

                categoriesAdapter.Update(dataset.Categories);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при импорте данных категорий: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            ImportCategories();
            // Добавьте вызовы других методов импорта для остальных таблиц, если необходимо
            // Обновите пользовательский интерфейс после импорта данных
            LoadData();
        }
        private void CategoriesGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CategoriesGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)CategoriesGrid.SelectedItem;
                NameCategoryTextBox.Text = selectedRow["NameCategory"].ToString();
            }
        } 
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }

    // Определите класс данных для десериализации данных из JSON файла
    public class CategoryData
    {
        public string NameCategory { get; set; }
        // Добавьте другие поля, если они есть
    }
}