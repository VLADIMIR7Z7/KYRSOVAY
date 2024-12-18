using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FreightTransportSystem
{
    public partial class TripManagerForm : Form
    {
        // Объявление переменных для элементов управления
        private DataGridView dataGridViewCars;
        private DataGridView dataGridViewDrivers;
        private DataGridViewTextBoxColumn licensePlate;
        private DataGridViewTextBoxColumn brand;
        private DataGridViewTextBoxColumn Грузоподъемность;
        private DataGridViewTextBoxColumn purpose;
        private DataGridViewTextBoxColumn yearOfManufacture;
        private DataGridViewTextBoxColumn yearOfRepair;
        private DataGridViewTextBoxColumn mileage;
        private DataGridViewTextBoxColumn photo;
        private DataGridViewTextBoxColumn fullName;
        private DataGridViewTextBoxColumn employeeNumber;
        private DataGridViewTextBoxColumn yearOfBirth;
        private DataGridViewTextBoxColumn experience;
        private DataGridViewTextBoxColumn category;
        private DataGridViewTextBoxColumn classType;
        private TextBox txtLicensePlate;
        private Label label1;
        private DateTimePicker dtpArrivalTime;
        private Label label3;
        private Button btnPlanTrip;
        private Button btnClearFields;
        private Button btnDeleteTrip;
        private DataGridViewTextBoxColumn Nomer;
        private DataGridViewTextBoxColumn driver;
        private DataGridViewTextBoxColumn arrivalTime;
        private DataGridViewTextBoxColumn orderNumber;
        private Button btnEditTrip;
        private TextBox txtDriverFullName;
        private Label label4;
        private Label label2;
        private Label label5;
        private Label label6;
        private Label label7;
        private DataGridView dataGridViewSelectedTrips;

        // Конструктор формы
        public TripManagerForm()
        {
            InitializeComponent(); // Инициализация компонентов формы
            LoadCars(); // Загрузка автомобилей
            LoadDrivers(); // Загрузка водителей
            LoadTrips(); // Загрузка поездок
        }

        // Метод для загрузки автомобилей в таблицу
        private void LoadCars()
        {
            dataGridViewCars.Rows.Clear(); // Очистка таблицы перед загрузкой
            foreach (var car in FleetManager.GetCars()) // Перебор всех автомобилей
            {
                // Добавление информации об автомобиле в таблицу
                dataGridViewCars.Rows.Add(car.LicensePlate, car.Brand, car.LoadCapacity, car.Purpose, car.YearOfManufacture, car.YearOfRepair, car.Mileage, car.Photo);
            }
        }

        // Метод для загрузки водителей в таблицу
        private void LoadDrivers()
        {
            dataGridViewDrivers.Rows.Clear(); // Очистка таблицы перед загрузкой
            foreach (var driver in DriverManager.GetDrivers()) // Перебор всех водителей
            {
                // Добавление информации о водителе в таблицу
                dataGridViewDrivers.Rows.Add(driver.FullName, driver.EmployeeNumber, driver.YearOfBirth, driver.Experience, driver.Category, driver.ClassType);
            }
        }

        // Метод для загрузки поездок в таблицу
        private void LoadTrips()
        {
            dataGridViewSelectedTrips.Rows.Clear(); // Очистка таблицы перед загрузкой
            foreach (var trip in TripManager.GetTrips()) // Перебор всех поездок
            {
                // Добавление информации о поездке в таблицу
                dataGridViewSelectedTrips.Rows.Add(trip.LicensePlate, trip.DriverFullName, trip.ArrivalTime.ToString(), trip.OrderNumber); // Добавляем номер заказа
            }
        }

        // Метод для добавления новой поездки
        private void btnPlanTrip_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Получение данных из полей ввода
                string licensePlate = txtLicensePlate.Text;
                string driverFullName = txtDriverFullName.Text; // Используем ФИО водителя
                DateTime arrivalTime = dtpArrivalTime.Value;

                // Получаем номер заказа из выбранной строки, если она есть
                string orderNumber = null;
                if (dataGridViewSelectedTrips.SelectedRows.Count > 0)
                {
                    var selectedRow = dataGridViewSelectedTrips.SelectedRows[0];
                    orderNumber = selectedRow.Cells["orderNumber"].Value?.ToString();
                }
                else
                {
                    // Генерация нового номера рейса, если не редактируем существующий
                    orderNumber = TripManager.GetNextOrderNumber();
                }

                List<string> errors = new List<string>(); // Список для хранения ошибок

                // Проверка, что все поля заполнены
                if (string.IsNullOrWhiteSpace(licensePlate) || string.IsNullOrWhiteSpace(driverFullName))
                {
                    errors.Add("Пожалуйста, заполните все поля для планирования рейса.");
                }

                // Проверка существования автомобиля
                if (!FleetManager.GetCars().Any(car => car.LicensePlate == licensePlate))
                {
                    errors.Add("Автомобиль с таким номером не существует.");
                }

                // Проверка существования водителя
                var driver = DriverManager.GetDrivers().FirstOrDefault(d => d.FullName == driverFullName); // Используем ФИО
                if (driver == null)
                {
                    errors.Add("Водитель с таким ФИО не существует.");
                }

                // Если редактируем существующий рейс, проверяем на дубликаты
                if (dataGridViewSelectedTrips.SelectedRows.Count > 0)
                {
                    if (TripManager.GetTrips().Any(trip => trip.OrderNumber == orderNumber && trip.OrderNumber != orderNumber))
                    {
                        errors.Add("Поездка с таким номером заказа уже существует.");
                    }
                }
                else
                {
                    // Проверка на дубликаты по номеру заказа
                    if (TripManager.GetTrips().Any(trip => trip.OrderNumber == orderNumber))
                    {
                        errors.Add("Поездка с таким номером заказа уже существует.");
                    }
                }

                // Если есть ошибки, выводим их
                if (errors.Count > 0)
                {
                    string errorMessage = string.Join(Environment.NewLine, errors);
                    MessageBox.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение метода, если есть ошибки
                }

                // Если редактируем существующий рейс
                if (dataGridViewSelectedTrips.SelectedRows.Count > 0)
                {
                    // Обновление существующей поездки
                    Trip existingTrip = TripManager.GetTrips().FirstOrDefault(t => t.OrderNumber == orderNumber);
                    if (existingTrip != null)
                    {
                        existingTrip.LicensePlate = licensePlate;
                        existingTrip.ArrivalTime = arrivalTime;
                        existingTrip.DriverFullName = driver.FullName; // Обновляем ФИО водителя
                    }
                }
                else
                {
                    // Создание новой поездки
                    Trip newTrip = new Trip(licensePlate, arrivalTime, driver.FullName, orderNumber); // Передаем номер заказа
                    TripManager.AddTrip(newTrip); // Добавление поездки в TripManager
                }

                // Обновление списка поездок в DataGridView
                LoadTrips(); // Обновляем список поездок

                // Очистка текстовых полей
                ClearTextFields();
            }
            catch (Exception ex)
            {
                // Обработка исключений и вывод сообщения об ошибке
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод для удаления выбранной поездки
        private void btnDeleteTrip_Click(object sender, EventArgs e)
        {
            // Проверяем, выбран ли элемент в DataGridView
            if (dataGridViewSelectedTrips.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewSelectedTrips.SelectedRows[0];

                // Проверяем, есть ли значение в ячейке "orderNumber"
                if (selectedRow.Cells["orderNumber"].Value != null)
                {
                    string orderNumber = selectedRow.Cells["orderNumber"].Value.ToString();

                    // Запрашиваем подтверждение удаления
                    DialogResult dialogResult = MessageBox.Show($"Вы уверены, что хотите удалить все поездки с номером заказа {orderNumber}?", "Подтверждение удаления", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {
                        // Удаление поездок по номеру заказа
                        TripManager.RemoveTrip(orderNumber); // Предполагается, что вы добавили этот метод в TripManager

                        // Удаление всех строк из DataGridView, соответствующих удаленным поездкам
                        LoadTrips(); // Обновляем список поездок
                    }
                }
                else
                {
                    MessageBox.Show("Выбранная строка пуста. Удаление невозможно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите поездку для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Метод для очист ки текстовых полей
        private void ClearTextFields()
        {
            txtLicensePlate.Clear(); // Очистка поля для номера автомобиля
            txtDriverFullName.Clear(); // Очистка поля для табельного номера водителя
            dtpArrivalTime.Value = DateTime.Now; // Сброс даты на текущее время
           
        }

        // Метод для обработки нажатия кнопки "Очистить поля"
        private void btnClearFields_Click_1(object sender, EventArgs e)
        {
            ClearTextFields(); // Вызов метода очистки текстовых полей
        }

        // Метод для обработки нажатия на ячейку в таблице выбранных поездок
        private void dataGridViewSelectedTrips_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewSelectedTrips.SelectedRows.Count > 0) // Проверка, выбрана ли строка
            {
                var selectedRow = dataGridViewSelectedTrips.SelectedRows[0];

                // Заполняем текстовые поля данными из выбранной строки
                txtLicensePlate.Text = selectedRow.Cells["Nomer"].Value?.ToString() ?? string.Empty; // Номер автомобиля
                txtDriverFullName.Text = selectedRow.Cells["driver"].Value?.ToString() ?? string.Empty; // ФИО водителя
                dtpArrivalTime.Value = DateTime.TryParse(selectedRow.Cells["arrivalTime"].Value?.ToString(), out DateTime arrivalTime) ? arrivalTime : DateTime.Now; // Дата прибытия
            }
        }

        // Метод для обработки нажатия на ячейку в таблице автомобилей
        private void dataGridViewCars_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewCars.SelectedRows.Count > 0) // Проверка, выбрана ли строка
            {
                var selectedRow = dataGridViewCars.SelectedRows[0];

                // Заполняем поле номера автомобиля данными из выбранной строки
                txtLicensePlate.Text = selectedRow.Cells["licensePlate"].Value?.ToString() ?? string.Empty; // Номер автомобиля
            }
        }

        // Метод для обработки нажатия на ячейку в таблице водителей
        private void dataGridViewDrivers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewDrivers.SelectedRows.Count > 0) // Проверка, выбрана ли строка
            {
                var selectedRow = dataGridViewDrivers.SelectedRows[0];

                // Заполняем поле табельного номера и ФИО водителя данными из выбранной строки
                txtDriverFullName.Text = selectedRow.Cells["employeeNumber"].Value?.ToString() ?? string.Empty; // Табельный номер
                                                                                                              // Здесь вы можете добавить логику для отображения ФИО, если у вас есть отдельное поле для этого
            }
        }




        private void InitializeComponent()
        {
            this.dataGridViewSelectedTrips = new System.Windows.Forms.DataGridView();
            this.Nomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.driver = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.arrivalTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCars = new System.Windows.Forms.DataGridView();
            this.licensePlate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.brand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Грузоподъемность = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purpose = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yearOfManufacture = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yearOfRepair = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mileage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.photo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewDrivers = new System.Windows.Forms.DataGridView();
            this.fullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.employeeNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yearOfBirth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.experience = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.classType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtLicensePlate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpArrivalTime = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.btnPlanTrip = new System.Windows.Forms.Button();
            this.btnClearFields = new System.Windows.Forms.Button();
            this.btnDeleteTrip = new System.Windows.Forms.Button();
            this.btnEditTrip = new System.Windows.Forms.Button();
            this.txtDriverFullName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelectedTrips)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCars)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDrivers)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewSelectedTrips
            // 
            this.dataGridViewSelectedTrips.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSelectedTrips.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Nomer,
            this.driver,
            this.arrivalTime,
            this.orderNumber});
            this.dataGridViewSelectedTrips.Location = new System.Drawing.Point(964, 40);
            this.dataGridViewSelectedTrips.Name = "dataGridViewSelectedTrips";
            this.dataGridViewSelectedTrips.RowHeadersWidth = 51;
            this.dataGridViewSelectedTrips.Size = new System.Drawing.Size(445, 456);
            this.dataGridViewSelectedTrips.TabIndex = 0;
            this.dataGridViewSelectedTrips.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSelectedTrips_CellContentClick);
            // 
            // Nomer
            // 
            this.Nomer.HeaderText = "Номер автомобиля";
            this.Nomer.MinimumWidth = 6;
            this.Nomer.Name = "Nomer";
            this.Nomer.Width = 125;
            // 
            // driver
            // 
            this.driver.HeaderText = "Водитель";
            this.driver.MinimumWidth = 6;
            this.driver.Name = "driver";
            this.driver.Width = 125;
            // 
            // arrivalTime
            // 
            this.arrivalTime.HeaderText = "Дата прибытия";
            this.arrivalTime.MinimumWidth = 6;
            this.arrivalTime.Name = "arrivalTime";
            this.arrivalTime.Width = 125;
            // 
            // orderNumber
            // 
            this.orderNumber.HeaderText = "Номер заказа";
            this.orderNumber.MinimumWidth = 6;
            this.orderNumber.Name = "orderNumber";
            this.orderNumber.Width = 125;
            // 
            // dataGridViewCars
            // 
            this.dataGridViewCars.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCars.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.licensePlate,
            this.brand,
            this.Грузоподъемность,
            this.purpose,
            this.yearOfManufacture,
            this.yearOfRepair,
            this.mileage,
            this.photo});
            this.dataGridViewCars.Location = new System.Drawing.Point(520, 40);
            this.dataGridViewCars.Name = "dataGridViewCars";
            this.dataGridViewCars.RowHeadersWidth = 51;
            this.dataGridViewCars.Size = new System.Drawing.Size(408, 196);
            this.dataGridViewCars.TabIndex = 1;
            this.dataGridViewCars.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCars_CellContentClick);
            // 
            // licensePlate
            // 
            this.licensePlate.HeaderText = "Номер автомобиля";
            this.licensePlate.MinimumWidth = 6;
            this.licensePlate.Name = "licensePlate";
            this.licensePlate.Width = 125;
            // 
            // brand
            // 
            this.brand.HeaderText = "Марка";
            this.brand.MinimumWidth = 6;
            this.brand.Name = "brand";
            this.brand.Width = 125;
            // 
            // Грузоподъемность
            // 
            this.Грузоподъемность.HeaderText = "Грузоподъемность";
            this.Грузоподъемность.MinimumWidth = 6;
            this.Грузоподъемность.Name = "Грузоподъемность";
            this.Грузоподъемность.Width = 125;
            // 
            // purpose
            // 
            this.purpose.HeaderText = "Назначение";
            this.purpose.MinimumWidth = 6;
            this.purpose.Name = "purpose";
            this.purpose.Width = 125;
            // 
            // yearOfManufacture
            // 
            this.yearOfManufacture.HeaderText = "Год выпуска";
            this.yearOfManufacture.MinimumWidth = 6;
            this.yearOfManufacture.Name = "yearOfManufacture";
            this.yearOfManufacture.Width = 125;
            // 
            // yearOfRepair
            // 
            this.yearOfRepair.HeaderText = "Год ремонта";
            this.yearOfRepair.MinimumWidth = 6;
            this.yearOfRepair.Name = "yearOfRepair";
            this.yearOfRepair.Width = 125;
            // 
            // mileage
            // 
            this.mileage.HeaderText = "Пробег";
            this.mileage.MinimumWidth = 6;
            this.mileage.Name = "mileage";
            this.mileage.Width = 125;
            // 
            // photo
            // 
            this.photo.HeaderText = "Фото";
            this.photo.MinimumWidth = 6;
            this.photo.Name = "photo";
            this.photo.Width = 125;
            // 
            // dataGridViewDrivers
            // 
            this.dataGridViewDrivers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDrivers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fullName,
            this.employeeNumber,
            this.yearOfBirth,
            this.experience,
            this.category,
            this.classType});
            this.dataGridViewDrivers.Location = new System.Drawing.Point(520, 315);
            this.dataGridViewDrivers.Name = "dataGridViewDrivers";
            this.dataGridViewDrivers.RowHeadersWidth = 51;
            this.dataGridViewDrivers.Size = new System.Drawing.Size(408, 181);
            this.dataGridViewDrivers.TabIndex = 2;
            this.dataGridViewDrivers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDrivers_CellContentClick);
            // 
            // fullName
            // 
            this.fullName.HeaderText = "ФИО";
            this.fullName.MinimumWidth = 6;
            this.fullName.Name = "fullName";
            this.fullName.Width = 125;
            // 
            // employeeNumber
            // 
            this.employeeNumber.HeaderText = "Табельный номер";
            this.employeeNumber.MinimumWidth = 6;
            this.employeeNumber.Name = "employeeNumber";
            this.employeeNumber.Width = 125;
            // 
            // yearOfBirth
            // 
            this.yearOfBirth.HeaderText = "Год рождения";
            this.yearOfBirth.MinimumWidth = 6;
            this.yearOfBirth.Name = "yearOfBirth";
            this.yearOfBirth.Width = 125;
            // 
            // experience
            // 
            this.experience.HeaderText = "Стаж";
            this.experience.MinimumWidth = 6;
            this.experience.Name = "experience";
            this.experience.Width = 125;
            // 
            // category
            // 
            this.category.HeaderText = "Категория";
            this.category.MinimumWidth = 6;
            this.category.Name = "category";
            this.category.Width = 125;
            // 
            // classType
            // 
            this.classType.HeaderText = "Тип класса";
            this.classType.MinimumWidth = 6;
            this.classType.Name = "classType";
            this.classType.Width = 125;
            // 
            // txtLicensePlate
            // 
            this.txtLicensePlate.Location = new System.Drawing.Point(298, 156);
            this.txtLicensePlate.Name = "txtLicensePlate";
            this.txtLicensePlate.Size = new System.Drawing.Size(143, 20);
            this.txtLicensePlate.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(295, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Номер автомобиля ";
            // 
            // dtpArrivalTime
            // 
            this.dtpArrivalTime.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpArrivalTime.Location = new System.Drawing.Point(298, 358);
            this.dtpArrivalTime.Name = "dtpArrivalTime";
            this.dtpArrivalTime.Size = new System.Drawing.Size(149, 20);
            this.dtpArrivalTime.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(295, 342);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Дата прибытия";
            // 
            // btnPlanTrip
            // 
            this.btnPlanTrip.Location = new System.Drawing.Point(54, 40);
            this.btnPlanTrip.Name = "btnPlanTrip";
            this.btnPlanTrip.Size = new System.Drawing.Size(159, 88);
            this.btnPlanTrip.TabIndex = 9;
            this.btnPlanTrip.Text = "Спланировать рейс\r\n\r\n";
            this.btnPlanTrip.UseVisualStyleBackColor = true;
            this.btnPlanTrip.Click += new System.EventHandler(this.btnPlanTrip_Click_1);
            // 
            // btnClearFields
            // 
            this.btnClearFields.Location = new System.Drawing.Point(54, 204);
            this.btnClearFields.Name = "btnClearFields";
            this.btnClearFields.Size = new System.Drawing.Size(159, 77);
            this.btnClearFields.TabIndex = 10;
            this.btnClearFields.Text = "Очистить поля";
            this.btnClearFields.UseVisualStyleBackColor = true;
            this.btnClearFields.Click += new System.EventHandler(this.btnClearFields_Click_1);
            // 
            // btnDeleteTrip
            // 
            this.btnDeleteTrip.Location = new System.Drawing.Point(54, 358);
            this.btnDeleteTrip.Name = "btnDeleteTrip";
            this.btnDeleteTrip.Size = new System.Drawing.Size(159, 85);
            this.btnDeleteTrip.TabIndex = 11;
            this.btnDeleteTrip.Text = "Удалить";
            this.btnDeleteTrip.UseVisualStyleBackColor = true;
            this.btnDeleteTrip.Click += new System.EventHandler(this.btnDeleteTrip_Click);
            // 
            // btnEditTrip
            // 
            this.btnEditTrip.Location = new System.Drawing.Point(54, 492);
            this.btnEditTrip.Name = "btnEditTrip";
            this.btnEditTrip.Size = new System.Drawing.Size(159, 72);
            this.btnEditTrip.TabIndex = 12;
            this.btnEditTrip.Text = "Редактировать рейс";
            this.btnEditTrip.UseVisualStyleBackColor = true;
            this.btnEditTrip.Click += new System.EventHandler(this.btnEditTrip_Click);
            // 
            // txtDriverFullName
            // 
            this.txtDriverFullName.Location = new System.Drawing.Point(298, 258);
            this.txtDriverFullName.Name = "txtDriverFullName";
            this.txtDriverFullName.Size = new System.Drawing.Size(149, 20);
            this.txtDriverFullName.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(295, 239);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Введите ФИО водителя:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 567);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(326, 65);
            this.label2.TabIndex = 15;
            this.label2.Text = "Для редатирования выберите рейс из списка\r\nДалее нажмите на столбец номер автомоб" +
    "иля\r\nПосле чего в полях вы можете изменить нужную информацию\r\nНажмите редактиров" +
    "ать\r\n\r\n";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(520, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(162, 19);
            this.label5.TabIndex = 16;
            this.label5.Text = "Список автомобилей:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(520, 288);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(142, 19);
            this.label6.TabIndex = 17;
            this.label6.Text = "Список водителей:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(961, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(118, 19);
            this.label7.TabIndex = 18;
            this.label7.Text = "Список рейсов:";
            // 
            // TripManagerForm
            // 
            this.ClientSize = new System.Drawing.Size(1432, 633);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtDriverFullName);
            this.Controls.Add(this.btnEditTrip);
            this.Controls.Add(this.btnDeleteTrip);
            this.Controls.Add(this.btnClearFields);
            this.Controls.Add(this.btnPlanTrip);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtpArrivalTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLicensePlate);
            this.Controls.Add(this.dataGridViewDrivers);
            this.Controls.Add(this.dataGridViewCars);
            this.Controls.Add(this.dataGridViewSelectedTrips);
            this.Name = "TripManagerForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelectedTrips)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCars)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDrivers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void btnEditTrip_Click(object sender, EventArgs e)
        {
            // Проверяем, что выбран рейс
            if (dataGridViewSelectedTrips.SelectedRows.Count > 0) // Проверка, выбрана ли строка
            {
                var selectedRow = dataGridViewSelectedTrips.SelectedRows[0];

                // Получаем номер заказа из выбранной строки
                string orderNumber = selectedRow.Cells["orderNumber"].Value?.ToString();

                // Проверяем, что все поля заполнены
                if (string.IsNullOrWhiteSpace(txtLicensePlate.Text) || string.IsNullOrWhiteSpace(txtDriverFullName.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля для редактирования рейса.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение метода, если есть ошибки
                }

                List<string> errors = new List<string>();

                // Проверка существования автомобиля
                if (!FleetManager.GetCars().Any(car => car.LicensePlate == txtLicensePlate.Text))
                {
                    errors.Add("Автомобиль с таким номером не существует.");
                }

                // Проверка существования водителя
                var driver = DriverManager.GetDrivers().FirstOrDefault(d => d.FullName == txtDriverFullName.Text);
                if (driver == null)
                {
                    errors.Add("Водитель с таким ФИО не существует.");
                }

                // Проверка на дубликаты по номеру заказа
                if (TripManager.GetTrips().Any(trip => trip.OrderNumber == orderNumber && trip.OrderNumber != orderNumber))
                {
                    errors.Add("Поездка с таким номером заказа уже существует.");
                }

                // Если есть ошибки, выводим их
                if (errors.Count > 0)
                {
                    MessageBox.Show(string.Join(Environment.NewLine, errors), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение метода, если есть ошибки
                }

                // Обновление существующей поездки
                Trip existingTrip = TripManager.GetTrips().FirstOrDefault(t => t.OrderNumber == orderNumber);
                if (existingTrip != null)
                {
                    existingTrip.LicensePlate = txtLicensePlate.Text; // Обновляем номер автомобиля
                    existingTrip.ArrivalTime = dtpArrivalTime.Value; // Обновляем дату прибытия
                    existingTrip.DriverFullName = driver.FullName; // Обновляем ФИО водителя
                }

                // Обновление списка поездок в DataGridView
                LoadTrips(); // Обновляем список поездок

                // Очистка текстовых полей
                ClearTextFields();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите рейс для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}