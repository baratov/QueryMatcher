using System.Collections.Generic;
using Xunit;
using QueryMatcher;
using System;

namespace QueryMatcherTests
{
    public class QuieryMatcherTests
    {
        public static IEnumerable<object[]> TestCases
        {
            get
            {
                yield return new object[]
                {
                    new QueryMatcherTestCase
                    {
                        Records = new List<IEnumerable<string>>
                        {
                            new List<string>{ "red", "sky", "coin", "bucket", "chair", "blue" },
                            new List<string>{ "apple", "chair", "purple", "red", "house" },
                            new List<string>{ "silver","blue","apple","coin","street" },
                        },
                        Queries = new List<ISet<string>>
                        {
                            new HashSet<string> { "red", "apple" }
                        },
                        ExpectedResult = new List<IEnumerable<IDictionary<string, int>>>
                        {
                            new List<IDictionary<string, int>>
                            {
                                new Dictionary<string, int>
                                {
                                    { "chair", 1 },
                                    { "purple", 1 },
                                    { "house", 1 }
                                },
                            }
                        }
                    },
                };
            }
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void MatchersShouldMatchRecords(QueryMatcherTestCase testCase)
        {
            var matchers = new List<Type>
            {
                typeof(LinqMatcher),
                typeof(SimpleMatcher),
                typeof(FastMatcher)
            };

            foreach (var matcher in matchers)
            {
                var result = ((IQueryMatcher)Activator.CreateInstance(matcher)).Match(testCase.Records, testCase.Queries);
                testCase.AssertResult(result);
            }            
        }
    }
}