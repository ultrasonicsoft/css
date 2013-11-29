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
        //private static string connectionString = "Data Source=" + Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Database\CaseControl_Database.sdf;Persist Security Info=False;";

        internal static SqlConnection DB_CONNECTION = null;

        static DBHelper()
        {
            ConfigureConnectionString();
        }

        internal static string ConnectionString
        {
            set
            {
                DB_CONNECTION = new SqlConnection(value);
            }
        }

        internal static void ConfigureConnectionString()
        {
            try
            {
                DBHelper.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["defaultConStr"];

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

        internal static bool CreateDatabaseStructure(string serverName)
        {
            DropDatabase(serverName);

            bool isSuccessful = true;
            try
            {
                StreamReader strReader = new StreamReader(@"QueryFile.txt");
                string str;
                bool result = true;

                while ((str = strReader.ReadLine()) != null & result != false)
                {
                    if (str != "")
                    {
                        result = ExecuteQuery(str, serverName);
                        if (result == false)
                        {
                            isSuccessful = false;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                isSuccessful = false;
            }
            return isSuccessful;
        }

        private static void DropDatabase(string serverName)
        {
            string connection = string.Format(@"server={0}\SQLExpress;database=Master;integrated Security=SSPI;", serverName);
            SqlConnection con = new SqlConnection(connection);
            bool result = true;

            SqlCommand cmd = new SqlCommand("drop database CaseControlDB", con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                con.Close();
            }

        }

        public static bool ExecuteQuery(string sqlQuery, string serverName)
        {
            string connection = string.Format(@"server={0}\SQLExpress;database=Master;integrated Security=SSPI;", serverName);
            SqlConnection con = new SqlConnection(connection);
            bool result = true;

            SqlCommand cmd = new SqlCommand(sqlQuery, con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = false;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
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
                Helper.ShowErrorMessageBox("Database server is down! Please check your database server or contact your vendor!", "Case Control SysteM");
                Environment.Exit(0);
            }
            return result;
        }

        internal static SqlCommand GetSelectCommand(string sqlQuery)
        {
            SqlCommand command = new SqlCommand();
            try
            {
                command.CommandText = sqlQuery;
                command.Connection = DB_CONNECTION;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                TestConnection(DB_CONNECTION.ConnectionString);
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
                command.Connection = DB_CONNECTION;
                DB_CONNECTION.Open();
                scalarValue = command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                TestConnection(DB_CONNECTION.ConnectionString);

            }
            finally
            {
                if (DB_CONNECTION.State != System.Data.ConnectionState.Closed)
                {
                    DB_CONNECTION.Close();
                }
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
                command.Connection = DB_CONNECTION;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                adapter.Fill(result);
                adapter.Dispose();
                command.Dispose();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                TestConnection(DB_CONNECTION.ConnectionString);

            }
            finally
            {
                if (DB_CONNECTION.State != System.Data.ConnectionState.Closed)
                {
                    DB_CONNECTION.Close();
                }
            }
            return result;
        }

        internal static int ExecuteNonQuery(string sqlQuery)
        {
            int totalRowsAffected = 0;
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = DB_CONNECTION;
                command.CommandText = sqlQuery;
                DB_CONNECTION.Open();
                totalRowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                TestConnection(DB_CONNECTION.ConnectionString);

            }
            finally
            {
                if (DB_CONNECTION.State != System.Data.ConnectionState.Closed)
                {
                    DB_CONNECTION.Close();
                }
            }
            return totalRowsAffected;
        }

        internal static int ExecuteNonQueryCommand(SqlCommand command)
        {
            int totalRowsAffected = 0;
            try
            {
                command.Connection = DB_CONNECTION;
                DB_CONNECTION.Open();
                totalRowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                TestConnection(DB_CONNECTION.ConnectionString);

            }
            finally
            {
                if (DB_CONNECTION.State != System.Data.ConnectionState.Closed)
                {
                    DB_CONNECTION.Close();
                }
            }
            return totalRowsAffected;
        }
    }
}