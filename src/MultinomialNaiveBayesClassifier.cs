using MyML.Abstracts;

namespace MyML
{
    public class MultinomialNaiveBayesClassifier : NaiveBayesClassifier
    {
        private int _vocabularySize;

        public MultinomialNaiveBayesClassifier()
        {
            CalculateVocabularySize();
        }

        /// <summary>
        /// Predicts class probabilities for a given text input using Multinomial Naive Bayes classification.
        /// Returns normalized probabilities (as percentages) for each class label.
        /// </summary>
        /// <remarks>
        /// The prediction process consists of four main steps:
        /// 
        /// 1. Calculate posterior probabilities in log space:
        ///    - Combines class prior probability with word likelihoods
        ///    - P(class|text) ∝ log(P(class)) + Σ log(P(word|class))
        /// 
        /// 2. Find maximum log probability for numerical stability
        ///    - Used for log-sum-exp trick to prevent overflow
        /// 
        /// 3. Convert log probabilities to normal space:
        ///    - Uses log-sum-exp trick to prevent numerical overflow
        ///    - Shifts all log probabilities by subtracting max value
        ///    - exp(log(p) - maxLogP) / Σ exp(log(p) - maxLogP)
        /// 
        /// 4. Normalize probabilities to percentages:
        ///    - Ensures all probabilities sum to 100%
        /// </remarks>
        /// <param name="text">Input text to classify</param>
        /// <returns>Dictionary mapping class labels to their predicted probabilities (as percentages)</returns>
        public override Dictionary<string, double> Predict(string text)
        {
            double maxLogProbability = double.MinValue;
            string? predictedLabel = null;
            int totalWordCount = totalWordsPerLabel.Values.Sum();
            IEnumerable<string> words = Tokenize(text);
            Dictionary<string, double> logProbabilities = new Dictionary<string, double>();

            foreach (var label in wordCountsPerLabel.Keys)
            {
                var labelWordCounts = wordCountsPerLabel[label];
                var totalClassCount = totalWordsPerLabel[label];

                double logLikelihood = CalculateProbability(
                    words,
                    labelWordCounts,
                    totalClassCount,
                    _vocabularySize
                );

                double logPrior = Math.Log((double)totalClassCount / totalWordCount);
                double logPosterior = logLikelihood + logPrior; 
                logProbabilities.Add(label, logPosterior);
                if (logPosterior > maxLogProbability)
                {
                    maxLogProbability = logPosterior;
                    predictedLabel = label;
                }
            }

            var result = new Dictionary<string, double>();
            double sumExp = 0.0;

            foreach (var kvp in logProbabilities)
            {
                double shiftedLogProb = kvp.Value - maxLogProbability;
                sumExp += Math.Exp(shiftedLogProb);
            }

            foreach (var kvp in logProbabilities)
            {
                double shiftedLogProb = kvp.Value - maxLogProbability;
                double normalizedProb = (Math.Exp(shiftedLogProb) / sumExp) * 100;
                result.Add(kvp.Key, normalizedProb);
            }

            return result;
        }

        /// <summary>
        /// Calculates the log probability of a document belonging to a specific class using
        /// the Multinomial Naive Bayes algorithm with Laplace (add-one) smoothing.
        /// </summary>
        /// <remarks>
        /// 
        /// 1. Uses log probabilities to prevent numerical underflow
        /// 2. Applies Laplace smoothing to handle unseen words
        /// 3. Assumes word independence (naive assumption)
        /// 
        /// The probability is calculated as:
        /// P(class|document) ∝ log(P(class)) + Σ log(P(word|class))
        /// 
        /// Where P(word|class) is smoothed using Laplace smoothing:
        /// P(word|class) = (count(word,class) + 1) / (totalWords + vocabularySize)
        /// </remarks>
        /// <param name="words">Collection of words from the document to classify</param>
        /// <param name="wordCountsForClass">Dictionary containing word counts for the current class</param>
        /// <param name="totalWordsInClass">Total number of words in the training data for this class</param>
        /// <param name="vocabularySize">Size of the entire vocabulary across all classes</param>
        /// <returns>
        /// Log probability of the document belonging to the class. Higher values indicate
        /// stronger association with the class.
        /// </returns>
        private double CalculateProbability(
             IEnumerable<string> words,
             Dictionary<string, int> wordCountsForClass,
             int totalWordsInClass,
             int vocabularySize)
        {
            double logProbability = 0.0; 
            foreach (var word in words)
            {
                int count = wordCountsForClass.TryGetValue(word, out int c) ? c : 0;

                // Laplace smoothing in log space
                double smoothedProb = Math.Log((count + 1.0) / (totalWordsInClass + vocabularySize));
                logProbability += smoothedProb;
            }

            return logProbability;
        }
        private void CalculateVocabularySize()
        {
            HashSet<string> uniqueWords = new();
            foreach (var labelDict in wordCountsPerLabel.Values)
            {
                uniqueWords.UnionWith(labelDict.Keys);
            }
            _vocabularySize = uniqueWords.Count;
        }
    }
}