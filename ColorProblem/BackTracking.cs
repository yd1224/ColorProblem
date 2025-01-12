namespace ColorProblem
{
    public static class BackTrackingColoring
    {
        private static int iterationCount = 0;
        private static int totalNodesChecked = 0;
        private static int totalNodesInMemory = 0;
        private static int deadEnds = 0;

        public static bool ColorMap(List<Region> regions)
        {
            regions = regions.OrderByDescending(r => r.Neighbours.Count).ToList();

            totalNodesInMemory = regions.Count;

            return ColorRegions(regions, 20);
        }

        private static bool ColorRegions(List<Region> regions, int currentRegionIndex)
        {
            iterationCount++;

            if (regions.All(r => r.Color != null))
            {
                return true;
            }

            var currentRegion = regions[currentRegionIndex];

            totalNodesChecked++;

            var colors = Enum.GetValues<Color>()
                .Select(color => new { Color = color, Conflicts = PredictConflicts(currentRegion, color) })
                .Where(predicate => predicate.Conflicts == 0)
                .ToList();

            if (colors.Count == 0)
            {
                return false;
            }

            foreach (var option in colors)
            {
                if (IsValidColor(currentRegion, option.Color))
                {
                    currentRegion.Color = option.Color;

                    var uncoloredRegions = regions.Where(r => r.Color == null).ToList();
                    if (uncoloredRegions.Count == 0)
                    {
                        return true;
                    }

                    var nextRegion = GetNextRegionToColor(uncoloredRegions);
                    int nextRegionIndex = regions.IndexOf(nextRegion);

                    if (ColorRegions(regions, nextRegionIndex))
                    {
                        return true;
                    }

                    currentRegion.Color = null;
                    deadEnds++;
                }
            }

            return false;
        }

        private static bool IsValidColor(Region region, Color color)
        {
            return region.Neighbours.All(neighbour => neighbour.Color != color);
        }

        private static int PredictConflicts(Region currentRegion, Color color)
        {
            return currentRegion.Neighbours.Count(neighbour => neighbour.Color == color);
        }

        private static Region GetNextRegionToColor(List<Region> uncoloredRegions)
        {
            return uncoloredRegions
                .OrderByDescending(r => r.Neighbours.Count(neighbour => neighbour.Color == null))
                .First();
        }

        public static int GetIterationCount()
        {
            return iterationCount;
        }

        public static int GetTotalNodesChecked()
        {
            return totalNodesChecked;
        }

        public static int GetTotalNodesInMemory()
        {
            return totalNodesInMemory;
        }
        
        public static int GetDeadEnds()
        {
            return deadEnds;
        }
    }
}
