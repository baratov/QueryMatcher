using System.Collections.Generic;
using System.Linq;

namespace QueryMatcher
{
    public class FastMatcher : IQueryMatcher
    {
        public IEnumerable<IEnumerable<IDictionary<string, int>>> Match(IEnumerable<IEnumerable<string>> records, IEnumerable<ISet<string>> queries)
        {
            var recordsList = records.ToList();

            var wordToRecordsMap = new Dictionary<string, ISet<int>>();
            var wordToCountMaps = new List<IDictionary<string, int>>();

            for (var i = 0; i < recordsList.Count; i++)
            {
                var wordToCountMap = new Dictionary<string, int>();

                foreach (var word in recordsList[i])
                {
                    if (wordToRecordsMap.ContainsKey(word))
                        wordToRecordsMap[word].Add(i);
                    else
                        wordToRecordsMap.Add(word, new SortedSet<int> { i });

                    
                    if (wordToCountMap.ContainsKey(word))
                        wordToCountMap[word]++;
                    else
                        wordToCountMap.Add(word, 1);
                }

                wordToCountMaps.Add(wordToCountMap);
            }

            foreach (var query in queries)
            {
                var matchedRecords = new SortedSet<int>(Enumerable.Range(0, recordsList.Count));
                foreach (var word in query)
                {
                    matchedRecords.IntersectWith(wordToRecordsMap[word]);
                }

                var queryResult = new List<IDictionary<string, int>>();

                foreach (var record in matchedRecords)
                {
                    var dict = new Dictionary<string, int>();
                    foreach(var wordWithCount in wordToCountMaps[record])
                    {
                        if (!query.Contains(wordWithCount.Key))
                        {
                            dict.Add(wordWithCount.Key, wordWithCount.Value);
                        }
                    }

                    queryResult.Add(dict);
                }
                        
                yield return queryResult;
            }
        }
    }
}
