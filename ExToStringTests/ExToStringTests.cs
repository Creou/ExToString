using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExToString;

namespace ExToStringTests
{
    [TestClass]
    public class ExToStringTests
    {
        private const String Data_TestMessage = "TESTMESSAGE";

        private const String Data_StackTraceMethod = "ThrowSimpleException";

        private const String Data_DataKey1 = "TestDataKey1";
        private const String Data_DataValue1 = "TestDataValue1";
        private const String Data_DataKey2 = "TestDataKey2";
        private const String Data_DataValue2 = "TestDataValue2";

        private const String Data_InnerMessage = "INNERMESSAGE";

        [TestMethod]
        public void TestNullException()
        {
            Exception ex = null;
            String exString = ex.ToFullExceptionString();

            Assert.IsTrue(exString == String.Empty, "A null exception should return an empty string.");
        }

        [TestMethod]
        public void TestSimpleException()
        {
            Exception ex = new Exception(Data_TestMessage);
            String exString = ex.ToFullExceptionString();

            Assert.IsTrue(exString.Contains(Data_TestMessage), "Exception string must include Data_TestMessage.");
        }

        [TestMethod]
        public void TestThrownExceptionStackTrace()
        {
            String exString = null;
            try
            {
                ThrowSimpleException(Data_TestMessage);
            }
            catch (Exception ex)
            {
                exString = ex.ToFullExceptionString();
            }

            Assert.IsTrue(exString.Contains(Data_StackTraceMethod), "Exception string must include Data_StackTraceMethod.");
        }

        [TestMethod]
        public void TestThrownExceptionData()
        {
            String exString = null;
            try
            {
                ThrowExceptionWithData(Data_TestMessage);
            }
            catch (Exception ex)
            {
                exString = ex.ToFullExceptionString();
            }

            Assert.IsTrue(exString.Contains(Data_DataKey1), "Exception string must include Data_DataKey1.");
            Assert.IsTrue(exString.Contains(Data_DataValue1), "Exception string must include Data_DataValue1.");
            Assert.IsTrue(exString.Contains(Data_DataKey2), "Exception string must include Data_DataKey2.");
            Assert.IsTrue(exString.Contains(Data_DataValue2), "Exception string must include Data_DataValue2.");
        }

        [TestMethod]
        public void TestThrownExceptionInner()
        {
            String exString = null;
            try
            {
                ThrowExceptionWithInner(Data_TestMessage, Data_InnerMessage);
            }
            catch (Exception ex)
            {
                exString = ex.ToFullExceptionString();
            }

            Assert.IsTrue(exString.Contains(Data_InnerMessage), "Exception string must include Data_InnerMessage.");
        }

        private static void ThrowSimpleException(String message)
        {
            throw new Exception(message);
        }

        private static void ThrowExceptionWithData(String message)
        {
            var ex = new Exception(message);
            ex.Data.Add(Data_DataKey1, Data_DataValue1);
            ex.Data.Add(Data_DataKey2, Data_DataValue2);

            throw ex;
        }

        private static void ThrowExceptionWithInner(String message, String innerMessage)
        {
            try
            {
                ThrowExceptionWithData(innerMessage);
            }
            catch (Exception inner)
            {
                var ex = new Exception(message, inner);
                ex.Data.Add(Data_DataKey1, Data_DataValue1);
                ex.Data.Add(Data_DataKey2, Data_DataValue2);

                throw ex;
            }
        }
    }
}
