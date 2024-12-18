using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FreightTransportSystem
{
    public partial class CarManagerForm : Form
    {
        private TextBox txtLicensePlate;
        private TextBox txtYearOfRepair;
        private TextBox txtYearOfManufacture;
        private TextBox txtPurpose;
        private TextBox txtLoadCapacity;
        private TextBox txtBrand;
        private TextBox txtPhoto;
        private TextBox txtMileage;
        private Button btnAddCar;
        private Button btnRemoveCar;
        private Button btnSelectPhoto;
        private Button btnClear;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private DataGridViewTextBoxColumn LicensePlate;
        private DataGridViewTextBoxColumn Brand;
        private DataGridViewTextBoxColumn LoadCapacity;
        private DataGridViewTextBoxColumn Purpose;
        private DataGridViewTextBoxColumn YearOfManufacture;
        private DataGridViewTextBoxColumn YearOfRepair;
        private DataGridViewTextBoxColumn Mileage;
        private DataGridViewTextBoxColumn Photo;
        private Label label7;
        private Button btnEditCar;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private DataGridView dataGridViewCars;

        public CarManagerForm()
        {
            InitializeComponent();
            LoadCars();
        }

        private void LoadCars()
        {
            dataGridViewCars.Rows.Clear();
            foreach (var car in FleetManager.GetCars())
            {
                dataGridViewCars.Rows.Add(car.LicensePlate, car.Brand, car.LoadCapacity, car.Purpose, car.YearOfManufacture, car.YearOfRepair, car.Mileage, car.Photo);
            }
        }

        private void ClearFields()
        {
            txtLicensePlate.Clear();
            txtBrand.Clear();
            txtLoadCapacity.Clear();
            txtPurpose.Clear();
            txtYearOfManufacture.Clear();
            txtYearOfRepair.Clear();
            txtMileage.Clear();
            txtPhoto.Clear();
        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            try
            {
                string licensePlate = txtLicensePlate.Text.Trim(); // Удаляем пробелы
                string brand = txtBrand.Text.Trim();
                string loadCapacity = txtLoadCapacity.Text.Trim();
                string purpose = txtPurpose.Text.Trim();
                string yearOfManufacture = txtYearOfManufacture.Text.Trim();
                string yearOfRepair = txtYearOfRepair.Text.Trim();
                string mileage = txtMileage.Text.Trim();
                string photo = txtPhoto.Text.Trim();

                List<string> errors = new List<string>();

                if (!Regex.IsMatch(licensePlate, @"^[a-zA-Zа-яА-Я0-9]{1,9}$"))
                {
                    errors.Add("Номер автомобиля должен содержать только буквы и цифры и не превышать 9 символов.");
                }

                if (!Regex.IsMatch(brand, @"^[a-zA-Zа-яА-Я0-9]{1,30}$"))
                {
                    errors.Add("Марка должна содержать только буквы и цифры и не превышать 30 символов.");
                }

                if (!Regex.IsMatch(loadCapacity, @"^[0-9]{1,5}$") || int.Parse(loadCapacity) > 40000)
                {
                    errors.Add("Грузоподъемность должна быть целым числом от 1 до 40000.");
                }

                if (!Regex.IsMatch(purpose, @"^[a-zA-Zа-яА-Я]+$"))
                {
                    errors.Add("Назначение должно содержать только буквы.");
                }

                if (!Regex.IsMatch(yearOfManufacture, @"^[0-9]{4}$"))
                {
                    errors.Add("Год выпуска должен быть целым числом из 4 цифр.");
                }

                if (!Regex.IsMatch(yearOfRepair, @"^[0-9]{4}$"))
                {
                    errors.Add("Год ремонта должен быть целым числом из 4 цифр.");
                }

                if (!Regex.IsMatch(mileage, @"^[0-9]{1,7}$") || int.Parse(mileage) > 1000000)
                {
                    errors.Add("Пробег должен быть целым числом от 1 до 1000000.");
                }

                if (errors.Count > 0)
                {
                    string errorMessage = string.Join(Environment.NewLine, errors);
                    MessageBox.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                double loadCapacityDouble = Convert.ToDouble(loadCapacity);
                double mileageDouble = Convert.ToDouble(mileage);
                int yearOfManufactureInt = Convert.ToInt32(yearOfManufacture);
                int yearOfRepairInt = Convert.ToInt32(yearOfRepair);

                Car newCar = new Car(licensePlate, brand, loadCapacityDouble, purpose, yearOfManufactureInt, yearOfRepairInt, mileageDouble, photo);
                FleetManager.GetCars().Add(newCar);
                LoadCars();
                ClearFields();
            }
            catch (FormatException)
            {
                MessageBox.Show("Ошибка ввода: Пожалуйста, убедитесь, что все поля заполнены корректными значениями.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemoveCar_Click(object sender, EventArgs e)
        {
            // Проверяем, выбран ли элемент в DataGridView
            if (dataGridViewCars.SelectedRows.Count > 0)
            {
                // Получаем номер автомобиля из выбранной строки
                var selectedRow = dataGridViewCars.SelectedRows[0];

                // Проверяем, есть ли значение в ячейке "LicensePlate"
                if (selectedRow.Cells["LicensePlate"].Value != null)
                {
                    string licensePlate = selectedRow.Cells["LicensePlate"].Value.ToString();

                    // Запрашиваем подтверждение удаления
                    DialogResult dialogResult = MessageBox.Show($"Вы уверены, что хотите удалить автомобиль с номером {licensePlate}?", "Подтверждение удаления", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {
                        // Ищем автомобиль по госномеру
                        Car carToRemove = FleetManager.FindCar(licensePlate);
                        if (carToRemove != null)
                        {
                            FleetManager.GetCars().Remove(carToRemove); // Удаляем автомобиль
                            LoadCars(); // Обновляем список автомобилей
                        }
                        else
                        {
                            MessageBox.Show("Автомобиль не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Выбранная строка пуста. Удаление невозможно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите автомобиль для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnSelectPhoto_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp|All Files|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtPhoto.Text = openFileDialog.FileName; // Сохраняем путь к файлу
            }
        }

        private void dataGridViewCars_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewCars.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewCars.SelectedRows[0];

                if (selectedRow.Cells["LicensePlate"].Value != null)
                    txtLicensePlate.Text = selectedRow.Cells["LicensePlate"].Value.ToString();
                else
                    txtLicensePlate.Clear();

                if (selectedRow.Cells["Brand"].Value != null)
                    txtBrand.Text = selectedRow.Cells["Brand"].Value.ToString();
                else
                    txtBrand.Clear();

                if (selectedRow.Cells["LoadCapacity"].Value != null)
                    txtLoadCapacity.Text = selectedRow.Cells["LoadCapacity"].Value.ToString();
                else
                    txtLoadCapacity.Clear();

                if (selectedRow.Cells["Purpose"].Value != null)
                    txtPurpose.Text = selectedRow.Cells["Purpose"].Value.ToString();
                else txtPurpose.Clear();

                if (selectedRow.Cells["YearOfManufacture"].Value != null)
                    txtYearOfManufacture.Text = selectedRow.Cells["YearOfManufacture"].Value.ToString();
                else
                    txtYearOfManufacture.Clear();

                if (selectedRow.Cells["YearOfRepair"].Value != null)
                    txtYearOfRepair.Text = selectedRow.Cells["YearOfRepair"].Value.ToString();
                else
                    txtYearOfRepair.Clear();

                if (selectedRow.Cells["Mileage"].Value != null)
                    txtMileage.Text = selectedRow.Cells["Mileage"].Value.ToString();
                else
                    txtMileage.Clear();

                if (selectedRow.Cells["Photo"].Value != null)
                    txtPhoto.Text = selectedRow.Cells["Photo"].Value.ToString();
                else
                    txtPhoto.Clear();
            }
        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
            if (dataGridViewCars.SelectedRows.Count > 0) // Проверка, выбрана ли строка
            {
                var selectedRow = dataGridViewCars.SelectedRows[0];

                // Проверка, что все поля заполнены
                if (string.IsNullOrWhiteSpace(txtLicensePlate.Text) ||
                    string.IsNullOrWhiteSpace(txtBrand.Text) ||
                    string.IsNullOrWhiteSpace(txtLoadCapacity.Text) ||
                    string.IsNullOrWhiteSpace(txtPurpose.Text) ||
                    string.IsNullOrWhiteSpace(txtYearOfManufacture.Text) ||
                    string.IsNullOrWhiteSpace(txtYearOfRepair.Text) ||
                    string.IsNullOrWhiteSpace(txtMileage.Text) ||
                    string.IsNullOrWhiteSpace(txtPhoto.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Прерываем выполнение метода, если есть пустые поля
                }

                // Получение номера автомобиля
                if (selectedRow.Cells["LicensePlate"].Value != null)
                {
                    string licensePlate = selectedRow.Cells["LicensePlate"].Value.ToString();
                    Car carToEdit = FleetManager.FindCar(licensePlate);
                    if (carToEdit != null)
                    {
                        // Получение данных из текстовых полей с удалением пробелов
                        string brand = txtBrand.Text.Trim();
                        string loadCapacity = txtLoadCapacity.Text.Trim();
                        string purpose = txtPurpose.Text.Trim();
                        string yearOfManufacture = txtYearOfManufacture.Text.Trim();
                        string yearOfRepair = txtYearOfRepair.Text.Trim();
                        string mileage = txtMileage.Text.Trim();
                        string photo = txtPhoto.Text.Trim();

                        List<string> errors = new List<string>(); // Список для хранения ошибок валидации

                        // Проверка номера автомобиля
                        if (!Regex.IsMatch(licensePlate, @"^[a-zA-Zа-яА-Я0-9]{1,9}$"))
                        {
                            errors.Add("Номер автомобиля должен содержать только буквы и цифры и не превышать 9 символов.");
                        }

                        // Проверка марки
                        if (!Regex.IsMatch(brand, @"^[a-zA-Zа-яА-Я0-9]{1,30}$"))
                        {
                            errors.Add("Марка должна содержать только буквы и цифры и не превышать 30 символов.");
                        }

                        // Проверка грузоподъемности
                        if (!Regex.IsMatch(loadCapacity, @"^[0-9]{1,5}$") || int.Parse(loadCapacity) > 40000)
                        {
                            errors.Add("Грузоподъемность должна быть целым числом от 1 до 40000.");
                        }

                        // Проверка назначения
                        if (!Regex.IsMatch(purpose, @"^[a-zA-Zа-яА-Я]+$"))
                        {
                            errors.Add("Назначение должно содержать только буквы.");
                        }

                        // Проверка года выпуска
                        if (!Regex.IsMatch(yearOfManufacture, @"^[0-9]{4}$"))
                        {
                            errors.Add("Год выпуска должен быть целым числом из 4 цифр.");
                        }

                        // Проверка года ремонта
                        if (!Regex.IsMatch(yearOfRepair, @"^[0-9]{4}$"))
                        {
                            errors.Add("Год ремонта должен быть целым числом из 4 цифр.");
                        }

                        // Проверка пробега
                        if (!Regex.IsMatch(mileage, @"^[0-9]{1,7}$") || int.Parse(mileage) > 1000000)
                        {
                            errors.Add("Пробег должен быть целым числом от 1 до 1000000.");
                        }

                        // Если есть ошибки, выводим их в сообщении
                        if (errors.Count > 0)
                        {
                            string errorMessage = string.Join(Environment.NewLine, errors);
                            MessageBox.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; // Прерываем выполнение метода, если есть ошибки
                        }

                        // Обновление информации о автомобиле
                        carToEdit.Brand = brand;
                        carToEdit.LoadCapacity = double.Parse(loadCapacity);
                        carToEdit.Purpose = purpose;
                        carToEdit.YearOfManufacture = int.Parse(yearOfManufacture);
                        carToEdit.YearOfRepair = int.Parse(yearOfRepair);
                        carToEdit.Mileage = double.Parse(mileage);
                        carToEdit.Photo = photo;

                        LoadCars(); // Обновление списка автомобилей
                        ClearFields(); // Очистка полей ввода
                    }
                    else
                    {
                        MessageBox.Show("Автомобиль не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Выбранная строка пуста. Редактирование невозможно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите автомобиль для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }




        private void InitializeComponent()
        {
            this.dataGridViewCars = new System.Windows.Forms.DataGridView();
            this.LicensePlate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Brand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LoadCapacity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Purpose = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YearOfManufacture = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YearOfRepair = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mileage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Photo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtLicensePlate = new System.Windows.Forms.TextBox();
            this.txtYearOfRepair = new System.Windows.Forms.TextBox();
            this.txtYearOfManufacture = new System.Windows.Forms.TextBox();
            this.txtPurpose = new System.Windows.Forms.TextBox();
            this.txtLoadCapacity = new System.Windows.Forms.TextBox();
            this.txtBrand = new System.Windows.Forms.TextBox();
            this.txtPhoto = new System.Windows.Forms.TextBox();
            this.txtMileage = new System.Windows.Forms.TextBox();
            this.btnAddCar = new System.Windows.Forms.Button();
            this.btnRemoveCar = new System.Windows.Forms.Button();
            this.btnSelectPhoto = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnEditCar = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCars)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewCars
            // 
            this.dataGridViewCars.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCars.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LicensePlate,
            this.Brand,
            this.LoadCapacity,
            this.Purpose,
            this.YearOfManufacture,
            this.YearOfRepair,
            this.Mileage,
            this.Photo});
            this.dataGridViewCars.Location = new System.Drawing.Point(352, 135);
            this.dataGridViewCars.Name = "dataGridViewCars";
            this.dataGridViewCars.RowHeadersWidth = 51;
            this.dataGridViewCars.Size = new System.Drawing.Size(1023, 549);
            this.dataGridViewCars.TabIndex = 0;
            this.dataGridViewCars.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCars_CellContentClick);
            // 
            // LicensePlate
            // 
            this.LicensePlate.HeaderText = "Номер автомобиля";
            this.LicensePlate.MinimumWidth = 6;
            this.LicensePlate.Name = "LicensePlate";
            this.LicensePlate.Width = 125;
            // 
            // Brand
            // 
            this.Brand.HeaderText = "Марка";
            this.Brand.MinimumWidth = 6;
            this.Brand.Name = "Brand";
            this.Brand.Width = 125;
            // 
            // LoadCapacity
            // 
            this.LoadCapacity.HeaderText = "Грузоподъемность";
            this.LoadCapacity.MinimumWidth = 6;
            this.LoadCapacity.Name = "LoadCapacity";
            this.LoadCapacity.Width = 125;
            // 
            // Purpose
            // 
            this.Purpose.HeaderText = "Назначение";
            this.Purpose.MinimumWidth = 6;
            this.Purpose.Name = "Purpose";
            this.Purpose.Width = 125;
            // 
            // YearOfManufacture
            // 
            this.YearOfManufacture.HeaderText = "Год выпуска";
            this.YearOfManufacture.MinimumWidth = 6;
            this.YearOfManufacture.Name = "YearOfManufacture";
            this.YearOfManufacture.Width = 125;
            // 
            // YearOfRepair
            // 
            this.YearOfRepair.HeaderText = "Год ремонта";
            this.YearOfRepair.MinimumWidth = 6;
            this.YearOfRepair.Name = "YearOfRepair";
            this.YearOfRepair.Width = 125;
            // 
            // Mileage
            // 
            this.Mileage.HeaderText = "Пробег";
            this.Mileage.MinimumWidth = 6;
            this.Mileage.Name = "Mileage";
            this.Mileage.Width = 125;
            // 
            // Photo
            // 
            this.Photo.HeaderText = "Фото";
            this.Photo.MinimumWidth = 6;
            this.Photo.Name = "Photo";
            this.Photo.Width = 125;
            // 
            // txtLicensePlate
            // 
            this.txtLicensePlate.Location = new System.Drawing.Point(24, 135);
            this.txtLicensePlate.Name = "txtLicensePlate";
            this.txtLicensePlate.Size = new System.Drawing.Size(147, 20);
            this.txtLicensePlate.TabIndex = 1;
            // 
            // txtYearOfRepair
            // 
            this.txtYearOfRepair.Location = new System.Drawing.Point(24, 580);
            this.txtYearOfRepair.Name = "txtYearOfRepair";
            this.txtYearOfRepair.Size = new System.Drawing.Size(147, 20);
            this.txtYearOfRepair.TabIndex = 2;
            // 
            // txtYearOfManufacture
            // 
            this.txtYearOfManufacture.Location = new System.Drawing.Point(24, 489);
            this.txtYearOfManufacture.Name = "txtYearOfManufacture";
            this.txtYearOfManufacture.Size = new System.Drawing.Size(147, 20);
            this.txtYearOfManufacture.TabIndex = 3;
            // 
            // txtPurpose
            // 
            this.txtPurpose.Location = new System.Drawing.Point(24, 393);
            this.txtPurpose.Name = "txtPurpose";
            this.txtPurpose.Size = new System.Drawing.Size(147, 20);
            this.txtPurpose.TabIndex = 4;
            // 
            // txtLoadCapacity
            // 
            this.txtLoadCapacity.Location = new System.Drawing.Point(24, 308);
            this.txtLoadCapacity.Name = "txtLoadCapacity";
            this.txtLoadCapacity.Size = new System.Drawing.Size(147, 20);
            this.txtLoadCapacity.TabIndex = 5;
            // 
            // txtBrand
            // 
            this.txtBrand.Location = new System.Drawing.Point(24, 225);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(147, 20);
            this.txtBrand.TabIndex = 6;
            // 
            // txtPhoto
            // 
            this.txtPhoto.Location = new System.Drawing.Point(205, 252);
            this.txtPhoto.Name = "txtPhoto";
            this.txtPhoto.Size = new System.Drawing.Size(121, 20);
            this.txtPhoto.TabIndex = 7;
            // 
            // txtMileage
            // 
            this.txtMileage.Location = new System.Drawing.Point(24, 655);
            this.txtMileage.Name = "txtMileage";
            this.txtMileage.Size = new System.Drawing.Size(121, 20);
            this.txtMileage.TabIndex = 8;
            // 
            // btnAddCar
            // 
            this.btnAddCar.Location = new System.Drawing.Point(12, 12);
            this.btnAddCar.Name = "btnAddCar";
            this.btnAddCar.Size = new System.Drawing.Size(170, 32);
            this.btnAddCar.TabIndex = 10;
            this.btnAddCar.Text = "Добавить автомобиль";
            this.btnAddCar.UseVisualStyleBackColor = true;
            this.btnAddCar.Click += new System.EventHandler(this.btnAddCar_Click);
            // 
            // btnRemoveCar
            // 
            this.btnRemoveCar.Location = new System.Drawing.Point(188, 14);
            this.btnRemoveCar.Name = "btnRemoveCar";
            this.btnRemoveCar.Size = new System.Drawing.Size(170, 30);
            this.btnRemoveCar.TabIndex = 11;
            this.btnRemoveCar.Text = "Удалить автомобиль";
            this.btnRemoveCar.UseVisualStyleBackColor = true;
            this.btnRemoveCar.Click += new System.EventHandler(this.btnRemoveCar_Click);
            // 
            // btnSelectPhoto
            // 
            this.btnSelectPhoto.Location = new System.Drawing.Point(205, 133);
            this.btnSelectPhoto.Name = "btnSelectPhoto";
            this.btnSelectPhoto.Size = new System.Drawing.Size(121, 34);
            this.btnSelectPhoto.TabIndex = 13;
            this.btnSelectPhoto.Text = "Добавить фото:";
            this.btnSelectPhoto.UseVisualStyleBackColor = true;
            this.btnSelectPhoto.Click += new System.EventHandler(this.btnSelectPhoto_Click_1);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(364, 14);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(170, 30);
            this.btnClear.TabIndex = 14;
            this.btnClear.Text = "Очистить поля";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(21, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Номер автомобиля:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 209);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Введите марку:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 292);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Введите грузоподьменость:\r\n";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 377);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Введите назначение:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 639);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Введите пробег:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 564);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Год ремонта:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 473);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Год выпуска:";
            // 
            // btnEditCar
            // 
            this.btnEditCar.Location = new System.Drawing.Point(540, 12);
            this.btnEditCar.Name = "btnEditCar";
            this.btnEditCar.Size = new System.Drawing.Size(170, 32);
            this.btnEditCar.TabIndex = 25;
            this.btnEditCar.Text = "Редактировать";
            this.btnEditCar.UseVisualStyleBackColor = true;
            this.btnEditCar.Click += new System.EventHandler(this.btnEditCar_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(205, 236);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Путь к фото:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(348, 107);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(162, 19);
            this.label9.TabIndex = 27;
            this.label9.Text = "Список автомобилей:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(537, 47);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(326, 52);
            this.label10.TabIndex = 28;
            this.label10.Text = "Для редатирования выберите автомобиль из списка\r\nДалее нажмите на столбец номер а" +
    "втомобиля\r\nПосле чего в полях вы можете изменить нужную информацию\r\nНажмите реда" +
    "ктировать\r\n";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(202, 170);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(134, 39);
            this.label11.TabIndex = 29;
            this.label11.Text = "Для выбора пути к фото \r\nнажмити на кнопку\r\n\"Добавить фото\"";
            // 
            // CarManagerForm
            // 
            this.ClientSize = new System.Drawing.Size(1387, 719);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnEditCar);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSelectPhoto);
            this.Controls.Add(this.btnRemoveCar);
            this.Controls.Add(this.btnAddCar);
            this.Controls.Add(this.txtMileage);
            this.Controls.Add(this.txtPhoto);
            this.Controls.Add(this.txtBrand);
            this.Controls.Add(this.txtLoadCapacity);
            this.Controls.Add(this.txtPurpose);
            this.Controls.Add(this.txtYearOfManufacture);
            this.Controls.Add(this.txtYearOfRepair);
            this.Controls.Add(this.txtLicensePlate);
            this.Controls.Add(this.dataGridViewCars);
            this.Name = "CarManagerForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCars)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       
    }
}
