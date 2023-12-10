using MyML.Abstracts;
using MyML.Interfaces;

namespace MyML
{
    public class MultinomialNaiveBayesClassifier : NaiveBayesClassifier
    {
        public override string Predict(string text)
        {
            double maxProbability = double.MinValue;
            string? predictedLabel = null;
            int totalWordCount = totalWordsPerLabel.Values.Sum();

            var words = Tokenize(text);
            foreach (var label in wordCountsPerLabel.Keys)
            {
                var labelWordCounts = wordCountsPerLabel[label];
                var totalClassCount = totalWordsPerLabel[label];

                var probability = CalculateProbability(words, labelWordCounts, totalClassCount, totalWordCount);
                var evidence = CalculateEvidence(words, wordCountsPerLabel, totalWordCount);
                var labelProbability = probability / evidence;

                if (labelProbability > maxProbability)
                {
                    maxProbability = labelProbability;
                    predictedLabel = label;
                }
            }

            return predictedLabel!;
        }


        private double CalculateProbability(IEnumerable<string> words, Dictionary<string, int> wordCounts, int totalClassCount, int totalWordCount)
        {
            double probability = 1;

            foreach (var word in words)
            {
                // Laplace smoothing to add one just to ensure that every word contributes a small, non-zero probability
                if (wordCounts.TryGetValue(word, out var count))
                {
                    probability *= (double)(count + 1) / (totalClassCount + totalWordCount);
                }
                else
                {
                    probability *= 1.0 / (totalClassCount + totalWordCount);
                }
            }
            return probability;
        }
        /// <summary>
        /// Computes the evidence term P(B) in Bayes' Theorem, which is the probability of the observed features (words, in this case) across all classes
        /// </summary>
        /// <param name="words"></param>
        /// <param name="wordCountsPerLabel"></param>
        /// <param name="totalWordCount"></param>
        /// <returns></returns>
        private double CalculateEvidence(IEnumerable<string> words, Dictionary<string, Dictionary<string, int>> wordCountsPerLabel, int totalWordCount)
        {
            double evidence = 1;

            foreach (var word in words)
            {
                double wordProbability = 0;
                foreach (var label in wordCountsPerLabel.Keys)
                {
                    if (wordCountsPerLabel[label].TryGetValue(word, out var count))
                    {
                        wordProbability += (double)count / totalWordCount;
                    }
                }
                double dealUnseenWord = wordProbability > 0 ? wordProbability : 1.0;
                evidence *= dealUnseenWord  / totalWordCount;
            }
            return evidence;
        }
    }
}