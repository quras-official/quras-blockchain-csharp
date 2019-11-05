using System;
using System.IO;

namespace Quras_gui_SP.Global
{
    public static class LogManager
    {
        public static void PrintErrorLogs(StreamWriter writer, Exception ex)
        {
            writer.WriteLine(ex.GetType());
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
    }
}
