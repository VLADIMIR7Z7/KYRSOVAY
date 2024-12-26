using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Windows.Forms;

namespace FreightTransportSystem
{
    public partial class ClientManagerForm : Form
    {
        private DataGridView dgvIndividualClients;
        private TextBox txtContactName;
        private TextBox txtIssuedBy;
        private TextBox txtPassportNumber;
        private TextBox txtPassportSeries;
        private TextBox txtPhone;
        private TextBox txtDirectorName;
        private TextBox txtLegalAddress;
        private TextBox txtBankName;
        private TextBox txtAccountNumber;
        private TextBox txtINN;
        private TextBox txtCompanyNameNEW;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private DateTimePicker txtIssueDate;
        private Label label9;
        private Label label16;
        private Button btnAddIndividualClient;
        private Button btnAddLegalEntityClient;
        private Button btnRemoveClient;
        private Button btnClear;
        private Label label17;
        private TextBox txtLegalEntityPhone;
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
        private Button btnEditIndividualClient;
        private Button btnEditLegalEntityClient;
        private Button btnSaveIndividualClientChanges;
        private Button btnSaveLegalEntityClientChanges;
        private DataGridView dgvLegalEntityClients;

        public ClientManagerForm()
        {
            InitializeComponent();
            LoadClients();
        }

        private void LoadClients()
        {
            dgvIndividualClients.Rows.Clear();
            dgvLegalEntityClients.Rows.Clear();

            foreach (var client in ClientManager.GetClients())
            {
                if (client is IndividualClient individualClient)
                {
                    dgvIndividualClients.Rows.Add(individualClient.ContactName, individualClient.Phone, individualClient.PassportSeries, individualClient.PassportNumber, individualClient.IssueDate.ToShortDateString(), individualClient.IssuedBy);
                }
                else if (client is LegalEntityClient legalEntityClient)
                {
                    dgvLegalEntityClients.Rows.Add(legalEntityClient.CompanyName, legalEntityClient.DirectorName, legalEntityClient.LegalAddress, legalEntityClient.Phone, legalEntityClient.BankName, legalEntityClient.AccountNumber, legalEntityClient.INN);
                }
            }
        }


        private void ClearIndividualClientFields()
        {
            txtContactName.Clear();
            txtPhone.Clear();
            txtPassportSeries.Clear();
            txtPassportNumber.Clear();
            txtIssuedBy.Clear();
            txtIssueDate.Value = DateTime.Now; 
        }

        private void ClearLegalEntityClientFields()
        {
            txtCompanyNameNEW.Clear();
            txtDirectorName.Clear();
            txtLegalAddress.Clear();
            txtLegalEntityPhone.Clear(); 
            txtBankName.Clear();
            txtAccountNumber.Clear();
            txtINN.Clear();
        }

        // Метод для проверки, состоит ли строка только из цифр
        private bool IsOnlyDigits(string input)
        {
            return input.All(char.IsDigit);
        }

        // Метод для проверки, состоит ли строка только из букв
        private bool IsOnlyLetters(string input)
        {
            return input.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)); // Разрешаем пробелы
        }

        // Метод для проверки корректности телефона
        private bool IsValidPhone(string phone)
        {
            return phone.Length == 11 && IsOnlyDigits(phone);
        }

        // Метод для проверки корректности ИНН
        private bool IsValidINN(string inn)
        {
            return inn.Length == 10 && IsOnlyDigits(inn);
        }

        // Метод для проверки корректности расчетного счета
        private bool IsValidAccountNumber(string accountNumber)
        {
            return accountNumber.Length == 20 && IsOnlyDigits(accountNumber);
        }

        // Метод для проверки корректности номера паспорта
        private bool IsValidPassportNumber(string passportNumber)
        {
            return passportNumber.Length == 10 && IsOnlyDigits(passportNumber);
        }

        // Метод для проверки корректности серии паспорта
        private bool IsValidPassportSeries(string passportSeries)
        {
            return passportSeries.Length == 4 && IsOnlyDigits(passportSeries);
        }
        // Метод для проверки корректности ФИО
        private bool IsValidContactName(string contactName)
        {
            return !string.IsNullOrWhiteSpace(contactName); // Проверка на пустоту
        }

        private void btnAddIndividualClient_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Считывание данных из полей ввода
                string contactName = txtContactName.Text;
                string phone = txtPhone.Text;
                string passportSeries = txtPassportSeries.Text;
                string passportNumber = txtPassportNumber.Text;
                string issuedBy = txtIssuedBy.Text;
                DateTime issueDate = txtIssueDate.Value;

                // Проверка заполненности полей
                if (string.IsNullOrWhiteSpace(contactName) || string.IsNullOrWhiteSpace(phone) ||
                    string.IsNullOrWhiteSpace(passportSeries) || string.IsNullOrWhiteSpace(passportNumber) ||
                    string.IsNullOrWhiteSpace(issuedBy))
                {
                    throw new Exception("Пожалуйста, заполните все поля для физического лица.");
                }

                // Проверка корректности ввода
                if (!IsValidContactName(contactName))
                {
                    throw new Exception("ФИО должно содержать только буквы и пробелы.");
                }

                if (!IsValidPhone(phone))
                {
                    throw new Exception("Телефон должен содержать 11 цифр.");
                }

                if (!IsValidPassportSeries(passportSeries)) // Изменено на проверку серии паспорта
                {
                    throw new Exception("Серия паспорта должна содержать 4 цифры.");
                }

                if (!IsValidPassportNumber(passportNumber))
                {
                    throw new Exception("Номер паспорта должен содержать 10 цифр.");
                }

                // Создание нового клиента
                IndividualClient newClient = new IndividualClient(contactName, phone, passportSeries, passportNumber, issueDate, issuedBy);
                ClientManager.GetClients().Add(newClient); // Добавление клиента в менеджер
                LoadClients(); // Обновление списка клиентов
                ClearIndividualClientFields(); // Очистка полей ввода
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

        private void btnAddLegalEntityClient_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Считывание данных из полей ввода
                string companyName = txtCompanyNameNEW.Text.Trim();
                string directorName = txtDirectorName.Text.Trim();
                string legalAddress = txtLegalAddress.Text.Trim();
                string phone = txtLegalEntityPhone.Text.Trim();
                string bankName = txtBankName.Text.Trim();
                string accountNumber = txtAccountNumber.Text.Trim();
                string inn = txtINN.Text.Trim();

                // Проверка заполненности полей
                if (string.IsNullOrWhiteSpace(companyName))
                {
                    throw new Exception("Пожалуйста, заполните поле 'Название компании'.");
                }
                if (string.IsNullOrWhiteSpace(directorName))
                {
                    throw new Exception("Пожалуйста, заполните поле 'ФИО директора'.");
                }
                if (string.IsNullOrWhiteSpace(legalAddress))
                {
                    throw new Exception("Пожалуйста, заполните поле 'Юридический адрес'.");
                }
                if (string.IsNullOrWhiteSpace(phone))
                {
                    throw new Exception("Пожалуйста, заполните поле 'Телефон'.");
                }
                if (string.IsNullOrWhiteSpace(bankName))
                {
                    throw new Exception("Пожалуйста, заполните поле 'Название банка'.");
                }
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    throw new Exception("Пожалуйста, заполните поле 'Расчетный счет'.");
                }

                if (!IsValidINN(inn))
                {
                    throw new Exception("ИНН должен содержать 10 цифр.");
                }

                // Проверка корректности ввода
                if (!IsValidPhone(phone))
                {
                    throw new Exception("Телефон должен содержать 11 цифр.");
                }

                if (!IsOnlyLetters(directorName) && !directorName.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                {
                    throw new Exception("ФИО директора должно содержать только буквы и пробелы.");
                }

                if (!IsValidAccountNumber(accountNumber))
                {
                    throw new Exception("Расчетный счет должен содержать 20 цифр.");
                }

                if (!IsOnlyDigits(inn))
                {
                    throw new Exception("ИНН должен содержать только цифры.");
                }

                // Создание нового юридического лица
                LegalEntityClient newClient = new LegalEntityClient(companyName, directorName, legalAddress, phone, bankName, accountNumber, inn);
                ClientManager.GetClients().Add(newClient); // Добавление клиента в менеджер
                LoadClients(); // Обновление списка клиентов
                ClearLegalEntityClientFields(); // Очистка полей ввода
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

        private void btnRemoveClient_Click_1(object sender, EventArgs e)
        {
            // Проверка, выбран ли элемент в DataGridView
            if (dgvIndividualClients.SelectedRows.Count > 0)
            {
                // Получение выбранной строки
                var selectedRow = dgvIndividualClients.SelectedRows[0];

                // Проверка наличия значения в ячейке "ContactName"
                if (selectedRow.Cells["ContactName"].Value != null)
                {
                    string contactName = selectedRow.Cells["ContactName"].Value.ToString();

                    // Запрос подтверждения удаления
                    DialogResult dialogResult = MessageBox.Show($"Вы уверены, что хотите удалить клиента с именем {contactName}?", "Подтверждение удаления", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {
                        // Поиск клиента по имени
                        var clientToRemove = ClientManager.GetClients()
                            .OfType<IndividualClient>() // Фильтрация только физических лиц
                            .FirstOrDefault(c => c.ContactName == contactName);

                        if (clientToRemove != null)
                        {
                            ClientManager.GetClients().Remove(clientToRemove); // Удаление клиента
                            LoadClients(); // Обновление списка клиентов
                        }
                        else
                        {
                            MessageBox.Show("Клиент не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Выбранная строка пуста. Удаление невозможно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (dgvLegalEntityClients.SelectedRows.Count > 0)
            {
                //Получение выбранной строки 
                var selectedRow = dgvLegalEntityClients.SelectedRows[0];

                // Проверка наличия значения в ячейке "CompanyName"
                if (selectedRow.Cells["CompanyNameNEW"].Value != null)
                {
                    string companyName = selectedRow.Cells["CompanyNameNEW"].Value.ToString(); // Получение имени компании

                    // Запрос подтверждения удаления
                    DialogResult dialogResult = MessageBox.Show($"Вы уверены, что хотите удалить юридическое лицо с названием {companyName}?", "Подтверждение удаления", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {
                        // Поиск клиента по названию компании
                        var clientToRemove = ClientManager.GetClients()
                            .OfType<LegalEntityClient>() // Фильтрация только юридических лиц
                            .FirstOrDefault(c => c.CompanyName == companyName);

                        if (clientToRemove != null)
                        {
                            ClientManager.GetClients().Remove(clientToRemove); // Удаление клиента
                            LoadClients(); // Обновление списка клиентов
                        }
                        else
                        {
                            MessageBox.Show("Клиент не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Пожалуйста, выберите клиента для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEditIndividualClient_Click(object sender, EventArgs e)
        {
            // Проверка, выбрана ли хотя бы одна строка в DataGridView
            if (dgvIndividualClients.SelectedRows.Count > 0)
            {
                // Получение первой выбранной строки
                var selectedRow = dgvIndividualClients.SelectedRows[0];

                // Проверка наличия значения в ячейке "ContactName"
                if (selectedRow.Cells["ContactName"].Value != null)
                {
                    // Получение имени клиента из выбранной строки и преобразование его в строку
                    string contactName = selectedRow.Cells["ContactName"].Value.ToString();

                    // Поиск клиента по имени среди всех индивидуальных клиентов
                    IndividualClient clientToEdit = ClientManager.GetClients()
                        .OfType<IndividualClient>() // Фильтрация только физических лиц
                        .FirstOrDefault(c => c.ContactName == contactName); // Поиск клиента с совпадающим именем

                    // Проверка, найден ли клиент
                    if (clientToEdit != null) // Если клиент найден
                    {
                        // Заполнение текстовых полей данными из объекта клиента
                        txtContactName.Text = clientToEdit.ContactName; 
                        txtPhone.Text = clientToEdit.Phone; 
                        txtPassportSeries.Text = clientToEdit.PassportSeries; 
                        txtPassportNumber.Text = clientToEdit.PassportNumber; 
                        txtIssuedBy.Text = clientToEdit.IssuedBy; 
                        txtIssueDate.Value = clientToEdit.IssueDate; 
                    }
                    else
                    {
                        MessageBox.Show("Клиент не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Выбранная строка пуста. Редактирование невозможно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEditLegalEntityClient_Click(object sender, EventArgs e)
        {
            // Проверка, выбрана ли хотя бы одна строка в DataGridView
            if (dgvLegalEntityClients.SelectedRows.Count > 0)
            {
                // Получение первой выбранной строки
                var selectedRow = dgvLegalEntityClients.SelectedRows[0];

                // Проверка наличия значения в ячейке "CompanyNameNEW"
                if (selectedRow.Cells["CompanyNameNEW"].Value != null)
                {
                    // Получение имени компании из выбранной строки и преобразование его в строку
                    string companyName = selectedRow.Cells["CompanyNameNEW"].Value.ToString(); // Получение имени компании

                    // Поиск клиента по имени компании среди всех юридических лиц
                    LegalEntityClient clientToEdit = ClientManager.GetClients()
                        .OfType<LegalEntityClient>() // Фильтрация только юридических лиц
                        .FirstOrDefault(c => c.CompanyName == companyName); // Поиск клиента с совпадающим именем компании

                    // Проверка, найден ли клиент
                    if (clientToEdit != null) // Если клиент найден
                    {
                        // Заполнение текстовых полей данными из объекта клиента
                        txtCompanyNameNEW.Text = clientToEdit.CompanyName; 
                        txtDirectorName.Text = clientToEdit.DirectorName; 
                        txtLegalAddress.Text = clientToEdit.LegalAddress; 
                        txtLegalEntityPhone.Text = clientToEdit.Phone; 
                        txtBankName.Text = clientToEdit.BankName; 
                        txtAccountNumber.Text = clientToEdit.AccountNumber; 
                        txtINN.Text = clientToEdit.INN; 
                    }
                    else
                    {
                        
                        MessageBox.Show("Клиент не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    
                    MessageBox.Show("Выбранная строка пуста. Редактирование невозможно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                
                MessageBox.Show("Пожалуйста, выберите клиента для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSaveIndividualClientChanges_Click(object sender, EventArgs e)
        {
            // Проверка, выбрана ли хотя бы одна строка в DataGridView
            if (dgvIndividualClients.SelectedRows.Count > 0)
            {
                // Получение первой выбранной строки
                var selectedRow = dgvIndividualClients.SelectedRows[0];

                // Проверка наличия значения в ячейке "ContactName"
                if (selectedRow.Cells["ContactName"].Value != null)
                {
                    // Получение ФИО выбранного клиента и преобразование его в строку
                    string contactName = selectedRow.Cells["ContactName"].Value.ToString();

                    // Поиск клиента по ФИО среди всех индивидуальных клиентов
                    IndividualClient clientToEdit = ClientManager.GetClients()
                        .OfType<IndividualClient>() // Фильтрация только физических лиц
                        .FirstOrDefault(c => c.ContactName == contactName); // Поиск клиента с совпадающим ФИО

                    // Проверка, найден ли клиент
                    if (clientToEdit != null) 
                    {
                        // Получение данных из полей ввода
                        string phone = txtPhone.Text.Trim();
                        string passportSeries = txtPassportSeries.Text.Trim(); 
                        string passportNumber = txtPassportNumber.Text.Trim(); 
                        string issuedBy = txtIssuedBy.Text.Trim(); 
                        DateTime issueDate = txtIssueDate.Value; 

                        List<string> errors = new List<string>();

                        // Проверка заполненности полей
                        if (string.IsNullOrWhiteSpace(contactName) || string.IsNullOrWhiteSpace(phone) ||
                            string.IsNullOrWhiteSpace(passportSeries) || string.IsNullOrWhiteSpace(passportNumber) ||
                            string.IsNullOrWhiteSpace(issuedBy))
                        {
                            errors.Add("Пожалуйста, заполните все поля для физического лица."); 
                        }

                        if (!IsValidContactName(contactName)) 
                        {
                            errors.Add("ФИО должно содержать только буквы и пробелы."); 
                        }

                        if (!IsValidPhone(phone)) 
                        {
                            errors.Add("Телефон должен содержать 11 цифр."); 
                        }

                        if (!IsValidPassportSeries(passportSeries)) 
                        {
                            errors.Add("Серия паспорта должна содержать 4 цифры."); 
                        }

                        if (!IsValidPassportNumber(passportNumber)) 
                        {
                            errors.Add("Номер паспорта должен содержать 10 цифр."); 
                        }

                        // Если есть ошибки, выводим их
                        if (errors.Count > 0)
                        {
                            
                            MessageBox.Show(string.Join(Environment.NewLine, errors), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; 
                        }

                        // Обновление данных клиента
                        clientToEdit.ContactName = contactName; 
                        clientToEdit.Phone = phone; 
                        clientToEdit.PassportSeries = passportSeries; 
                        clientToEdit.PassportNumber = passportNumber; 
                        clientToEdit.IssuedBy = issuedBy; 
                        clientToEdit.IssueDate = issueDate; 

                        LoadClients(); // Обновление списка клиентов в интерфейсе
                        ClearIndividualClientFields(); // Очистка полей ввода
                                                       
                        MessageBox.Show("Изменения успешно сохранены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                       
                        MessageBox.Show("Кли ент не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                  
                    MessageBox.Show("Выбранная строка пуста. Сохранение невозможно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                
                MessageBox.Show("Пожалуйста, выберите клиента для сохранения изменений.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSaveLegalEntityClientChanges_Click(object sender, EventArgs e)
        {
            if (dgvLegalEntityClients.SelectedRows.Count > 0) // Проверка, выбрана ли строка
            {
                // Получаем выбранную строку 
                var selectedRow = dgvLegalEntityClients.SelectedRows[0];

                // Проверка на выбор знаечния
                if (selectedRow.Cells["CompanyNameNEW"].Value != null)
                {
                    string companyName = selectedRow.Cells["CompanyNameNEW"].Value.ToString(); // Полчение Имени
                   // Поиск клиента по ФИО среди всех юр клиентов
                    LegalEntityClient clientToEdit = ClientManager.GetClients()
                        .OfType<LegalEntityClient>() // Фильтрация только юридических лиц
                        .FirstOrDefault(c => c.CompanyName == companyName); // Поиск клиента с совпадающим именем компании

                    if (clientToEdit != null) 
                    {
                        // Получение данных из полей ввода
                        string directorName = txtDirectorName.Text.Trim();
                        string legalAddress = txtLegalAddress.Text.Trim();
                        string phone = txtLegalEntityPhone.Text.Trim();
                        string bankName = txtBankName.Text.Trim();
                        string accountNumber = txtAccountNumber.Text.Trim();
                        string inn = txtINN.Text.Trim();

                        List<string> errors = new List<string>();

                        // Проверка заполненности полей
                        if (string.IsNullOrWhiteSpace(companyName) || string.IsNullOrWhiteSpace(directorName) ||
                            string.IsNullOrWhiteSpace(legalAddress) || string.IsNullOrWhiteSpace(phone) ||
                            string.IsNullOrWhiteSpace(bankName) || string.IsNullOrWhiteSpace(accountNumber))
                        {
                            errors.Add("Пожалуйста, заполните все поля для юридического лица.");
                        }

                        // Проверка корректности ввода
                        if (!IsValidINN(inn))
                        {
                            errors.Add("ИНН должен содержать 10 цифр.");
                        }

                        if (!IsValidPhone(phone))
                        {
                            errors.Add("Телефон должен содержать 11 цифр.");
                        }

                        if (!IsOnlyLetters(directorName))
                        {
                            errors.Add("ФИО директора должно содержать только буквы и пробелы.");
                        }

                        if (!IsValidAccountNumber(accountNumber))
                        {
                            errors.Add("Расчетный счет должен содержать 20 цифр.");
                        }

                        // Если есть ошибки, выводим их
                        if (errors.Count > 0)
                        {
                            MessageBox.Show(string.Join(Environment.NewLine, errors), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; 
                        }

                        // Обновление данных клиента
                        clientToEdit.CompanyName = companyName;
                        clientToEdit.DirectorName = directorName;
                        clientToEdit.LegalAddress = legalAddress;
                        clientToEdit.Phone = phone;
                        clientToEdit.BankName = bankName;
                        clientToEdit.AccountNumber = accountNumber;
                        clientToEdit.INN = inn;

                        LoadClients(); // Обновление списка клиентов
                        ClearLegalEntityClientFields(); // Очистка полей ввода
                        MessageBox.Show("Изменения успешно сохранены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Клиент не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Выбранная строка пуста. Сохранение невозможно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для сохранения изменений.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Обработчик события нажатия кнопки очистки полей
        private void btnClear_Click_1(object sender, EventArgs e)
        {
            ClearIndividualClientFields(); // Очистка полей ввода индивидуального клиента
            ClearLegalEntityClientFields(); // Очистка полей ввода юридического лица
        }

 
        private void InitializeComponent()
        {
            this.dgvIndividualClients = new System.Windows.Forms.DataGridView();
            this.ContactName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PassportSeries = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PassportNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IssueDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IssuedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvLegalEntityClients = new System.Windows.Forms.DataGridView();
            this.CompanyNameNEW = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DirectorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LegalAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LegalEntityPhone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BankName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccountNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.INN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtContactName = new System.Windows.Forms.TextBox();
            this.txtIssuedBy = new System.Windows.Forms.TextBox();
            this.txtPassportNumber = new System.Windows.Forms.TextBox();
            this.txtPassportSeries = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtDirectorName = new System.Windows.Forms.TextBox();
            this.txtLegalAddress = new System.Windows.Forms.TextBox();
            this.txtBankName = new System.Windows.Forms.TextBox();
            this.txtAccountNumber = new System.Windows.Forms.TextBox();
            this.txtINN = new System.Windows.Forms.TextBox();
            this.txtCompanyNameNEW = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtIssueDate = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.btnAddIndividualClient = new System.Windows.Forms.Button();
            this.btnAddLegalEntityClient = new System.Windows.Forms.Button();
            this.btnRemoveClient = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.txtLegalEntityPhone = new System.Windows.Forms.TextBox();
            this.btnEditIndividualClient = new System.Windows.Forms.Button();
            this.btnEditLegalEntityClient = new System.Windows.Forms.Button();
            this.btnSaveIndividualClientChanges = new System.Windows.Forms.Button();
            this.btnSaveLegalEntityClientChanges = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIndividualClients)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLegalEntityClients)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvIndividualClients
            // 
            this.dgvIndividualClients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIndividualClients.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ContactName,
            this.Phone,
            this.PassportSeries,
            this.PassportNumber,
            this.IssueDate,
            this.IssuedBy});
            this.dgvIndividualClients.Location = new System.Drawing.Point(575, 28);
            this.dgvIndividualClients.Name = "dgvIndividualClients";
            this.dgvIndividualClients.Size = new System.Drawing.Size(644, 256);
            this.dgvIndividualClients.TabIndex = 0;
            
            // 
            // ContactName
            // 
            this.ContactName.HeaderText = "ФИО";
            this.ContactName.Name = "ContactName";
            // 
            // Phone
            // 
            this.Phone.HeaderText = "Телефон ";
            this.Phone.Name = "Phone";
            // 
            // PassportSeries
            // 
            this.PassportSeries.HeaderText = "Серия паспорта ";
            this.PassportSeries.Name = "PassportSeries";
            // 
            // PassportNumber
            // 
            this.PassportNumber.HeaderText = "Номер паспорта ";
            this.PassportNumber.Name = "PassportNumber";
            // 
            // IssueDate
            // 
            this.IssueDate.HeaderText = "Дата выдачи ";
            this.IssueDate.Name = "IssueDate";
            // 
            // IssuedBy
            // 
            this.IssuedBy.HeaderText = "Кем выдан";
            this.IssuedBy.Name = "IssuedBy";
            // 
            // dgvLegalEntityClients
            // 
            this.dgvLegalEntityClients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLegalEntityClients.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CompanyNameNEW,
            this.DirectorName,
            this.LegalAddress,
            this.LegalEntityPhone,
            this.BankName,
            this.AccountNumber,
            this.INN});
            this.dgvLegalEntityClients.Location = new System.Drawing.Point(575, 337);
            this.dgvLegalEntityClients.Name = "dgvLegalEntityClients";
            this.dgvLegalEntityClients.Size = new System.Drawing.Size(729, 264);
            this.dgvLegalEntityClients.TabIndex = 1;
            // 
            // CompanyNameNEW
            // 
            this.CompanyNameNEW.HeaderText = "Название компании";
            this.CompanyNameNEW.Name = "CompanyNameNEW";
            // 
            // DirectorName
            // 
            this.DirectorName.HeaderText = "ФИО директора";
            this.DirectorName.Name = "DirectorName";
            // 
            // LegalAddress
            // 
            this.LegalAddress.HeaderText = "Юр адрес";
            this.LegalAddress.Name = "LegalAddress";
            // 
            // LegalEntityPhone
            // 
            this.LegalEntityPhone.HeaderText = "Телефон ";
            this.LegalEntityPhone.Name = "LegalEntityPhone";
            // 
            // BankName
            // 
            this.BankName.HeaderText = "Название банка";
            this.BankName.Name = "BankName";
            // 
            // AccountNumber
            // 
            this.AccountNumber.HeaderText = "Расчетный счет";
            this.AccountNumber.Name = "AccountNumber";
            // 
            // INN
            // 
            this.INN.HeaderText = "ИНН";
            this.INN.Name = "INN";
            // 
            // txtContactName
            // 
            this.txtContactName.Location = new System.Drawing.Point(194, 85);
            this.txtContactName.Name = "txtContactName";
            this.txtContactName.Size = new System.Drawing.Size(124, 20);
            this.txtContactName.TabIndex = 2;
            // 
            // txtIssuedBy
            // 
            this.txtIssuedBy.Location = new System.Drawing.Point(194, 500);
            this.txtIssuedBy.Name = "txtIssuedBy";
            this.txtIssuedBy.Size = new System.Drawing.Size(124, 20);
            this.txtIssuedBy.TabIndex = 4;
            // 
            // txtPassportNumber
            // 
            this.txtPassportNumber.Location = new System.Drawing.Point(194, 337);
            this.txtPassportNumber.Name = "txtPassportNumber";
            this.txtPassportNumber.Size = new System.Drawing.Size(124, 20);
            this.txtPassportNumber.TabIndex = 6;
            // 
            // txtPassportSeries
            // 
            this.txtPassportSeries.Location = new System.Drawing.Point(194, 261);
            this.txtPassportSeries.Name = "txtPassportSeries";
            this.txtPassportSeries.Size = new System.Drawing.Size(124, 20);
            this.txtPassportSeries.TabIndex = 7;
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(194, 170);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(124, 20);
            this.txtPhone.TabIndex = 8;
            // 
            // txtDirectorName
            // 
            this.txtDirectorName.Location = new System.Drawing.Point(373, 170);
            this.txtDirectorName.Name = "txtDirectorName";
            this.txtDirectorName.Size = new System.Drawing.Size(124, 20);
            this.txtDirectorName.TabIndex = 15;
            // 
            // txtLegalAddress
            // 
            this.txtLegalAddress.Location = new System.Drawing.Point(373, 261);
            this.txtLegalAddress.Name = "txtLegalAddress";
            this.txtLegalAddress.Size = new System.Drawing.Size(124, 20);
            this.txtLegalAddress.TabIndex = 14;
            // 
            // txtBankName
            // 
            this.txtBankName.Location = new System.Drawing.Point(373, 425);
            this.txtBankName.Name = "txtBankName";
            this.txtBankName.Size = new System.Drawing.Size(124, 20);
            this.txtBankName.TabIndex = 13;
            // 
            // txtAccountNumber
            // 
            this.txtAccountNumber.Location = new System.Drawing.Point(373, 500);
            this.txtAccountNumber.Name = "txtAccountNumber";
            this.txtAccountNumber.Size = new System.Drawing.Size(124, 20);
            this.txtAccountNumber.TabIndex = 12;
            // 
            // txtINN
            // 
            this.txtINN.Location = new System.Drawing.Point(373, 567);
            this.txtINN.Name = "txtINN";
            this.txtINN.Size = new System.Drawing.Size(124, 20);
            this.txtINN.TabIndex = 11;
            // 
            // txtCompanyNameNEW
            // 
            this.txtCompanyNameNEW.Location = new System.Drawing.Point(373, 85);
            this.txtCompanyNameNEW.Name = "txtCompanyNameNEW";
            this.txtCompanyNameNEW.Size = new System.Drawing.Size(124, 20);
            this.txtCompanyNameNEW.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(191, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "ФИО:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(370, 245);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Юридический адрес:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(197, 484);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Кем выдан:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(191, 403);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Дата выдачи паспорта:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(197, 245);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Серия паспорта:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(376, 551);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "ИНН:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(197, 321);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Номер паспорта:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(376, 484);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Расчетный счет:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(191, 154);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "Телефон:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(370, 69);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(113, 13);
            this.label11.TabIndex = 26;
            this.label11.Text = "Название компании:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(370, 409);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(93, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "Название банка:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(370, 154);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(110, 13);
            this.label13.TabIndex = 28;
            this.label13.Text = "ФИО руководителя:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label14.Location = new System.Drawing.Point(191, 28);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(74, 15);
            this.label14.TabIndex = 29;
            this.label14.Text = "Физ лицо:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label15.Location = new System.Drawing.Point(376, 28);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(68, 15);
            this.label15.TabIndex = 30;
            this.label15.Text = "Юр лицо:";
            // 
            // txtIssueDate
            // 
            this.txtIssueDate.Location = new System.Drawing.Point(194, 425);
            this.txtIssueDate.Name = "txtIssueDate";
            this.txtIssueDate.Size = new System.Drawing.Size(126, 20);
            this.txtIssueDate.TabIndex = 31;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(572, 6);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 19);
            this.label9.TabIndex = 32;
            this.label9.Text = "Физ лицо:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label16.Location = new System.Drawing.Point(576, 315);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(76, 19);
            this.label16.TabIndex = 33;
            this.label16.Text = "Юр лицо:";
            // 
            // btnAddIndividualClient
            // 
            this.btnAddIndividualClient.Location = new System.Drawing.Point(12, 37);
            this.btnAddIndividualClient.Name = "btnAddIndividualClient";
            this.btnAddIndividualClient.Size = new System.Drawing.Size(146, 34);
            this.btnAddIndividualClient.TabIndex = 35;
            this.btnAddIndividualClient.Text = "Добавить физическое лицо";
            this.btnAddIndividualClient.UseVisualStyleBackColor = true;
            this.btnAddIndividualClient.Click += new System.EventHandler(this.btnAddIndividualClient_Click_1);
            // 
            // btnAddLegalEntityClient
            // 
            this.btnAddLegalEntityClient.Location = new System.Drawing.Point(12, 85);
            this.btnAddLegalEntityClient.Name = "btnAddLegalEntityClient";
            this.btnAddLegalEntityClient.Size = new System.Drawing.Size(146, 40);
            this.btnAddLegalEntityClient.TabIndex = 36;
            this.btnAddLegalEntityClient.Text = "Добавить юридическое лицо";
            this.btnAddLegalEntityClient.UseVisualStyleBackColor = true;
            this.btnAddLegalEntityClient.Click += new System.EventHandler(this.btnAddLegalEntityClient_Click_1);
            // 
            // btnRemoveClient
            // 
            this.btnRemoveClient.Location = new System.Drawing.Point(12, 157);
            this.btnRemoveClient.Name = "btnRemoveClient";
            this.btnRemoveClient.Size = new System.Drawing.Size(146, 44);
            this.btnRemoveClient.TabIndex = 37;
            this.btnRemoveClient.Text = "Удалить клиента";
            this.btnRemoveClient.UseVisualStyleBackColor = true;
            this.btnRemoveClient.Click += new System.EventHandler(this.btnRemoveClient_Click_1);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(12, 234);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(146, 24);
            this.btnClear.TabIndex = 38;
            this.btnClear.Text = "Очистить поля";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click_1);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(376, 321);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(55, 13);
            this.label17.TabIndex = 40;
            this.label17.Text = "Телефон:";
            // 
            // txtLegalEntityPhone
            // 
            this.txtLegalEntityPhone.Location = new System.Drawing.Point(373, 337);
            this.txtLegalEntityPhone.Name = "txtLegalEntityPhone";
            this.txtLegalEntityPhone.Size = new System.Drawing.Size(124, 20);
            this.txtLegalEntityPhone.TabIndex = 39;
            // 
            // btnEditIndividualClient
            // 
            this.btnEditIndividualClient.Location = new System.Drawing.Point(12, 274);
            this.btnEditIndividualClient.Name = "btnEditIndividualClient";
            this.btnEditIndividualClient.Size = new System.Drawing.Size(146, 36);
            this.btnEditIndividualClient.TabIndex = 41;
            this.btnEditIndividualClient.Text = "Редактировать физ лицо";
            this.btnEditIndividualClient.UseVisualStyleBackColor = true;
            this.btnEditIndividualClient.Click += new System.EventHandler(this.btnEditIndividualClient_Click);
            // 
            // btnEditLegalEntityClient
            // 
            this.btnEditLegalEntityClient.Location = new System.Drawing.Point(13, 367);
            this.btnEditLegalEntityClient.Name = "btnEditLegalEntityClient";
            this.btnEditLegalEntityClient.Size = new System.Drawing.Size(146, 40);
            this.btnEditLegalEntityClient.TabIndex = 42;
            this.btnEditLegalEntityClient.Text = "Редактировать юр лицо";
            this.btnEditLegalEntityClient.UseVisualStyleBackColor = true;
            this.btnEditLegalEntityClient.Click += new System.EventHandler(this.btnEditLegalEntityClient_Click);
            // 
            // btnSaveIndividualClientChanges
            // 
            this.btnSaveIndividualClientChanges.Location = new System.Drawing.Point(13, 321);
            this.btnSaveIndividualClientChanges.Name = "btnSaveIndividualClientChanges";
            this.btnSaveIndividualClientChanges.Size = new System.Drawing.Size(145, 40);
            this.btnSaveIndividualClientChanges.TabIndex = 43;
            this.btnSaveIndividualClientChanges.Text = "Внести изменения для физ лица";
            this.btnSaveIndividualClientChanges.UseVisualStyleBackColor = true;
            this.btnSaveIndividualClientChanges.Click += new System.EventHandler(this.btnSaveIndividualClientChanges_Click);
            // 
            // btnSaveLegalEntityClientChanges
            // 
            this.btnSaveLegalEntityClientChanges.Location = new System.Drawing.Point(14, 426);
            this.btnSaveLegalEntityClientChanges.Name = "btnSaveLegalEntityClientChanges";
            this.btnSaveLegalEntityClientChanges.Size = new System.Drawing.Size(145, 41);
            this.btnSaveLegalEntityClientChanges.TabIndex = 44;
            this.btnSaveLegalEntityClientChanges.Text = "Внести изменения для юр лица";
            this.btnSaveLegalEntityClientChanges.UseVisualStyleBackColor = true;
            this.btnSaveLegalEntityClientChanges.Click += new System.EventHandler(this.btnSaveLegalEntityClientChanges_Click);
            // 
            // ClientManagerForm
            // 
            this.ClientSize = new System.Drawing.Size(1372, 644);
            this.Controls.Add(this.btnSaveLegalEntityClientChanges);
            this.Controls.Add(this.btnSaveIndividualClientChanges);
            this.Controls.Add(this.btnEditLegalEntityClient);
            this.Controls.Add(this.btnEditIndividualClient);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.txtLegalEntityPhone);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnRemoveClient);
            this.Controls.Add(this.btnAddLegalEntityClient);
            this.Controls.Add(this.btnAddIndividualClient);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtIssueDate);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDirectorName);
            this.Controls.Add(this.txtLegalAddress);
            this.Controls.Add(this.txtBankName);
            this.Controls.Add(this.txtAccountNumber);
            this.Controls.Add(this.txtINN);
            this.Controls.Add(this.txtCompanyNameNEW);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.txtPassportSeries);
            this.Controls.Add(this.txtPassportNumber);
            this.Controls.Add(this.txtIssuedBy);
            this.Controls.Add(this.txtContactName);
            this.Controls.Add(this.dgvLegalEntityClients);
            this.Controls.Add(this.dgvIndividualClients);
            this.Name = "ClientManagerForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvIndividualClients)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLegalEntityClients)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
 
    }
}