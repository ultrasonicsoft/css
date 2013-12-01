using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Windows;
using System.Data.SqlClient;

namespace CaseControl
{
    internal class DBHelper
    {

        //internal static SqlConnection DB_CONNECTION = null;

        static DBHelper()
        {
            ConfigureConnectionString();
        }

        internal static void ConfigureConnectionString()
        {
            try
            {
                //if (DB_CONNECTION == null)
                //{
                //    DB_CONNECTION = new SqlConnection(Properties.Settings.Default.ConnectionString);
                //}
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        internal static bool IsDatabaseInstalled(string connectionString)
        {
            bool result = true;
            try
            {
                SqlConnection testConnection = new SqlConnection(connectionString);
                testConnection.Open();
                testConnection.Close();
            }
            catch (Exception ex)
            {
                result = false;
                Helper.LogException(ex);
            }
            return result;
        }

        internal static bool TestConnection(string connectionString)
        {
            bool result = true;
            try
            {
                SqlConnection testConnection = new SqlConnection(connectionString);
                testConnection.Open();
                testConnection.Close();
            }
            catch (Exception ex)
            {
                result = false;
                Helper.LogException(ex);
                Helper.LogMessage("Connection String:" + connectionString);
                //Helper.ShowErrorMessageBox("Database server is down! Please check your database server or contact your vendor!", "Case Control SysteM");
                //Environment.Exit(0);
            }
            return result;
        }

        internal static SqlCommand GetSelectCommand(string sqlQuery)
        {
            SqlCommand command = new SqlCommand();
            try
            {
                command.CommandText = sqlQuery;
                command.Connection = new SqlConnection(Properties.Settings.Default.ConnectionString);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                TestConnection(Properties.Settings.Default.ConnectionString);
            }
            return command;
        }

        internal static object GetScalarValue(string sqlQuery)
        {
            object scalarValue = null;
            SqlCommand command = new SqlCommand();
            try
            {
                command.CommandText = sqlQuery;
                command.Connection = new SqlConnection(Properties.Settings.Default.ConnectionString);
                command.Connection.Open();
                scalarValue = command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                TestConnection(Properties.Settings.Default.ConnectionString);

            }
            return scalarValue;
        }

        internal static DataSet GetSelectDataSet(string sqlQuery)
        {
            DataSet result = new DataSet();
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = sqlQuery;
                command.Connection = new SqlConnection(Properties.Settings.Default.ConnectionString);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                adapter.Fill(result);
                adapter.Dispose();
                command.Dispose();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                TestConnection(Properties.Settings.Default.ConnectionString);

            }
            return result;
        }

        internal static int ExecuteNonQuery(string sqlQuery)
        {
            int totalRowsAffected = 0;
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = new SqlConnection(Properties.Settings.Default.ConnectionString);
                command.CommandText = sqlQuery;
                command.Connection.Open();
                totalRowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                TestConnection(Properties.Settings.Default.ConnectionString);

            }
            return totalRowsAffected;
        }

        internal static int ExecuteNonQueryCommand(SqlCommand command)
        {
            int totalRowsAffected = 0;
            try
            {
                command.Connection = new SqlConnection(Properties.Settings.Default.ConnectionString);
                command.Connection.Open();
                totalRowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                TestConnection(Properties.Settings.Default.ConnectionString);

            }
           
            return totalRowsAffected;
        }
    }
}