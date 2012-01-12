using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ExToString
{
    public static class ExceptionToString
    {
        public static String ToFullExceptionString(this Exception exception)
        {
            return exception.ToFullExceptionString(0);
        }

        private static String ToFullExceptionString(this Exception exception, int innerDepthCount)
        {
            try
            {
                if (exception == null)
                {
                    return String.Empty;
                }

                var indent = innerDepthCount > 0 ? new String(' ', 4 * innerDepthCount) : String.Empty;

                StringBuilder exceptionMessage = new StringBuilder();
                exceptionMessage.Append(indent);
                exceptionMessage.Append(exception.GetType().FullName);
                exceptionMessage.Append(": ");
                exceptionMessage.Append(exception.Message);

                String exDataString = null;
                if (exception.Data != null && exception.Data.Count > 0)
                {
                    var exData = new StringBuilder();
                    bool first = true;
                    foreach (DictionaryEntry keyPair in exception.Data)
                    {
                        if (first) { exData.AppendLine(); }
                        exData.Append(String.Format("{2}    {0} - {1}", keyPair.Key, keyPair.Value, indent));
                    }
                    exDataString = exData.ToString();

                    exceptionMessage.AppendFormat("\n\n{1}Data: {0}", exDataString, indent);
                }

                var tabbedStackTrace = exception.StackTrace != null
                           ? ((innerDepthCount > 0)
                                ? exception.StackTrace.Replace(Environment.NewLine, Environment.NewLine + indent)
                                : exception.StackTrace)
                           : null;

                if (!String.IsNullOrEmpty(tabbedStackTrace))
                {
                    exceptionMessage.AppendFormat("\n\n{1}[Stack:\n{1}{0}\n{1}]", tabbedStackTrace, indent);
                }

                if (exception.InnerException != null)
                {
                    String innerExceptionString = exception.InnerException.ToFullExceptionString(innerDepthCount + 1);

                    exceptionMessage.AppendFormat("\n\n{2}(Inner-{0}:\n{1}\n{2})", innerDepthCount, innerExceptionString, indent);
                }

                return exceptionMessage.ToString();
            }
            // This method will be used in exception handling and must not error.
            catch (Exception ex)
            {
                return "UNABLE TO GET FULL EXCEPTION TRACE.";
            }
        }
    }
}
