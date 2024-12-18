using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FreightTransportSystem
{
    // Форма для управления водителями
    public partial class DriverManagerForm : Form
    {
        // Элементы управления для ввода данных о водителе
        private TextBox txtFullName; // Полное имя водителя
        private TextBox txtExperience; // Стаж водителя
        private TextBox txtEmployeeNumber; // Табельный номер водителя
        private DateTimePicker dtpYearOfBirth; // Дата рождения водителя

        // Метки для отображения информации
        private Label lblFullName; // Метка для полного имени
        private Label lblEmployeeNumber; // Метка для табельного номера
        private Label lblExperience; // Метка для стажа
        private Label lblClassType; // Метка для класса
        private Label lblYearOfBirth; // Метка для года рождения

        // Кнопки для управления водителями
        private Button btnAddDriver; // Кнопка для добавления водителя
        private Button btnRemoveDriver; // Кнопка для удаления водителя
        private Button btnClearFields; // Кнопка для очистки полей ввода

        // Столбцы для DataGridView с информацией о водителях
        private DataGridViewTextBoxColumn colFullName; // Столбец для полного имени
        private DataGridViewTextBoxColumn colEmployeeNumber; // Столбец для табельного номера
        private DataGridViewTextBoxColumn colYearOfBirth; // Столбец для года рождения
        private DataGridViewTextBoxColumn colExperience; // Столбец для стажа
        private DataGridViewTextBoxColumn colCategory; // Столбец для категории
        private DataGridViewTextBoxColumn colClassType; // Столбец для класса
        private CheckedListBox clbCategories;
        private Label lblCategory;
        private ComboBox cmbClassTypes;
        private Button btnEditDriver;
        private Label label1;
        private Label label2;

        // DataGridView для отображения списка водителей
        private DataGridView dataGridViewDrivers;

        // Конструктор формы
        public DriverManagerForm()
        {
            InitializeComponent(); // Инициализация компонентов формы
            LoadDrivers(); // Загрузка списка водителей при инициализации формы
        }

        // Метод для загрузки водителей в DataGridView
        private void LoadDrivers()
        {
            dataGridViewDrivers.Rows.Clear(); // Очистка текущих строк в DataGridView
            foreach (var driver in DriverManager.GetDrivers()) // Перебор всех водителей
            {
                // Добавление информации о водителе в DataGridView
                dataGridViewDrivers.Rows.Add(driver.FullName, driver.EmployeeNumber, driver.YearOfBirth, driver.Experience, driver.Category, driver.ClassType);
            }
        }

        // Метод для очистки полей ввода
        private void ClearFields()
        {
            txtFullName.Clear(); // Очистка поля полного имени
            txtEmployeeNumber.Clear(); // Очистка поля табельного номера
            dtpYearOfBirth.Value = DateTime.Now; // Установка текущей даты в поле даты рождения
            txtExperience.Clear(); // Очистка поля стажа         
            clbCategories.ClearSelected(); // Очистка выбранных категорий

            // Очистка выбранных категорий
            for (int i = 0; i < clbCategories.Items.Count; i++)
            {
                clbCategories.SetItemChecked(i, false); // Снимаем отметку со всех элементов
            }

            cmbClassTypes.SelectedIndex = -1; // Сброс выбора класса
        }

        // Обработчик события нажатия кнопки "Добавить водителя"
        private void btnAddDriver_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Получение данных из текстовых полей с удалением пробелов
                string fullName = txtFullName.Text.Trim();
                string employeeNumber = txtEmployeeNumber.Text.Trim();
                string experience = txtExperience.Text.Trim();
                string category = string.Join(", ", clbCategories.CheckedItems.Cast<string>()); // Получение выбранных категорий
                string classType = cmbClassTypes.SelectedItem?.ToString(); // Получение выбранного класса

                List<string> errors = new List<string>(); // Список для хранения ошибок валидации

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

                // Проверка выбранной категории
                if (string.IsNullOrWhiteSpace(category))
                {
                    errors.Add("Ошибка: необходимо выбрать хотя бы одну категорию.");
                }

                // Проверка выбранного класса
                if (string.IsNullOrWhiteSpace(classType))
                {
                    errors.Add("Ошибка: необходимо выбрать класс.");
                }

                // Если есть ошибки, выводим их в сообщении
                if (errors.Count > 0)
                {
                    string errorMessage = string.Join(Environment.NewLine, errors);
                    MessageBox.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Прерываем выполнение метода, если есть ошибки
                }

                // Преобразование стажа в целое число и создание нового водителя
                int experienceInt = Convert.ToInt32(experience);
                Driver newDriver = new Driver(fullName, employeeNumber, dtpYearOfBirth.Value.Year, experienceInt, category, classType);
                DriverManager.GetDrivers().Add(newDriver); // Добавление нового водителя в список
                LoadDrivers(); // Обновление списка водителей в DataGridView
                ClearFields(); // Очистка полей ввода
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

        // Обработчик события нажатия кнопки "Удалить водителя"
        private void btnRemoveDriver_Click_1(object sender, EventArgs e)
        {
            if (dataGridViewDrivers.SelectedRows.Count > 0) // Проверка, выбрана ли строка
            {
                var selectedRow = dataGridViewDrivers.SelectedRows[0]; // Получение выбранной строки
                if (selectedRow.Cells["colEmployeeNumber"].Value != null) // Проверка наличия табельного номера
                {
                    string employeeNumber = selectedRow.Cells["colEmployeeNumber"].Value.ToString(); // Получение табельного номера
                    Driver driverToRemove = DriverManager.FindDriver(employeeNumber); // Поиск водителя по табельному номеру
                    if (driverToRemove != null) // Если водитель найден
                    {
                        DriverManager.GetDrivers().Remove(driverToRemove); // Удаление водителя из списка
                        LoadDrivers(); // Обновление списка водителей в DataGridView
                        ClearFields(); // Очистка полей ввода
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

        // Обработчик события нажатия кнопки "Очистить поля"
        private void btnClearFields_Click_1(object sender, EventArgs e)
        {
            ClearFields(); // Очистка всех полей ввода
        }

        // Обработчик события нажатия на ячейку в DataGridView
        private void dataGridViewDrivers_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewDrivers.SelectedRows.Count > 0) // Проверка, выбрана ли строка
            {
                var selectedRow = dataGridViewDrivers.SelectedRows[0]; // Получение выбранной строки

                // Заполнение полей ввода данными из выбранной строки
                if (selectedRow.Cells["colFullName"].Value != null)
                    txtFullName.Text = selectedRow.Cells["colFullName"].Value.ToString();
                else
                    txtFullName.Clear();

                if (selectedRow.Cells["colEmployeeNumber"].Value != null)
                    txtEmployeeNumber.Text = selectedRow.Cells["colEmployeeNumber"].Value.ToString();
                else
                    txtEmployeeNumber.Clear();

                if (selectedRow.Cells["colExperience"].Value != null)
                    txtExperience.Text = selectedRow.Cells["colExperience"].Value.ToString();
                else
                    txtExperience.Clear();

                if (selectedRow.Cells["colCategory"].Value != null)
                {
                    string categories = selectedRow.Cells["colCategory"].Value.ToString();
                    for (int i = 0; i < clbCategories.Items.Count; i++)
                    {
                        clbCategories.SetItemChecked(i, categories.Contains(clbCategories.Items[i].ToString()));
                    }
                }
                else
                    clbCategories.ClearSelected();

                if (selectedRow.Cells["colClassType"].Value != null)
                {
                    string classType = selectedRow.Cells["colClassType"].Value.ToString();
                    cmbClassTypes.SelectedItem = classType; // Установка выбранного класса
                }
                else
                {
                    cmbClassTypes.SelectedIndex = -1; // Сброс выбора
                }

                if (selectedRow.Cells["colYearOfBirth"].Value != null)
                    dtpYearOfBirth.Value = new DateTime(Convert.ToInt32(selectedRow.Cells["colYearOfBirth"].Value), 1, 1);
                else
                    dtpYearOfBirth.Value = DateTime.Now; // Установка текущей даты, если год рождения не указан
            }
        }

        private void InitializeComponent()
        {
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.txtExperience = new System.Windows.Forms.TextBox();
            this.txtEmployeeNumber = new System.Windows.Forms.TextBox();
            this.dtpYearOfBirth = new System.Windows.Forms.DateTimePicker();
            this.dataGridViewDrivers = new System.Windows.Forms.DataGridView();
            this.colFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmployeeNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYearOfBirth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExperience = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClassType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblFullName = new System.Windows.Forms.Label();
            this.lblEmployeeNumber = new System.Windows.Forms.Label();
            this.lblExperience = new System.Windows.Forms.Label();
            this.lblClassType = new System.Windows.Forms.Label();
            this.lblYearOfBirth = new System.Windows.Forms.Label();
            this.btnAddDriver = new System.Windows.Forms.Button();
            this.btnRemoveDriver = new System.Windows.Forms.Button();
            this.btnClearFields = new System.Windows.Forms.Button();
            this.clbCategories = new System.Windows.Forms.CheckedListBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cmbClassTypes = new System.Windows.Forms.ComboBox();
            this.btnEditDriver = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDrivers)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(25, 88);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(158, 20);
            this.txtFullName.TabIndex = 0;
            // 
            // txtExperience
            // 
            this.txtExperience.Location = new System.Drawing.Point(25, 286);
            this.txtExperience.Name = "txtExperience";
            this.txtExperience.Size = new System.Drawing.Size(158, 20);
            this.txtExperience.TabIndex = 4;
            // 
            // txtEmployeeNumber
            // 
            this.txtEmployeeNumber.Location = new System.Drawing.Point(25, 151);
            this.txtEmployeeNumber.Name = "txtEmployeeNumber";
            this.txtEmployeeNumber.Size = new System.Drawing.Size(158, 20);
            this.txtEmployeeNumber.TabIndex = 5;
            // 
            // dtpYearOfBirth
            // 
            this.dtpYearOfBirth.Location = new System.Drawing.Point(25, 225);
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
            this.dataGridViewDrivers.Location = new System.Drawing.Point(619, 88);
            this.dataGridViewDrivers.Name = "dataGridViewDrivers";
            this.dataGridViewDrivers.Size = new System.Drawing.Size(644, 466);
            this.dataGridViewDrivers.TabIndex = 7;
            this.dataGridViewDrivers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDrivers_CellContentClick_1);
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
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Location = new System.Drawing.Point(25, 69);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(82, 13);
            this.lblFullName.TabIndex = 8;
            this.lblFullName.Text = "Введите ФИО:";
            // 
            // lblEmployeeNumber
            // 
            this.lblEmployeeNumber.AutoSize = true;
            this.lblEmployeeNumber.Location = new System.Drawing.Point(25, 135);
            this.lblEmployeeNumber.Name = "lblEmployeeNumber";
            this.lblEmployeeNumber.Size = new System.Drawing.Size(145, 13);
            this.lblEmployeeNumber.TabIndex = 10;
            this.lblEmployeeNumber.Text = "Введите табельный номер:";
            // 
            // lblExperience
            // 
            this.lblExperience.AutoSize = true;
            this.lblExperience.Location = new System.Drawing.Point(25, 270);
            this.lblExperience.Name = "lblExperience";
            this.lblExperience.Size = new System.Drawing.Size(80, 13);
            this.lblExperience.TabIndex = 11;
            this.lblExperience.Text = "Введите стаж:";
            // 
            // lblClassType
            // 
            this.lblClassType.AutoSize = true;
            this.lblClassType.Location = new System.Drawing.Point(28, 466);
            this.lblClassType.Name = "lblClassType";
            this.lblClassType.Size = new System.Drawing.Size(85, 13);
            this.lblClassType.TabIndex = 13;
            this.lblClassType.Text = "Введите класс:";
            // 
            // lblYearOfBirth
            // 
            this.lblYearOfBirth.AutoSize = true;
            this.lblYearOfBirth.Location = new System.Drawing.Point(25, 209);
            this.lblYearOfBirth.Name = "lblYearOfBirth";
            this.lblYearOfBirth.Size = new System.Drawing.Size(133, 13);
            this.lblYearOfBirth.TabIndex = 14;
            this.lblYearOfBirth.Text = "Выберите год рождения:";
            // 
            // btnAddDriver
            // 
            this.btnAddDriver.Location = new System.Drawing.Point(28, 12);
            this.btnAddDriver.Name = "btnAddDriver";
            this.btnAddDriver.Size = new System.Drawing.Size(186, 27);
            this.btnAddDriver.TabIndex = 15;
            this.btnAddDriver.Text = "Добавить водителя ";
            this.btnAddDriver.UseVisualStyleBackColor = true;
            this.btnAddDriver.Click += new System.EventHandler(this.btnAddDriver_Click_1);
            // 
            // btnRemoveDriver
            // 
            this.btnRemoveDriver.Location = new System.Drawing.Point(220, 12);
            this.btnRemoveDriver.Name = "btnRemoveDriver";
            this.btnRemoveDriver.Size = new System.Drawing.Size(186, 27);
            this.btnRemoveDriver.TabIndex = 16;
            this.btnRemoveDriver.Text = "Удалить водителя ";
            this.btnRemoveDriver.UseVisualStyleBackColor = true;
            this.btnRemoveDriver.Click += new System.EventHandler(this.btnRemoveDriver_Click_1);
            // 
            // btnClearFields
            // 
            this.btnClearFields.Location = new System.Drawing.Point(412, 12);
            this.btnClearFields.Name = "btnClearFields";
            this.btnClearFields.Size = new System.Drawing.Size(186, 27);
            this.btnClearFields.TabIndex = 17;
            this.btnClearFields.Text = "Очитсить поля";
            this.btnClearFields.UseVisualStyleBackColor = true;
            this.btnClearFields.Click += new System.EventHandler(this.btnClearFields_Click_1);
            // 
            // clbCategories
            // 
            this.clbCategories.FormattingEnabled = true;
            this.clbCategories.Items.AddRange(new object[] {
            "А",
            "А1",
            "B",
            "BE",
            "B1",
            "C",
            "CE",
            "C1",
            "C1E",
            "D",
            "DE",
            "D1",
            "D1E",
            "M",
            "Tm",
            "Tb"});
            this.clbCategories.Location = new System.Drawing.Point(25, 347);
            this.clbCategories.Name = "clbCategories";
            this.clbCategories.Size = new System.Drawing.Size(158, 94);
            this.clbCategories.TabIndex = 18;
            this.clbCategories.SelectedIndexChanged += new System.EventHandler(this.clbCategories_SelectedIndexChanged);
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(25, 331);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(109, 13);
            this.lblCategory.TabIndex = 12;
            this.lblCategory.Text = "Введите категорию:";
            // 
            // cmbClassTypes
            // 
            this.cmbClassTypes.FormattingEnabled = true;
            this.cmbClassTypes.Items.AddRange(new object[] {
            "1 класс",
            "2 класс",
            "3 класс"});
            this.cmbClassTypes.Location = new System.Drawing.Point(28, 495);
            this.cmbClassTypes.Name = "cmbClassTypes";
            this.cmbClassTypes.Size = new System.Drawing.Size(121, 21);
            this.cmbClassTypes.TabIndex = 19;
            // 
            // btnEditDriver
            // 
            this.btnEditDriver.Location = new System.Drawing.Point(604, 12);
            this.btnEditDriver.Name = "btnEditDriver";
            this.btnEditDriver.Size = new System.Drawing.Size(186, 27);
            this.btnEditDriver.TabIndex = 20;
            this.btnEditDriver.Text = "Редактировать";
            this.btnEditDriver.UseVisualStyleBackColor = true;
            this.btnEditDriver.Click += new System.EventHandler(this.btnEditDriver_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(615, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 19);
            this.label1.TabIndex = 21;
            this.label1.Text = "Список водителей:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(217, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(326, 78);
            this.label2.TabIndex = 22;
            this.label2.Text = "Для редатирования выберите водителя из списка\r\nДалее нажмите на столбец ФИО\r\nПосл" +
    "е чего в полях вы можете изменить нужную информацию\r\nНажмите редактировать\r\n\r\n\r\n" +
    "";
            // 
            // DriverManagerForm
            // 
            this.ClientSize = new System.Drawing.Size(1305, 598);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnEditDriver);
            this.Controls.Add(this.cmbClassTypes);
            this.Controls.Add(this.clbCategories);
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
            this.Controls.Add(this.txtFullName);
            this.Name = "DriverManagerForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDrivers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void clbCategories_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnEditDriver_Click(object sender, EventArgs e)
        {
            if (dataGridViewDrivers.SelectedRows.Count > 0) // Проверка, выбрана ли строка
            {
                var selectedRow = dataGridViewDrivers.SelectedRows[0]; // Получение выбранной строки

                // Получение табельного номера выбранного водителя
                string employeeNumber = selectedRow.Cells["colEmployeeNumber"].Value.ToString();
                Driver driverToEdit = DriverManager.FindDriver(employeeNumber); // Поиск водителя по табельному номеру

                if (driverToEdit != null) // Если водитель найден
                {
                    // Получение данных из текстовых полей с удалением пробелов
                    string fullName = txtFullName.Text.Trim();
                    string experience = txtExperience.Text.Trim();
                    string category = string.Join(", ", clbCategories.CheckedItems.Cast<string>()); // Получение выбранных категорий
                    string classType = cmbClassTypes.SelectedItem?.ToString(); // Получение выбранного класса
                    int yearOfBirth = dtpYearOfBirth.Value.Year; // Получение года рождения

                    List<string> errors = new List<string>(); // Список для хранения ошибок валидации

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
                    string newEmployeeNumber = txtEmployeeNumber.Text.Trim();
                    if (string.IsNullOrWhiteSpace(newEmployeeNumber))
                    {
                        errors.Add("Ошибка ввода в поле Табельный номер: поле не может быть пустым.");
                    }
                    else if (!Regex.IsMatch(newEmployeeNumber, @"^\d{1,6}$"))
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

                    // Проверка выбранной категории
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        errors.Add("Ошибка: необходимо выбрать хотя бы одну категорию.");
                    }

                    // Проверка выбранного класса
                    if (string.IsNullOrWhiteSpace(classType))
                    {
                        errors.Add("Ошибка: необходимо выбрать класс.");
                    }

                    // Проверка года рождения
                    if (yearOfBirth < 1900 || yearOfBirth > DateTime.Now.Year)
                    {
                        errors.Add("Ошибка ввода в поле Год Рождения: год должен быть в диапазоне от 1900 до текущего года.");
                    }

                    // Если есть ошибки, выводим их в сообщении
                    if (errors.Count > 0)
                    {
                        string errorMessage = string.Join(Environment.NewLine, errors);
                        MessageBox.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Прерываем выполнение метода, если есть ошибки
                    }

                    // Обновление информации о водителе
                    driverToEdit.FullName = fullName;
                    driverToEdit.EmployeeNumber = newEmployeeNumber; // Обновление табельного номера

                    // В алидация стажа перед обновлением
                    if (int.TryParse(experience, out int experienceInt) && experienceInt >= 1 && experienceInt <= 99)
                    {
                        driverToEdit.Experience = experienceInt;
                    }

                    // Обновление остальных полей
                    driverToEdit.YearOfBirth = yearOfBirth;
                    driverToEdit.Category = category; // Обновление категорий
                    driverToEdit.ClassType = classType; // Обновление класса

                    LoadDrivers(); // Обновление списка водителей в DataGridView
                    ClearFields(); // Очистка полей ввода
                }
                else
                {
                    MessageBox.Show("Водитель не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите водителя для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
 