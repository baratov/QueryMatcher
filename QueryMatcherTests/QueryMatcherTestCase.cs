using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace QueryMatcherTests
{
    public class QueryMatcherTestCase
    {
        public IEnumerable<IEnumerable<string>> Records { get; set; }
        public IEnumerable<ISet<string>> Queries { get; set; }
        public IEnumerable<IEnumerable<IDictionary<string, int>>> ExpectedResult { get; set; }

        public void AssertResult(IEnumerable<IEnumerable<IDictionary<string, int>>> actualResult)
        {
            var expected = ExpectedResult.ToList();
            var actual = actualResult.ToList();

            Assert.Equal(expected.Count, actual.Count);

            for (var i = 0; i < expected.Count; i++)
            {
                var innerExpected = expected[i].ToList();
                var innerActual = actual[i].ToList();

                Assert.Equal(innerExpected.Count, innerActual.Count);

                for (var j = 0; j < innerExpected.Count; j++)
                {
                    var expectedDict = innerExpected[j];
                    var actualDict = innerActual[j];

                    Assert.Equal(expectedDict.Keys, actualDict.Keys);

                    foreach (var key in expectedDict.Keys)
                    {
                        Assert.Equal(expectedDict[key], actualDict[key]);
                    }
                }
            }
        }
    }
}