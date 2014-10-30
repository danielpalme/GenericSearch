using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericSearch.Grammar.Test
{
    [TestClass]
    public class SearchExtensions_IronyTest
    {
        private readonly IQueryable<Document> sampleInput = new[]
            {
                new Document() { Title = "one" },
                new Document() { Title = "one two" },
                new Document() { Title = "one two three" },
                new Document() { Title = "one two three four" },
                new Document() { Title = "one two three four five" },
                new Document() { Title = "one two three four five six" },
                new Document() { Title = "one two three four five six seven" },
                new Document() { Title = "one two three four five six seven eight" },
                new Document() { Title = "one two three four five six seven eight nine" },
                new Document() { Title = "one two three four five six seven eight nine ten" }
            }
            .AsQueryable();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetMatches_NullAsInput_ThrowsArgumentNullException()
        {
            IQueryable<Document> sampleInput = null;

            var matches = sampleInput.FilterUsingIrony(d => d.Title, "Test").ToArray();
        }

        [TestMethod]
        public void GetMatches_NullAsSearchTerm_AllElementsReturned()
        {
            var matches = this.sampleInput.FilterUsingIrony(d => d.Title, null).ToArray();
            Assert.AreEqual(this.sampleInput.Count(), matches.Length);
        }

        [TestMethod]
        public void GetMatches_EmptySearchTerm_AllElementsReturned()
        {
            var matches = this.sampleInput.FilterUsingIrony(d => d.Title, string.Empty).ToArray();
            Assert.AreEqual(this.sampleInput.Count(), matches.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSearchException))]
        public void GetMatches_InvalidSearchTerm_ThrowsArgumentNullException()
        {
            var matches = this.sampleInput.FilterUsingIrony(d => d.Title, "(Test and Test").ToArray();
        }

        [TestMethod]
        public void GetMatches_SingleWordAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.sampleInput.FilterUsingIrony(d => d.Title, "ten").ToArray();
            Assert.AreEqual(1, matches.Length);
        }

        [TestMethod]
        public void GetMatches_SeveralWordsAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.sampleInput.FilterUsingIrony(d => d.Title, "seven ten").ToArray();
            Assert.AreEqual(1, matches.Length);

            matches = this.sampleInput.FilterUsingIrony(d => d.Title, "seven and ten").ToArray();
            Assert.AreEqual(1, matches.Length);

            matches = this.sampleInput.FilterUsingIrony(d => d.Title, "seven & ten").ToArray();
            Assert.AreEqual(1, matches.Length);
        }

        [TestMethod]
        public void GetMatches_SeveralOrWordsAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.sampleInput.FilterUsingIrony(d => d.Title, "seven or ten").ToArray();
            Assert.AreEqual(4, matches.Length);

            matches = this.sampleInput.FilterUsingIrony(d => d.Title, "seven | ten").ToArray();
            Assert.AreEqual(4, matches.Length);
        }

        [TestMethod]
        public void GetMatches_ExcludeWordAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.sampleInput.FilterUsingIrony(d => d.Title, "not ten").ToArray();
            Assert.AreEqual(9, matches.Length);

            matches = this.sampleInput.FilterUsingIrony(d => d.Title, "- ten").ToArray();
            Assert.AreEqual(9, matches.Length);

            matches = this.sampleInput.FilterUsingIrony(d => d.Title, "-ten").ToArray();
            Assert.AreEqual(9, matches.Length);
        }

        [TestMethod]
        public void GetMatches_MixedCaseAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.sampleInput.FilterUsingIrony(d => d.Title, "\"NiNe teN\"").ToArray();
            Assert.AreEqual(1, matches.Length);
        }

        [TestMethod]
        public void GetMatches_PhraseAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.sampleInput.FilterUsingIrony(d => d.Title, "\"nine ten\"").ToArray();
            Assert.AreEqual(1, matches.Length);

            matches = this.sampleInput.FilterUsingIrony(d => d.Title, "\"seven ten\"").ToArray();
            Assert.AreEqual(0, matches.Length);
        }

        [TestMethod]
        public void GetMatches_ComplexExpressionsAsSearchTerm_CorrectElementsReturned()
        {
            var matches = this.sampleInput.FilterUsingIrony(d => d.Title, "(two or ten) and six").ToArray();
            Assert.AreEqual(5, matches.Length);

            matches = this.sampleInput.FilterUsingIrony(d => d.Title, "two or ten and six").ToArray();
            Assert.AreEqual(9, matches.Length);

            matches = this.sampleInput.FilterUsingIrony(d => d.Title, "not (two or ten)").ToArray();
            Assert.AreEqual(1, matches.Length);

            matches = this.sampleInput.FilterUsingIrony(d => d.Title, "not (three or ten) one").ToArray();
            Assert.AreEqual(2, matches.Length);

            matches = this.sampleInput.FilterUsingIrony(d => d.Title, "not (three or ten) not(two or ten)").ToArray();
            Assert.AreEqual(1, matches.Length);

            matches = this.sampleInput.FilterUsingIrony(d => d.Title, "not (four or ten) not \"two three\"").ToArray();
            Assert.AreEqual(2, matches.Length);
        }
    }
}
