using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using WpfApp10.MagAsiaDataSetTableAdapters;

namespace WpfApp10
{
    public partial class IngredientsWindow : Window
    {
        private MagAsiaDataSet dataset = new MagAsiaDataSet();
        private IngredientsTableAdapter ingredientsAdapter = new IngredientsTableAdapter();

        public IngredientsWindow()
        {
            InitializeComponent();
            IngredientsGrid.ItemsSource = dataset.Ingredients.DefaultView;
            LoadData();

            // Привязываем обработчик события PreviewTextInput к текстовому полю для валидации
            NameIngredientTextBox.PreviewTextInput += TextBoxName_PreviewTextInput;

            // Устанавливаем обработчик события для изменения выбора строки в таблице
            IngredientsGrid.SelectionChanged += IngredientsGrid_SelectionChanged;
        }

        private void IngredientsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IngredientsGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)IngredientsGrid.SelectedItem;
                NameIngredientTextBox.Text = selectedRow["NameIngredient"].ToString();
                DescriptionIngredientTextBox.Text = selectedRow["DescriptionIngredient"].ToString();
            }
        }

        private void LoadData()
        {
            try
            {
                ingredientsAdapter.Fill(dataset.Ingredients);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameIngredientTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните поле \"Наименование\".", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataRow newRow = dataset.Ingredients.NewRow();
            newRow["NameIngredient"] = NameIngredientTextBox.Text;
            newRow["DescriptionIngredient"] = DescriptionIngredientTextBox.Text;

            dataset.Ingredients.Rows.Add(newRow);

            ingredientsAdapter.Update(dataset.Ingredients);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (IngredientsGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)IngredientsGrid.SelectedItem;
                selectedRow.BeginEdit();
                selectedRow["NameIngredient"] = NameIngredientTextBox.Text;
                selectedRow["DescriptionIngredient"] = DescriptionIngredientTextBox.Text;
                selectedRow.EndEdit();

                ingredientsAdapter.Update(dataset.Ingredients);
                MessageBox.Show("Запись успешно обновлена.");
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите запись для обновления.");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (IngredientsGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)IngredientsGrid.SelectedItem;
                selectedRow.Delete();

                ingredientsAdapter.Update(dataset.Ingredients);
            }
        }

        

        // Обработчик события для валидации ввода только букв
        private void TextBoxName_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                // Проверяем, что вводятся только буквы
                if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"^[a-zA-Zа-яА-Я]+$"))
                {
                    e.Handled = true; // Отмена ввода, если символ не является буквой
                }
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
