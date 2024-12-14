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
                string licensePlate = txtLicensePlate.Text;
                string brand = txtBrand.Text;
                string loadCapacity = txtLoadCapacity.Text;
                string purpose = txtPurpose.Text;
                string yearOfManufacture = txtYearOfManufacture.Text;
                string yearOfRepair = txtYearOfRepair.Text;
                string mileage = txtMileage.Text;
                string photo = txtPhoto.Text;

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
                    string errorMessage = "";
                    foreach (var error in errors)
                    {
                        errorMessage += error + Environment.NewLine;
                    }
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
                else
                    txtPurpose.Clear();

                if (selectedRow.Cells["YearOfManufacture"].Value != null)  // Добавлено поле YearOfManufacture
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

        private void InitializeComponent()
        {
            this.dataGridViewCars = new System.Windows.Forms.DataGridView();
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
            this.LicensePlate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Brand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LoadCapacity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Purpose = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YearOfManufacture = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YearOfRepair = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mileage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Photo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label7 = new System.Windows.Forms.Label();
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
            this.dataGridViewCars.Location = new System.Drawing.Point(606, 62);
            this.dataGridViewCars.Name = "dataGridViewCars";
            this.dataGridViewCars.Size = new System.Drawing.Size(713, 516);
            this.dataGridViewCars.TabIndex = 0;
            this.dataGridViewCars.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCars_CellContentClick);
            // 
            // txtLicensePlate
            // 
            this.txtLicensePlate.Location = new System.Drawing.Point(259, 84);
            this.txtLicensePlate.Multiline = true;
            this.txtLicensePlate.Name = "txtLicensePlate";
            this.txtLicensePlate.Size = new System.Drawing.Size(121, 50);
            this.txtLicensePlate.TabIndex = 1;
            // 
            // txtYearOfRepair
            // 
            this.txtYearOfRepair.Location = new System.Drawing.Point(259, 545);
            this.txtYearOfRepair.Multiline = true;
            this.txtYearOfRepair.Name = "txtYearOfRepair";
            this.txtYearOfRepair.Size = new System.Drawing.Size(121, 50);
            this.txtYearOfRepair.TabIndex = 2;
            // 
            // txtYearOfManufacture
            // 
            this.txtYearOfManufacture.Location = new System.Drawing.Point(259, 438);
            this.txtYearOfManufacture.Multiline = true;
            this.txtYearOfManufacture.Name = "txtYearOfManufacture";
            this.txtYearOfManufacture.Size = new System.Drawing.Size(121, 50);
            this.txtYearOfManufacture.TabIndex = 3;
            // 
            // txtPurpose
            // 
            this.txtPurpose.Location = new System.Drawing.Point(259, 342);
            this.txtPurpose.Multiline = true;
            this.txtPurpose.Name = "txtPurpose";
            this.txtPurpose.Size = new System.Drawing.Size(121, 50);
            this.txtPurpose.TabIndex = 4;
            // 
            // txtLoadCapacity
            // 
            this.txtLoadCapacity.Location = new System.Drawing.Point(259, 257);
            this.txtLoadCapacity.Multiline = true;
            this.txtLoadCapacity.Name = "txtLoadCapacity";
            this.txtLoadCapacity.Size = new System.Drawing.Size(121, 50);
            this.txtLoadCapacity.TabIndex = 5;
            // 
            // txtBrand
            // 
            this.txtBrand.Location = new System.Drawing.Point(259, 174);
            this.txtBrand.Multiline = true;
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(121, 50);
            this.txtBrand.TabIndex = 6;
            // 
            // txtPhoto
            // 
            this.txtPhoto.Location = new System.Drawing.Point(411, 257);
            this.txtPhoto.Multiline = true;
            this.txtPhoto.Name = "txtPhoto";
            this.txtPhoto.Size = new System.Drawing.Size(121, 155);
            this.txtPhoto.TabIndex = 7;
            // 
            // txtMileage
            // 
            this.txtMileage.Location = new System.Drawing.Point(411, 84);
            this.txtMileage.Multiline = true;
            this.txtMileage.Name = "txtMileage";
            this.txtMileage.Size = new System.Drawing.Size(121, 50);
            this.txtMileage.TabIndex = 8;
            // 
            // btnAddCar
            // 
            this.btnAddCar.Location = new System.Drawing.Point(53, 82);
            this.btnAddCar.Name = "btnAddCar";
            this.btnAddCar.Size = new System.Drawing.Size(170, 79);
            this.btnAddCar.TabIndex = 10;
            this.btnAddCar.Text = "Добавить автомобиль";
            this.btnAddCar.UseVisualStyleBackColor = true;
            this.btnAddCar.Click += new System.EventHandler(this.btnAddCar_Click);
            // 
            // btnRemoveCar
            // 
            this.btnRemoveCar.Location = new System.Drawing.Point(53, 257);
            this.btnRemoveCar.Name = "btnRemoveCar";
            this.btnRemoveCar.Size = new System.Drawing.Size(170, 86);
            this.btnRemoveCar.TabIndex = 11;
            this.btnRemoveCar.Text = "Удалить автомобиль";
            this.btnRemoveCar.UseVisualStyleBackColor = true;
            this.btnRemoveCar.Click += new System.EventHandler(this.btnRemoveCar_Click);
            // 
            // btnSelectPhoto
            // 
            this.btnSelectPhoto.Location = new System.Drawing.Point(411, 159);
            this.btnSelectPhoto.Name = "btnSelectPhoto";
            this.btnSelectPhoto.Size = new System.Drawing.Size(121, 65);
            this.btnSelectPhoto.TabIndex = 13;
            this.btnSelectPhoto.Text = "Добавить фото:";
            this.btnSelectPhoto.UseVisualStyleBackColor = true;
            this.btnSelectPhoto.Click += new System.EventHandler(this.btnSelectPhoto_Click_1);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(53, 414);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(170, 86);
            this.btnClear.TabIndex = 14;
            this.btnClear.Text = "Очистить поля";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(256, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Номер автомобиля:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(256, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Введите марку:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(256, 241);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Введите грузоподьменость:\r\n";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(256, 326);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Введите назначение:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(408, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Введите пробег:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(256, 529);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Год ремонта:";
            // 
            // LicensePlate
            // 
            this.LicensePlate.HeaderText = "Номер автомобиля";
            this.LicensePlate.Name = "LicensePlate";
            // 
            // Brand
            // 
            this.Brand.HeaderText = "Марка";
            this.Brand.Name = "Brand";
            // 
            // LoadCapacity
            // 
            this.LoadCapacity.HeaderText = "Грузоподъемность";
            this.LoadCapacity.Name = "LoadCapacity";
            // 
            // Purpose
            // 
            this.Purpose.HeaderText = "Назначение";
            this.Purpose.Name = "Purpose";
            // 
            // YearOfManufacture
            // 
            this.YearOfManufacture.HeaderText = "Год выпуска";
            this.YearOfManufacture.Name = "YearOfManufacture";
            // 
            // YearOfRepair
            // 
            this.YearOfRepair.HeaderText = "Год ремонта";
            this.YearOfRepair.Name = "YearOfRepair";
            // 
            // Mileage
            // 
            this.Mileage.HeaderText = "Пробег";
            this.Mileage.Name = "Mileage";
            // 
            // Photo
            // 
            this.Photo.HeaderText = "Фото";
            this.Photo.Name = "Photo";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(256, 422);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Год выпуска:";
            // 
            // CarManagerForm
            // 
            this.ClientSize = new System.Drawing.Size(1387, 651);
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
