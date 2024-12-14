using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FreightTransportSystem
{
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
        private DataGridViewTextBoxColumn OrderDate;
        private DataGridViewTextBoxColumn SenderName;
        private DataGridViewTextBoxColumn LoadingAddress;
        private DataGridViewTextBoxColumn ReceiverName;
        private DataGridViewTextBoxColumn UnloadingAddress;
        private DataGridViewTextBoxColumn RouteLength;
        private DataGridViewTextBoxColumn OrderCost;
        private DataGridViewTextBoxColumn CargoName;
        private DataGridViewTextBoxColumn CargoUnit;
        private DataGridViewTextBoxColumn CargoQuantity;
        private DataGridViewTextBoxColumn CargoWeight;
        private DataGridViewTextBoxColumn CargoInsuranceValue;

        // Дополнительный DataGridView (возможно, для других данных)
        private DataGridView dataGridView1;

        // Конструктор формы
        public OrderManagerForm()
        {
            InitializeComponent();
            LoadOrders(); // Загрузка заказов при инициализации формы
            LoadClientData(); // Загрузка данных клиентов при
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            DataStorage.SaveData(); // Сохранение данных перед закрытием формы

        }


        private void LoadOrders()
        {
            dataGridViewOrders.Rows.Clear(); // Очистка текущих строк в DataGridView
            foreach (var order in OrderManager.GetOrders())
            {
                // Если у заказа есть грузы, добавляем их в отдельные строки
                if (order.CargoList.Count > 0)
                {
                    foreach (var cargo in order.CargoList)
                    {
                        dataGridViewOrders.Rows.Add(
                            order.OrderDate,
                            order.SenderName,
                            order.LoadingAddress,
                            order.ReceiverName,
                            order.UnloadingAddress,
                            order.RouteLength,
                            order.OrderCost,
                            cargo.Name,
                            cargo.Unit,
                            cargo.Quantity,
                            cargo.Weight,
                            cargo.InsuranceValue
                        );
                    }
                }
                else
                {
                    // Если грузов нет, добавляем строку без информации о грузе
                    dataGridViewOrders.Rows.Add(
                        order.OrderDate,
                        order.SenderName,
                        order.LoadingAddress,
                        order.ReceiverName,
                        order.UnloadingAddress,
                        order.RouteLength,
                        order.OrderCost
                    );
                }
            }
        }

        private void LoadClientData()
        {
            // Загрузка данных клиентов в DataGridView
            var individualClients = ClientManager.GetClients().OfType<IndividualClient>().ToList();
            var legalEntityClients = ClientManager.GetClients().OfType<LegalEntityClient>().ToList();

            // Заполнение DataGridView для физических лицa
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

        private void ClearFields()
        {
            txtSenderName.Clear();
            txtLoadingAddress.Clear();
            txtReceiverName.Clear();
            txtUnloadingAddress.Clear();
            txtRouteLength.Clear();
            txtOrderCost.Clear();
            dtpOrderDate.Value = DateTime.Now; // Устанавливаем дату на текущее значение
        }


        private void btnAddOrder_Click_1(object sender, EventArgs e)
        {
            List<string> errors = new List<string>();

            // Получение данных из текстовых полей
            DateTime orderDate = dtpOrderDate.Value; // Получаем дату из DateTimePicker
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
            if (!Regex.IsMatch(loadingAddress, @"^[a-zA-Z0-9/ ]+$"))
            {
                errors.Add("Адрес загрузки может содержать только буквы, цифры и символ '/'.");
            }

            if (!Regex.IsMatch(unloadingAddress, @"^[a-zA-Z0-9/ ]+$"))
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
            CargoOrder newOrder = new CargoOrder(orderDate, senderName, loadingAddress, receiverName, unloadingAddress, routeLength, orderCost, new List<Cargo>());
            OrderManager.GetOrders().Add(newOrder); // Добавление заказа в менеджер заказов
            LoadOrders(); // Обновление отображения заказов
        }



        private void btnAddCargo_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewOrders.SelectedRows[0];
                string orderDateString = selectedRow.Cells["OrderDate"].Value?.ToString();

                if (DateTime.TryParse(orderDateString, out DateTime orderDate))
                {
                    CargoOrder selectedOrder = OrderManager.GetOrders().Find(o => o.OrderDate == orderDate);

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
                        if (!Regex.IsMatch(cargoUnit, @"^[a -zA-Zа-яА-Я ]+$"))
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

                        // Создаем новый объект Cargo
                        Cargo newCargo = new Cargo(cargoName, cargoUnit, cargoQuantity, cargoWeight, cargoInsuranceValue);
                        selectedOrder.CargoList.Add(newCargo); // Добавляем новый груз

                        // Обновляем отображение заказов
                        LoadOrders(); // Обновляем отображение заказов

                        // Сохраняем данные в файл
                        DataStorage.SaveData(); // Сохраняем данные в файл

                        MessageBox.Show("Груз успешно добавлен к заказу.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Выбранный заказ не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Некорректная дата заказа. Пожалуйста, проверьте данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для добавления груза.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnRemoveOrder_Click_1(object sender, EventArgs e)
        {

            if (dataGridViewOrders.SelectedRows.Count > 0)
            {
                DateTime orderDate = Convert.ToDateTime(dataGridViewOrders.SelectedRows[0].Cells["OrderDate"].Value);
                CargoOrder orderToRemove = OrderManager.GetOrders().Find(o => o.OrderDate == orderDate);
                if (orderToRemove != null)
                {
                    OrderManager.GetOrders().Remove(orderToRemove); // Удаление заказа
                    LoadOrders(); // Обновление списка заказов
                }
            }

        }

        private void btnClearFields_Click_1(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];

                // Заполняем текстовые поля данными из выбранной строки
                txtSenderName.Text = selectedRow.Cells["ContactName"].Value?.ToString() ?? string.Empty;
                txtReceiverName.Text = selectedRow.Cells["Phone"].Value?.ToString() ?? string.Empty;
                txtLoadingAddress.Text = selectedRow.Cells["PassportSeries"].Value?.ToString() ?? string.Empty;
                txtUnloadingAddress.Text = selectedRow.Cells["PassportNumber"].Value?.ToString() ?? string.Empty;
                txtRouteLength.Text = selectedRow.Cells["IssueDate"].Value?.ToString() ?? string.Empty;
                txtOrderCost.Text = selectedRow.Cells["IssuedBy"].Value?.ToString() ?? string.Empty;
            }
        }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView2.SelectedRows[0];

                // Заполняем текстовые поля данными из выбранной строки
                txtSenderName.Text = selectedRow.Cells["CompanyNameNEW"].Value?.ToString() ?? string.Empty;
                txtReceiverName.Text = selectedRow.Cells["DirectorName"].Value?.ToString() ?? string.Empty;
                txtLoadingAddress.Text = selectedRow.Cells["LegalAddress"].Value?.ToString() ?? string.Empty;
                txtUnloadingAddress.Text = selectedRow.Cells["LegalEntityPhone"].Value?.ToString() ?? string.Empty;
                txtRouteLength.Text = selectedRow.Cells["BankName"].Value?.ToString() ?? string.Empty;
                txtOrderCost.Text = selectedRow.Cells["AccountNumber"].Value?.ToString() ?? string.Empty;
            }
        }

        private void dataGridViewOrders_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewOrders.SelectedRows[0];

                // Проверяем, есть ли значение в ячейке "OrderDate"
                if (selectedRow.Cells["OrderDate"].Value != null &&
                    DateTime.TryParse(selectedRow.Cells["OrderDate"].Value.ToString(), out DateTime orderDate))
                {
                    dtpOrderDate.Value = orderDate; // Устанавливаем значение в DateTimePicker
                }
                else
                {
                   
                    return; // Выходим из метода, если дата некорректна
                }

                // Заполняем текстовые поля данными из выбранного заказа
                txtSenderName.Text = selectedRow.Cells["SenderName"].Value?.ToString();
                txtLoadingAddress.Text = selectedRow.Cells["LoadingAddress"].Value?.ToString();
                txtReceiverName.Text = selectedRow.Cells["ReceiverName"].Value?.ToString();
                txtUnloadingAddress.Text = selectedRow.Cells["UnloadingAddress"].Value?.ToString();
                txtRouteLength.Text = selectedRow.Cells["RouteLength"].Value?.ToString();
                txtOrderCost.Text = selectedRow.Cells["OrderCost"].Value?.ToString();
            }
        }


        private void InitializeComponent()
        {
            this.dataGridViewOrders = new System.Windows.Forms.DataGridView();
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
            this.OrderDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SenderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LoadingAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceiverName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnloadingAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RouteLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CargoName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CargoUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CargoQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CargoWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CargoInsuranceValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewOrders
            // 
            this.dataGridViewOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOrders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OrderDate,
            this.SenderName,
            this.LoadingAddress,
            this.ReceiverName,
            this.UnloadingAddress,
            this.RouteLength,
            this.OrderCost,
            this.CargoName,
            this.CargoUnit,
            this.CargoQuantity,
            this.CargoWeight,
            this.CargoInsuranceValue});
            this.dataGridViewOrders.Location = new System.Drawing.Point(1110, 94);
            this.dataGridViewOrders.Name = "dataGridViewOrders";
            this.dataGridViewOrders.Size = new System.Drawing.Size(404, 467);
            this.dataGridViewOrders.TabIndex = 0;
            this.dataGridViewOrders.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewOrders_CellContentClick_1);
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
            this.dataGridView2.Location = new System.Drawing.Point(710, 346);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(369, 231);
            this.dataGridView2.TabIndex = 1;
            this.dataGridView2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick_1);
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
            this.dataGridView1.Location = new System.Drawing.Point(710, 71);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(369, 231);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick_1);
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
            this.dtpOrderDate.Location = new System.Drawing.Point(254, 110);
            this.dtpOrderDate.Name = "dtpOrderDate";
            this.dtpOrderDate.Size = new System.Drawing.Size(200, 20);
            this.dtpOrderDate.TabIndex = 3;
            // 
            // txtSenderName
            // 
            this.txtSenderName.Location = new System.Drawing.Point(254, 157);
            this.txtSenderName.Multiline = true;
            this.txtSenderName.Name = "txtSenderName";
            this.txtSenderName.Size = new System.Drawing.Size(200, 35);
            this.txtSenderName.TabIndex = 4;
            this.txtSenderName.Text = "\r\n";
            // 
            // txtOrderCost
            // 
            this.txtOrderCost.Location = new System.Drawing.Point(254, 523);
            this.txtOrderCost.Multiline = true;
            this.txtOrderCost.Name = "txtOrderCost";
            this.txtOrderCost.Size = new System.Drawing.Size(200, 35);
            this.txtOrderCost.TabIndex = 6;
            // 
            // txtUnloadingAddress
            // 
            this.txtUnloadingAddress.Location = new System.Drawing.Point(254, 366);
            this.txtUnloadingAddress.Multiline = true;
            this.txtUnloadingAddress.Name = "txtUnloadingAddress";
            this.txtUnloadingAddress.Size = new System.Drawing.Size(200, 35);
            this.txtUnloadingAddress.TabIndex = 7;
            // 
            // txtRouteLength
            // 
            this.txtRouteLength.Location = new System.Drawing.Point(254, 448);
            this.txtRouteLength.Multiline = true;
            this.txtRouteLength.Name = "txtRouteLength";
            this.txtRouteLength.Size = new System.Drawing.Size(200, 35);
            this.txtRouteLength.TabIndex = 8;
            // 
            // txtReceiverName
            // 
            this.txtReceiverName.Location = new System.Drawing.Point(254, 302);
            this.txtReceiverName.Multiline = true;
            this.txtReceiverName.Name = "txtReceiverName";
            this.txtReceiverName.Size = new System.Drawing.Size(200, 35);
            this.txtReceiverName.TabIndex = 10;
            // 
            // txtLoadingAddress
            // 
            this.txtLoadingAddress.Location = new System.Drawing.Point(254, 227);
            this.txtLoadingAddress.Multiline = true;
            this.txtLoadingAddress.Name = "txtLoadingAddress";
            this.txtLoadingAddress.Size = new System.Drawing.Size(200, 35);
            this.txtLoadingAddress.TabIndex = 11;
            // 
            // btnAddOrder
            // 
            this.btnAddOrder.Location = new System.Drawing.Point(12, 134);
            this.btnAddOrder.Name = "btnAddOrder";
            this.btnAddOrder.Size = new System.Drawing.Size(224, 79);
            this.btnAddOrder.TabIndex = 12;
            this.btnAddOrder.Text = "Добавить заказ";
            this.btnAddOrder.UseVisualStyleBackColor = true;
            this.btnAddOrder.Click += new System.EventHandler(this.btnAddOrder_Click_1);
            // 
            // btnRemoveOrder
            // 
            this.btnRemoveOrder.Location = new System.Drawing.Point(12, 289);
            this.btnRemoveOrder.Name = "btnRemoveOrder";
            this.btnRemoveOrder.Size = new System.Drawing.Size(224, 79);
            this.btnRemoveOrder.TabIndex = 13;
            this.btnRemoveOrder.Text = "Удалить заказ";
            this.btnRemoveOrder.UseVisualStyleBackColor = true;
            this.btnRemoveOrder.Click += new System.EventHandler(this.btnRemoveOrder_Click_1);
            // 
            // btnClearFields
            // 
            this.btnClearFields.Location = new System.Drawing.Point(12, 440);
            this.btnClearFields.Name = "btnClearFields";
            this.btnClearFields.Size = new System.Drawing.Size(224, 79);
            this.btnClearFields.TabIndex = 14;
            this.btnClearFields.Text = "Очистить поля ";
            this.btnClearFields.UseVisualStyleBackColor = true;
            this.btnClearFields.Click += new System.EventHandler(this.btnClearFields_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(251, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Введите дату заказа:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(251, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Введите имя отправитлея:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(251, 206);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Введите адрес загрузки:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(251, 286);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Имя получателя:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(251, 346);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(140, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Введите адрес разгрузки:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(251, 432);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Длина маршрута:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(251, 499);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Стоимость:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(707, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Физ лица:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(707, 326);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "Юр лица:";
            // 
            // btnAddCargo
            // 
            this.btnAddCargo.Location = new System.Drawing.Point(498, 110);
            this.btnAddCargo.Name = "btnAddCargo";
            this.btnAddCargo.Size = new System.Drawing.Size(151, 57);
            this.btnAddCargo.TabIndex = 24;
            this.btnAddCargo.Text = "Добавить груз";
            this.btnAddCargo.UseVisualStyleBackColor = true;
            this.btnAddCargo.Click += new System.EventHandler(this.btnAddCargo_Click);
            // 
            // txtCargoName
            // 
            this.txtCargoName.Location = new System.Drawing.Point(498, 206);
            this.txtCargoName.Multiline = true;
            this.txtCargoName.Name = "txtCargoName";
            this.txtCargoName.Size = new System.Drawing.Size(151, 39);
            this.txtCargoName.TabIndex = 25;
            // 
            // txtCargoInsuranceValue
            // 
            this.txtCargoInsuranceValue.Location = new System.Drawing.Point(498, 523);
            this.txtCargoInsuranceValue.Multiline = true;
            this.txtCargoInsuranceValue.Name = "txtCargoInsuranceValue";
            this.txtCargoInsuranceValue.Size = new System.Drawing.Size(151, 39);
            this.txtCargoInsuranceValue.TabIndex = 26;
            // 
            // txtCargoWeight
            // 
            this.txtCargoWeight.Location = new System.Drawing.Point(498, 444);
            this.txtCargoWeight.Multiline = true;
            this.txtCargoWeight.Name = "txtCargoWeight";
            this.txtCargoWeight.Size = new System.Drawing.Size(151, 39);
            this.txtCargoWeight.TabIndex = 27;
            // 
            // txtCargoQuantity
            // 
            this.txtCargoQuantity.Location = new System.Drawing.Point(498, 366);
            this.txtCargoQuantity.Multiline = true;
            this.txtCargoQuantity.Name = "txtCargoQuantity";
            this.txtCargoQuantity.Size = new System.Drawing.Size(151, 39);
            this.txtCargoQuantity.TabIndex = 28;
            // 
            // txtCargoUnit
            // 
            this.txtCargoUnit.Location = new System.Drawing.Point(498, 289);
            this.txtCargoUnit.Multiline = true;
            this.txtCargoUnit.Name = "txtCargoUnit";
            this.txtCargoUnit.Size = new System.Drawing.Size(151, 39);
            this.txtCargoUnit.TabIndex = 29;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(495, 190);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(91, 13);
            this.label10.TabIndex = 30;
            this.label10.Text = "Название груза:\r\n";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(495, 273);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(112, 13);
            this.label11.TabIndex = 31;
            this.label11.Text = "Единица измерения:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(495, 350);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(69, 13);
            this.label12.TabIndex = 32;
            this.label12.Text = "Количество:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(495, 428);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 13);
            this.label13.TabIndex = 33;
            this.label13.Text = "Вес:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(495, 506);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(120, 13);
            this.label14.TabIndex = 34;
            this.label14.Text = "Страховая стоимость:";
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
            // CargoName
            // 
            this.CargoName.HeaderText = "Название груза";
            this.CargoName.Name = "CargoName";
            // 
            // CargoUnit
            // 
            this.CargoUnit.HeaderText = "Единица измерения";
            this.CargoUnit.Name = "CargoUnit";
            // 
            // CargoQuantity
            // 
            this.CargoQuantity.HeaderText = "Кол-во";
            this.CargoQuantity.Name = "CargoQuantity";
            // 
            // CargoWeight
            // 
            this.CargoWeight.HeaderText = "Вес";
            this.CargoWeight.Name = "CargoWeight";
            // 
            // CargoInsuranceValue
            // 
            this.CargoInsuranceValue.HeaderText = "Страховая стоимость ";
            this.CargoInsuranceValue.Name = "CargoInsuranceValue";
            // 
            // OrderManagerForm
            // 
            this.ClientSize = new System.Drawing.Size(1750, 678);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        
    }
}
