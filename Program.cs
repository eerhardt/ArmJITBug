using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.IO;

namespace TestIt
{
    class Program
    {
        static void Main(string[] args)
        {
            var lockFile = new LockFileFormat().Read(Path.Combine(AppContext.BaseDirectory, "dotnet.new.project.lock.json"));

            Console.WriteLine(new LockFileLookup(lockFile));
        }
    }

    internal class LockFileLookup
    {
        private readonly Dictionary<KeyValuePair<string, NuGetVersion>, LockFileLibrary> _packages;
        private readonly Dictionary<string, LockFileLibrary> _projects;

        public LockFileLookup(LockFile lockFile)
        {
            _packages = new Dictionary<KeyValuePair<string, NuGetVersion>, LockFileLibrary>();
            _projects = new Dictionary<string, LockFileLibrary>(StringComparer.OrdinalIgnoreCase);

            foreach (var library in lockFile.Libraries)
            {
                var libraryType = LibraryType.Parse(library.Type);

                if (libraryType == LibraryType.Package)
                {
                    _packages[new KeyValuePair<string, NuGetVersion>(library.Name, library.Version)] = library;
                }
                if (libraryType == LibraryType.Project)
                {
                    _projects[library.Name] = library;
                }
            }
        }
    }
}
