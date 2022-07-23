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
    public partial class MainForm : Form
    {
        DatabaseHelper dbHelper;
        public MainForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            { 
                if(dbHelper == null)
                    dbHelper = new DatabaseHelper();

                string query = @"SELECT v.Id, v.RegNo, v.Owner, v.ContactNo, m.Name AS Vehicle
                                FROM Vehicle v
                                LEFT JOIN Make m ON m.Id = v.MakeId";
                DataTable dt = dbHelper.GetDt(query);
                dataGridViewVehicle.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEditVehicle addEditVehicle = new AddEditVehicle(string.Empty);
            addEditVehicle.ShowDialog();
            LoadData();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewVehicle.SelectedRows.Count == 1)
            {
                int index = dataGridViewVehicle.SelectedRows[0].Index;
                string id = dataGridViewVehicle.Rows[index].Cells["Id"].Value.ToString();

                AddEditVehicle addEditVehicle = new AddEditVehicle(id);
                addEditVehicle.ShowDialog();
                LoadData();
            }
            else
                MessageBox.Show("Please select a row");
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewVehicle.SelectedRows.Count == 1)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete the selected row?", "Confirm", MessageBoxButtons.YesNo);

                if (result != DialogResult.Yes) return;

                int index = dataGridViewVehicle.SelectedRows[0].Index;
                string id = dataGridViewVehicle.Rows[index].Cells["Id"].Value.ToString();

                if (dbHelper == null)
                    dbHelper = new DatabaseHelper();

                string query = $"DELETE FROM Vehicle WHERE Id = '{id}'";
                dbHelper.DeleteRecord(query);

                LoadData();
            }
            else
                MessageBox.Show("Please select a row");
        }
    }
}
