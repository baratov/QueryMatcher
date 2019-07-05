using System.Collections.Generic;

namespace QueryMatcher
{
    public class SimpleMatcher : IQueryMatcher
    {
        public IEnumerable<IEnumerable<IDictionary<string, int>>> Match(IEnumerable<IEnumerable<string>> records, IEnumerable<ISet<string>> queries)
        {
            foreach (var query in queries)
            {
                var queryResult = new List<IDictionary<string, int>>();

                foreach (var record in records)
                {
                    if (new HashSet<string>(record).IsProperSupersetOf(query))
                    {
                        var result = new Dictionary<string, int>();
                        foreach (var item in record)
                        {
                            if (!query.Contains(item))
                            {
                                if (result.ContainsKey(item))
                                    result[item]++;
                                else
                                    result.Add(item, 1);
                            }
                        }

                        queryResult.Add(result);
                    }
                }

                yield return queryResult;
            }
        }
    }
}
