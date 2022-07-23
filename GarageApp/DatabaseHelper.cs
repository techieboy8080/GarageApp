using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageApp
{
    internal class DatabaseHelper
    {
        public SQLiteConnection conn;
        public SQLiteCommand cmd;

        private string DatabaseName = "garagedb.db";

        public DatabaseHelper()
        {
            conn = new SQLiteConnection($"Data Source={DatabaseName}");
            if (!File.Exists(DatabaseName))
            {
                SQLiteConnection.CreateFile(DatabaseName);
                OpenConnection();
                string query = @"CREATE TABLE 'Make' (
	                                'Id'	INTEGER,
	                                'Name'	TEXT,
	                                PRIMARY KEY('Id' AUTOINCREMENT)
                                );


                                CREATE TABLE 'Vehicle' (
	                                'Id'	INTEGER,
	                                'RegNo'	TEXT,
	                                'Owner'	TEXT,
	                                'ContactNo'	TEXT,
	                                'MakeId'	INTEGER,
	                                PRIMARY KEY('Id' AUTOINCREMENT)
                                );";
                cmd = new SQLiteCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
        }

        private void OpenConnection()
        {
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();
        }

        private void CloseConnection()
        {
            if (conn.State != System.Data.ConnectionState.Closed)
                conn.Close();
        }

        public DataTable GetDt(string query)
        {
            DataTable dt = new DataTable();
            OpenConnection();
            cmd = new SQLiteCommand(query, conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();
            return dt;
        }

        public int InsertRecord(string query)
        {
            OpenConnection();
            cmd = new SQLiteCommand(query, conn);
            int insertedRows = cmd.ExecuteNonQuery();
            CloseConnection();
            return insertedRows;
        }

        public int UpdateRecord(string query)
        {
            OpenConnection();
            cmd = new SQLiteCommand(query, conn);
            int updatedRows = cmd.ExecuteNonQuery();
            CloseConnection();
            return updatedRows;
        }

        public int DeleteRecord(string query)
        {
            OpenConnection();
            cmd = new SQLiteCommand(query, conn);
            int deletedRows = cmd.ExecuteNonQuery();
            CloseConnection();
            return deletedRows;
        }
    }
}
