using System;
using System.Data;
using System.Windows;
using WpfApp10.MagAsiaDataSetTableAdapters;

namespace WpfApp10
{
    public partial class PaymentsWindow : Window
    {
        private MagAsiaDataSet dataset = new MagAsiaDataSet();
        private PaymentsTableAdapter paymentsAdapter = new PaymentsTableAdapter();

        public PaymentsWindow()
        {
            InitializeComponent();
            PaymentsGrid.ItemsSource = dataset.Payments.DefaultView;
            LoadData();

            // Привязываем обработчик события PreviewTextInput для валидации ввода только букв
            PaymentMethodTextBox.PreviewTextInput += TextBox_PreviewTextInput;

            // Добавляем обработчик события MouseDoubleClick для таблицы
            PaymentsGrid.MouseDoubleClick += PaymentsGrid_MouseDoubleClick;
        }
        private void PaymentsGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (PaymentsGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)PaymentsGrid.SelectedItem;
                PaymentMethodTextBox.Text = selectedRow["PaymentMethod"].ToString();
            }
        }

        private void LoadData()
        {
            try
            {
                paymentsAdapter.Fill(dataset.Payments);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PaymentMethodTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните поле 'Метод оплаты'.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataRow newRow = dataset.Payments.NewRow();
            newRow["PaymentMethod"] = PaymentMethodTextBox.Text;

            dataset.Payments.Rows.Add(newRow);

            paymentsAdapter.Update(dataset.Payments);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentsGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)PaymentsGrid.SelectedItem;
                selectedRow.BeginEdit();
                selectedRow["PaymentMethod"] = PaymentMethodTextBox.Text;
                selectedRow.EndEdit();

                paymentsAdapter.Update(dataset.Payments);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentsGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)PaymentsGrid.SelectedItem;
                selectedRow.Delete();

                paymentsAdapter.Update(dataset.Payments);
            }
        }

        private void PaymentsGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PaymentsGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)PaymentsGrid.SelectedItem;
                PaymentMethodTextBox.Text = selectedRow["PaymentMethod"].ToString();
            }
        }

        // Обработчик события для валидации ввода только букв
        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"^[а-яА-Яa-zA-Z\s]+$"))
            {
                e.Handled = true; // Отмена ввода, если символ не является буквой
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
