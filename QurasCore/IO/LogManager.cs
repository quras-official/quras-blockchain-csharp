using System;
using System.IO;

namespace Quras.IO.Logger
{
    public static class LogManager
    {
        public static void PrintErrorLogs(StreamWriter writer, Exception ex)
        {
            string nowDateTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
            writer.WriteLine(nowDateTime + ex.GetType());
            writer.WriteLine(ex.Message);
            writer.WriteLine(ex.StackTrace);
            if (ex is AggregateException ex2)
            {
                foreach (Exception inner in ex2.InnerExceptions)
                {
                    writer.WriteLine();
                    PrintErrorLogs(writer, inner);
                }
            }
            else if (ex.InnerException != null)
            {
                writer.WriteLine();
                PrintErrorLogs(writer, ex.InnerException);
            }
        }

        public static void WriteExceptionLogs(Exception ex)
        {
            using (FileStream fs = new FileStream("error.log", FileMode.Create, FileAccess.Write, FileShare.None))
            using (StreamWriter w = new StreamWriter(fs))
            {
                LogManager.PrintErrorLogs(w, ex);
            }
        }

        public static void WriteString(string str)
        {
            using (FileStream fs = new FileStream("note.log", FileMode.Append, FileAccess.Write, FileShare.None))
            using (StreamWriter w = new StreamWriter(fs))
            {
                w.WriteLine(str);
            }
        }
    }
}
