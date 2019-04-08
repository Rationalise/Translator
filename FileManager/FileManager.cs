using System;
using System.IO;
using Microsoft.Office.Interop.Word;
using System.Xml;
using LogManager;
using Microsoft.Win32;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using iTextSharp.text.pdf;

namespace FileManager
{
    public class FileManager
    { 
        /// <summary>
        /// CheckFolderTrackChanges
        /// </summary>
        /// <param name="folderPath">Folder to check</param>
        /// <returns>TRUE if folder contains track changes</returns>
        public bool CheckFolderTrackChanges(string folderPath)
        {
            string functionName = "CheckFolderTrackChanges";
            Logger.Info(functionName, "Checking track changes..");
            foreach (string file in Directory.EnumerateFiles(folderPath))
            {
                if (FileIsSupported_DOC(file))
                {
                    Logger.Info(functionName, "Track change found in : " + file);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// CheckFolderTrackChanges
        /// </summary>
        /// <param name="folderPath">Folder to check</param>
        /// <param name="filePathsSupported">string array of files in folder which are supported</param>
        /// <returns>TRUE if folder contains supported files </returns>
        public bool CheckFolderForSupportedFiles(string folderPath, out string[] filePathsSupported)
        {
            string functionName = "CheckFolderTrackChanges";
            Logger.Info(functionName, "Checking track changes for ..");
            List<string> listFilePathsWithoutTrackChanges = new List<string>();
            foreach (string file in Directory.EnumerateFiles(folderPath))
            {
                Logger.Info(functionName, file);
                if (FileIsSupported_DOC(file) && FileIsSupported_PDF(file))
                {
                    listFilePathsWithoutTrackChanges.Add(file);
                }
            }
            filePathsSupported = listFilePathsWithoutTrackChanges.ToArray();
            if (listFilePathsWithoutTrackChanges.Count > 0)
                return true;
            else
                return false;
        }

        public bool FileIsSupported_DOC(string filePath)
        {
            string functionName = "CheckFileNoTrackChanges";
            if (!Path.GetExtension(filePath).ToUpper().StartsWith(".DOC"))
            {
                return true;
            }
            EnableAllOfficeDisabledItems();
            Microsoft.Office.Interop.Word.Application ap = new Microsoft.Office.Interop.Word.Application();
            Document document = ap.Documents.Open(filePath);
            bool trackChanges = false;
            if (document != null)
            {
                trackChanges = document.TrackRevisions;
                document.Close();
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(document);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(ap);
            if(trackChanges)
            {
                Logger.Warn(functionName, filePath + " contains track changes");
            }
            return !trackChanges;
        }

        public string CheckXmlTagContent(string xmlFilePath, string tagName, string attribute)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFilePath);
            using (XmlNodeList elementList = xmlDocument.GetElementsByTagName(tagName))
            {
                if (elementList.Count > 0)
                {
                    if (elementList[0].Attributes[attribute] != null)
                        return elementList[0].Attributes[attribute].Value;
                }
            }
            return string.Empty;
        }

        public void EnableAllOfficeDisabledItems()
        {
            string keyName = @"Software\Microsoft\Office\15.0\Word\Resiliency\DisabledItems";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true))
            {
                if (key == null)
                {
                    // Key doesn't exist. Do whatever you want to handle
                    // this case
                }
                else
                {
                    string[] subkeyNames = key.GetValueNames();
                    foreach(string subkeyName in subkeyNames)
                    {
                        if(subkeyName.ToUpper() != "DEFAULT")
                        {
                            key.DeleteValue(subkeyName);
                        }
                    }
                }
            }
        }

        public string GetLanguagePairOfReport(string excelFilePath)
        {
            Excel.Application xlApp = new Excel.Application();//create a new Excel application
            Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(excelFilePath);//open the workbook
            Excel.Worksheet xlWorkSheet = xlWorkBook.Worksheets["Analyze Files Report"];//get the worksheet object
            Excel.Range colRange = xlWorkSheet.Columns["A:A"];//get the range object where you want to search from
            string searchString = "Language:";

            Excel.Range resultRange = colRange.Find(
                What: searchString,
                LookIn: Excel.XlFindLookIn.xlValues,
                LookAt: Excel.XlLookAt.xlPart,
                SearchOrder: Excel.XlSearchOrder.xlByRows,
                SearchDirection: Excel.XlSearchDirection.xlNext
                );// search searchString in the range, if find result, return a range

            string languagePair = string.Empty;
            if (resultRange is null)
            {
                //MessageBox.Show("Did not found " + searchString + " in column A");
            }
            else
            {
                languagePair = xlWorkSheet.Cells[resultRange.Row, resultRange.Column+1].Text;
                //then you could handle how to display the row to the label according to resultRange
            }

            xlWorkBook.Close(false);
            xlApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);

            return languagePair;
        }

        public void CleanupResources()
        {
            Process cmd = new Process();
            cmd.StartInfo.WorkingDirectory = @"C:\Windows\System32";
            cmd.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Verb = "runas";

            cmd.StartInfo.Arguments = "/C taskkill /f /im winword.exe";
            cmd.Start();

            cmd.StartInfo.Arguments = "/C taskkill /f /im excel.exe";
            cmd.Start();
        }

        public bool FileIsSupported_PDF(string pdfFilePath)
        {
            const string functionName = "PDFIsSupported";
            if (!Path.GetExtension(pdfFilePath).ToUpper().StartsWith(".PDF"))
            {
                return true;
            }

            PdfReader pdf = new PdfReader(pdfFilePath);
            if (!pdf.IsOpenedWithFullPermissions)
            {
                Logger.Warn(functionName, pdfFilePath + " is not permissioned");
            }
            return pdf.IsOpenedWithFullPermissions;
        }
    }
}
