using DeveImageOptimizerWPF.ViewModel.ObservableData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DeveImageOptimizerWPF.Tests.ViewModel.ObservableData
{
    public class LoggerExtractinatorTests
    {
        [Fact]
        public void LogsCorrectlyAndStopsLoggingLater()
        {
            Console.WriteLine("Before");
            var instance = LoggerExtractinator.CreateLoggerExtractinatorAndSetupConsoleRedirection();
            Console.WriteLine("During");
            Thread.Sleep(100);
            instance.DestroyAndRevertConsoleOut();
            Console.WriteLine("After");

            Thread.Sleep(100);
            Assert.Single(instance.LogLines);
            Assert.Single(instance.LogLinesEntry);

            var theLine = instance.LogLines.Single();
            var theEntry = instance.LogLinesEntry.Single();

            Assert.Contains("During", theLine);
            Assert.Equal("During", theEntry.Message);
        }
    }
}
