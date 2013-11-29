using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SqlCompactToSqlExpress_Export;
using System.IO;
using System.Data.SqlClient;

namespace WinClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowseSDF_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSDFPath.Text = dialog.FileName;
            }
        }

        //private void CreateDatabaseStructure()
        //{
        //    try
        //    {
        //        StreamReader strReader = new StreamReader("QueryFile.txt");
        //        string str;
        //        bool result = true;

        //        while ((str = strReader.ReadLine()) != null & result != false)
        //        {
        //            if (str != "")
        //            {
        //                result = ExecuteQuery(str);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //public bool ExecuteQuery(string sqlQuery)
        //{
        //    string connection = @"server=(local)\SQLExpress;database=Master;integrated Security=SSPI;";
        //    SqlConnection con = new SqlConnection(connection);
        //    bool result = true;

        //    SqlCommand cmd = new SqlCommand(sqlQuery, con);

        //    try
        //    {
        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        result = false;
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }

        //    return result;
        //}

        private void MigrateData()
        {

            SdfToSqlExpressExporter exporter = new SdfToSqlExpressExporter();
            string sdfFile = txtSDFPath.Text;
            if (!File.Exists(sdfFile))
            {
                MessageBox.Show("File does not exists! Please verify.", "Data Migration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string sqlExpressConString = txtConStr.Text;

            bool result = exporter.DoDataMigration(sdfFile, sqlExpressConString);
            if (result)
            {
                MessageBox.Show("Data migration successful!");
            }
            else
            {
                MessageBox.Show("Data migration failed!");
            }
        }

        private void btnConfigureData_Click(object sender, EventArgs e)
        {
            //CreateDatabaseStructure();
            MigrateData();
        }


    }
}
