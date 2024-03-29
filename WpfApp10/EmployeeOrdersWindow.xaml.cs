using System;
using System.Data;
using System.Windows;
using WpfApp10.MagAsiaDataSetTableAdapters;

namespace WpfApp10
{
    public partial class EmployeeOrdersWindow : Window
    {
        private MagAsiaDataSet dataset = new MagAsiaDataSet();
        private EmployeeOrdersTableAdapter employeeOrdersAdapter = new EmployeeOrdersTableAdapter();
        private EmployeesTableAdapter employeesAdapter = new EmployeesTableAdapter();
        private OrdersTableAdapter ordersAdapter = new OrdersTableAdapter();

        public EmployeeOrdersWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                employeeOrdersAdapter.Fill(dataset.EmployeeOrders);
                employeesAdapter.Fill(dataset.Employees);
                ordersAdapter.Fill(dataset.Orders);

                EmployeeOrdersGrid.ItemsSource = dataset.EmployeeOrders.DefaultView;

                // Установка источников данных для комбобоксов
                EmployeeComboBox.ItemsSource = dataset.Employees.DefaultView;
                EmployeeComboBox.DisplayMemberPath = "FirstNameE"; // Указываем поле, которое должно отображаться в комбобоксе
                EmployeeComboBox.SelectedValuePath = "EmployeeID"; // Указываем поле, которое должно использоваться для выбора значения

                OrderComboBox.ItemsSource = dataset.Orders.DefaultView;
                OrderComboBox.DisplayMemberPath = "OrderID"; // Указываем поле, которое должно отображаться в комбобоксе
                OrderComboBox.SelectedValuePath = "OrderID"; // Указываем поле, которое должно использоваться для выбора значения
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRow newRow = dataset.EmployeeOrders.NewRow();
                newRow["EmployeeID"] = (int)EmployeeComboBox.SelectedValue;
                newRow["OrderID"] = (int)OrderComboBox.SelectedValue;

                dataset.EmployeeOrders.Rows.Add(newRow);
                employeeOrdersAdapter.Update(dataset.EmployeeOrders);
                MessageBox.Show("Новая запись успешно добавлена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении записи: " + ex.Message);
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selectedRow = (DataRowView)EmployeeOrdersGrid.SelectedItem;
                if (selectedRow != null)
                {
                    selectedRow.BeginEdit();
                    selectedRow["EmployeeID"] = (int)EmployeeComboBox.SelectedValue;
                    selectedRow["OrderID"] = (int)OrderComboBox.SelectedValue;
                    selectedRow.EndEdit();

                    employeeOrdersAdapter.Update(dataset.EmployeeOrders);
                    MessageBox.Show("Запись успешно обновлена.");
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите запись для обновления.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении записи: " + ex.Message);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selectedRow = (DataRowView)EmployeeOrdersGrid.SelectedItem;
                if (selectedRow != null)
                {
                    if (MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        selectedRow.Delete();
                        employeeOrdersAdapter.Update(dataset.EmployeeOrders);
                        MessageBox.Show("Запись успешно удалена.");
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите запись для удаления.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении записи: " + ex.Message);
            }
        }

        private void EmployeeOrdersGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView selectedRow = (DataRowView)EmployeeOrdersGrid.SelectedItem;
                if (selectedRow != null)
                {
                    // Загрузка значений выбранной записи в элементы управления
                    EmployeeComboBox.SelectedValue = selectedRow["EmployeeID"];
                    OrderComboBox.SelectedValue = selectedRow["OrderID"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке выбранной записи: " + ex.Message);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}