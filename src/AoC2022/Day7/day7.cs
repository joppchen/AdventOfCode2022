using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AoC2022.Day7
{
    internal static class Main
    {
        public static void Solve(int dayNumber)
        {
            Console.WriteLine($"Day {dayNumber}:");

            var filename = "day" + Convert.ToString(dayNumber);

            // var textFile = $"../../../../../resources/{filename}.txt";
            var textFile = $"../../../../../resources/{filename}_example.txt";

            if (File.Exists(textFile))
            {
                var terminalLines = File.ReadLines(textFile).ToList();

                //INode currentDir = new ElfDir("current", null, null);
                INode root = GenerateFileTreeMockup();
                INode rootGenerated = GenerateFileTreeFromTerminalOutput(terminalLines);

                // Test: Find all directories in a given directory
                var subDirs = ListDirsInDir(root);
                var allSubDirs = ListAllDirs(root);
                var filesInDir = ListFilesInDir(root);
                var allFiles = ListAllFiles(root);
                var totalSize = filesInDir.Sum(x => x.Size);

                var goodCandidatesForDeletion =
                    allSubDirs.Where(d => ListFilesInDir(d).Sum(f => f.Size) <= 100000).ToList();
                var totalSizeOnlyCountingImmediateFiles =
                    goodCandidatesForDeletion.Sum(d => ListFilesInDir(d).Sum(f => f.Size));
                var totalSizeCountingAllSubFiles = goodCandidatesForDeletion.Sum(d =>
                    ListAllDirs(d).Sum(dr => ListFilesInDir(dr).Sum(f => f.Size)));


                // Task 1
                Console.WriteLine("TASK 1");
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var result = 1337; // Answer:
                watch.Stop();
                Console.WriteLine($"Task 1: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");

                // Console.WriteLine("");
                // Console.WriteLine("TASK 2");
                // watch = System.Diagnostics.Stopwatch.StartNew();
                //
                // result = 1337; // Answer:
                // watch.Stop();
                // Console.WriteLine($"Task 2: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");
            }
            else
            {
                Console.WriteLine($"File not found: '{textFile}'");
            }
        }

        private static INode GenerateFileTreeFromTerminalOutput(List<string> terminalLines)
        {
            string[] commands = {"cd", "ls"};
            string[] dirOperations = {"directory name (go to named child)", "/ (root folder)", ".. (go to parent)"};
            string[] listResults = {"123 abs", "dir xyz"};
            
            INode root = ElfDir.Create("/", null, new List<INode>());
            INode currentDir;

            foreach (var line in terminalLines)
            {
                if (IsCommand(line))
                {
                    if (Regex.Match(line, "cd").Success)
                    {
                        if (line.Contains("/"))
                        {
                            // Already done further up
                            // INode currentDir = root;
                        }

                        if (line.Contains(".."))
                        {
                            
                        }
                        else
                        {
                            // step into
                        }
                    }
                }
            }

            throw new NotImplementedException();
        }

        private static INode GenerateFileTreeMockup()
        {
            INode root = ElfDir.Create("/", null, new List<INode>());
            //cd /
            INode currentDir = root;
            // ls --> for each item until next command, add to parents child list
            INode aDir = ElfDir.Create("a", root, new List<INode>());
            currentDir.Children.Add(aDir);
            INode bFile = ElfFile.Create("b.txt", root, 14848514);
            currentDir.Children.Add(bFile);
            INode cFile = ElfFile.Create("c.dat", root, 8504156);
            currentDir.Children.Add(cFile);
            INode dDir = ElfDir.Create("d", root, new List<INode>());
            currentDir.Children.Add(dDir);
            // cd a
            currentDir = aDir;
            // ls
            INode eDir = ElfDir.Create("e", aDir, new List<INode>());
            currentDir.Children.Add(eDir);
            INode fFile = ElfFile.Create("f", aDir, 29116);
            currentDir.Children.Add(fFile);
            INode gFile = ElfFile.Create("g", aDir, 2557);
            currentDir.Children.Add(gFile);
            INode hFile = ElfFile.Create("h.lst", aDir, 62596);
            currentDir.Children.Add(hFile);
            // cd e
            currentDir = eDir;
            // ls
            INode iFile = ElfFile.Create("i", eDir, 584);
            currentDir.Children.Add(iFile);
            // cd ..
            currentDir = currentDir.Parent;
            // cd ..
            currentDir = currentDir.Parent;
            // cd d
            currentDir = dDir;
            // ls
            var jFile = ElfFile.Create("j", dDir, 4060174);
            currentDir.Children.Add(jFile);
            var dLogFile = ElfFile.Create("d.log", dDir, 8033020);
            currentDir.Children.Add(dLogFile);
            var dExtFile = ElfFile.Create("d.ext", dDir, 5626152);
            currentDir.Children.Add(dExtFile);
            var kFile = ElfFile.Create("k", dDir, 7214296);
            currentDir.Children.Add(kFile);
            return root;
        }

        private static IEnumerable<INode> ListDirsInDir(INode dir)
        {
            // return dir.Children.FindAll(n => n.Size == 0);
            return dir.Children.FindAll(n => n is ElfDir);
        }

        private static IEnumerable<INode> ListFilesInDir(INode dir)
        {
            return dir.Children.FindAll(n => n is ElfFile);
        }

        private static IEnumerable<INode> ListAllDirs(INode dir)
        {
            var accumulatedSubDirs = new List<INode> {dir};
            var subDirs = ListDirsInDir(dir).ToList();
            if (!subDirs.Any()) return accumulatedSubDirs;

            foreach (INode subDir in subDirs)
            {
                accumulatedSubDirs = accumulatedSubDirs.Concat(ListAllDirs(subDir)).ToList();
            }

            return accumulatedSubDirs;
        }

        private static IEnumerable<INode> ListAllFiles(INode dir)
        {
            var accumulatedSubDirs = new List<INode> {dir};
            var subDirs = ListDirsInDir(dir).ToList();
            if (!subDirs.Any()) return accumulatedSubDirs;

            foreach (INode subDir in subDirs)
            {
                accumulatedSubDirs = accumulatedSubDirs.Concat(ListAllDirs(subDir)).ToList();
            }

            return accumulatedSubDirs;
        }

        private static bool IsCommand(string line)
        {
            return line[0] == '$';
        }

        internal static void ParseCommandLine(string line)
        {
            // ls, cd
        }
    }

    internal interface INode
    {
        string Name { get; }
        long Size { get; }
        INode Parent { get; }
        List<INode> Children { get; }
    }

    internal class ElfDir : INode
    {
        public string Name { get; }
        public long Size { get; }
        public INode Parent { get; }
        public List<INode> Children { get; }

        private ElfDir(string name, INode parent, List<INode> children, long size)
        {
            Name = name;
            Parent = parent;
            Children = children;
            Size = size;
        }

        public static ElfDir Create(string name, INode parent, List<INode> children)
        {
            return new ElfDir(name, parent, children, 0);
        }
    }

    internal class ElfFile : INode
    {
        public string Name { get; }
        public INode Parent { get; }
        public List<INode> Children { get; }
        public long Size { get; }

        private ElfFile(string name, INode parent, List<INode> children, long size)
        {
            Name = name;
            Parent = parent;
            Children = children;
            Size = size;
        }

        public static INode Create(string name, INode parent, long size)
        {
            return new ElfFile(name, parent, null, size);
        }
    }
}