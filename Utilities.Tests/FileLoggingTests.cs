using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SODA.Utilities.Tests.Mocks;

namespace SODA.Utilities.Tests
{
    [TestFixture]
    public class FileLoggingTests
    {
        [SetUp]
        public void TestInitialize()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }

        [TearDown]
        public void TestCleanUp()
        {
            if (File.Exists(FileMocks.FileThatDoesNotExist()))
                File.Delete(FileMocks.FileThatDoesNotExist());
        }

        [Test]
        [Category("SimpleFileLogger")]
        public void Default_New_Uses_DefaultLogFile_And_DefaultMaxLogSize()
        {
            Assert.False(File.Exists(SimpleFileLogger.DefaultLogFile));

            using (var fileLogger = new SimpleFileLogger())
            {
                Assert.True(File.Exists(SimpleFileLogger.DefaultLogFile));
            }

            File.Delete(SimpleFileLogger.DefaultLogFile);
        }

        [TestCase(StringMocks.NullInput)]
        [TestCase(StringMocks.EmptyInput)]
        //[ExpectedException(typeof(ArgumentNullException))]
        [Category("SimpleFileLogger")]
        public void New_With_Empty_LogFilePath_Throws_ArgumentNullException(string input)
        {
            Assert.That(() => new SimpleFileLogger(input), Throws.TypeOf<ArgumentNullException>());
        }

        [TestCase(-1)]
        [TestCase(0)]
        //[ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Category("SimpleFileLogger")]
        public void New_With_NonPositive_MaxLogBytes_Throws_ArgumentOutOfRangeException(int input)
        {
            Assert.That(() => new SimpleFileLogger(input), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [Category("SimpleFileLogger")]
        public void New_Creates_NonExistent_File()
        {
            Assert.False(File.Exists(FileMocks.FileThatDoesNotExist()));

            using (var logger = new SimpleFileLogger(FileMocks.FileThatDoesNotExist()))
            {
                Assert.True(File.Exists(FileMocks.FileThatDoesNotExist()));
            }
        }

        [Test]
        [Category("SimpleFileLogger")]
        public void New_ReThrows_Target_File_Exceptions()
        {
            //make sure the target file exists
            File.WriteAllText(FileMocks.FileThatDoesNotExist(), String.Empty);

            //open the target file in exclusive mode
            using (var exclusive = File.Open(FileMocks.FileThatDoesNotExist(), FileMode.Open, FileAccess.Read, FileShare.None))
            {
                Assert.That(
                    () => {
                        using (var shouldFail = new SimpleFileLogger(FileMocks.FileThatDoesNotExist()))
                        {
                            //no-op
                        }
                    },
                    Throws.InstanceOf<IOException>()
                );
            }
        }

        [Test]
        [Category("SimpleFileLogger")]
        public void New_Writes_Log_Header()
        {
            using (var logger = new SimpleFileLogger(FileMocks.FileThatDoesNotExist()))
            {
                //no-op
            }

            Assert.That(
                File.ReadAllText(FileMocks.FileThatDoesNotExist()),
                Does.Contain("Log Start").IgnoreCase
            );
        }

        [Test]
        [Category("SimpleFileLogger")]
        public void New_Rolls_Log_After_MaximumLogSize()
        {
            //create some test data that is just larger than the MaximumLogSize
            byte[] data = Enumerable.Range(1, SimpleFileLogger.DefaultMaximumLogBytes + 1)
                                    .Select(i => Convert.ToByte(i % 255))
                                    .ToArray();

            File.WriteAllBytes(FileMocks.FileThatDoesNotExist(), data);

            using (var logger = new SimpleFileLogger(FileMocks.FileThatDoesNotExist()))
            {
                //no-op
            }

            Assert.That(
                File.ReadAllText(FileMocks.FileThatDoesNotExist()),
                Does.Contain("Log Rollover").IgnoreCase
            );

            Assert.That(
                File.ReadAllBytes(FileMocks.FileThatDoesNotExist()).LongLength,
                Is.LessThan(SimpleFileLogger.DefaultMaximumLogBytes + 1)
            );
        }

        [Test]
        [Category("SimpleFileLogger")]
        public void Writes_Blank_Line_To_File()
        {
            using (var logger = new SimpleFileLogger(FileMocks.FileThatDoesNotExist()))
            {
                logger.WriteLine();
            }

            Assert.That(
                File.ReadAllLines(FileMocks.FileThatDoesNotExist()).Length,
                Is.EqualTo(2)
            );
        }

        [Test]
        [Category("SimpleFileLogger")]
        public void Writes_Simple_Message_To_File()
        {
            string message = "This is a log message";

            using (var logger = new SimpleFileLogger(FileMocks.FileThatDoesNotExist()))
            {
                logger.WriteLine(message);
            }

            Assert.That(
                File.ReadAllText(FileMocks.FileThatDoesNotExist()),
                Does.Contain(message)
            );
        }

        [Test]
        [Category("SimpleFileLogger")]
        public void Writes_Format_Message_To_File()
        {
            string format = "This is a log format with 2 params {0} {1}";
            string param1 = "param1";
            string param2 = "param2";

            using (var logger = new SimpleFileLogger(FileMocks.FileThatDoesNotExist()))
            {
                logger.WriteLine(format, param1, param2);
            }

            Assert.That(
                File.ReadAllText(FileMocks.FileThatDoesNotExist()),
                Does.Contain(param1).And.Contains(param2)
            );
        }

        [Test]
        [Category("SimpleFileLogger")]
        public void Writes_Exception_To_File()
        {
            Exception ex = null;

            using (var logger = new SimpleFileLogger(FileMocks.FileThatDoesNotExist()))
            {
                try
                {
                    throw new Exception("This is the exception message");
                }
                catch (Exception caught)
                {
                    ex = caught;
                }

                logger.Write("Exception occured!", ex);
            }

            Assert.That(
                File.ReadAllText(FileMocks.FileThatDoesNotExist()),
                Does.Contain(ex.Message).And.Contains(ex.StackTrace)
            );
        }

        [Test]
        [Category("SimpleFileLogger")]
        public void Writes_InnerException_To_File()
        {
            Exception outer = null;
            Exception inner = null;

            using (var logger = new SimpleFileLogger(FileMocks.FileThatDoesNotExist()))
            {
                try
                {
                    throw new Exception("This is the inner exception message");
                }
                catch (Exception caught)
                {
                    inner = caught;
                }

                try
                {
                    throw new Exception("This is the outer exception message", inner);
                }
                catch (Exception caught)
                {
                    outer = caught;
                }

                logger.Write("Exception occured!", outer);
            }

            Assert.That(
                File.ReadAllText(FileMocks.FileThatDoesNotExist()),
                Does.Contain(inner.Message).And.Contains(inner.StackTrace)
            );
        }
    }
}