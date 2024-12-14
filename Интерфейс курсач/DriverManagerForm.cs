using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FreightTransportSystem
{
    public partial class DriverManagerForm : Form
    {
        private TextBox txtFullName;
        private TextBox txtClassType;
        private TextBox txtCategory;
        private TextBox txtExperience;
        private TextBox txtEmployeeNumber;
        private DateTimePicker dtpYearOfBirth;
        private Label lblFullName;
        private Label lblEmployeeNumber;
        private Label lblExperience;
        private Label lblCategory;
        private Label lblClassType;
        private Label lblYearOfBirth;
        private Button btnAddDriver;
        private Button btnRemoveDriver;
        private Button btnClearFields;
        private DataGridViewTextBoxColumn colFullName;
        private DataGridViewTextBoxColumn colEmployeeNumber;
        private DataGridViewTextBoxColumn colYearOfBirth;
        private DataGridViewTextBoxColumn colExperience;
        private DataGridViewTextBoxColumn colCategory;
        private DataGridViewTextBoxColumn colClassType;
        private DataGridView dataGridViewDrivers;

        public DriverManagerForm()
        {
            InitializeComponent();
            LoadDrivers();
        }

        private void LoadDrivers()
        {
            dataGridViewDrivers.Rows.Clear();
            foreach (var driver in DriverManager.GetDrivers())
            {
                dataGridViewDrivers.Rows.Add(driver.FullName, driver.EmployeeNumber, driver.YearOfBirth, driver.Experience, driver.Category, driver.ClassType);
            }
        }

        private void ClearFields()
        {
            txtFullName.Clear();
            txtEmployeeNumber.Clear();
            dtpYearOfBirth.Value = DateTime.Now;
            txtExperience.Clear();
            txtCategory.Clear();
            txtClassType.Clear();
        }

        private void btnAddDriver_Click_1(object sender, EventArgs e)
        {
            try
            {
                string fullName = txtFullName.Text;
                string employeeNumber = txtEmployeeNumber.Text;
                string experience = txtExperience.Text;
                string category = txtCategory.Text;
                string classType = txtClassType.Text;

                List<string> errors = new List<string>();

                // Проверка ФИО
                if (string.IsNullOrWhiteSpace(fullName))
                {
                    errors.Add("Ошибка ввода в поле ФИО: поле не может быть пустым.");
                }
                else if (!Regex.IsMatch(fullName, @"^[а-яА-ЯёЁa-zA-Z\s]+$"))
                {
                    errors.Add("Ошибка ввода в поле ФИО: ФИО должно содержать только буквы.");
                }

                // Проверка табельного номера
                if (string.IsNullOrWhiteSpace(employeeNumber))
                {
                    errors.Add("Ошибка ввода в поле Табельный номер: поле не может быть пустым.");
                }
                else if (!Regex.IsMatch(employeeNumber, @"^\d{1,6}$"))
                {
                    errors.Add("Ошибка ввода в поле Табельный номер: Табельный номер должен содержать только цифры и не превышать 6 символов.");
                }

                // Проверка стажа
                if (string.IsNullOrWhiteSpace(experience))
                {
                    errors.Add("Ошибка ввода в поле Стаж: поле не может быть пустым.");
                }
                else if (!Regex.IsMatch(experience, @"^\d{1,2}$") || int.Parse(experience) > 99)
                {
                    errors.Add("Ошибка ввода в поле Стаж: Стаж должен быть целым числом от 1 до 99.");
                }

                // Проверка категории
                if (string.IsNullOrWhiteSpace(category))
                {
                    errors.Add("Ошибка ввода в поле Категория: поле не может быть пустым.");
                }
                else if (!Regex.IsMatch(category, @"^[a-zA-Z0-9]{1,5}$"))
                {
                    errors.Add("Ошибка ввода в поле Категория: Категория должна содержать только буквы и цифры и не превышать 5 символов.");
                }

                // Проверка класса
                if (string.IsNullOrWhiteSpace(classType))
                {
                    errors.Add("Ошибка ввода в поле Класс: поле не может быть пустым.");
                }
                else if (!Regex.IsMatch(classType, @"^\d{1}$"))
                {
                    errors.Add("Ошибка ввода в поле Класс: Класс должен быть одной цифрой.");
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

                int experienceInt = Convert.ToInt32(experience);
                Driver newDriver = new Driver(fullName, employeeNumber, dtpYearOfBirth.Value.Year, experienceInt, category, classType);
                DriverManager.GetDrivers().Add(newDriver);
                LoadDrivers();
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

        private void btnRemoveDriver_Click_1(object sender, EventArgs e)
        {
            if (dataGridViewDrivers.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewDrivers.SelectedRows[0];
                if (selectedRow.Cells["colEmployeeNumber"].Value != null)
                {
                    string employeeNumber = selectedRow.Cells["colEmployeeNumber"].Value.ToString();
                    Driver driverToRemove = DriverManager.FindDriver(employeeNumber);
                    if (driverToRemove != null)
                    {
                        DriverManager.GetDrivers().Remove(driverToRemove);
                        LoadDrivers();
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("Водитель не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Выбранная строка не содержит табельного номера.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите водителя для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClearFields_Click_1(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void dataGridViewDrivers_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewDrivers.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewDrivers.SelectedRows[0];

                if (selectedRow.Cells["colFullName"].Value != null)
                    txtFullName.Text = selectedRow.Cells["colFullName"].Value.ToString();
                else
                    txtFullName.Text = string.Empty;

                if (selectedRow.Cells["colEmployeeNumber"].Value != null)
                    txtEmployeeNumber.Text = selectedRow.Cells["colEmployeeNumber"].Value.ToString();
                else
                    txtEmployeeNumber.Text = string.Empty;

                if (selectedRow.Cells["colExperience"].Value != null)
                    txtExperience.Text = selectedRow.Cells["colExperience"].Value.ToString();
                else
                    txtExperience.Text = string.Empty;

                if (selectedRow.Cells["colCategory"].Value != null)
                    txtCategory.Text = selectedRow.Cells["colCategory"].Value.ToString();
                else
                    txtCategory.Text = string.Empty;

                if (selectedRow.Cells["colClassType"].Value != null)
                    txtClassType.Text = selectedRow.Cells["colClassType"].Value.ToString();
                else
                    txtClassType.Text = string.Empty;

                if (selectedRow.Cells["colYearOfBirth"].Value != null)
                    dtpYearOfBirth.Value = new DateTime(Convert.ToInt32(selectedRow.Cells["colYearOfBirth"].Value), 1, 1);
                else
                    dtpYearOfBirth.Value = DateTime.Now;
            }
        }

        private void InitializeComponent()
        {
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.txtClassType = new System.Windows.Forms.TextBox();
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.txtExperience = new System.Windows.Forms.TextBox();
            this.txtEmployeeNumber = new System.Windows.Forms.TextBox();
            this.dtpYearOfBirth = new System.Windows.Forms.DateTimePicker();
            this.dataGridViewDrivers = new System.Windows.Forms.DataGridView();
            this.lblFullName = new System.Windows.Forms.Label();
            this.lblEmployeeNumber = new System.Windows.Forms.Label();
            this.lblExperience = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblClassType = new System.Windows.Forms.Label();
            this.lblYearOfBirth = new System.Windows.Forms.Label();
            this.btnAddDriver = new System.Windows.Forms.Button();
            this.btnRemoveDriver = new System.Windows.Forms.Button();
            this.btnClearFields = new System.Windows.Forms.Button();
            this.colFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmployeeNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYearOfBirth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExperience = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClassType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDrivers)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(293, 66);
            this.txtFullName.Multiline = true;
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(158, 33);
            this.txtFullName.TabIndex = 0;
            // 
            // txtClassType
            // 
            this.txtClassType.Location = new System.Drawing.Point(293, 425);
            this.txtClassType.Multiline = true;
            this.txtClassType.Name = "txtClassType";
            this.txtClassType.Size = new System.Drawing.Size(158, 33);
            this.txtClassType.TabIndex = 2;
            // 
            // txtCategory
            // 
            this.txtCategory.Location = new System.Drawing.Point(293, 340);
            this.txtCategory.Multiline = true;
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Size = new System.Drawing.Size(158, 33);
            this.txtCategory.TabIndex = 3;
            // 
            // txtExperience
            // 
            this.txtExperience.Location = new System.Drawing.Point(293, 264);
            this.txtExperience.Multiline = true;
            this.txtExperience.Name = "txtExperience";
            this.txtExperience.Size = new System.Drawing.Size(158, 33);
            this.txtExperience.TabIndex = 4;
            // 
            // txtEmployeeNumber
            // 
            this.txtEmployeeNumber.Location = new System.Drawing.Point(293, 129);
            this.txtEmployeeNumber.Multiline = true;
            this.txtEmployeeNumber.Name = "txtEmployeeNumber";
            this.txtEmployeeNumber.Size = new System.Drawing.Size(158, 33);
            this.txtEmployeeNumber.TabIndex = 5;
            // 
            // dtpYearOfBirth
            // 
            this.dtpYearOfBirth.Location = new System.Drawing.Point(293, 203);
            this.dtpYearOfBirth.Name = "dtpYearOfBirth";
            this.dtpYearOfBirth.Size = new System.Drawing.Size(200, 20);
            this.dtpYearOfBirth.TabIndex = 6;
            // 
            // dataGridViewDrivers
            // 
            this.dataGridViewDrivers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDrivers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFullName,
            this.colEmployeeNumber,
            this.colYearOfBirth,
            this.colExperience,
            this.colCategory,
            this.colClassType});
            this.dataGridViewDrivers.Location = new System.Drawing.Point(564, 47);
            this.dataGridViewDrivers.Name = "dataGridViewDrivers";
            this.dataGridViewDrivers.Size = new System.Drawing.Size(642, 466);
            this.dataGridViewDrivers.TabIndex = 7;
            this.dataGridViewDrivers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDrivers_CellContentClick_1);
            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Location = new System.Drawing.Point(293, 47);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(82, 13);
            this.lblFullName.TabIndex = 8;
            this.lblFullName.Text = "Введите ФИО:";
            // 
            // lblEmployeeNumber
            // 
            this.lblEmployeeNumber.AutoSize = true;
            this.lblEmployeeNumber.Location = new System.Drawing.Point(293, 113);
            this.lblEmployeeNumber.Name = "lblEmployeeNumber";
            this.lblEmployeeNumber.Size = new System.Drawing.Size(145, 13);
            this.lblEmployeeNumber.TabIndex = 10;
            this.lblEmployeeNumber.Text = "Введите табельный номер:";
            // 
            // lblExperience
            // 
            this.lblExperience.AutoSize = true;
            this.lblExperience.Location = new System.Drawing.Point(293, 248);
            this.lblExperience.Name = "lblExperience";
            this.lblExperience.Size = new System.Drawing.Size(80, 13);
            this.lblExperience.TabIndex = 11;
            this.lblExperience.Text = "Введите стаж:";
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(293, 324);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(109, 13);
            this.lblCategory.TabIndex = 12;
            this.lblCategory.Text = "Введите категорию:";
            // 
            // lblClassType
            // 
            this.lblClassType.AutoSize = true;
            this.lblClassType.Location = new System.Drawing.Point(293, 409);
            this.lblClassType.Name = "lblClassType";
            this.lblClassType.Size = new System.Drawing.Size(85, 13);
            this.lblClassType.TabIndex = 13;
            this.lblClassType.Text = "Введите класс:";
            // 
            // lblYearOfBirth
            // 
            this.lblYearOfBirth.AutoSize = true;
            this.lblYearOfBirth.Location = new System.Drawing.Point(293, 187);
            this.lblYearOfBirth.Name = "lblYearOfBirth";
            this.lblYearOfBirth.Size = new System.Drawing.Size(133, 13);
            this.lblYearOfBirth.TabIndex = 14;
            this.lblYearOfBirth.Text = "Выберите год рождения:";
            // 
            // btnAddDriver
            // 
            this.btnAddDriver.Location = new System.Drawing.Point(53, 66);
            this.btnAddDriver.Name = "btnAddDriver";
            this.btnAddDriver.Size = new System.Drawing.Size(186, 74);
            this.btnAddDriver.TabIndex = 15;
            this.btnAddDriver.Text = "Добавить водителя ";
            this.btnAddDriver.UseVisualStyleBackColor = true;
            this.btnAddDriver.Click += new System.EventHandler(this.btnAddDriver_Click_1);
            // 
            // btnRemoveDriver
            // 
            this.btnRemoveDriver.Location = new System.Drawing.Point(53, 223);
            this.btnRemoveDriver.Name = "btnRemoveDriver";
            this.btnRemoveDriver.Size = new System.Drawing.Size(186, 71);
            this.btnRemoveDriver.TabIndex = 16;
            this.btnRemoveDriver.Text = "Удалить водителя ";
            this.btnRemoveDriver.UseVisualStyleBackColor = true;
            this.btnRemoveDriver.Click += new System.EventHandler(this.btnRemoveDriver_Click_1);
            // 
            // btnClearFields
            // 
            this.btnClearFields.Location = new System.Drawing.Point(53, 380);
            this.btnClearFields.Name = "btnClearFields";
            this.btnClearFields.Size = new System.Drawing.Size(186, 78);
            this.btnClearFields.TabIndex = 17;
            this.btnClearFields.Text = "Очитсить поля";
            this.btnClearFields.UseVisualStyleBackColor = true;
            this.btnClearFields.Click += new System.EventHandler(this.btnClearFields_Click_1);
            // 
            // colFullName
            // 
            this.colFullName.HeaderText = "ФИО";
            this.colFullName.Name = "colFullName";
            // 
            // colEmployeeNumber
            // 
            this.colEmployeeNumber.HeaderText = "Табельный номер ";
            this.colEmployeeNumber.Name = "colEmployeeNumber";
            // 
            // colYearOfBirth
            // 
            this.colYearOfBirth.HeaderText = "Год Рождения";
            this.colYearOfBirth.Name = "colYearOfBirth";
            // 
            // colExperience
            // 
            this.colExperience.HeaderText = "Стаж";
            this.colExperience.Name = "colExperience";
            // 
            // colCategory
            // 
            this.colCategory.HeaderText = "Категория";
            this.colCategory.Name = "colCategory";
            // 
            // colClassType
            // 
            this.colClassType.HeaderText = "Класс";
            this.colClassType.Name = "colClassType";
            // 
            // DriverManagerForm
            // 
            this.ClientSize = new System.Drawing.Size(1305, 598);
            this.Controls.Add(this.btnClearFields);
            this.Controls.Add(this.btnRemoveDriver);
            this.Controls.Add(this.btnAddDriver);
            this.Controls.Add(this.lblYearOfBirth);
            this.Controls.Add(this.lblClassType);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.lblExperience);
            this.Controls.Add(this.lblEmployeeNumber);
            this.Controls.Add(this.lblFullName);
            this.Controls.Add(this.dataGridViewDrivers);
            this.Controls.Add(this.dtpYearOfBirth);
            this.Controls.Add(this.txtEmployeeNumber);
            this.Controls.Add(this.txtExperience);
            this.Controls.Add(this.txtCategory);
            this.Controls.Add(this.txtClassType);
            this.Controls.Add(this.txtFullName);
            this.Name = "DriverManagerForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDrivers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        
    }
}
 