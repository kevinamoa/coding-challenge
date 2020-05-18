using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;
        private readonly IDictionary<Color, IEnumerable<Shirt>> _colorShirtLookup;
        private readonly IDictionary<Size, IEnumerable<Shirt>> _sizeShirtLookup;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts ?? throw new ArgumentNullException(nameof(shirts));

            _colorShirtLookup = Color.All
                .AsParallel()
                .ToDictionary(color => color, color => shirts.Where(shirt => shirt.Color == color));
            
            _sizeShirtLookup = Size.All
                .AsParallel()
                .ToDictionary(size => size, size => shirts.Where(shirt => shirt.Size == size));
        }

        public SearchResults Search(SearchOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (options.Colors == null) throw new ArgumentNullException(nameof(options.Colors));
            if (options.Sizes == null) throw new ArgumentNullException(nameof(options.Sizes));

            var matchedShirts = (options.Colors.Any(), options.Sizes.Any()) switch
            {
                // Both color and size search options have been selected
                (true, true) => options.Colors
                    .AsParallel()
                    .SelectMany(color => _colorShirtLookup[color])
                    .Intersect(options.Sizes.AsParallel().SelectMany(size => _sizeShirtLookup[size]))
                    .ToList(),

                // Only color search options have been selected
                (true, false) => options.Colors
                    .AsParallel()
                    .SelectMany(color => _colorShirtLookup[color])
                    .ToList(),

                // Only size search options have been selected
                (false, true) => options.Sizes
                    .AsParallel()
                    .SelectMany(size => _sizeShirtLookup[size])
                    .ToList(),

                // No options selected
                _ => _shirts
            };
            
            return BuildSearchResults(matchedShirts);
        }

        public SearchResults BuildSearchResults(List<Shirt> matchedShirts)
        {
            var colorCountLookup = new ConcurrentDictionary<Color, int>(Color.All.ToDictionary(color => color, _ => 0));
            var sizeColorLookup = new ConcurrentDictionary<Size, int>(Size.All.ToDictionary(size => size, _ => 0));

            matchedShirts.AsParallel().ForAll(shirt =>
            {
                colorCountLookup.AddOrUpdate(shirt.Color, 1, (_, count) => count + 1);
                sizeColorLookup.AddOrUpdate(shirt.Size, 1, (_, count) => count + 1);
            });

            return new SearchResults
            {
                Shirts = matchedShirts,
                ColorCounts = colorCountLookup.Select(map => new ColorCount(map.Key, map.Value)).ToList(),
                SizeCounts = sizeColorLookup.Select(map => new SizeCount(map.Key, map.Value)).ToList()
            };
        }
    }  
}