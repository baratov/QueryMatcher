using System.Collections.Generic;
using System.Linq;

namespace QueryMatcher
{
    public class LinqMatcher : IQueryMatcher
    {
        public IEnumerable<IEnumerable<IDictionary<string, int>>> Match(IEnumerable<IEnumerable<string>> records, IEnumerable<ISet<string>> queries)
        {
            return queries
                .Select(query => records
                    .Where(record => new HashSet<string>(record).IsProperSupersetOf(query))
                    .Select(record => record
                        .Where(item => !query.Contains(item))
                        .GroupBy(item => item)
                        .ToDictionary(x => x.Key, x => x.Count())));
        }
    }
}
