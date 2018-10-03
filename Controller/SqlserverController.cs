using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;

namespace ScpController.Controller
{
    class SqlserverController
    {
        #region Private Variables

        public SqlConnection sqlConnection;

        #endregion

        #region Public Properties

        public string ConnectString { get; set; }

        #endregion

        #region Constructor

        public SqlserverController()
        {
            ConnectString = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;
        }

        #endregion

        #region Public Methods
        
        public void InsertWorkItem(string table, Dictionary<string, string> updateValues)
        {
            string prefix = "INSERT INTO" + table;

            using (sqlConnection = new SqlConnection(ConnectString))
            {
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand(ConnectString))
                {
                    command.ExecuteNonQuery();
                }
            }
            
        }

        public void CreateTable()
        {
            try
            {
                using (sqlConnection = new SqlConnection(ConnectString))
                {
                    sqlConnection.Open();

                    using (SqlCommand command = new SqlCommand(
                        "CREATE TABLE Customer(" +
                        "First_Name char(50)," +
                        "Last_Name char(50)," +
                        "Address char(50)," +
                        "City char(50)," +
                        "Country char(25)," +
                        "Birth_Date datetime);", sqlConnection
                        ))

                        command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
