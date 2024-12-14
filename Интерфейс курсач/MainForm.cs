﻿using System;
using System.Windows.Forms;

namespace FreightTransportSystem
{
    public partial class MainForm : Form
    {
        private Button btnManageCars;
        private Button btnManageOrders;
        private Button btnManageTrips;
        private Button btnManageClients;
        private Button btnManageDrivers;
        private Button btnExit;

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnManageCars_Click_1(object sender, EventArgs e)
        {
            CarManagerForm carManager = new CarManagerForm();
            carManager.ShowDialog();
        }

        private void btnManageDrivers_Click_1(object sender, EventArgs e)
        {
            DriverManagerForm driverManager = new DriverManagerForm();
            driverManager.ShowDialog();
        }

        private void btnManageClients_Click_1(object sender, EventArgs e)
        {
            ClientManagerForm clientManager = new ClientManagerForm();
            clientManager.ShowDialog();
        }

        private void btnManageOrders_Click_1(object sender, EventArgs e)
        {
            OrderManagerForm orderManager = new OrderManagerForm();
            orderManager.ShowDialog();
        }

        private void btnManageTrips_Click_1(object sender, EventArgs e)
        {
            TripManagerForm tripManager = new TripManagerForm();
            tripManager.ShowDialog();
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeComponent()
        {
            this.btnManageCars = new System.Windows.Forms.Button();
            this.btnManageOrders = new System.Windows.Forms.Button();
            this.btnManageTrips = new System.Windows.Forms.Button();
            this.btnManageClients = new System.Windows.Forms.Button();
            this.btnManageDrivers = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnManageCars
            // 
            this.btnManageCars.Location = new System.Drawing.Point(436, 36);
            this.btnManageCars.Name = "btnManageCars";
            this.btnManageCars.Size = new System.Drawing.Size(304, 53);
            this.btnManageCars.TabIndex = 0;
            this.btnManageCars.Text = "Управление автомобилями";
            this.btnManageCars.UseVisualStyleBackColor = true;
            this.btnManageCars.Click += new System.EventHandler(this.btnManageCars_Click_1);
            // 
            // btnManageOrders
            // 
            this.btnManageOrders.Location = new System.Drawing.Point(436, 324);
            this.btnManageOrders.Name = "btnManageOrders";
            this.btnManageOrders.Size = new System.Drawing.Size(304, 53);
            this.btnManageOrders.TabIndex = 1;
            this.btnManageOrders.Text = "Управление заказами";
            this.btnManageOrders.UseVisualStyleBackColor = true;
            this.btnManageOrders.Click += new System.EventHandler(this.btnManageOrders_Click_1);
            // 
            // btnManageTrips
            // 
            this.btnManageTrips.Location = new System.Drawing.Point(436, 409);
            this.btnManageTrips.Name = "btnManageTrips";
            this.btnManageTrips.Size = new System.Drawing.Size(304, 53);
            this.btnManageTrips.TabIndex = 2;
            this.btnManageTrips.Text = "Управление поездками";
            this.btnManageTrips.UseVisualStyleBackColor = true;
            this.btnManageTrips.Click += new System.EventHandler(this.btnManageTrips_Click_1);
            // 
            // btnManageClients
            // 
            this.btnManageClients.Location = new System.Drawing.Point(436, 236);
            this.btnManageClients.Name = "btnManageClients";
            this.btnManageClients.Size = new System.Drawing.Size(304, 53);
            this.btnManageClients.TabIndex = 3;
            this.btnManageClients.Text = "Управление клиентами";
            this.btnManageClients.UseVisualStyleBackColor = true;
            this.btnManageClients.Click += new System.EventHandler(this.btnManageClients_Click_1);
            // 
            // btnManageDrivers
            // 
            this.btnManageDrivers.Location = new System.Drawing.Point(436, 140);
            this.btnManageDrivers.Name = "btnManageDrivers";
            this.btnManageDrivers.Size = new System.Drawing.Size(304, 53);
            this.btnManageDrivers.TabIndex = 4;
            this.btnManageDrivers.Text = "Управление водителями";
            this.btnManageDrivers.UseVisualStyleBackColor = true;
            this.btnManageDrivers.Click += new System.EventHandler(this.btnManageDrivers_Click_1);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(436, 493);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(304, 53);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Выход";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click_1);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1237, 580);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnManageDrivers);
            this.Controls.Add(this.btnManageClients);
            this.Controls.Add(this.btnManageTrips);
            this.Controls.Add(this.btnManageOrders);
            this.Controls.Add(this.btnManageCars);
            this.Name = "MainForm";
            this.ResumeLayout(false);

        }

        
    }
}