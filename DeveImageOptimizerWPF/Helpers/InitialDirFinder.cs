using System.IO;

namespace DeveImageOptimizerWPF.Helpers
{
    public static class InitialDirFinder
    {
        public static string FindStartingDirectoryBasedOnInput(string inputDir)
        {
            var startDir = inputDir;

            while (!Directory.Exists(startDir) && !string.IsNullOrWhiteSpace(startDir))
            {
                startDir = Path.GetDirectoryName(startDir);
            }

            return startDir;
        }
    }
}
