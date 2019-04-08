using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iTextSharp.text.pdf;
using iTextSharp.text.exceptions;

namespace FileManager
{
    [TestClass]
    public class FileManagerUnitTests
    {
        [TestMethod]
        public void PDFIsPasswordProtected()
        {
            string pdfFilePath = @"D:\TranslationFiles\zTest\175\Source\Dex Ac 2018 stab.EU.pdf";
            try
            {
                PdfReader pdf = new PdfReader(pdfFilePath);
                //Assert.Fail();
            }
            catch(BadPasswordException ex)
            {
                //Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void PDFIsOpenedWithFullPermissions()
        {
            string pdfFilePath = @"D:\TranslationFiles\zTest\175\Source\Dex Ac 2018 stab.EU.pdf";
            PdfReader pdf = new PdfReader(pdfFilePath);
            Assert.AreEqual(true, pdf.IsOpenedWithFullPermissions);
        }
    }
}
