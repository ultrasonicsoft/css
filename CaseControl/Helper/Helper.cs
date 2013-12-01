using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;
using System.IO;

namespace CaseControl
{
    internal class Helper
    {
        internal static bool IsDatabaseServerConfigured()
        {
            //string databasePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + System.IO.Path.DirectorySeparatorChar.ToString() + Constants.DATABASE_CONFIG;
            
            //if (!File.Exists(databasePath))
            //{
            //    Helper.LogMessage("Error: Database setting file not found!");
            //    throw new Exception("Database setting file not found!");
            //}
            //string connectionString = File.ReadAllText(databasePath);
            return DBHelper.TestConnection(Properties.Settings.Default.ConnectionString);
        }

        internal static void ShowInformationMessageBox(string message, string caption = null)
        {
            caption = caption?? "Client Information";
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        internal static void ShowErrorMessageBox(string message, string caption = null)
        {
            caption = caption?? "Client Information";
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        internal static MessageBoxResult ShowQuestionMessageBox(string message, string caption = null,MessageBoxButton buttons=MessageBoxButton.YesNoCancel)
        {
            caption = caption?? "Client Information";
            return MessageBox.Show(message, caption, buttons, MessageBoxImage.Question);
        }

        internal static void LogException(Exception ex)
        {
            try
            {
                string errorFile = System.IO.Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\\Errors.log";
                StringBuilder message = new StringBuilder();
                message.AppendFormat(Environment.NewLine);
                message.AppendFormat("==================================================================================================================================");
                message.AppendFormat(Environment.NewLine);
                message.AppendFormat("Date Time: {0}", DateTime.Now.ToString());
                message.AppendFormat(Environment.NewLine);
                message.AppendFormat("Error Message:{0}", ex.Message);
                message.AppendFormat(Environment.NewLine);
                message.AppendFormat("Stack Trace:{0}", ex.StackTrace);

                File.AppendAllText(errorFile, message.ToString());
            }
            catch(Exception exx)
            {
                MessageBox.Show(exx.Message);
            }
        }

        internal static void LogMessage(string logMessage)
        {
            try
            {
                string errorFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\\Errors.log";
                StringBuilder message = new StringBuilder();
                message.AppendFormat(Environment.NewLine);
                message.AppendFormat("==================================================================================================================================");
                message.AppendFormat(Environment.NewLine);
                message.AppendFormat("Date Time: {0}", DateTime.Now.ToString());
                message.AppendFormat(Environment.NewLine);
                message.AppendFormat("Error Message:{0}", logMessage);

                File.AppendAllText(errorFile, message.ToString());
            }
            catch (Exception exx)
            {
                MessageBox.Show(exx.Message);
            }
        }

       
    }
}
