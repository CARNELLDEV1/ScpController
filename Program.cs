using System;
using ScpController.Controller;
using System.Configuration;

namespace ScpController
{
    class Program
    {
        static void Main(string[] args)
        {
            // Confirm smartscan remote and local data folder
            string remotePath = ConfigurationManager.AppSettings["RemotePath"];
            string localPath = ConfigurationManager.AppSettings["LocalPath"];

            // Configure a winscp controller to manipulate remote data
            var scpController = new WinscpController();

            Console.WriteLine("Started");

            // Build pipeline to remote file server
            try
            {
                scpController.BuildPipeline(remotePath, localPath);
                scpController.RetrieveData();
            }
            catch (Exception)
            {
                throw;
            }

            Console.WriteLine("Sucessful!");

        }
    }
}
