using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Input;
using WpfApp10.MagAsiaDataSetTableAdapters;

namespace WpfApp10
{
    public partial class ReviewsWindow : Window
    {
        private MagAsiaDataSet dataset = new MagAsiaDataSet();
        private ReviewsTableAdapter reviewsAdapter = new ReviewsTableAdapter();
        private ProductsTableAdapter productsAdapter = new ProductsTableAdapter();
        private CustomersTableAdapter customersAdapter = new CustomersTableAdapter();

        public ReviewsWindow()
        {
            InitializeComponent();
            ReviewsGrid.ItemsSource = dataset.Reviews.DefaultView;
            LoadData();

            // Привязываем обработчики событий PreviewTextInput к текстовым полям для валидации
            RatingTextBox.PreviewTextInput += RatingTextBox_PreviewTextInput;
            ReviewsGrid.SelectionChanged += ReviewsGrid_SelectionChanged;
        }
      
        private void LoadData()
        {
            try
            {
                reviewsAdapter.Fill(dataset.Reviews);
                productsAdapter.Fill(dataset.Products);
                customersAdapter.Fill(dataset.Customers);

                // Загрузка данных для выпадающих списков
                ProductIDComboBox.DisplayMemberPath = "NameProduct"; // Устанавливаем отображаемое свойство для продуктов
                ProductIDComboBox.SelectedValuePath = "ProductID"; // Устанавливаем свойство, используемое для выбора значения
                ProductIDComboBox.ItemsSource = dataset.Products.DefaultView;

                CustomerIDComboBox.DisplayMemberPath = "CustomerID"; // Устанавливаем отображаемое свойство для клиентов
                CustomerIDComboBox.SelectedValuePath = "CustomerID"; // Устанавливаем свойство, используемое для выбора значения
                CustomerIDComboBox.ItemsSource = dataset.Customers.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductIDComboBox.Text) || string.IsNullOrWhiteSpace(CustomerIDComboBox.Text) ||
                string.IsNullOrWhiteSpace(RatingTextBox.Text) || string.IsNullOrWhiteSpace(ReviewDateTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                int rating = int.Parse(RatingTextBox.Text);
                if (rating < 1 || rating > 5)
                {
                    MessageBox.Show("Рейтинг должен быть в диапазоне от 1 до 5.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DataRow newRow = dataset.Reviews.NewRow();
                newRow["ProductID"] = int.Parse(ProductIDComboBox.SelectedValue.ToString());
                newRow["CustomerID"] = int.Parse(CustomerIDComboBox.SelectedValue.ToString());
                newRow["Rating"] = rating;
                newRow["ReviewText"] = ReviewTextBox.Text;
                newRow["ReviewDate"] = DateTime.Parse(ReviewDateTextBox.Text);

                dataset.Reviews.Rows.Add(newRow);

                reviewsAdapter.Update(dataset.Reviews);
                MessageBox.Show("Новый отзыв успешно добавлен.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении отзыва: " + ex.Message);
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReviewsGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)ReviewsGrid.SelectedItem;
                selectedRow.BeginEdit();
                selectedRow["ProductID"] = int.Parse(ProductIDComboBox.SelectedValue.ToString());
                selectedRow["CustomerID"] = int.Parse(CustomerIDComboBox.SelectedValue.ToString());
                int rating = int.Parse(RatingTextBox.Text);
                if (rating < 1 || rating > 5)
                {
                    MessageBox.Show("Рейтинг должен быть в диапазоне от 1 до 5.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    selectedRow.CancelEdit();
                    return;
                }
                selectedRow["Rating"] = rating;
                selectedRow["ReviewText"] = ReviewTextBox.Text;
                selectedRow["ReviewDate"] = DateTime.Parse(ReviewDateTextBox.Text);
                selectedRow.EndEdit();

                reviewsAdapter.Update(dataset.Reviews);
                MessageBox.Show("Отзыв успешно обновлен.");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReviewsGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)ReviewsGrid.SelectedItem;
                selectedRow.Delete();

                reviewsAdapter.Update(dataset.Reviews);
                MessageBox.Show("Отзыв успешно удален.");
            }
        }


       
            private void ReviewsGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ReviewsGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)ReviewsGrid.SelectedItem;
                ProductIDComboBox.SelectedValue = selectedRow["ProductID"];
                CustomerIDComboBox.SelectedValue = selectedRow["CustomerID"];
                RatingTextBox.Text = selectedRow["Rating"].ToString();
                ReviewTextBox.Text = selectedRow["ReviewText"].ToString();
                ReviewDateTextBox.Text = selectedRow["ReviewDate"].ToString();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void RatingTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем вводимые символы для поля с рейтингом
            if (!IsRatingValid(e.Text))
            {
                e.Handled = true;
            }
        }

        private bool IsRatingValid(string text)
        {
            // Проверяем, что вводимые символы являются цифрами
            return text.All(char.IsDigit);
        }

        private bool IsDateValid(string text)
        {
            // Проверяем, что вводимые символы являются цифрами или разделителями даты
            return text.All(char.IsDigit) || text == "/" || text == ".";
        }

        private void ReviewDateTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
