using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using WpfApp10.MagAsiaDataSetTableAdapters;


namespace WpfApp10
{
    public partial class ReviewWindow : Window
    {
        private readonly int _customerId;
        private readonly MagAsiaDataSet _dataset = new MagAsiaDataSet();
        private readonly ProductsTableAdapter _productsTableAdapter = new ProductsTableAdapter();
        private readonly ReviewsTableAdapter _reviewsTableAdapter = new ReviewsTableAdapter();

        public ReviewWindow(int customerId)
        {
            InitializeComponent();
            _customerId = customerId;
            LoadProducts();
            LoadReviews();
        }

        private void LoadProducts()
        {
            try
            {
                _productsTableAdapter.Fill(_dataset.Products);
                ProductComboBox.ItemsSource = _dataset.Products.Select(product => product.NameProduct).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке продуктов: " + ex.Message);
            }
        }

        private void AddReviewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ProductComboBox.SelectedItem != null && !string.IsNullOrWhiteSpace(ReviewTextBox.Text))
                {
                    string selectedProductName = ProductComboBox.SelectedItem.ToString();
                    int productId = _dataset.Products
                        .Where(product => product.NameProduct == selectedProductName)
                        .Select(product => product.ProductID)
                        .FirstOrDefault();

                    if (int.TryParse(RatingTextBox.Text, out int rating) && rating >= 1 && rating <= 5)
                    {
                        string reviewText = ReviewTextBox.Text;
                        DateTime reviewDate = DateTime.Now;

                        _reviewsTableAdapter.Insert(productId, _customerId, rating, reviewText, reviewDate);
                        LoadReviews();
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, введите корректный рейтинг от 1 до 5.");
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите продукт и введите текст отзыва.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении отзыва: {ex.Message}");
            }
        }

        private void LoadReviews()
        {
            try
            {
                _reviewsTableAdapter.Fill(_dataset.Reviews);
                ReviewGrd.ItemsSource = new ObservableCollection<MagAsiaDataSet.ReviewsRow>(_dataset.Reviews);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке отзывов: " + ex.Message);
            }
        }

        private void DeleteReviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReviewGrd.SelectedItem != null)
            {
                MagAsiaDataSet.ReviewsRow selectedReview = (MagAsiaDataSet.ReviewsRow)ReviewGrd.SelectedItem;
                int reviewId = selectedReview.ReviewID;
                try
                {
                    _reviewsTableAdapter.DeleteQuery(reviewId); 
                    LoadReviews();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении отзыва: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите отзыв для удаления.");
            }
        }

        private void UpdateReviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReviewGrd.SelectedItem != null)
            {
                MagAsiaDataSet.ReviewsRow selectedReview = (MagAsiaDataSet.ReviewsRow)ReviewGrd.SelectedItem;

                try
                {
                    selectedReview.Rating = int.Parse(RatingTextBox.Text);
                    selectedReview.ReviewText = ReviewTextBox.Text;

                    _reviewsTableAdapter.Update(selectedReview);
                    LoadReviews();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении отзыва: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите отзыв для обновления.");
            }
        }

        private void DWprg_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите завершить работу приложения?",
                                                       "Подтверждение выхода",
                                                       MessageBoxButton.YesNo,
                                                       MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
