using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ExToString
{
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        try
    //        {
    //            RunCode(false);

    //        }
    //        catch (Exception ex)
    //        {
    //            var data = ex.ToFullExceptionString();
    //            Console.WriteLine(data);
    //        }

    //        try
    //        {
    //            RunCode(true);

    //        }
    //        catch (Exception ex)
    //        {
    //            var data = ex.ToFullExceptionString();
    //            Console.WriteLine(data);
    //        }

    //        Console.ReadLine();
    //    }

    //    private static void RunCode(bool innerWithData)
    //    {
    //        try
    //        {
    //            if (innerWithData)
    //            {
    //                RunInnerCodeWithData();
    //            }
    //            else
    //            {
    //                RunInnerCode();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            var i = new Exception("inner2", ex);
    //            i.Data.Add("Inner data", 7);
    //            var e = new InvalidOperationException("outer message", new Exception("inner1", i));
    //            e.Data.Add("Some data", 5);
    //            throw e;
    //        }
    //    }

    //    private static void RunInnerCode()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    private static void RunInnerCodeWithData()
    //    {
    //        var v = new NotImplementedException();
    //        v.Data.Add("Some more inner data", 54);
    //        throw v;
    //    }
    //}

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

        private static String ToFullExceptionStringOld(this Exception exception, int innerDepthCount)
        {
            try
            {
                if (exception == null)
                {
                    return String.Empty;
                }

                var indent = innerDepthCount > 0 ? new String(' ', 4 * innerDepthCount) : String.Empty;

                String exDataString = null;
                if (exception.Data != null && exception.Data.Count > 0)
                {
                    var exData = new StringBuilder();
                    foreach (DictionaryEntry keyPair in exception.Data)
                    {
                        exData.AppendLine(String.Format("{2}{0} - {1}", keyPair.Key, keyPair.Value, indent));
                    }
                    exDataString = exData.ToString();
                }

                var tabbedStackTrace = exception.StackTrace != null
                           ? ((innerDepthCount > 0)
                                ? exception.StackTrace.Replace(Environment.NewLine, Environment.NewLine + indent)
                                : exception.StackTrace)
                           : null;

                if (exception.InnerException == null)
                {
                    if (String.IsNullOrEmpty(exDataString))
                    {
                        return String.Format("{3}{0}: {1}\n\n{3}[Stack:\n{3}{2}\n{3}]", exception.GetType().FullName,
                                             exception.Message, tabbedStackTrace, indent);
                    }
                    else
                    {
                        return String.Format("{4}{0}: {1}\n\n{4}Data:\n{2}\n{4}[Stack:\n{4}{3}\n{4}]", exception.GetType().FullName,
                                             exception.Message, exDataString, tabbedStackTrace, indent);
                    }
                }
                else
                {
                    String innerExceptionString = exception.InnerException.ToFullExceptionString(innerDepthCount + 1);

                    if (String.IsNullOrEmpty(exDataString))
                    {
                        return String.Format("{5}{0}: {1}\n\n{5}[Stack:\n{5}{2}\n{5}]\n\n{5}(Inner-{3}:\n{4}\n{5})", exception.GetType().FullName,
                                             exception.Message,
                                             tabbedStackTrace, innerDepthCount,
                                            innerExceptionString,
                                            indent);
                    }
                    else
                    {
                        return String.Format("{6}{0}: {1}\n\n{6}Data:\n{2}\n{6}[Stack:\n{6}{3}\n{6}]\n\n{6}(Inner-{4}:\n{5}\n{6})", exception.GetType().FullName,
                         exception.Message,
                         exDataString,
                         tabbedStackTrace,
                         innerDepthCount,
                         innerExceptionString,
                         indent);
                    }
                }
            }
            // This method will be used in exception handling and must not error.
            catch (Exception ex)
            {
                return "UNABLE TO GET FULL EXCEPTION TRACE.";
            }
        }
    }
}
