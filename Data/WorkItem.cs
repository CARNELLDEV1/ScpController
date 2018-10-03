using System;
using WinSCP;
using System.IO;
using System.Configuration;

namespace ScpController.Data
{
    public class WorkItem
    {
        #region Public Properties

        private RemoteFileInfo workItemFile;

        public string JobNumber {get;private set;}
        public string JobFolder {get; private set;}
        public string Status { get; private set; }
        public string EstWork { get; private set; }
        public string EstCost { get; private set; }
        public string InContract { get; private set; }
        public string Area {get; private set; }
        public string DateAdded{ get; private set; }
        public string Assigned { get; private set; }
        public string AssignedTo { get; private set; }
        public string DataStarted { get; private set; }
        public string ImageUploaded { get; private set; }
        public string ReportUploaded { get; private set; }
        public string CommitedToMaster { get; private set; }
        public string ClientEmailed { get; private set; }
        public string DateCompleted { get; private set; }
        public string DateAssigned { get; private set; }
        public string DataSurveyed { get; private set; }
        public int Smartscan { get; private set; }
        public string Road { get; private set; }

        #endregion

        #region Constructor

        public WorkItem(RemoteFileInfo file)
        {
            //Default parameters for work items
            JobFolder = ConfigurationManager.AppSettings["LocalPath"];
            Status = "Unassinged";
            EstWork = "null";
            EstCost = "null";
            InContract = "Yes";
            DateAdded = DateTime.Now.Date.ToShortDateString();
            Assigned = "No";
            AssignedTo = "null";
            DateAssigned = "null";
            DataSurveyed = "null";
            Smartscan = 1;
            DataStarted = DateTime.Now.Date.ToShortDateString();
            ImageUploaded = "No";
            ReportUploaded = "No";
            CommitedToMaster = "null";
            DateCompleted = "null";
            ClientEmailed = "null";
            GetParameters(file);
        }

        #endregion

        /// <summary>
        /// Retrieve file parameters from winscp file
        /// </summary>
        /// <param name="file"></param>
        private void GetParameters(RemoteFileInfo file)
        {
            this.workItemFile = file;

            if (workItemFile.Name != string.Empty)
            {
                JobNumber = Path.GetFileNameWithoutExtension(workItemFile.Name).Split('-')[0];
                Road = Path.GetFileNameWithoutExtension(workItemFile.Name).Split('-')[1];
                Area = Path.GetFileNameWithoutExtension(workItemFile.Name).Split('-')[2];
            }

        }

    }
}
