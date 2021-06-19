﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Wordladder
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = new Arguments(args);
            var start = arguments.Start;
            var end = arguments.End;
            var dictionary = new WordDictionaryReader(arguments.DictionaryFilePath) //SRP
                .LoadDictionary(Settings.MaxLength);

            new ArgumentValidator().Validate(arguments, dictionary); //SRP

            var finder = new WordFinderFactory().CreateWordFinder(); //SRP
            var watch = new Stopwatch();

            watch.Start();

            Console.Write($"Searching between «{start}» and «{end}»...");

            var winner = finder.Find(arguments.ToSearch(dictionary));
            
            if (winner != null)
                //OCP
                new OutputWriter(winner)
                    .WriteOutput(CreateWriters(arguments.OutputFilePath));
            else
                Console.WriteLine("Sorry but the target word could reached!");

            Console.WriteLine();
            Console.WriteLine($"It took {watch.Elapsed} to fulfill the search...");
        }

        private static IEnumerable<IWordOutputWriter> CreateWriters(string outputFilePath)
        {
            return new List<IWordOutputWriter>
            {
                new StdOutWordChainWriter(),
                new FileWordOutputWriter(outputFilePath)
            };
        }
    }
}