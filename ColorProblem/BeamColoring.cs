namespace ColorProblem
{
    public class BeamSearchColoring
    {
        private static readonly Random Random = new Random();
        private const int SuccessorCount = 3;
        
        public static int iterations = 0;
        public static int totalStatesGenerated = 0;
        public static int statesInMemory = 0;

        private static readonly List<Color> Colors = new List<Color> { Color.Red, Color.Green, Color.Blue, Color.Yellow };

        public static bool ColorMapWithBeamSearch(List<Region> regions, int initialBeamWidth, int finalBeamWidth)
        {
            // Step 1: Generate initial candidates
            var initialStates = GenerateInitialCandidates(regions, initialBeamWidth, ref totalStatesGenerated);
            statesInMemory = initialStates.Count;

            if (initialStates.Any(IsFullyColored))
            {
                var solution = initialStates.First(IsFullyColored);
                ApplyColoring(regions, solution);
                return true;
            }

            int currentBeamWidth = initialBeamWidth;

            while (currentBeamWidth >= finalBeamWidth)
            {
                iterations++;

                // Step 1: Score each candidate
                var successors = GenerateSuccessors(initialStates, ref totalStatesGenerated);
                statesInMemory = successors.Count;

                List<(List<Region> potentialSolution, int score)> scoredSuccessors = successors.Select(
                    c => (potentialSolution: c, score: CalculateScore(c))
                ).ToList();

                if (scoredSuccessors.Any(s => s.score == regions.Count))
                {
                    var solution = scoredSuccessors.First(s => s.score == regions.Count).potentialSolution;
                    ApplyColoring(regions, solution);
                    return true;
                }

                // Step 2: Select the next beam probabilistically
                initialStates = SelectNextBeam(scoredSuccessors, currentBeamWidth - 1);
                statesInMemory = initialStates.Count;

                // Step 3: Check if a fully valid solution exists
                foreach (var candidate in successors.Where(IsFullyColored))
                {
                    ApplyColoring(regions, candidate);
                    return true;
                }

                currentBeamWidth--;
            }

            return false;
        }

        private static List<List<Region>> GenerateSuccessors(List<List<Region>> states, ref int totalStatesGenerated)
        {
            var successors = new List<List<Region>>();
            foreach (var state in states)
            {
                for (int i = 0; i < SuccessorCount; i++)
                {
                    var successor = state.Select(r => new Region(r.Name) { Neighbours = r.Neighbours, Color = r.Color }).ToList();
                    var conflictingRegions = successor.Where(region =>
                        region.Color == null || region.Neighbours.Any(n =>
                            successor.First(c => c.Name == n.Name).Color == region.Color));

                    foreach (var region in successor)
                    {
                        // Randomly assign colors to conflicting regions
                        if (conflictingRegions.Contains(region))
                        {
                            region.Color = Colors[Random.Next(Colors.Count)];
                        }
                    }
                    successors.Add(successor);
                    totalStatesGenerated++;
                }
            }

            return successors;
        }

        private static List<List<Region>> GenerateInitialCandidates(List<Region> regions, int beamWidth, ref int totalStatesGenerated)
        {
            var candidates = new List<List<Region>>();
            for (int i = 0; i < beamWidth; i++)
            {
                var candidate = regions.Select(r => new Region(r.Name) { Neighbours = r.Neighbours, Color = null }).ToList();
                foreach (var region in candidate)
                {
                    region.Color = Colors[Random.Next(Colors.Count)];
                }
                candidates.Add(candidate);
                totalStatesGenerated++;
            }
            return candidates;
        }
        
        private static int CalculateScore(List<Region> candidate)
        {
            return candidate.Count(r => r.Color != null && r.Neighbours.All(n =>
                candidate.First(c => c.Name == n.Name).Color != r.Color));
        }

        private static List<List<Region>> SelectNextBeam(List<(List<Region> Candidate, int Score)> scoredCandidates, int nextBeamWidth)
        {
            scoredCandidates = scoredCandidates.OrderByDescending(sc => sc.Score).ToList();
            var totalScore = scoredCandidates.Sum(sc => sc.Score);
            var probabilities = scoredCandidates.Select(sc => (double)sc.Score / totalScore).ToList();

            var selectedBeam = new List<List<Region>>();
            for (int i = 0; i < nextBeamWidth; i++)
            {
                double randomValue = Random.NextDouble();
                double cumulativeProbability = 0.0;

                for (int j = 0; j < probabilities.Count; j++)
                {
                    cumulativeProbability += probabilities[j];
                    if (randomValue <= cumulativeProbability)
                    {
                        selectedBeam.Add(scoredCandidates[j].Candidate);
                        break;
                    }
                }
            }

            return selectedBeam;
        }

        private static bool IsFullyColored(List<Region> candidate)
        {
            return candidate.All(r => r.Color != null && r.Neighbours.All(n => candidate.First(c => c.Name == n.Name).Color != r.Color));
        }

        private static void ApplyColoring(List<Region> regions, List<Region> successfulCandidate)
        {
            for (int i = 0; i < regions.Count; i++)
            {
                regions[i].Color = successfulCandidate[i].Color;
            }
        }
    }
}

