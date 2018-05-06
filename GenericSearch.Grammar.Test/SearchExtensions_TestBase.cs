using System;
using System.Linq;
using System.Linq.Expressions;
using GenericSearch.Common;
using Xunit;

namespace GenericSearch.Grammar.Test
{
    public abstract class SearchExtensions_TestBase
    {
        private readonly Func<IQueryable<Document>, string, Expression<Func<Document, string>>[], SearchResult<Document>> filter;

        private readonly IQueryable<Document> sampeInput = new[]
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

        private readonly IQueryable<Document> sampeInputSpecialCharacters = new[]
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

        [Fact]
        public void GetMatches_NullAsInput_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                this.filter(
                    null,
                    "Test",
                    new Expression<Func<Document, string>>[] { d => d.Title }));
        }

        [Fact]
        public void GetMatches_NoProperties_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                this.filter(
                    this.sampeInput,
                    "Test",
                    new Expression<Func<Document, string>>[] { }));
        }

        [Fact]
        public void GetMatches_NullAsSearchTerm_AllElementsReturned()
        {
            var matches = this.filter(
                this.sampeInput,
                null,
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Equal(this.sampeInput.Count(), matches.ToArray().Length);
            Assert.Empty(matches.Terms);
        }

        [Fact]
        public void GetMatches_EmptySearchTerm_AllElementsReturned()
        {
            var matches = this.filter(
                this.sampeInput,
                string.Empty,
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Equal(this.sampeInput.Count(), matches.ToArray().Length);
            Assert.Empty(matches.Terms);
        }

        [Fact]
        public void GetMatches_InvalidSearchTermMissingBracket_ThrowsInvalidSearchException()
        {
            Assert.Throws<InvalidSearchException>(() => this.filter(
                this.sampeInput,
                "(Test and Test",
                new Expression<Func<Document, string>>[] { d => d.Title }));
        }

        [Fact]
        public void GetMatches_InvalidSearchTermNotWithoutTerm_ThrowsInvalidSearchException()
        {
            Assert.Throws<InvalidSearchException>(() => this.filter(
                this.sampeInput,
                "not",
                new Expression<Func<Document, string>>[] { d => d.Title }));
        }

        [Fact]
        public void GetMatches_SearchTermWithUmlaut_CorrectElementsReturned()
        {
            var matches = this.filter(
                this.sampeInputSpecialCharacters,
                "ü",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Equal(2, matches.ToArray().Length);
            Assert.Single(matches.Terms);

            matches = this.filter(
               this.sampeInputSpecialCharacters,
               "ß",
               new Expression<Func<Document, string>>[] { d => d.Title });
            Assert.Equal(2, matches.ToArray().Length);
            Assert.Single(matches.Terms);
        }

        [Fact]
        public void GetMatches_SingleWordAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.filter(
                this.sampeInput,
                "ten",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Single(matches.ToArray());
            Assert.Single(matches.Terms);
        }

        [Fact]
        public void GetMatches_SeveralWordsAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.filter(
                this.sampeInput,
                "seven ten",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Single(matches.ToArray());
            Assert.Equal(2, matches.Terms.Count());

            matches = this.filter(
                this.sampeInput,
                "seven and ten",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Single(matches.ToArray());
            Assert.Equal(2, matches.Terms.Count());

            matches = this.filter(
                this.sampeInput,
                "seven & ten",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Single(matches.ToArray());
            Assert.Equal(2, matches.Terms.Count());
        }

        [Fact]
        public void GetMatches_SeveralOrWordsAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.filter(
                this.sampeInput,
                "seven or ten",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Equal(4, matches.ToArray().Length);
            Assert.Equal(2, matches.Terms.Count());

            matches = this.filter(
                this.sampeInput,
                "seven | ten",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Equal(4, matches.ToArray().Length);
            Assert.Equal(2, matches.Terms.Count());
        }

        [Fact]
        public void GetMatches_ExcludeWordAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.filter(
                this.sampeInput,
                "not ten",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Equal(9, matches.ToArray().Length);
            Assert.Single(matches.Terms);

            matches = this.filter(
                this.sampeInput,
                "- ten",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Equal(9, matches.ToArray().Length);
            Assert.Single(matches.Terms);

            matches = this.filter(
                this.sampeInput,
                "-ten",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Equal(9, matches.ToArray().Length);
            Assert.Single(matches.Terms);
        }

        /* Ignored since "Contains" is used for string comparision, which does not support case insensitivity.
         * But if IQueryable is used with EntityFramework, it will work */
        [Fact(Skip = "Only supported by EntityFramework")]
        public void GetMatches_MixedCaseAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.filter(
                this.sampeInput,
                "\"NiNe teN\"",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Single(matches.ToArray());
            Assert.Single(matches.Terms);
        }

        [Fact]
        public void GetMatches_PhraseAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.filter(
                this.sampeInput,
                "\"nine ten\"",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Single(matches.ToArray());
            Assert.Single(matches.Terms);

            matches = this.filter(
                this.sampeInput,
                "\"seven ten\"",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Empty(matches.ToArray());
            Assert.Single(matches.Terms);
        }

        [Fact]
        public void GetMatches_ComplexExpressionsAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.filter(
                this.sampeInput,
                "(two or ten) and six",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Equal(5, matches.ToArray().Length);
            Assert.Equal(3, matches.Terms.Count());

            matches = this.filter(
                this.sampeInput,
                "two or ten and six",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Equal(9, matches.ToArray().Length);
            Assert.Equal(3, matches.Terms.Count());

            matches = this.filter(
                this.sampeInput,
                "not (two or ten)",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Single(matches.ToArray());
            Assert.Equal(2, matches.Terms.Count());

            matches = this.filter(
                this.sampeInput,
                "not (three or ten) one",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Equal(2, matches.ToArray().Length);
            Assert.Equal(3, matches.Terms.Count());

            matches = this.filter(
                this.sampeInput,
                "not (three or ten) not(two or ten)",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Single(matches.ToArray());
            Assert.Equal(3, matches.Terms.Count());

            matches = this.filter(
                this.sampeInput,
                "not (four or ten) not \"two three\"",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Equal(2, matches.ToArray().Length);
            Assert.Equal(3, matches.Terms.Count());
        }

        [Fact]
        public void GetMatches_ServeralSearchProperties_CorrectElementsReturned()
        {
            var matches = this.filter(
                this.sampeInput,
                "two",
                new Expression<Func<Document, string>>[] { d => d.Title, b => b.Name });

            Assert.Equal(9, matches.ToArray().Length);
            Assert.Single(matches.Terms);

            matches = this.filter(
                this.sampeInput,
                "two de",
                new Expression<Func<Document, string>>[] { d => d.Title, b => b.Name });

            Assert.Equal(2, matches.ToArray().Length);
            Assert.Equal(2, matches.Terms.Count());

            matches = this.filter(
                this.sampeInput,
                "two or abc",
                new Expression<Func<Document, string>>[] { d => d.Title, b => b.Name });

            Assert.Equal(10, matches.ToArray().Length);
            Assert.Equal(2, matches.Terms.Count());

            matches = this.filter(
                this.sampeInput,
                "two not cd",
                new Expression<Func<Document, string>>[] { d => d.Title, b => b.Name });

            Assert.Equal(7, matches.ToArray().Length);
            Assert.Equal(2, matches.Terms.Count());

            matches = this.filter(
                this.sampeInput,
                "not (two or abc)",
                new Expression<Func<Document, string>>[] { d => d.Title, b => b.Name });

            Assert.Empty(matches.ToArray());
            Assert.Equal(2, matches.Terms.Count());
        }

        [Fact]
        public void GetMatches_PhraseWithBrackets_CorrectTermParsed()
        {
            var matches = this.filter(
                this.sampeInput,
                "\"(te(st) abc def)\"",
                new Expression<Func<Document, string>>[] { d => d.Title });

            Assert.Single(matches.Terms);
            Assert.Equal("(te(st) abc def)", matches.Terms.First());
        }
    }
}
