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

                // Type and message
                StringBuilder exceptionMessage = new StringBuilder();
                exceptionMessage.Append(indent);
                exceptionMessage.AppendLine(exception.GetType().FullName);
                exceptionMessage.AppendLine();
                exceptionMessage.Append(indent);
                exceptionMessage.Append(" Message: ");
                exceptionMessage.Append(exception.Message);

                // Reflected properties
                var exType = exception.GetType();
                var properties = exType.GetProperties();
                //List<String> handled = new List<string>(new String[] { "Data", "HelpLink", "HResult", "InnerException", "Message", "Source", "StackTrace", "TargetSite" });
                List<String> handled = new List<string>(new String[] { "Data", "InnerException", "Message", "StackTrace" });
                var propertiesToDisplay = properties.Where(prop => !handled.Contains(prop.Name));
                if (propertiesToDisplay.Count() > 0)
                {
                    exceptionMessage.AppendLine();
                    foreach (var property in propertiesToDisplay)
                    {
                        var value = property.GetValue(exception, null);
                        exceptionMessage.AppendFormat("\n{2} {0}: {1}", property.Name, value, indent);
                    }
                }

                // Data
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

                    exceptionMessage.AppendFormat("\n\n{1} Data: {0}", exDataString, indent);
                }

                // Stack trace
                var tabbedStackTrace = exception.StackTrace != null
                           ? ((innerDepthCount > 0)
                                ? indent + exception.StackTrace.Replace(Environment.NewLine, Environment.NewLine + indent)
                                : exception.StackTrace)
                           : null;

                if (!String.IsNullOrEmpty(tabbedStackTrace))
                {
                    exceptionMessage.AppendFormat("\n\n{1} Stack:\n{1} [\n{0}\n{1} ]", tabbedStackTrace, indent);
                }

                // Inner exception
                if (exception.InnerException != null)
                {
                    String innerExceptionString = exception.InnerException.ToFullExceptionString(innerDepthCount + 1);

                    exceptionMessage.AppendFormat("\n\n{2} Inner-{0}:\n{2} (\n{1}\n{2} )", innerDepthCount, innerExceptionString, indent);
                }

                return exceptionMessage.ToString();
            }
            // This method will be used in exception handling and must not error.
            catch (Exception ex)
            {
                return "UNABLE TO GET FULL EXCEPTION OUTPUT.\n\nDefault output:\n" + ex.ToString();
            }
        }
    }
}
