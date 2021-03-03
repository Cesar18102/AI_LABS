using System.Linq;
using System.Collections.Generic;

namespace AI_LAB_1.Preprocessing.AI
{
    public class Counter
    {
        public int CountA { get; internal set; }
        public int CountB { get; internal set; }
    }

    public class Probability
    {
        public double ProbabilityA { get; internal set; }
        public double ProbabilityB { get; internal set; }
    }

    public class BaesianClassifier<TX>
    {
        private Dictionary<TX, Counter> GlobalStats { get; set; } = new Dictionary<TX, Counter>();

        public void Teach(IEnumerable<TX> data, bool isGroupA)
        {
            Dictionary<TX, int> stats = GetStats(data);

            foreach (KeyValuePair<TX, int> kvp in stats)
            {
                if (!GlobalStats.ContainsKey(kvp.Key))
                    GlobalStats.Add(kvp.Key, new Counter());

                if (isGroupA)
                    GlobalStats[kvp.Key].CountA += kvp.Value;
                else
                    GlobalStats[kvp.Key].CountB += kvp.Value;
            }
        }

        public Probability GetProbabilityForItem(TX item)
        {
            if (!GlobalStats.ContainsKey(item))
                return new Probability();

            Counter counter = GlobalStats[item];
            double probabilityA = counter.CountA / (counter.CountA + counter.CountB);

            return new Probability() 
            {
                ProbabilityA = probabilityA, 
                ProbabilityB = 1 - probabilityA 
            };
        }

        public Probability GetNormalizedProbabilityForItem(TX item)
        {
            Counter counter = GlobalStats.ContainsKey(item) ? GlobalStats[item] : new Counter();
            double probabilityA = counter.CountA == 0 ? 0 : counter.CountA / (counter.CountA + counter.CountB);

            double normalizedProbabilityA = (counter.CountA * probabilityA + 0.5) / (counter.CountA + 1);
            return new Probability()
            {
                ProbabilityA = normalizedProbabilityA,
                ProbabilityB = 1 - normalizedProbabilityA
            };
        }

        public Probability Predict(IEnumerable<TX> test)
        {
            Probability probability = new Probability()
            {
                ProbabilityA = 0.5,
                ProbabilityB = 0.5
            };

            foreach (TX item in test)
            {
                Probability normalizedItemProbability = GetNormalizedProbabilityForItem(item);
                probability.ProbabilityA *= normalizedItemProbability.ProbabilityA;
                probability.ProbabilityB *= normalizedItemProbability.ProbabilityB;
            }

            return probability;
        }

        private Dictionary<TX, int> GetStats(IEnumerable<TX> data)
        {
            return data.GroupBy(item => item).ToDictionary(
                group => group.Key, group => group.Count()
            );
        }
    }
}
