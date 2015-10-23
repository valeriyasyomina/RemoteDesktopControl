using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileLoggerLib
{
    interface IFileLogger
    {
        void WriteLogFile(string info);
    }
}
