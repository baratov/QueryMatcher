using System;
using BenchmarkDotNet.Running;

namespace QueryMatcherBenchmark
{ 
    public class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Benchmark>();
            Console.WriteLine(summary);
        }
    }
}
