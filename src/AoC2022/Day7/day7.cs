using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2022.Day7
{
    internal static class Main
    {
        public static void Solve(int dayNumber)
        {
            Console.WriteLine($"Day {dayNumber}:");

            var filename = "day" + Convert.ToString(dayNumber);

            var textFile = $"../../../../../resources/{filename}.txt";
            //var textFile = $"../../../../../resources/{filename}_example.txt";

            const long totalDiskSpace = 70000000;
            const long minimumRequiredUnusedSpace = 30000000;

            if (File.Exists(textFile))
            {
                var terminalLines = File.ReadLines(textFile).ToList();

                INode root = GenerateFileTreeFromTerminalOutput(terminalLines);

                // Tests for structuring thoughts
                var immediateSubDirsInRoot = ImmediateSubDirs(root);
                var allDirsInclRoot = AllSubDirs(root);
                var immediateFilesInRoot = ImmediateFilesInDir(root);
                var sizeOfImmediateFilesInRoot = ImmediateFilesInDir(root).Sum(x => x.Size);
                var totalNumberOfFilesInRoot = AllFilesInDir(root);
                var sizeOfAllFilesInRoot = AllFilesInDir(root).Sum(f => f.Size);

                // Task 1
                Console.WriteLine("TASK 1");
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var goodCandidatesForDeletion = AllSubDirs(root)
                    .Where(dir => AllFilesInDir(dir).Sum(f => f.Size) <= 100000).ToList();
                var totalSizeCountingAllSubFiles = goodCandidatesForDeletion.Sum(d =>
                    AllSubDirs(d).Sum(dr => ImmediateFilesInDir(dr).Sum(f => f.Size)));
                var totalSizeAlternativeMethod =
                    goodCandidatesForDeletion.Sum(dir => AllFilesInDir(dir).Sum(f => f.Size));

                var result = totalSizeCountingAllSubFiles; // Answer: 1543140
                watch.Stop();
                Console.WriteLine($"Task 1: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");

                Console.WriteLine("");
                Console.WriteLine("TASK 2");
                watch = System.Diagnostics.Stopwatch.StartNew();
                var unusedSpace = totalDiskSpace - sizeOfAllFilesInRoot;
                var spaceNeededToBeFreedUp = minimumRequiredUnusedSpace - unusedSpace;
                // List all directories with size larger than or equal to needed space
                var candidateForDeletion =
                    allDirsInclRoot.Where(dir => AllFilesInDir(dir).Sum(f => f.Size) >= spaceNeededToBeFreedUp);
                var smallestCandidateSize = candidateForDeletion.Min(dir => AllFilesInDir(dir).Sum(f => f.Size));
                // If needed to select actual directory:
                //INode smallestCandidate = candidateForDeletion.Aggregate((min, x) => AllFilesInDir(x).Sum(f => f.Size) < AllFilesInDir(min).Sum(f => f.Size) ? x : min);

                result = smallestCandidateSize; // Answer: 1117448
                watch.Stop();
                Console.WriteLine($"Task 2: {result}. Elapsed time [ms]: {watch.ElapsedMilliseconds}");
            }
            else
            {
                Console.WriteLine($"File not found: '{textFile}'");
            }
        }

        private static IEnumerable<INode> AllFilesInDir(INode dir)
        {
            return AllSubDirs(dir).SelectMany(ImmediateFilesInDir).ToList();
        }

        private static INode GenerateFileTreeFromTerminalOutput(List<string> terminalLines)
        {
            INode root = ElfDir.Create("/", null, new List<INode>());
            INode currentDir = root;

            var lastCommand = "cd";

            foreach (var line in terminalLines)
            {
                if (IsCommand(line))
                {
                    if (Regex.Match(line, "cd").Success)
                    {
                        currentDir = ChangeDirectory(line, currentDir, root);
                        lastCommand = "cd";
                    }

                    if (Regex.Match(line, "ls").Success)
                    {
                        lastCommand = "ls";
                    }
                }
                else
                {
                    if (!lastCommand.Equals("ls")) continue;
                    var lineElements = line.Split(" ");
                    if (lineElements[0].Equals("dir"))
                    {
                        AddSubDir(line, currentDir);
                    }
                    else if (long.TryParse(lineElements[0], out var fileSize))
                    {
                        var fileName = lineElements[1];
                        INode newFile = ElfFile.Create(fileName, currentDir, fileSize);
                        currentDir.Children.Add(newFile);
                    }
                }
            }

            return root;
        }

        private static void AddSubDir(string line, INode currentDir)
        {
            var folderName = line.Split(" ")[1];
            INode newDir = ElfDir.Create(folderName, parent: currentDir, new List<INode>());
            currentDir.Children.Add(newDir);
        }

        private static INode ChangeDirectory(string line, INode currentDir, INode root)
        {
            if (line.Contains("/"))
            {
                //cd /
                currentDir = root;
            }
            else if (line.Contains(".."))
            {
                currentDir = currentDir.Parent;
            }
            else
            {
                // step into, here meaning add child directory
                var folderName = line.Split(" ")[2];
                currentDir = currentDir.Children.Find(d => d.Name.Equals(folderName));
            }

            return currentDir;
        }

        private static IEnumerable<INode> ImmediateSubDirs(INode dir)
        {
            return dir.Children.FindAll(n => n is ElfDir);
        }

        private static IEnumerable<INode> ImmediateFilesInDir(INode dir)
        {
            return dir.Children.FindAll(n => n is ElfFile);
        }

        private static IEnumerable<INode> AllSubDirs(INode dir)
        {
            var accumulatedSubDirs = new List<INode> {dir};
            var subDirs = ImmediateSubDirs(dir).ToList();
            if (!subDirs.Any()) return accumulatedSubDirs;

            foreach (INode subDir in subDirs)
            {
                accumulatedSubDirs = accumulatedSubDirs.Concat(AllSubDirs(subDir)).ToList();
            }

            return accumulatedSubDirs;
        }

        private static bool IsCommand(string line)
        {
            return line[0] == '$';
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