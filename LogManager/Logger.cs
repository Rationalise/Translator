using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace LogManager
{
    public class Logger
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void Fatal(string callSite, string log)
        {
            logger.Fatal(log);
        }
        public static void Fatal(string callSite, Exception ex)
        {
            logger.Fatal(ex);
        }

        public static void Error(string callSite, string log)
        {
            logger.Error(log);
            Console.WriteLine(log);
        }
        public static void Error(string callSite, Exception ex)
        {
            logger.Error(ex);
            Console.WriteLine(ex);
        }

        public static void Warn(string callSite, string log)
        {
            logger.Warn(log);
            Console.WriteLine(log);
        }
        public static void Warn(string callSite, Exception ex)
        {
            logger.Fatal(ex);
            Console.WriteLine(ex);
        }

        public static void Info(string callSite, string log)
        {
            logger.Warn(log);
            Console.WriteLine(log);
        }
        public static void Info(string callSite, Exception ex)
        {
            logger.Fatal(ex);
            Console.WriteLine(ex);
        }

        public static void Debug(string callSite, string log)
        {
            logger.Debug(log);
            Console.WriteLine(log);
        }
        public static void Debug(string callSite, Exception ex)
        {
            logger.Fatal(ex);
            Console.WriteLine(ex);
        }

        public static void Trace(string callSite, string log)
        {
            logger.Trace(log);
            Console.WriteLine(log);
        }
        public static void Trace(string callSite, Exception ex)
        {
            logger.Fatal(ex);
            Console.WriteLine(ex);
        }
    }
}
