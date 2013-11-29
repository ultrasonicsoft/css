using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;

namespace SqlCompactToSqlExpress_Export
{
    public class SdfToSqlExpressExporter
    {
        public bool DoDataMigration(string sdfFilePath, string sqlExpressConnectionString)
        {
            return MigrateData(sdfFilePath, sqlExpressConnectionString);
        }

        private bool MigrateData(string sdfFilePath, string sqlExpressConnectionString)
        {
            bool isSuccessful = true;
            try
            {
                string sdfConnectionString = string.Format("Data Source={0}",sdfFilePath);
                SqlCeConnection ceCon = new SqlCeConnection(sdfConnectionString);
                SqlConnection con = new SqlConnection(sqlExpressConnectionString);

                SqlCeDataAdapter ceAdp = new SqlCeDataAdapter("SELECT * FROM INFORMATION_SCHEMA.TABLES", ceCon);
                DataSet dataset = new DataSet("dataset");
                dataset.Tables.Add("Names");

                ceAdp.Fill(dataset.Tables["Names"]);

                DataSet sdfDataset = new DataSet("sdfDataset");
                for (int i = 0; i < dataset.Tables["Names"].Rows.Count; i++)
                {
                    string tableName = (string)dataset.Tables["Names"].Rows[i]["table_name"];
                    sdfDataset.Tables.Add(tableName);
                    ceAdp = new SqlCeDataAdapter("select * from [" + tableName + "]", ceCon);
                    SqlCeCommandBuilder cb = new SqlCeCommandBuilder(ceAdp);
                    try
                    {
                        ceAdp.Fill(sdfDataset.Tables[tableName]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("exception= " + e.Message);
                    }
                }

                foreach (DataTable tableName in sdfDataset.Tables)
                {
                    con.Open();
                    using (SqlBulkCopy bulkcopy = new SqlBulkCopy(con))
                    {
                        bulkcopy.DestinationTableName = "dbo.[" + tableName + "]";
                        try
                        {
                            bulkcopy.WriteToServer(tableName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccessful = false;
            }
            return isSuccessful;
        }
    }
}
