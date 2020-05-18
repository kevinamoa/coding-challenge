using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEngineTests : SearchEngineTestsBase
    {
        [Test]
        public void SearchEngineTest_WhenNoShirtsProvided_ThrowsExpectedNullArgumentException()
        {
            var shirts = (List<Shirt>) null;

            Assert.Throws<ArgumentNullException>(() => new SearchEngine(shirts));
        }

        [Test]
        public void SearchEngineSearch_WhenNoOptionsProvided_ThrowsExpectedException()
        {
            var shirts = new List<Shirt>();
            var searchEngine = new SearchEngine(shirts);

            var options = (SearchOptions) null;

            Assert.Throws<ArgumentNullException>(() => searchEngine.Search(options), nameof(options));
        }

        [Test]
        public void SearchEngineSearch_WhenNullColorsOption_ThrowsExpectedException()
        {
            var shirts = new List<Shirt>();
            var searchEngine = new SearchEngine(shirts);

            var options = new SearchOptions
            {
                Colors = null,
                Sizes = new List<Size>()
            };

            Assert.Throws<ArgumentNullException>(() => searchEngine.Search(options), nameof(options.Colors));
        }

        [Test]
        public void SearchEngineSearch_WhenNullSizesOption_ThrowsExpectedException()
        {
            var shirts = new List<Shirt>();
            var searchEngine = new SearchEngine(shirts);

            var options = new SearchOptions
            {
                Colors = new List<Color>(),
                Sizes = null
            };

            Assert.Throws<ArgumentNullException>(() => searchEngine.Search(options), nameof(options.Sizes));
        }

        [Test]
        public void SearchEngineSearch_FindsExpectedRedSmall()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> {Color.Red},
                Sizes = new List<Size> {Size.Small}
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void SearchEngineSearch_WhenNoShirtsToBeFound_ReturnsNoShirts()
        {
            var shirts = new List<Shirt>();
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> {Color.Black}
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void SearchEngineSearch_WhenNoOptions_ReturnsAllShirts()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "Blue - Small", Size.Small, Color.Blue),
                new Shirt(Guid.NewGuid(), "White - Large", Size.Large, Color.White),
            };

            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions();

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void SearchEngineSearch_WhenOnlyColorOption_ReturnsColorWithAllSizes()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "Blue - Small", Size.Small, Color.Blue),
                new Shirt(Guid.NewGuid(), "White - Large", Size.Large, Color.White),
            };

            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> {Color.Black}
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void SearchEngineSearch_WhenOnlySizeOption_ReturnsSizeWithAllColors()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "Blue - Small", Size.Small, Color.Blue),
                new Shirt(Guid.NewGuid(), "White - Large", Size.Large, Color.White),
            };

            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions
            {
                Sizes = new List<Size> { Size.Medium }
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }
    }
}
