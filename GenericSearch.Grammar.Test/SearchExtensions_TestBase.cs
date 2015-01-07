using System;
using System.Linq;
using System.Linq.Expressions;
using GenericSearch.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericSearch.Grammar.Test
{
    [TestClass]
    public abstract class SearchExtensions_TestBase
    {
        private readonly Func<IQueryable<Document>, string, Expression<Func<Document, string>>[], SearchResult<Document>> filter;

        private readonly IQueryable<Document> sampleInput = new[]
            {
                new Document() { Title = "one", Name = "abc" },
                new Document() { Title = "one two", Name = "bcd" },
                new Document() { Title = "one two three", Name = "cde" },
                new Document() { Title = "one two three four", Name = "def" },
                new Document() { Title = "one two three four five", Name = "efg" },
                new Document() { Title = "one two three four five six", Name = "fgh" },
                new Document() { Title = "one two three four five six seven", Name = "ghi" },
                new Document() { Title = "one two three four five six seven eight", Name = "hij" },
                new Document() { Title = "one two three four five six seven eight nine", Name = "ijk" },
                new Document() { Title = "one two three four five six seven eight nine ten", Name = "jkl" }
            }
            .AsQueryable();

        private readonly IQueryable<Document> sampleInputSpecialCharacters = new[]
            {
                new Document() { Title = "oneü", Name = "abc" },
                new Document() { Title = "one twoü", Name = "bcd" },
                new Document() { Title = "one two threeß", Name = "cde" },
                new Document() { Title = "one two three fourß", Name = "def" }
            }
           .AsQueryable();

        protected SearchExtensions_TestBase(Func<IQueryable<Document>, string, Expression<Func<Document, string>>[], SearchResult<Document>> filter)
        {
            this.filter = filter;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetMatches_NullAsInput_ThrowsArgumentNullException()
        {
            IQueryable<Document> sampleInput = null;
            var matches = this.filter(
                sampleInput,
                "Test",
                new Expression<Func<Document, string>>[] { d => d.Title });

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetMatches_NoProperties_ThrowsArgumentException()
        {
            var matches = this.filter(
                sampleInput,
                "Test",
                new Expression<Func<Document, string>>[] { });

        }

        [TestMethod]
        public void GetMatches_NullAsSearchTerm_AllElementsReturned()
        {
            var matches = this.filter(
                sampleInput,
                null,
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(this.sampleInput.Count(), matches.ToArray().Length);
            Assert.AreEqual(0, matches.Terms.Count());
        }

        [TestMethod]
        public void GetMatches_EmptySearchTerm_AllElementsReturned()
        {
            var matches = this.filter(
                sampleInput,
                string.Empty,
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(this.sampleInput.Count(), matches.ToArray().Length);
            Assert.AreEqual(0, matches.Terms.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSearchException))]
        public void GetMatches_InvalidSearchTermMissingBracket_ThrowsInvalidSearchException()
        {
            var matches = this.filter(
                sampleInput,
                "(Test and Test",
                new Expression<Func<Document, string>>[] { d => d.Title });

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSearchException))]
        public void GetMatches_InvalidSearchTermNotWithoutTerm_ThrowsInvalidSearchException()
        {
            var matches = this.filter(
                sampleInput,
                "not",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(0, matches.Terms.Count());
        }

        [TestMethod]
        public void GetMatches_SearchTermWithUmlaut_CorrectElementsReturned()
        {
            var matches = this.filter(
                sampleInputSpecialCharacters,
                "ü",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(2, matches.ToArray().Length);
            Assert.AreEqual(1, matches.Terms.Count());

            matches = this.filter(
               sampleInputSpecialCharacters,
               "ß",
               new Expression<Func<Document, string>>[] { d => d.Title });
            Assert.AreEqual(2, matches.ToArray().Length);
            Assert.AreEqual(1, matches.Terms.Count());
        }

        [TestMethod]
        public void GetMatches_SingleWordAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.filter(
                sampleInput,
                "ten",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(1, matches.ToArray().Length);
            Assert.AreEqual(1, matches.Terms.Count());
        }

        [TestMethod]
        public void GetMatches_SeveralWordsAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.filter(
                sampleInput,
                "seven ten",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(1, matches.ToArray().Length);
            Assert.AreEqual(2, matches.Terms.Count());

            matches = this.filter(
                sampleInput,
                "seven and ten",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(1, matches.ToArray().Length);
            Assert.AreEqual(2, matches.Terms.Count());

            matches = this.filter(
                sampleInput,
                "seven & ten",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(1, matches.ToArray().Length);
            Assert.AreEqual(2, matches.Terms.Count());
        }

        [TestMethod]
        public void GetMatches_SeveralOrWordsAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.filter(
                sampleInput,
                "seven or ten",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(4, matches.ToArray().Length);
            Assert.AreEqual(2, matches.Terms.Count());

            matches = this.filter(
                sampleInput,
                "seven | ten",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(4, matches.ToArray().Length);
            Assert.AreEqual(2, matches.Terms.Count());
        }

        [TestMethod]
        public void GetMatches_ExcludeWordAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.filter(
                sampleInput,
                "not ten",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(9, matches.ToArray().Length);
            Assert.AreEqual(1, matches.Terms.Count());

            matches = this.filter(
                sampleInput,
                "- ten",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(9, matches.ToArray().Length);
            Assert.AreEqual(1, matches.Terms.Count());

            matches = this.filter(
                sampleInput,
                "-ten",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(9, matches.ToArray().Length);
            Assert.AreEqual(1, matches.Terms.Count());
        }

        /* Ignored since "Contains" is used for string comparision, which does not support case insensitivity.
         * But if IQueryable is used with EntityFramework, it will work */
        [TestMethod]
        [Ignore]
        public void GetMatches_MixedCaseAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.filter(
                sampleInput,
                "\"NiNe teN\"",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(1, matches.ToArray().Length);
            Assert.AreEqual(1, matches.Terms.Count());
        }

        [TestMethod]
        public void GetMatches_PhraseAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.filter(
                sampleInput,
                "\"nine ten\"",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(1, matches.ToArray().Length);
            Assert.AreEqual(1, matches.Terms.Count());

            matches = this.filter(
                sampleInput,
                "\"seven ten\"",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(0, matches.ToArray().Length);
            Assert.AreEqual(1, matches.Terms.Count());
        }

        [TestMethod]
        public void GetMatches_ComplexExpressionsAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.filter(
                sampleInput,
                "(two or ten) and six",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(5, matches.ToArray().Length);
            Assert.AreEqual(3, matches.Terms.Count());

            matches = this.filter(
                sampleInput,
                "two or ten and six",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(9, matches.ToArray().Length);
            Assert.AreEqual(3, matches.Terms.Count());

            matches = this.filter(
                sampleInput,
                "not (two or ten)",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(1, matches.ToArray().Length);
            Assert.AreEqual(2, matches.Terms.Count());

            matches = this.filter(
                sampleInput,
                "not (three or ten) one",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(2, matches.ToArray().Length);
            Assert.AreEqual(3, matches.Terms.Count());

            matches = this.filter(
                sampleInput,
                "not (three or ten) not(two or ten)",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(1, matches.ToArray().Length);
            Assert.AreEqual(3, matches.Terms.Count());

            matches = this.filter(
                sampleInput,
                "not (four or ten) not \"two three\"",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(2, matches.ToArray().Length);
            Assert.AreEqual(3, matches.Terms.Count());
        }

        [TestMethod]
        public void GetMatches_ServeralSearchProperties_CorrectElementsReturned()
        {
            var matches = this.filter(
                sampleInput,
                "two",
                new Expression<Func<Document, string>>[] { d => d.Title, b => b.Name });

            Assert.AreEqual(9, matches.ToArray().Length);
            Assert.AreEqual(1, matches.Terms.Count());

            matches = this.filter(
                sampleInput,
                "two de",
                new Expression<Func<Document, string>>[] { d => d.Title, b => b.Name });

            Assert.AreEqual(2, matches.ToArray().Length);
            Assert.AreEqual(2, matches.Terms.Count());

            matches = this.filter(
                sampleInput,
                "two or abc",
                new Expression<Func<Document, string>>[] { d => d.Title, b => b.Name });

            Assert.AreEqual(10, matches.ToArray().Length);
            Assert.AreEqual(2, matches.Terms.Count());

            matches = this.filter(
                sampleInput,
                "two not cd",
                new Expression<Func<Document, string>>[] { d => d.Title, b => b.Name });

            Assert.AreEqual(7, matches.ToArray().Length);
            Assert.AreEqual(2, matches.Terms.Count());

            matches = this.filter(
                sampleInput,
                "not (two or abc)",
                new Expression<Func<Document, string>>[] { d => d.Title, b => b.Name });

            Assert.AreEqual(0, matches.ToArray().Length);
            Assert.AreEqual(2, matches.Terms.Count());
        }

        [TestMethod]
        public void GetMatches_PhraseWithBrackets_CorrectTermParsed()
        {
            var matches = this.filter(
                sampleInput, "\"(te(st) abc def)\"",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.AreEqual(1, matches.Terms.Count());
            Assert.AreEqual("(te(st) abc def)", matches.Terms.First());
        }
    }
}
