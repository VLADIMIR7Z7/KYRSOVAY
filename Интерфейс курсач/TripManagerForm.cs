using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FreightTransportSystem
{
    public partial class TripManagerForm : Form
    {
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
        private TextBox txtDriverNumber;
        private Label label1;
        private Label label2;
        private DateTimePicker dtpArrivalTime;
        private Label label3;
        private Button btnPlanTrip;
        private Button btnClearFields;
        private Button btnDeleteTrip;
        private DataGridViewTextBoxColumn Nomer;
        private DataGridViewTextBoxColumn driver;
        private DataGridViewTextBoxColumn arrivalTime;
        private DataGridView dataGridViewSelectedTrips;

        public TripManagerForm()
        {
            InitializeComponent();
            LoadCars();
            LoadDrivers();
            LoadTrips(); // Загрузка поездок
        }

        private void LoadCars()
        {
            dataGridViewCars.Rows.Clear();
            foreach (var car in FleetManager.GetCars())
            {
                dataGridViewCars.Rows.Add(car.LicensePlate, car.Brand, car.LoadCapacity, car.Purpose, car.YearOfManufacture, car.YearOfRepair, car.Mileage, car.Photo);
            }
        }

        private void LoadDrivers()
        {
            dataGridViewDrivers.Rows.Clear();
            foreach (var driver in DriverManager.GetDrivers())
            {
                dataGridViewDrivers.Rows.Add(driver.FullName, driver.EmployeeNumber, driver.YearOfBirth, driver.Experience, driver.Category, driver.ClassType);
            }
        }

        private void LoadTrips()
        {
            dataGridViewSelectedTrips.Rows.Clear(); // Очистка таблицы перед загрузкой
            DataStorage.LoadTrips(); // Загрузка поездок из DataStorage
            foreach (var trip in TripManager.GetTrips())
            {
                dataGridViewSelectedTrips.Rows.Add(trip.LicensePlate, trip.DriverFullName, trip.ArrivalTime.ToString());
            }
        }

        private void btnPlanTrip_Click_1(object sender, EventArgs e)
        {
            string licensePlate = txtLicensePlate.Text;
            string driverNumber = txtDriverNumber.Text;
            DateTime arrivalTime = dtpArrivalTime.Value;

            // Проверка существования автомобиля
            if (!FleetManager.GetCars().Any(car => car.LicensePlate == licensePlate))
            {
                MessageBox.Show("Автомобиль с таким номером не существует.");
                return;
            }

            // Проверка существования водителя
            var driver = DriverManager.GetDrivers().FirstOrDefault(d => d.EmployeeNumber == driverNumber);
            if (driver == null)
            {
                MessageBox.Show("Водитель с таким табельным номером не существует.");
                return;
            }

            // Проверка на дубликаты перед добавлением
            var trips = TripManager.GetTrips() ?? new List<Trip>();
            if (trips.Any(trip => trip.LicensePlate == licensePlate && trip.DriverFullName == driver.FullName && trip.ArrivalTime == arrivalTime))
            {
                MessageBox.Show("Поездка с таким номером автомобиля и временем прибытия уже существует.");
                return;
            }

            // Добавление поездки
            Trip newTrip = new Trip(licensePlate, arrivalTime, driver.FullName); // Сохраняем ФИО водителя
            trips.Add(newTrip);

            // Добавление новой строки в dataGridViewSelectedTrips
            dataGridViewSelectedTrips.Rows.Add(newTrip.LicensePlate, newTrip.DriverFullName, newTrip.ArrivalTime.ToString());

            // Сохранение поездок в файл
            DataStorage.SaveTrips();

            // Очистка текстовых полей
            ClearTextFields();
        }

        private void ClearTextFields()
        {
            txtLicensePlate.Clear();
            txtDriverNumber.Clear();
            dtpArrivalTime.Value = DateTime.Now; // Сброс даты на текущее время
        }


        private void btnClearFields_Click_1(object sender, EventArgs e)
        {
            ClearTextFields();
        }

        private void btnDeleteTrip_Click(object sender, EventArgs e)
        {
            if (dataGridViewSelectedTrips.SelectedRows.Count > 0)
            {
                // Получаем выбранную строку
                var selectedRow = dataGridViewSelectedTrips.SelectedRows[0];

                // Используйте правильные имена столбцов
                string licensePlate = selectedRow.Cells["Nomer"].Value?.ToString(); // Замените на правильное имя
                DateTime arrivalTime = Convert.ToDateTime(selectedRow.Cells["arrivalTime"].Value);
                string driverFullName = selectedRow.Cells["driver"].Value?.ToString(); // Получаем ФИО водителя

                // Удаляем поездку из TripManager
                var tripToRemove = TripManager.GetTrips().FirstOrDefault(trip => trip.LicensePlate == licensePlate && trip.ArrivalTime == arrivalTime && trip.DriverFullName == driverFullName);
                if (tripToRemove != null)
                {
                    TripManager.GetTrips().Remove(tripToRemove);
                    dataGridViewSelectedTrips.Rows.Remove(selectedRow); // Удаляем строку из DataGridView

                    // Сохраняем изменения в файл
                    DataStorage.SaveTrips();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для удаления.");
            }
        }

        private void InitializeComponent()
        {
            this.dataGridViewSelectedTrips = new System.Windows.Forms.DataGridView();
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
            this.txtDriverNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpArrivalTime = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.btnPlanTrip = new System.Windows.Forms.Button();
            this.btnClearFields = new System.Windows.Forms.Button();
            this.btnDeleteTrip = new System.Windows.Forms.Button();
            this.Nomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.driver = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.arrivalTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.arrivalTime});
            this.dataGridViewSelectedTrips.Location = new System.Drawing.Point(964, 40);
            this.dataGridViewSelectedTrips.Name = "dataGridViewSelectedTrips";
            this.dataGridViewSelectedTrips.Size = new System.Drawing.Size(445, 456);
            this.dataGridViewSelectedTrips.TabIndex = 0;
            // 
            // dataGridViewCars
            // 
            this.dataGridViewCars.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCars.Columns.AddRange (new System.Windows.Forms.DataGridViewColumn[] {
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
            this.dataGridViewCars.Size = new System.Drawing.Size(408, 196);
            this.dataGridViewCars.TabIndex = 1;
            // 
            // licensePlate
            // 
            this.licensePlate.HeaderText = "Номер автомобиля";
            this.licensePlate.Name = "licensePlate";
            // 
            // brand
            // 
            this.brand.HeaderText = "Марка";
            this.brand.Name = "brand";
            // 
            // Грузоподъемность
            // 
            this.Грузоподъемность.HeaderText = "Грузоподъемность";
            this.Грузоподъемность.Name = "Грузоподъемность";
            // 
            // purpose
            // 
            this.purpose.HeaderText = "Назначение";
            this.purpose.Name = "purpose";
            // 
            // yearOfManufacture
            // 
            this.yearOfManufacture.HeaderText = "Год выпуска";
            this.yearOfManufacture.Name = "yearOfManufacture";
            // 
            // yearOfRepair
            // 
            this.yearOfRepair.HeaderText = "Год ремонта";
            this.yearOfRepair.Name = "yearOfRepair";
            // 
            // mileage
            // 
            this.mileage.HeaderText = "Пробег";
            this.mileage.Name = "mileage";
            // 
            // photo
            // 
            this.photo.HeaderText = "Фото";
            this.photo.Name = "photo";
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
            this.dataGridViewDrivers.Size = new System.Drawing.Size(408, 181);
            this.dataGridViewDrivers.TabIndex = 2;
            // 
            // fullName
            // 
            this.fullName.HeaderText = "ФИО";
            this.fullName.Name = "fullName";
            // 
            // employeeNumber
            // 
            this.employeeNumber.HeaderText = "Табельный номер";
            this.employeeNumber.Name = "employeeNumber";
            // 
            // yearOfBirth
            // 
            this.yearOfBirth.HeaderText = "Год рождения";
            this.yearOfBirth.Name = "yearOfBirth";
            // 
            // experience
            // 
            this.experience.HeaderText = "Стаж";
            this.experience.Name = "experience";
            // 
            // category
            // 
            this.category.HeaderText = "Категория";
            this.category.Name = "category";
            // 
            // classType
            // 
            this.classType.HeaderText = "Тип класса";
            this.classType.Name = "classType";
            // 
            // txtLicensePlate
            // 
            this.txtLicensePlate.Location = new System.Drawing.Point(300, 122);
            this.txtLicensePlate.Multiline = true;
            this.txtLicensePlate.Name = "txtLicensePlate";
            this.txtLicensePlate.Size = new System.Drawing.Size(143, 45);
            this.txtLicensePlate.TabIndex = 3;
            // 
            // txtDriverNumber
            // 
            this.txtDriverNumber.Location = new System.Drawing.Point(300, 252);
            this.txtDriverNumber.Multiline = true;
            this.txtDriverNumber.Name = "txtDriverNumber";
            this.txtDriverNumber.Size = new System.Drawing.Size(143, 45);
            this.txtDriverNumber.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(297, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Номер автомобиля ";
            // 
            // label2
            // ```csharp
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(297, 236);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Табельный номер водителя ";
            // 
            // dtpArrivalTime
            // 
            this.dtpArrivalTime.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpArrivalTime.Location = new System.Drawing.Point(300, 357);
            this.dtpArrivalTime.Name = "dtpArrivalTime";
            this.dtpArrivalTime.Size = new System.Drawing.Size(149, 20);
            this.dtpArrivalTime.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(297, 341);
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
            // Nomer
            // 
            this.Nomer.HeaderText = "Номер автомобиля";
            this.Nomer.Name = "Nomer";
            // 
            // driver
            // 
            this.driver.HeaderText = "Водитель";
            this.driver.Name = "driver";
            // 
            // arrivalTime
            // 
            this.arrivalTime.HeaderText = "Дата прибытия";
            this.arrivalTime.Name = "arrivalTime";
            // 
            // TripManagerForm
            // 
            this.ClientSize = new System.Drawing.Size(1432, 633);
            this.Controls.Add(this.btnDeleteTrip);
            this.Controls.Add(this.btnClearFields);
            this.Controls.Add(this.btnPlanTrip);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtpArrivalTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDriverNumber);
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
    }
}