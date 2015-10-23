using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileLoggerLib
{
    public enum LoggerType { SERVER, CLIENT };
    public class FileLogger : IFileLogger
    {
        private string fileName;
        private LoggerType loggerType;
      

        public FileLogger(string fName, LoggerType lType)
        {
            fileName = fName;
            loggerType = lType;

           
            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            fileStream.Close();
        }

        public void WriteLogFile(string info)
        {
            String dateTimeInfo = DateTime.Now.Day.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Year.ToString() + " " +
                DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Second.ToString() + " >>> ";

            FileStream fileStream = new FileStream(fileName, FileMode.Append); //создаем файловый поток для дозаписи в конец файла
            StreamWriter writer = new StreamWriter(fileStream); //создаем «потоковый писатель» и связываем его с файловым потоком 
            
            if (loggerType == LoggerType.SERVER)
            {               
                writer.Write(dateTimeInfo + "SERVER >>> " + info); //записываем в файл                
            }
            else if (loggerType == LoggerType.CLIENT)
            {
                writer.Write(dateTimeInfo + "CLIENT >>> " + info); //записываем в файл
            }
            writer.Close();
        }
    }
}
