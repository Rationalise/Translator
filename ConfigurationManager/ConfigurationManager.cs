using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace Translator
{
    public class AppConfigurationManager
    {
        static string jobIntervalMinutes { get { return ConfigurationManager.AppSettings[ConfigurationKeys.JobIntervalMinutes]; } }
        static string url { get { return ConfigurationManager.AppSettings[ConfigurationKeys.Url]; } }
        string tempFolderPath { get { return ConfigurationManager.AppSettings[ConfigurationKeys.TempFolderPath]; } }

        public static string Url = url;
        public static string JobIntervalMinutes = jobIntervalMinutes;
        public string PreJobFolder = string.Empty;
        public string PreJobSourceFolder = string.Empty;
        public string PreJobTMFolder = string.Empty;
        public string PreJobReportFolder = string.Empty;
        public string PreJobOutputFolder = string.Empty;

        public string PostJobFolder = string.Empty;
        public string PostJobSourceFolder = string.Empty;
        public string PostJobTMFolder = string.Empty;
        public string PostJobReportFolder = string.Empty;
        public string PostJobOutputFolder = string.Empty;

        private string preJobParentDirectory = "PRE";
        private string postJobParentDirectory = "POST";

        public struct SubUrl
        {
            public const string ProcessJob = "process_job";
            public const string ResetJobs = "reset_jobs";
        }

        public AppConfigurationManager(string jobId)
        {
            PreJobFolder = Path.Combine(tempFolderPath, preJobParentDirectory, jobId);
            PreJobSourceFolder =  Path.Combine(PreJobFolder, "Source");
            PreJobTMFolder = Path.Combine(PreJobFolder, "TM");
            PreJobOutputFolder = Path.Combine(PreJobFolder, "Output");
            PreJobReportFolder = Path.Combine(PreJobOutputFolder, "Reports");

            PostJobFolder = Path.Combine(tempFolderPath, postJobParentDirectory, jobId);
            PostJobSourceFolder = Path.Combine(PostJobFolder, "Source");
            PostJobTMFolder = Path.Combine(PostJobFolder, "TM");
            PostJobOutputFolder = Path.Combine(PostJobFolder, "Output");
            PostJobReportFolder = Path.Combine(PostJobOutputFolder, "Reports");
        }

        public struct ConfigurationKeys
        {
            public static string JobIntervalMinutes = "JobIntervalMinutes";
            public static string Url = "Url";
            public static string TempFolderPath = "TempFolderPath";
        }
    }
}
