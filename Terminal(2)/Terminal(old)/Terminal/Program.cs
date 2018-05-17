using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using System.Data.SqlServerCe;

namespace Terminal
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            Application.Run(new Form1());
        }
    }



    public class Cache
    {
        public DataSet CacheDS;
        public  SqlCeConnection Connection;
        private SqlCeCommand GetProducts;
        private SqlCeCommand GetMakes;
        private SqlCeCommand GetDeffectCategories;
        private SqlCeCommand GetRepairCategories;
        private SqlCeCommand GetWorkers;

        /// <summary> Инициализация </summary>
        public void Initialize()
        {
            Connection = new SqlCeConnection();
            //Connection.ConnectionString =
            //    "Provider=Microsoft.Jet.OLEDB.4.0; " +
            //    "Data Source=\"C:\\Documents and Settings\\COMP\\Мои документы\\Работа\\ReworksDB.mdb\"";

            string cmd = "SELECT * FROM Изделие";
            GetProducts = new SqlCeCommand(cmd, Connection);
            cmd = "SELECT * FROM Модель";
            GetMakes = new SqlCeCommand(cmd, Connection);
            cmd = "SELECT * FROM ВидДефекта";
            GetDeffectCategories = new SqlCeCommand(cmd, Connection);
            cmd = "SELECT * FROM ВидРемонта";
            GetRepairCategories = new SqlCeCommand(cmd, Connection);
            cmd = "SELECT * FROM Сотрудник";
            GetWorkers = new SqlCeCommand(cmd, Connection);
        }

        /// <summary> Попытка подключения </summary>
        public void AttemptToConnect()
        {
            //try
            //{
            //Connection.Open();
            CacheDS = new DataSet();
            //SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM ВидРемонта", Connection);
            //int i = da.Fill(CacheDS);
            //MessageBox.Show(i.ToString());
            //}
            //catch
            //{
            //MessageBox.Show("Error");
            //}

        }
    }

    /// <summary> Универсальный ListView </summary>
    public class AdvancedListView : ListView
    {
        private Image Rectangle;
        private SolidBrush BackColorSolidBrush;
        private SolidBrush HighLightSolidBrush;

        public AdvancedListView()
        {
            
            BackColorSolidBrush = new SolidBrush(Color.Azure);
            HighLightSolidBrush = new SolidBrush(Color.Blue);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics Graphics = Graphics.FromImage(Rectangle);
            Graphics.FillRegion(BackColorSolidBrush, ClientRectangle);
            
        }

    }
}