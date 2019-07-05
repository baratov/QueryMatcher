using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using QueryMatcher;

namespace QueryMatcherBenchmark
{
    [RankColumn]
    [WarmupCount(3)]
    [IterationCount(10)]
    public class Benchmark
    {
        private IEnumerable<IEnumerable<string>> _records;
        private IEnumerable<ISet<string>> _queries;

        [Params(1)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            string line;
            var records = new List<IEnumerable<string>>();
            var queries = new List<ISet<string>>();

            using (var file = new StreamReader("records.txt"))
            {
                while ((line = file.ReadLine()) != null)
                {
                    records.Add(line.Split(","));
                }

            }
            _records = records;

            using (var file = new StreamReader("queries.txt"))
            {
                while ((line = file.ReadLine()) != null)
                {
                    queries.Add(new HashSet<string>(line.Split(",")));
                }
            }
            _queries = queries;
        }

        [Benchmark]
        public void FastMatcher()
        {
            // Serialization added to make sure that the result completely evaluated
            JsonConvert.SerializeObject(new FastMatcher().Match(_records, _queries));
        }

        [Benchmark]
        public void SimpleMatcher()
        {
            // Serialization added to make sure that the result completely evaluated
            JsonConvert.SerializeObject(new SimpleMatcher().Match(_records, _queries));
        }

        [Benchmark]
        public void LinqMatcher()
        {
            // Serialization added to make sure that the result completely evaluated
            JsonConvert.SerializeObject(new LinqMatcher().Match(_records, _queries));
        }
    }
}
