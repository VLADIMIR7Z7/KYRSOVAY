using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FreightTransportSystem
{
    // Форма для управления заказами
    public partial class OrderManagerForm : Form
    {
        // DataGridView для отображения заказов
        private DataGridView dataGridViewOrders;


        // DataGridView для отображения информации о грузах
        private DataGridView dataGridView2;



        // Столбцы для DataGridView с информацией о юридических лицах
        private DataGridViewTextBoxColumn CompanyNameNEW;
        private DataGridViewTextBoxColumn DirectorName;
        private DataGridViewTextBoxColumn LegalAddress;
        private DataGridViewTextBoxColumn LegalEntityPhone;
        private DataGridViewTextBoxColumn BankName;
        private DataGridViewTextBoxColumn AccountNumber;
        private DataGridViewTextBoxColumn INN;
        private DataGridViewTextBoxColumn ContactName;
        private DataGridViewTextBoxColumn Phone;
        private DataGridViewTextBoxColumn PassportSeries;
        private DataGridViewTextBoxColumn PassportNumber;
        private DataGridViewTextBoxColumn IssueDate;
        private DataGridViewTextBoxColumn IssuedBy;

        // Элементы управления для ввода данных о заказе
        private DateTimePicker dtpOrderDate;
        private TextBox txtSenderName;
        private TextBox txtOrderCost;
        private TextBox txtUnloadingAddress;
        private TextBox txtRouteLength;
        private TextBox txtReceiverName;
        private TextBox txtLoadingAddress;

        // Кнопки для управления заказами
        private Button btnAddOrder;
        private Button btnRemoveOrder;
        private Button btnClearFields;

        // Метки для отображения информации
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;

        // Кнопка для добавления информации о грузе
        private Button btnAddCargo;

        // Элементы управления для ввода данных о грузе
        private TextBox txtCargoName;
        private TextBox txtCargoInsuranceValue;
        private TextBox txtCargoWeight;
        private TextBox txtCargoQuantity;
        private TextBox txtCargoUnit;

        // Метки для отображения информации о грузе
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private DataGridViewTextBoxColumn OrderId;
        private DataGridViewTextBoxColumn OrderDate;
        private DataGridViewTextBoxColumn SenderName;
        private DataGridViewTextBoxColumn LoadingAddress;
        private DataGridViewTextBoxColumn ReceiverName;
        private DataGridViewTextBoxColumn UnloadingAddress;
        private DataGridViewTextBoxColumn RouteLength;
        private DataGridViewTextBoxColumn OrderCost;
        private DataGridView dataGridViewCargo;
        private Button btnEditOrder;
        private Button btnEditCargo;
        private DataGridViewTextBoxColumn CargoId;
        private DataGridViewTextBoxColumn CargoName;
        private DataGridViewTextBoxColumn CargoUnit;
        private DataGridViewTextBoxColumn CargoQuantity;
        private DataGridViewTextBoxColumn CargoWeight;
        private DataGridViewTextBoxColumn CargoInsuranceValue;
        private Button btnRemoveCargo;
        private Label label16;
        private Label label17;
        private Button btnSaveOrderChanges;
        private Button btnSaveCargoChanges;
        private Button btnShowCargo;
        private Button btnSelectSender;
        private Button btnSelectReceiver;

        // Дополнительный DataGridView (возможно, для других данных)
        private DataGridView dataGridView1;

        // Конструктор формы
        public OrderManagerForm()
        {
            InitializeComponent(); // Инициализация компонентов формы
            LoadOrders(); // Загрузка заказов при инициализации формы
            LoadClientData(); // Загрузка данных клиентов при инициализации формы
            ClearFields();
        }

        // Обработка события закрытия формы
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            DataStorage.SaveData(); // Сохранение данных перед закрытием формы
        }

        // Метод для загрузки заказов в DataGridView
        private void LoadOrders()
        {
            dataGridViewOrders.Rows.Clear(); // Очистка текущих строк в DataGridView
            foreach (var order in OrderManager.GetOrders())
            {
                dataGridViewOrders.Rows.Add(
                    order.OrderId, // Номер заказа
                    order.OrderDate, // Дата заказа
                    order.SenderName, // Имя отправителя
                    order.LoadingAddress, // Адрес загрузки
                    order.ReceiverName, // Имя получателя
                    order.UnloadingAddress, // Адрес разгрузки
                    order.RouteLength, // Длина маршрута
                    order.OrderCost // Стоимость заказа
                );
            }
        }
        
        private void LoadCargoForOrder(int orderId)
        {
            dataGridViewCargo.Rows.Clear(); // Очистка текущих строк в DataGridView для грузов
            var order = OrderManager.GetOrders().FirstOrDefault(o => o.OrderId == orderId); //  Поиск заказа по его идентификатору
            if (order != null)
            {
                int cargoId = 1; // Начинаем с 1
                foreach (var cargo in order.CargoList)
                {
                    dataGridViewCargo.Rows.Add(
                        cargoId++, // Номер груза
                        cargo.Name, // Название груза
                        cargo.Unit, // Единица измерения
                        cargo.Quantity, // Количество
                        cargo.Weight, // Вес
                        cargo.InsuranceValue // Страховая стоимость
                    );
                }
            }
        }


        // Метод для загрузки данных клиентов в DataGridView
        private void LoadClientData()
        {
            // Загрузка данных клиентов в DataGridView
            var individualClients = ClientManager.GetClients().OfType<IndividualClient>().ToList();
            var legalEntityClients = ClientManager.GetClients().OfType<LegalEntityClient>().ToList();

            // Заполнение DataGridView для физических лиц
            dataGridView1.Rows.Clear();
            foreach (var client in individualClients)
            {
                dataGridView1.Rows.Add(client.ContactName, client.Phone, client.PassportSeries, client.PassportNumber, client.IssueDate.ToShortDateString(), client.IssuedBy);
            }

            // Заполнение DataGridView для юридических лиц
            dataGridView2.Rows.Clear();
            foreach (var client in legalEntityClients)
            {
                dataGridView2.Rows.Add(client.CompanyName, client.DirectorName, client.LegalAddress, client.Phone, client.BankName, client.AccountNumber, client.INN);
            }
        }

        // Метод для очистки полей ввода
        private void ClearFields()
        {
            txtSenderName.Clear();
            txtLoadingAddress.Clear();
            txtReceiverName.Clear();
            txtUnloadingAddress.Clear();
            txtRouteLength.Clear();
            txtOrderCost.Clear();
            dtpOrderDate.Value = DateTime.Now; // Устанавливаем дату на текущее значение
            txtCargoName.Clear();
            txtCargoUnit.Clear();
            txtCargoQuantity.Clear();
            txtCargoWeight.Clear();
            txtCargoInsuranceValue.Clear();
        }

        // Обработчик события нажатия кнопки "Добавить заказ"
        private void btnAddOrder_Click_1(object sender, EventArgs e)
        {
           
            // Генерация нового уникального номера заказа
            int orderId = OrderManager.GetOrders().Any() ? OrderManager.GetOrders().Max(o => o.OrderId) + 1 : 1;
            
            // Cчитываение данных
            DateTime orderDate = dtpOrderDate.Value; 
            string senderName = txtSenderName.Text.Trim();
            string loadingAddress = txtLoadingAddress.Text.Trim();
            string receiverName = txtReceiverName.Text.Trim();
            string unloadingAddress = txtUnloadingAddress.Text.Trim();

            List<string> errors = new List<string>();
            // Проверка длины маршрута
            if (!double.TryParse(txtRouteLength.Text, out double routeLength) || routeLength <= 0 || routeLength > 10000000)
            {
                errors.Add("Длина маршрута должна быть числом от 1 до 10,000,000.");
            }

            // Проверка стоимости
            if (!double.TryParse(txtOrderCost.Text, out double orderCost) || orderCost <= 0 || orderCost > 100000)
            {
                errors.Add("Стоимость заказа должна быть числом от 1 до 100,000.");
            }

            // Проверка существования отправителя
            bool senderExists = ClientManager.GetClients().OfType<IndividualClient>()
                .Any(c => c.ContactName.Equals(senderName, StringComparison.OrdinalIgnoreCase)) ||
                ClientManager.GetClients().OfType<LegalEntityClient>()
                .Any(c => c.CompanyName.Equals(senderName, StringComparison.OrdinalIgnoreCase));

            // Проверка существования получателя
            bool receiverExists = ClientManager.GetClients().OfType<IndividualClient>()
                .Any(c => c.ContactName.Equals(receiverName, StringComparison.OrdinalIgnoreCase)) ||
                ClientManager.GetClients().OfType<LegalEntityClient>()
                .Any(c => c.CompanyName.Equals(receiverName, StringComparison.OrdinalIgnoreCase));

            // Если отправитель или получатель не существуют, добавляем сообщение об ошибке
            if (!senderExists)
            {
                errors.Add("Отправитель не существует. Пожалуйста, проверьте введенные данные.");
            }

            if (!receiverExists)
            {
                errors.Add("Получатель не существует. Пожалуйста, проверьте введенные данные.");
            }

            // Проверка адресов
            if (!Regex.IsMatch(loadingAddress, @"^[a-zA-Zа-яА-Я0-9/ ]+$"))
            {
                errors.Add("Адрес загрузки может содержать только буквы, цифры и символ '/'.");
            }

            if (!Regex.IsMatch(unloadingAddress, @"^[a-zA-Zа-яА-Я0-9/ ]+$"))
            {
                errors.Add("Адрес разгрузки может содержать только буквы, цифры и символ '/'.");
            }

            // Если есть ошибки, выводим их
            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join(Environment.NewLine, errors), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Создание нового заказа
            CargoOrder newOrder = new CargoOrder(orderId, orderDate, senderName, loadingAddress, receiverName, unloadingAddress, routeLength, orderCost, new List<Cargo>());
            OrderManager.AddOrder(newOrder); // Добавление заказа в менеджер заказов
            LoadOrders(); // Обновление отображения заказов
            ClearFields(); // Очистка полей ввода
        }

        // Обработчик события нажатия кнопки "Добавить груз"
        private void btnAddCargo_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0) // Проверка, выбрана ли строка заказа
            {
                var selectedOrderRow = dataGridViewOrders.SelectedRows[0]; // Получение выбранной строки
                int orderId = (int)selectedOrderRow.Cells["OrderId"].Value; // Получаем номер заказа

                // Ищем заказ по номеру
                CargoOrder selectedOrder = OrderManager.GetOrders().FirstOrDefault(o => o.OrderId == orderId);

                if (selectedOrder != null)
                {
                    List<string> errors = new List<string>();

                    // Получаем данные о грузе из текстовых полей
                    string cargoName = txtCargoName.Text.Trim();
                    string cargoUnit = txtCargoUnit.Text.Trim();

                    // Проверка названия груза
                    if (!Regex.IsMatch(cargoName, @"^[a-zA-Zа-яА-Я ]+$"))
                    {
                        errors.Add("Название груза может содержать только буквы.");
                    }

                    // Проверка единицы измерения
                    if (!Regex.IsMatch(cargoUnit, @"^[a-zA-Zа-яА-Я ]+$"))
                    {
                        errors.Add("Единица измерения может содержать только буквы.");
                    }

                    // Проверка количества
                    if (!double.TryParse(txtCargoQuantity.Text, out double cargoQuantity) || cargoQuantity <= 0 || cargoQuantity > 100)
                    {
                        errors.Add("Количество должно быть числом от 1 до 100.");
                    }

                    // Проверка веса
                    if (!double.TryParse(txtCargoWeight.Text, out double cargoWeight) || cargoWeight <= 0 || cargoWeight > 10000000)
                    {
                        errors.Add("Вес должен быть числом от 1 до 10,000,000.");
                    }

                    // Проверка страховой стоимости
                    if (!double.TryParse(txtCargoInsuranceValue.Text, out double cargoInsuranceValue) || cargoInsuranceValue <= 0 || cargoInsuranceValue > 10000000)
                    {
                        errors.Add("Страховая стоимость должна быть числом от 1 до 10,000,000.");
                    }

                    // Если есть ошибки, выводим их
                    if (errors.Count > 0)
                    {
                        MessageBox.Show(string.Join(Environment.NewLine, errors), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Генерация нового уникального номера груза
                    int cargoId = selectedOrder.CargoList.Any() ? selectedOrder.CargoList.Max(c => c.CargoId) + 1 : 1;

                    // Создаем новый объект Cargo
                    Cargo newCargo = new Cargo(cargoId, cargoName, cargoUnit, cargoQuantity, cargoWeight, cargoInsuranceValue);
                    selectedOrder.CargoList.Add(newCargo); // Добавляем новый груз

                    // Обновляем отображение грузов
                    LoadCargoForOrder(orderId); // Обновляем отображение грузов для текущего заказа
                    ClearFields(); // Очистка полей ввода
                    DataStorage.SaveData(); // Сохраняем данные в файл

                    MessageBox.Show("Груз успешно добавлен к заказу.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Заказ с указанным номером не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ из таблицы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        
        private void btnRemoveOrder_Click_1(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0)
            {
                // Получаем выбраный элемент
                var selectedRow = dataGridViewOrders.SelectedRows[0];

                // Получение id заказа из ячейки "OrderId" и преобразование его в целое число
                int orderId = (int)selectedRow.Cells["OrderId"].Value;

                // Удаление заказа по номеру заказа
                OrderManager.RemoveOrder(orderId);
                LoadOrders(); // Обновление списка заказов
                dataGridViewCargo.Rows.Clear(); // Очистка грузов, связанных с удаленным заказом
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

       
        private void btnClearFields_Click_1(object sender, EventArgs e)
        {
            ClearFields(); 
        }

        private void btnEditOrder_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0) // Проверка, выбрана ли строка
            {
                var selectedRow = dataGridViewOrders.SelectedRows[0]; // Получение выбранной строки

                // Прверка на получение Id
                if (selectedRow.Cells["OrderId"].Value != null)
                {
                    // Получение id заказа из ячейки "OrderId" и преобразование его в целое число
                    int orderId = (int)selectedRow.Cells["OrderId"].Value; 
                    CargoOrder orderToEdit = OrderManager.GetOrders().FirstOrDefault(o => o.OrderId == orderId); // Поиск заказа по номеру

                    if (orderToEdit != null) // Если заказ найден
                    {
                        // Заполнение полей данными из объекта
                        txtSenderName.Text = orderToEdit.SenderName;
                        txtLoadingAddress.Text = orderToEdit.LoadingAddress;
                        txtReceiverName.Text = orderToEdit.ReceiverName;
                        txtUnloadingAddress.Text = orderToEdit.UnloadingAddress;
                        txtRouteLength.Text = orderToEdit.RouteLength.ToString();
                        txtOrderCost.Text = orderToEdit.OrderCost.ToString();
                        dtpOrderDate.Value = orderToEdit.OrderDate; // Установка даты заказа
                    }
                    else
                    {
                        MessageBox.Show("Заказ не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Выбранная строка пуста. Редактирование невозможно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEditCargo_Click(object sender, EventArgs e)
        {
            if (dataGridViewCargo.SelectedRows.Count > 0) // Проверка, выбрана ли строка
            {
                var selectedRow = dataGridViewCargo.SelectedRows[0]; // Получение выбранной строки

                // Проверка выбран ли Id
                if (selectedRow.Cells["CargoId"].Value != null)
                {
                    // Получение id заказа из ячейки "CargoerId" и преобразование его в целое число
                    int cargoId = (int)selectedRow.Cells["CargoId"].Value;
                    Cargo cargoToEdit = OrderManager.GetOrders() 
                        .SelectMany(o => o.CargoList) // Получаем все грузы из всех заказов
                        .FirstOrDefault(c => c.CargoId == cargoId); // Поиск груза по номеру

                    if (cargoToEdit != null) // Если груз найден
                    {
                        // Заполнение полей данными из объекта
                        txtCargoName.Text = cargoToEdit.Name;
                        txtCargoUnit.Text = cargoToEdit.Unit;
                        txtCargoQuantity.Text = cargoToEdit.Quantity.ToString();
                        txtCargoWeight.Text = cargoToEdit.Weight.ToString();
                        txtCargoInsuranceValue.Text = cargoToEdit.InsuranceValue.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Груз не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Выбранная строка пуста. Редактирование невозможно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите груз для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnRemoveCargo_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0) // Проверка, выбрана ли строка заказа
            {
                var selectedOrderRow = dataGridViewOrders.SelectedRows[0]; // Получение выбранной строки 
                int orderId = (int)selectedOrderRow.Cells["OrderId"].Value; // Получаем номер заказа

                var order = OrderManager.GetOrders().FirstOrDefault(o => o.OrderId == orderId); // Поиск заказа по номеру
                if (order != null)
                {
                    // Проверяем, что выбран груз
                    if (dataGridViewCargo.SelectedRows.Count > 0)
                    {
                        var selectedCargoRow = dataGridViewCargo.SelectedRows[0]; // Получение выбранной строки
                        int cargoIndex = selectedCargoRow.Index; // Получаем индекс выбранного груза

                        // Проверяем, что индекс в пределах допустимого диапазона
                        if (cargoIndex >= 0 && cargoIndex < order.CargoList.Count)
                        {
                            // Запрашиваем подтверждение удаления
                            DialogResult dialogResult = MessageBox.Show($"Вы уверены, что хотите удалить груз ?", "Подтверждение удаления", MessageBoxButtons.YesNo);

                            // Если пользователь подтвердил удаление
                            if (dialogResult == DialogResult.Yes)
                            {
                                order.CargoList.RemoveAt(cargoIndex); // Удаляем груз по индексу
                                LoadCargoForOrder(orderId); // Обновляем отображение грузов
                                MessageBox.Show("Груз успешно удален.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Груз не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, выберите груз для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Заказ не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ перед удалением груза.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void btnSaveOrderChanges_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0) // Проверка, выбрана ли строка
            {
                var selectedRow = dataGridViewOrders.SelectedRows[0]; // Получение выбранной строки

                // Проверка что выбран номер заказа
                if (selectedRow.Cells["OrderId"].Value != null)
                {
                    int orderId = (int)selectedRow.Cells["OrderId"].Value; // Получаем номер заказа
                    CargoOrder orderToEdit = OrderManager.GetOrders().FirstOrDefault(o => o.OrderId == orderId); // Поиск заказа по номеру

                    if (orderToEdit != null) // Если заказ найден
                    {

                        List<string> errors = new List<string>();


                        // Считываем данные из полей 
                        DateTime orderDate = dtpOrderDate.Value;
                        string senderName = txtSenderName.Text.Trim();
                        string loadingAddress = txtLoadingAddress.Text.Trim();
                        string receiverName = txtReceiverName.Text.Trim();
                        string unloadingAddress = txtUnloadingAddress.Text.Trim();

                        // Проверка длины маршрута
                        if (!double.TryParse(txtRouteLength.Text, out double routeLength) || routeLength <= 0 || routeLength > 10000000)
                        {
                            errors.Add("Длина маршрута должна быть числом от 1 до 10,000,000.");
                        }

                        // Проверка стоимости
                        if (!double.TryParse(txtOrderCost.Text, out double orderCost) || orderCost <= 0 || orderCost > 100000)
                        {
                            errors.Add("Стоимость заказа должна быть числом от 1 до 100,000.");
                        }

                        // Проверка существования отправителя
                        bool senderExists = ClientManager.GetClients().OfType<IndividualClient>()
                            .Any(c => c.ContactName.Equals(senderName, StringComparison.OrdinalIgnoreCase)) ||
                            ClientManager.GetClients().OfType<LegalEntityClient>()
                            .Any(c => c.CompanyName.Equals(senderName, StringComparison.OrdinalIgnoreCase));

                        // Проверка существования получателя
                        bool receiverExists = ClientManager.GetClients().OfType<IndividualClient>()
                            .Any(c => c.ContactName.Equals(receiverName, StringComparison.OrdinalIgnoreCase)) ||
                            ClientManager.GetClients().OfType<LegalEntityClient>()
                            .Any(c => c.CompanyName.Equals(receiverName, StringComparison.OrdinalIgnoreCase));

                        // Если отправитель или получатель не существуют, добавляем сообщение об ошибке
                        if (!senderExists)
                        {
                            errors.Add("Отправитель не существует. Пожалуйста, проверьте введенные данные.");
                        }

                        if (!receiverExists)
                        {
                            errors.Add("Получатель не существует. Пожалуйста, проверьте введенные данные.");
                        }

                        // Проверка адресов
                        if (!Regex.IsMatch(loadingAddress, @"^[a-zA-Zа-яА-Я0-9/ ]+$"))
                        {
                            errors.Add("Адрес загрузки может содержать только буквы, цифры и символ '/'.");
                        }

                        if (!Regex.IsMatch(unloadingAddress, @"^[a-zA-Zа-яА-Я0-9/ ]+$"))
                        {
                            errors.Add("Адрес разгрузки может содержать только буквы, цифры и символ '/'.");
                        }

                        // Если есть ошибки, выводим их
                        if (errors.Count > 0)
                        {
                            MessageBox.Show(string.Join(Environment.NewLine, errors), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Проверка на изменения
                        if (txtSenderName.Text.Trim() != orderToEdit.SenderName ||
                            txtLoadingAddress.Text.Trim() != orderToEdit.LoadingAddress ||
                            txtReceiverName.Text.Trim() != orderToEdit.ReceiverName ||
                            txtUnloadingAddress.Text.Trim() != orderToEdit.UnloadingAddress ||
                            txtRouteLength.Text.Trim() != orderToEdit.RouteLength.ToString() ||
                            txtOrderCost.Text.Trim() != orderToEdit.OrderCost.ToString())
                        {
                            // Обновление данных заказа
                            orderToEdit.SenderName = txtSenderName.Text.Trim();
                            orderToEdit.LoadingAddress = txtLoadingAddress.Text.Trim();
                            orderToEdit.ReceiverName = txtReceiverName.Text.Trim();
                            orderToEdit.UnloadingAddress = txtUnloadingAddress.Text.Trim();
                            orderToEdit.RouteLength = double.Parse(txtRouteLength.Text.Trim());
                            orderToEdit.OrderCost = double.Parse(txtOrderCost.Text.Trim());
                            orderToEdit.OrderDate = dtpOrderDate.Value; // Обновление даты заказа

                            LoadOrders(); // Обновление списка заказов
                            ClearFields(); // Очистка полей ввода
                            MessageBox.Show("Заказ успешно обновлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Нет изменений для сохранения.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Заказ не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Выбранная строка пуста. Сохранение невозможно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для сохранения изменений.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void btnSaveCargoChanges_Click(object sender, EventArgs e)
        {
            if (dataGridViewCargo.SelectedRows.Count > 0) // Проверка, выбрана ли строка
            {
                var selectedRow = dataGridViewCargo.SelectedRows[0]; // Получение выбранной строки

                // Получение номера груза
                if (selectedRow.Cells["CargoId"].Value != null)
                {
                    int cargoId = (int)selectedRow.Cells["CargoId"].Value;
                    Cargo cargoToEdit = OrderManager.GetOrders()
                        .SelectMany(o => o.CargoList) // Получаем все грузы из всех заказов
                        .FirstOrDefault(c => c.CargoId == cargoId); // Поиск груза по номеру

                    if (cargoToEdit != null) // Если груз найден
                    {

                        List<string> errors = new List<string>();

                        // Получаем данные о грузе из текстовых полей
                        string cargoName = txtCargoName.Text.Trim();
                        string cargoUnit = txtCargoUnit.Text.Trim();

                        // Проверка названия груза
                        if (!Regex.IsMatch(cargoName, @"^[a-zA-Zа-яА-Я ]+$"))
                        {
                            errors.Add("Название груза может содержать только буквы.");
                        }

                        // Проверка единицы измерения
                        if (!Regex.IsMatch(cargoUnit, @"^[a-zA-Zа-яА-Я ]+$"))
                        {
                            errors.Add("Единица измерения может содержать только буквы.");
                        }

                        // Проверка количества
                        if (!double.TryParse(txtCargoQuantity.Text, out double cargoQuantity) || cargoQuantity <= 0 || cargoQuantity > 100)
                        {
                            errors.Add("Количество должно быть числом от 1 до 100.");
                        }

                        // Проверка веса
                        if (!double.TryParse(txtCargoWeight.Text, out double cargoWeight) || cargoWeight <= 0 || cargoWeight > 10000000)
                        {
                            errors.Add("Вес должен быть числом от 1 до 10,000,000.");
                        }

                        // Проверка страховой стоимости
                        if (!double.TryParse(txtCargoInsuranceValue.Text, out double cargoInsuranceValue) || cargoInsuranceValue <= 0 || cargoInsuranceValue > 10000000)
                        {
                            errors.Add("Страховая стоимость должна быть числом от 1 до 10,000,000.");
                        }

                        // Если есть ошибки, выводим их
                        if (errors.Count > 0)
                        {
                            MessageBox.Show(string.Join(Environment.NewLine, errors), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        // Проверка на изменения
                        if (txtCargoName.Text.Trim() != cargoToEdit.Name ||
                            txtCargoUnit.Text.Trim() != cargoToEdit.Unit ||
                            txtCargoQuantity.Text.Trim() != cargoToEdit.Quantity.ToString() ||
                            txtCargoWeight.Text.Trim() != cargoToEdit.Weight.ToString() ||
                            txtCargoInsuranceValue.Text.Trim() != cargoToEdit.InsuranceValue.ToString())
                        {
                            // Обновление данных груза
                            cargoToEdit.Name = txtCargoName.Text.Trim();
                            cargoToEdit.Unit = txtCargoUnit.Text.Trim();
                            cargoToEdit.Quantity = double.Parse(txtCargoQuantity.Text.Trim());
                            cargoToEdit.Weight = double.Parse(txtCargoWeight.Text.Trim());
                            cargoToEdit.InsuranceValue = double.Parse(txtCargoInsuranceValue.Text.Trim());

                            LoadCargoForOrder(cargoToEdit.CargoId); // Обновление отображения грузов для текущего заказа
                            ClearFields(); // Очистка полей ввода
                            MessageBox.Show("Груз успешно обновлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Нет изменений для сохранения.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Груз не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Выбранная строка пуста. Сохранение невозможно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите груз для сохранения изменений.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnShowCargo_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0) // Проверка, выбрана ли строка
            {
                var selectedRow = dataGridViewOrders.SelectedRows[0]; // Получение выбранной строки

                // Получение номера заказа
                if (selectedRow.Cells["OrderId"].Value != null)
                {
                    int orderId = (int)selectedRow.Cells["OrderId"].Value;
                    LoadCargoForOrder(orderId); // Загружаем грузы для выбранного заказа
                }
                else
                {
                    MessageBox.Show("Выбранная строка пуста. Показать грузы невозможно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для показа грузов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSelectSender_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) // Проверка, выбрана ли строка в таблице физических лиц
            {
                var selectedRow = dataGridView1.SelectedRows[0]; // Получение выбранной строки
                txtSenderName.Text = selectedRow.Cells["ContactName"].Value?.ToString() ?? string.Empty; // Установка имени отправителя
                dataGridView1.ClearSelection(); // Сброс выделения
            }
            else if (dataGridView2.SelectedRows.Count > 0) // Проверка, выбрана ли строка в таблице юридических лиц
            {
                var selectedRow = dataGridView2.SelectedRows[0]; // Получение выбранной строки
                txtSenderName.Text = selectedRow.Cells["CompanyNameNEW"].Value?.ToString() ?? string.Empty; // Установка имени отправителя
                dataGridView2.ClearSelection(); // Сброс выделения
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите отправителя из списка.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSelectReceiver_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) // Проверка, выбрана ли строка в таблице физических лиц
            {
                var selectedRow = dataGridView1.SelectedRows[0]; // Получение выбранной строки
                txtReceiverName.Text = selectedRow.Cells["ContactName"].Value?.ToString() ?? string.Empty; // Установка имени получателя
                dataGridView1.ClearSelection(); // Сброс выделения
            }
            else if (dataGridView2.SelectedRows.Count > 0) // Проверка, выбрана ли строка в таблице юридических лиц
            {
                var selectedRow = dataGridView2.SelectedRows[0]; // Получение выбранной строки
                txtReceiverName.Text = selectedRow.Cells["CompanyNameNEW"].Value?.ToString() ?? string.Empty; // Установка имени получателя
                dataGridView2.ClearSelection(); // Сброс выделения
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите получателя из списка.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSelectOrder_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0) // Проверка, выбрана ли строка
            {
                var selectedRow = dataGridViewOrders.SelectedRows[0]; // Получение выбранной строки

                // Проверяем, что ячейка OrderId не равна null
                if (selectedRow.Cells["OrderId"].Value != null)
                {
                    int orderId = (int)selectedRow.Cells["OrderId"].Value; // Получаем номер заказа

                }
                else
                {
                    MessageBox.Show("Выбранный заказ не имеет идентификатора. Пожалуйста, выберите другой заказ.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ из таблицы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void InitializeComponent()
        {
            this.dataGridViewOrders = new System.Windows.Forms.DataGridView();
            this.OrderId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SenderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LoadingAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceiverName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnloadingAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RouteLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.CompanyNameNEW = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DirectorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LegalAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LegalEntityPhone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BankName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccountNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.INN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ContactName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PassportSeries = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PassportNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IssueDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IssuedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtpOrderDate = new System.Windows.Forms.DateTimePicker();
            this.txtSenderName = new System.Windows.Forms.TextBox();
            this.txtOrderCost = new System.Windows.Forms.TextBox();
            this.txtUnloadingAddress = new System.Windows.Forms.TextBox();
            this.txtRouteLength = new System.Windows.Forms.TextBox();
            this.txtReceiverName = new System.Windows.Forms.TextBox();
            this.txtLoadingAddress = new System.Windows.Forms.TextBox();
            this.btnAddOrder = new System.Windows.Forms.Button();
            this.btnRemoveOrder = new System.Windows.Forms.Button();
            this.btnClearFields = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnAddCargo = new System.Windows.Forms.Button();
            this.txtCargoName = new System.Windows.Forms.TextBox();
            this.txtCargoInsuranceValue = new System.Windows.Forms.TextBox();
            this.txtCargoWeight = new System.Windows.Forms.TextBox();
            this.txtCargoQuantity = new System.Windows.Forms.TextBox();
            this.txtCargoUnit = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.dataGridViewCargo = new System.Windows.Forms.DataGridView();
            this.CargoId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CargoName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CargoUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CargoQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CargoWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CargoInsuranceValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnEditOrder = new System.Windows.Forms.Button();
            this.btnEditCargo = new System.Windows.Forms.Button();
            this.btnRemoveCargo = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.btnSaveOrderChanges = new System.Windows.Forms.Button();
            this.btnSaveCargoChanges = new System.Windows.Forms.Button();
            this.btnShowCargo = new System.Windows.Forms.Button();
            this.btnSelectSender = new System.Windows.Forms.Button();
            this.btnSelectReceiver = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCargo)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewOrders
            // 
            this.dataGridViewOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOrders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OrderId,
            this.OrderDate,
            this.SenderName,
            this.LoadingAddress,
            this.ReceiverName,
            this.UnloadingAddress,
            this.RouteLength,
            this.OrderCost});
            this.dataGridViewOrders.Location = new System.Drawing.Point(981, 38);
            this.dataGridViewOrders.Name = "dataGridViewOrders";
            this.dataGridViewOrders.Size = new System.Drawing.Size(564, 298);
            this.dataGridViewOrders.TabIndex = 0;
          
            // 
            // OrderId
            // 
            this.OrderId.HeaderText = "Номер заказа";
            this.OrderId.Name = "OrderId";
            // 
            // OrderDate
            // 
            this.OrderDate.HeaderText = "Дата заказа";
            this.OrderDate.Name = "OrderDate";
            // 
            // SenderName
            // 
            this.SenderName.HeaderText = "Имя отправителя";
            this.SenderName.Name = "SenderName";
            // 
            // LoadingAddress
            // 
            this.LoadingAddress.HeaderText = "Адрес загрузки";
            this.LoadingAddress.Name = "LoadingAddress";
            // 
            // ReceiverName
            // 
            this.ReceiverName.HeaderText = "Имя получателя";
            this.ReceiverName.Name = "ReceiverName";
            // 
            // UnloadingAddress
            // 
            this.UnloadingAddress.HeaderText = "Адрес разгрузки";
            this.UnloadingAddress.Name = "UnloadingAddress";
            // 
            // RouteLength
            // 
            this.RouteLength.HeaderText = "Длина маршрута";
            this.RouteLength.Name = "RouteLength";
            // 
            // OrderCost
            // 
            this.OrderCost.HeaderText = "Стоимость заказа";
            this.OrderCost.Name = "OrderCost";
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CompanyNameNEW,
            this.DirectorName,
            this.LegalAddress,
            this.LegalEntityPhone,
            this.BankName,
            this.AccountNumber,
            this.INN});
            this.dataGridView2.Location = new System.Drawing.Point(664, 314);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(265, 228);
            this.dataGridView2.TabIndex = 1;
           
            // 
            // CompanyNameNEW
            // 
            this.CompanyNameNEW.HeaderText = "Название компание";
            this.CompanyNameNEW.Name = "CompanyNameNEW";
            // 
            // DirectorName
            // 
            this.DirectorName.HeaderText = "Имя директора";
            this.DirectorName.Name = "DirectorName";
            // 
            // LegalAddress
            // 
            this.LegalAddress.HeaderText = "Юридический адрес";
            this.LegalAddress.Name = "LegalAddress";
            // 
            // LegalEntityPhone
            // 
            this.LegalEntityPhone.HeaderText = "Номер телефона юридического лица";
            this.LegalEntityPhone.Name = "LegalEntityPhone";
            // 
            // BankName
            // 
            this.BankName.HeaderText = "Название банка";
            this.BankName.Name = "BankName";
            // 
            // AccountNumber
            // 
            this.AccountNumber.HeaderText = "Номер счета";
            this.AccountNumber.Name = "AccountNumber";
            // 
            // INN
            // 
            this.INN.HeaderText = "ИНН";
            this.INN.Name = "INN";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ContactName,
            this.Phone,
            this.PassportSeries,
            this.PassportNumber,
            this.IssueDate,
            this.IssuedBy});
            this.dataGridView1.Location = new System.Drawing.Point(664, 34);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(265, 231);
            this.dataGridView1.TabIndex = 2;
           
            // 
            // ContactName
            // 
            this.ContactName.HeaderText = "Имя контакта";
            this.ContactName.Name = "ContactName";
            // 
            // Phone
            // 
            this.Phone.HeaderText = "Номер телефона";
            this.Phone.Name = "Phone";
            // 
            // PassportSeries
            // 
            this.PassportSeries.HeaderText = "Серия паспорта";
            this.PassportSeries.Name = "PassportSeries";
            // 
            // PassportNumber
            // 
            this.PassportNumber.HeaderText = "Номер паспорта";
            this.PassportNumber.Name = "PassportNumber";
            // 
            // IssueDate
            // 
            this.IssueDate.HeaderText = "Дата выдачи";
            this.IssueDate.Name = "IssueDate";
            // 
            // IssuedBy
            // 
            this.IssuedBy.HeaderText = "Кем выдан";
            this.IssuedBy.Name = "IssuedBy";
            // 
            // dtpOrderDate
            // 
            this.dtpOrderDate.Location = new System.Drawing.Point(12, 122);
            this.dtpOrderDate.Name = "dtpOrderDate";
            this.dtpOrderDate.Size = new System.Drawing.Size(200, 20);
            this.dtpOrderDate.TabIndex = 3;
            // 
            // txtSenderName
            // 
            this.txtSenderName.Location = new System.Drawing.Point(12, 214);
            this.txtSenderName.Name = "txtSenderName";
            this.txtSenderName.Size = new System.Drawing.Size(200, 20);
            this.txtSenderName.TabIndex = 4;
            this.txtSenderName.Text = "\r\n";
            // 
            // txtOrderCost
            // 
            this.txtOrderCost.Location = new System.Drawing.Point(12, 609);
            this.txtOrderCost.Name = "txtOrderCost";
            this.txtOrderCost.Size = new System.Drawing.Size(200, 20);
            this.txtOrderCost.TabIndex = 6;
            // 
            // txtUnloadingAddress
            // 
            this.txtUnloadingAddress.Location = new System.Drawing.Point(12, 452);
            this.txtUnloadingAddress.Name = "txtUnloadingAddress";
            this.txtUnloadingAddress.Size = new System.Drawing.Size(200, 20);
            this.txtUnloadingAddress.TabIndex = 7;
            // 
            // txtRouteLength
            // 
            this.txtRouteLength.Location = new System.Drawing.Point(12, 534);
            this.txtRouteLength.Name = "txtRouteLength";
            this.txtRouteLength.Size = new System.Drawing.Size(200, 20);
            this.txtRouteLength.TabIndex = 8;
            // 
            // txtReceiverName
            // 
            this.txtReceiverName.Location = new System.Drawing.Point(12, 388);
            this.txtReceiverName.Name = "txtReceiverName";
            this.txtReceiverName.Size = new System.Drawing.Size(200, 20);
            this.txtReceiverName.TabIndex = 10;
            // 
            // txtLoadingAddress
            // 
            this.txtLoadingAddress.Location = new System.Drawing.Point(12, 284);
            this.txtLoadingAddress.Name = "txtLoadingAddress";
            this.txtLoadingAddress.Size = new System.Drawing.Size(200, 20);
            this.txtLoadingAddress.TabIndex = 11;
            // 
            // btnAddOrder
            // 
            this.btnAddOrder.Location = new System.Drawing.Point(12, 12);
            this.btnAddOrder.Name = "btnAddOrder";
            this.btnAddOrder.Size = new System.Drawing.Size(131, 35);
            this.btnAddOrder.TabIndex = 12;
            this.btnAddOrder.Text = "Добавить заказ";
            this.btnAddOrder.UseVisualStyleBackColor = true;
            this.btnAddOrder.Click += new System.EventHandler(this.btnAddOrder_Click_1);
            // 
            // btnRemoveOrder
            // 
            this.btnRemoveOrder.Location = new System.Drawing.Point(164, 14);
            this.btnRemoveOrder.Name = "btnRemoveOrder";
            this.btnRemoveOrder.Size = new System.Drawing.Size(147, 33);
            this.btnRemoveOrder.TabIndex = 13;
            this.btnRemoveOrder.Text = "Удалить заказ";
            this.btnRemoveOrder.UseVisualStyleBackColor = true;
            this.btnRemoveOrder.Click += new System.EventHandler(this.btnRemoveOrder_Click_1);
            // 
            // btnClearFields
            // 
            this.btnClearFields.Location = new System.Drawing.Point(317, 12);
            this.btnClearFields.Name = "btnClearFields";
            this.btnClearFields.Size = new System.Drawing.Size(114, 35);
            this.btnClearFields.TabIndex = 14;
            this.btnClearFields.Text = "Очистить поля ";
            this.btnClearFields.UseVisualStyleBackColor = true;
            this.btnClearFields.Click += new System.EventHandler(this.btnClearFields_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Выберите дату заказа:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 198);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(201, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Введите имя отправитлея (компании):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 263);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Введите адрес загрузки:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 372);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(151, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Имя получателя (компания):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 432);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(140, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Введите адрес разгрузки:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 518);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Длина маршрута:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 585);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Стоимость:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(661, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 15);
            this.label8.TabIndex = 22;
            this.label8.Text = "Физ лица:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(661, 293);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 15);
            this.label9.TabIndex = 23;
            this.label9.Text = "Юр лица:";
            // 
            // btnAddCargo
            // 
            this.btnAddCargo.Location = new System.Drawing.Point(12, 49);
            this.btnAddCargo.Name = "btnAddCargo";
            this.btnAddCargo.Size = new System.Drawing.Size(131, 35);
            this.btnAddCargo.TabIndex = 24;
            this.btnAddCargo.Text = "Добавить груз";
            this.btnAddCargo.UseVisualStyleBackColor = true;
            this.btnAddCargo.Click += new System.EventHandler(this.btnAddCargo_Click);
            // 
            // txtCargoName
            // 
            this.txtCargoName.Location = new System.Drawing.Point(255, 135);
            this.txtCargoName.Name = "txtCargoName";
            this.txtCargoName.Size = new System.Drawing.Size(151, 20);
            this.txtCargoName.TabIndex = 25;
            // 
            // txtCargoInsuranceValue
            // 
            this.txtCargoInsuranceValue.Location = new System.Drawing.Point(255, 452);
            this.txtCargoInsuranceValue.Name = "txtCargoInsuranceValue";
            this.txtCargoInsuranceValue.Size = new System.Drawing.Size(151, 20);
            this.txtCargoInsuranceValue.TabIndex = 26;
            // 
            // txtCargoWeight
            // 
            this.txtCargoWeight.Location = new System.Drawing.Point(255, 373);
            this.txtCargoWeight.Name = "txtCargoWeight";
            this.txtCargoWeight.Size = new System.Drawing.Size(151, 20);
            this.txtCargoWeight.TabIndex = 27;
            // 
            // txtCargoQuantity
            // 
            this.txtCargoQuantity.Location = new System.Drawing.Point(255, 295);
            this.txtCargoQuantity.Name = "txtCargoQuantity";
            this.txtCargoQuantity.Size = new System.Drawing.Size(151, 20);
            this.txtCargoQuantity.TabIndex = 28;
            // 
            // txtCargoUnit
            // 
            this.txtCargoUnit.Location = new System.Drawing.Point(255, 218);
            this.txtCargoUnit.Name = "txtCargoUnit";
            this.txtCargoUnit.Size = new System.Drawing.Size(151, 20);
            this.txtCargoUnit.TabIndex = 29;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(252, 119);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(91, 13);
            this.label10.TabIndex = 30;
            this.label10.Text = "Название груза:\r\n";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(252, 202);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(112, 13);
            this.label11.TabIndex = 31;
            this.label11.Text = "Единица измерения:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(252, 279);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(69, 13);
            this.label12.TabIndex = 32;
            this.label12.Text = "Количество:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(252, 357);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 13);
            this.label13.TabIndex = 33;
            this.label13.Text = "Вес:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(252, 435);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(120, 13);
            this.label14.TabIndex = 34;
            this.label14.Text = "Страховая стоимость:";
            // 
            // dataGridViewCargo
            // 
            this.dataGridViewCargo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCargo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CargoId,
            this.CargoName,
            this.CargoUnit,
            this.CargoQuantity,
            this.CargoWeight,
            this.CargoInsuranceValue});
            this.dataGridViewCargo.Location = new System.Drawing.Point(981, 378);
            this.dataGridViewCargo.Name = "dataGridViewCargo";
            this.dataGridViewCargo.Size = new System.Drawing.Size(543, 298);
            this.dataGridViewCargo.TabIndex = 37;
            
            // 
            // CargoId
            // 
            this.CargoId.HeaderText = "Номер груза";
            this.CargoId.Name = "CargoId";
            // 
            // CargoName
            // 
            this.CargoName.HeaderText = "Название груза";
            this.CargoName.Name = "CargoName";
            // 
            // CargoUnit
            // 
            this.CargoUnit.HeaderText = "Ед Изм";
            this.CargoUnit.Name = "CargoUnit";
            // 
            // CargoQuantity
            // 
            this.CargoQuantity.HeaderText = "кол-во";
            this.CargoQuantity.Name = "CargoQuantity";
            // 
            // CargoWeight
            // 
            this.CargoWeight.HeaderText = "Вес";
            this.CargoWeight.Name = "CargoWeight";
            // 
            // CargoInsuranceValue
            // 
            this.CargoInsuranceValue.HeaderText = "Стоимость";
            this.CargoInsuranceValue.Name = "CargoInsuranceValue";
            // 
            // btnEditOrder
            // 
            this.btnEditOrder.Location = new System.Drawing.Point(436, 14);
            this.btnEditOrder.Name = "btnEditOrder";
            this.btnEditOrder.Size = new System.Drawing.Size(157, 33);
            this.btnEditOrder.TabIndex = 38;
            this.btnEditOrder.Text = "Редактировать заказ";
            this.btnEditOrder.UseVisualStyleBackColor = true;
            this.btnEditOrder.Click += new System.EventHandler(this.btnEditOrder_Click);
            // 
            // btnEditCargo
            // 
            this.btnEditCargo.Location = new System.Drawing.Point(436, 111);
            this.btnEditCargo.Name = "btnEditCargo";
            this.btnEditCargo.Size = new System.Drawing.Size(157, 31);
            this.btnEditCargo.TabIndex = 39;
            this.btnEditCargo.Text = "Редактировать груз";
            this.btnEditCargo.UseVisualStyleBackColor = true;
            this.btnEditCargo.Click += new System.EventHandler(this.btnEditCargo_Click);
            // 
            // btnRemoveCargo
            // 
            this.btnRemoveCargo.Location = new System.Drawing.Point(164, 53);
            this.btnRemoveCargo.Name = "btnRemoveCargo";
            this.btnRemoveCargo.Size = new System.Drawing.Size(147, 29);
            this.btnRemoveCargo.TabIndex = 40;
            this.btnRemoveCargo.Text = "Удалить груз";
            this.btnRemoveCargo.UseVisualStyleBackColor = true;
            this.btnRemoveCargo.Click += new System.EventHandler(this.btnRemoveCargo_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label16.Location = new System.Drawing.Point(978, 14);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(115, 15);
            this.label16.TabIndex = 41;
            this.label16.Text = "Список заказов:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label17.Location = new System.Drawing.Point(978, 351);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(165, 15);
            this.label17.TabIndex = 42;
            this.label17.Text = "Список грузов в зказах:";
            // 
            // btnSaveOrderChanges
            // 
            this.btnSaveOrderChanges.Location = new System.Drawing.Point(436, 62);
            this.btnSaveOrderChanges.Name = "btnSaveOrderChanges";
            this.btnSaveOrderChanges.Size = new System.Drawing.Size(157, 23);
            this.btnSaveOrderChanges.TabIndex = 43;
            this.btnSaveOrderChanges.Text = "Внести изменения в заказ";
            this.btnSaveOrderChanges.UseVisualStyleBackColor = true;
            this.btnSaveOrderChanges.Click += new System.EventHandler(this.btnSaveOrderChanges_Click);
            // 
            // btnSaveCargoChanges
            // 
            this.btnSaveCargoChanges.Location = new System.Drawing.Point(436, 161);
            this.btnSaveCargoChanges.Name = "btnSaveCargoChanges";
            this.btnSaveCargoChanges.Size = new System.Drawing.Size(157, 23);
            this.btnSaveCargoChanges.TabIndex = 44;
            this.btnSaveCargoChanges.Text = "Внести изменения в груз";
            this.btnSaveCargoChanges.UseVisualStyleBackColor = true;
            this.btnSaveCargoChanges.Click += new System.EventHandler(this.btnSaveCargoChanges_Click);
            // 
            // btnShowCargo
            // 
            this.btnShowCargo.Location = new System.Drawing.Point(436, 208);
            this.btnShowCargo.Name = "btnShowCargo";
            this.btnShowCargo.Size = new System.Drawing.Size(157, 23);
            this.btnShowCargo.TabIndex = 45;
            this.btnShowCargo.Text = "Грузы заказа";
            this.btnShowCargo.UseVisualStyleBackColor = true;
            this.btnShowCargo.Click += new System.EventHandler(this.btnShowCargo_Click);
            // 
            // btnSelectSender
            // 
            this.btnSelectSender.Location = new System.Drawing.Point(13, 160);
            this.btnSelectSender.Name = "btnSelectSender";
            this.btnSelectSender.Size = new System.Drawing.Size(197, 23);
            this.btnSelectSender.TabIndex = 46;
            this.btnSelectSender.Text = "Выбрать отправителя";
            this.btnSelectSender.UseVisualStyleBackColor = true;
            this.btnSelectSender.Click += new System.EventHandler(this.btnSelectSender_Click);
            // 
            // btnSelectReceiver
            // 
            this.btnSelectReceiver.Location = new System.Drawing.Point(12, 330);
            this.btnSelectReceiver.Name = "btnSelectReceiver";
            this.btnSelectReceiver.Size = new System.Drawing.Size(197, 23);
            this.btnSelectReceiver.TabIndex = 47;
            this.btnSelectReceiver.Text = "Выбрать получателя";
            this.btnSelectReceiver.UseVisualStyleBackColor = true;
            this.btnSelectReceiver.Click += new System.EventHandler(this.btnSelectReceiver_Click);
            // 
            // OrderManagerForm
            // 
            this.ClientSize = new System.Drawing.Size(1750, 693);
            this.Controls.Add(this.btnSelectReceiver);
            this.Controls.Add(this.btnSelectSender);
            this.Controls.Add(this.btnShowCargo);
            this.Controls.Add(this.btnSaveCargoChanges);
            this.Controls.Add(this.btnSaveOrderChanges);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.btnRemoveCargo);
            this.Controls.Add(this.btnEditCargo);
            this.Controls.Add(this.btnEditOrder);
            this.Controls.Add(this.dataGridViewCargo);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtCargoUnit);
            this.Controls.Add(this.txtCargoQuantity);
            this.Controls.Add(this.txtCargoWeight);
            this.Controls.Add(this.txtCargoInsuranceValue);
            this.Controls.Add(this.txtCargoName);
            this.Controls.Add(this.btnAddCargo);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClearFields);
            this.Controls.Add(this.btnRemoveOrder);
            this.Controls.Add(this.btnAddOrder);
            this.Controls.Add(this.txtLoadingAddress);
            this.Controls.Add(this.txtReceiverName);
            this.Controls.Add(this.txtRouteLength);
            this.Controls.Add(this.txtUnloadingAddress);
            this.Controls.Add(this.txtOrderCost);
            this.Controls.Add(this.txtSenderName);
            this.Controls.Add(this.dtpOrderDate);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.dataGridViewOrders);
            this.Name = "OrderManagerForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCargo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

      

        
    }
}
