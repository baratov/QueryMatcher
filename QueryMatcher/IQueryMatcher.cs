using System.Collections.Generic;

namespace QueryMatcher
{
    public interface IQueryMatcher
    {
        // Result dictionary per matching record per query
        IEnumerable<IEnumerable<IDictionary<string, int>>> Match(IEnumerable<IEnumerable<string>> records, IEnumerable<ISet<string>> queries);
    }
}
