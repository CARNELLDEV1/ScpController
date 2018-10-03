using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using ScpController.Data;
using System.Data.SqlClient;
using System.Data;

using WinSCP;

namespace ScpController.Controller
{
    class WinscpController
    {
        #region Public Properties

        public int PortNumber { get; set; }
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RemotePath { get; set; }
        public string LocalPath { get; set; }

        public SessionOptions sessionOptions;
        public Protocol Protocol { get; set; }

        public EmailController EmailController { get; set; }

        #endregion

        #region Constructor

        public WinscpController()
        {
            sessionOptions = new SessionOptions()
            {
                Protocol = Protocol.Ftp,
                HostName = ConfigurationManager.AppSettings["HostName"],
                PortNumber = int.Parse(ConfigurationManager.AppSettings["PortNumber"]),
                UserName = ConfigurationManager.AppSettings["UserName"],
                Password = ConfigurationManager.AppSettings["Password"],
            };
        }

        #endregion

        #region Public Method

        public void BuildPipeline(string folderPath, string localPath)
        {
            if (folderPath != null && localPath != null)
            {
                RemotePath = folderPath;
                LocalPath = localPath;
            }
            else
            {
                Console.WriteLine("Pipeline is not built, either path is empty or not existed");
            }
        }

        public void RetrieveData()
        {
            using (Session session = new Session())
            {
                session.Open(sessionOptions);
                TransferOptions transferOptions = new TransferOptions
                {
                    TransferMode = TransferMode.Binary
                };

                TransferOperationResult transferResult;
                RemovalOperationResult removeResult;
                List<RemoteFileInfo> files = session.EnumerateRemoteFiles(RemotePath, "*.zip*", EnumerationOptions.AllDirectories).ToList();

                if (files != null)
                {
                    foreach (RemoteFileInfo file in files)
                    {
                        string fileName = Path.GetFileName(file.Name);
                        WorkItem workItem = new WorkItem(file);
                        UpdateWorkItem(workItem);

                        string localFile = Path.Combine(LocalPath, file.Name);
                        transferResult = session.GetFiles(file.FullName, localFile, false, transferOptions);
                        transferResult.Check();
                        removeResult = session.RemoveFiles(file.Name);
                        //EmailController.SendEmail(file.Name);
                    }
                }

                session.Close();
            }
        }

        private void UpdateWorkItem(WorkItem workitem)
        {

            string table = ConfigurationManager.AppSettings["TaskList"];
            string sqlQuery =
                $"INSERT INTO {table} (" +
                     "[job number]," +
                     "[job folder]," +
                     "[status]," +
                     "[Est work (days)]," +
                     "[est cost]," +
                     "[in contract]," +
                     "[area]," +
                     "[date added]," +
                     "[assigned]," +
                     "[assigned to]," +
                     "[date started]," +
                     "[images uploaded]," +
                     "[reports uploaded]," +
                     "[commited to master]," +
                     "[client emailed]," +
                     "[date completed]," +
                     "[date assigned]," +
                     "[date surveyed]," +
                     "[smartscan]," +
                     "[road])" +
                "VALUES(" +
                    $"{workitem.JobNumber}" + "," +
                    $"{workitem.JobFolder}" + "," +
                    $"{workitem.Status}" + "," +
                    $"{workitem.EstWork}" + "," +
                    $"{workitem.EstCost}" + "," +
                    $"{workitem.InContract}" + "," +
                    $"'{workitem.Area}'" + "," +
                    $"{workitem.DateAdded}" + "," +
                    $"{workitem.Assigned}" + "," +
                    $"{workitem.AssignedTo}" + "," +
                    $"{workitem.DataStarted}" + "," +
                    $"{workitem.ImageUploaded}" + "," +
                    $"{workitem.ReportUploaded}" + "," +
                    $"{workitem.CommitedToMaster}" + "," +
                    $"{workitem.ClientEmailed}" + "," +
                    $"{workitem.DateCompleted}" + "," +
                    $"{workitem.DateAssigned}" + "," +
                    $"{workitem.DataSurveyed}" + "," +
                    $"{workitem.Smartscan}" + "," +
                    $"'{workitem.Road}'" + ");";

            Console.WriteLine(sqlQuery); ;

            string connectString;
            connectString = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sqlQuery;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }

        }
        #endregion

    }
}
