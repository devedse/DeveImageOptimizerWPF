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
            int waitedXTimes = 0;
            while(!instance.LogLines.Any())
            {
                waitedXTimes++;
                Thread.Sleep(10);
            }
            instance.DestroyAndRevertConsoleOut();
            Console.WriteLine($"After: {waitedXTimes}");

            Assert.Single(instance.LogLines);
            Assert.Single(instance.LogLinesEntry);

            var theLine = instance.LogLines.Single();
            var theEntry = instance.LogLinesEntry.Single();

            Assert.Contains("During", theLine);
            Assert.Equal("During", theEntry.Message);
        }
    }
}
