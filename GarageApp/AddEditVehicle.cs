using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarageApp
{
    public partial class AddEditVehicle : Form
    {
        DatabaseHelper dbHelper;
        string _vehicleId = string.Empty;

        public AddEditVehicle(string vehicleId)
        {
            InitializeComponent();
            LoadDropDown();
            _vehicleId = vehicleId;

            if (!string.IsNullOrEmpty(_vehicleId))
                LoadForm();
        }

        private void LoadDropDown()
        {
            try
            {
                if (dbHelper == null)
                    dbHelper = new DatabaseHelper();

                string query = "SELECT Id, Name FROM Make ORDER BY Name ASC";
                DataTable dt = dbHelper.GetDt(query);

                comboVehicle.DataSource = dt;
                comboVehicle.DisplayMember = "Name";
                comboVehicle.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadForm()
        {
            try 
            {
                if (dbHelper == null)
                    dbHelper = new DatabaseHelper();

                string query = $"SELECT RegNo, Owner, ContactNo, MakeId FROM Vehicle WHERE Id = '{_vehicleId}'";

                DataTable dt = dbHelper.GetDt(query);

                if (dt.Rows.Count == 1)
                {
                    txtOwner.Text = dt.Rows[0]["Owner"].ToString();
                    txtContactNo.Text = dt.Rows[0]["ContactNo"].ToString();
                    txtRegNo.Text = dt.Rows[0]["RegNo"].ToString();
                    comboVehicle.SelectedValue = dt.Rows[0]["MakeId"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dbHelper == null)
                    dbHelper = new DatabaseHelper();

                if (string.IsNullOrEmpty(_vehicleId))
                {
                    string query = $@"INSERT INTO Vehicle
                                    (RegNo, Owner, ContactNo, MakeId)
                                    VALUES
                                    ('{txtRegNo.Text}','{txtOwner.Text}','{txtContactNo.Text}','{comboVehicle.SelectedValue}')";
                    dbHelper.InsertRecord(query);
                }
                else
                {
                    string query = $@"UPDATE Vehicle SET
                                        RegNo = '{txtRegNo.Text}',
                                        Owner = '{txtOwner.Text}',
                                        ContactNo = '{txtContactNo.Text}',
                                        MakeId = '{comboVehicle.SelectedValue}'
                                    WHERE Id = '{_vehicleId}'";
                    dbHelper.UpdateRecord(query);
                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
