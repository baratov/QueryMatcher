using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QueryMatcher;

namespace QueryMatcherConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var argsAreProvided = args.Count() == 2;
            var recordsPath = argsAreProvided ? args[0] : "https://s3.amazonaws.com/idt-code-challenge/records.txt";
            var queriesPath = argsAreProvided ? args[1] : "https://s3.amazonaws.com/idt-code-challenge/queries.txt";

            var records = new List<IEnumerable<string>>();
            var queries = new List<ISet<string>>();

            using (var client = new HttpClient())
            {
                Console.WriteLine("Downloading records");
                using (var response = await client.GetAsync(recordsPath))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        using (var reader = new StreamReader(stream))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                records.Add(line.Split(","));
                            }

                        }
                    }
                }

                Console.WriteLine("Downloading queries");
                using (var response = await client.GetAsync(queriesPath))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        using (var reader = new StreamReader(stream))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                queries.Add(new HashSet<string>(line.Split(",")));
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Matching");
            var result = new SimpleMatcher().Match(records, queries).ToList();

            for (var i = 0; i < queries.Count; i++)
            {
                Console.WriteLine("=========================================================");
                Console.WriteLine($"Query: {JsonConvert.SerializeObject(queries[i])}");
                foreach (var matchedRecord in result[i])
                {
                    Console.WriteLine($"Matched record result: {JsonConvert.SerializeObject(matchedRecord)}");
                }
            }
        }        
    }
}
