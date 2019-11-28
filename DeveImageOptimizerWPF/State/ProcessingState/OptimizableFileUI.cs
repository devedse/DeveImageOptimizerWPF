using DeveCoolLib.Collections;
using DeveImageOptimizer.State;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;

namespace DeveImageOptimizerWPF.State.ProcessingState
{
    [AddINotifyPropertyChangedInterface]
    public class OptimizableFileUI
    {
        public string Path { get; set; }
        public string RelativePath { get; set; }

        public OptimizationResult OptimizationResult { get; set; } = OptimizationResult.InProgress;

        public long OriginalSize { get; set; }
        public long OptimizedSize { get; set; }

        public TimeSpan Duration { get; set; } = TimeSpan.Zero;

        public ObservableCollection<string> Errors { get; } = new ObservableCollection<string>();

        public OptimizableFileUI(OptimizableFile optimizableFile)
        {
            Set(optimizableFile);
        }

        public void Set(OptimizableFile optimizableFile)
        {
            Path = optimizableFile.Path;
            RelativePath = optimizableFile.RelativePath;

            OptimizationResult = optimizableFile.OptimizationResult;

            OriginalSize = optimizableFile.OriginalSize;
            OptimizedSize = optimizableFile.OptimizedSize;

            Duration = optimizableFile.Duration;

            ListSynchronizerV2.SynchronizeLists(optimizableFile.Errors, Errors);
        }
    }
}
