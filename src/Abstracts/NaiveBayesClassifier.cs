using MyML.Interfaces;

namespace MyML.Abstracts
{
    public abstract class NaiveBayesClassifier : INaiveBayesClassifier
    {
        protected Dictionary<string, Dictionary<string, int>> wordCountsPerLabel;
        protected Dictionary<string, int> totalWordsPerLabel;
        protected readonly int maxSamplesPerClass;
        protected readonly bool useUndersampling;
        protected readonly Random random;

        public NaiveBayesClassifier(int maxSamplesPerClass = 1000, bool useUndersampling = true)
        {
            wordCountsPerLabel = new Dictionary<string, Dictionary<string, int>>();
            totalWordsPerLabel = new Dictionary<string, int>();
            this.maxSamplesPerClass = maxSamplesPerClass;
            this.useUndersampling = useUndersampling;
            this.random = new Random();
        }

        public virtual void Train(IEnumerable<ClassifierModel> trainingData)
        {
            var balancedData = BalanceDataset(trainingData.ToList());

            foreach (var data in balancedData)
            {
                if (!wordCountsPerLabel.ContainsKey(data.Label))
                {
                    wordCountsPerLabel[data.Label] = new Dictionary<string, int>();
                    totalWordsPerLabel[data.Label] = 0;
                }

                Dictionary<string, int> targetDictionary = wordCountsPerLabel[data.Label];
                IEnumerable<string> words = Tokenize(data.Text);

                foreach (var word in words)
                {
                    if (!targetDictionary.ContainsKey(word))
                        targetDictionary[word] = 0;
                    targetDictionary[word]++;
                }
                totalWordsPerLabel[data.Label]++;
            }
        }

        protected virtual IEnumerable<ClassifierModel> BalanceDataset(List<ClassifierModel> data)
        {
            // Group data by label
            var groupedData = data.GroupBy(x => x.Label)
                                .ToDictionary(g => g.Key, g => g.ToList());

            // Find minority and majority class sizes
            int minClassSize = groupedData.Values.Min(x => x.Count);
            int maxClassSize = groupedData.Values.Max(x => x.Count);

            // Determine target size based on strategy
            int targetSize = useUndersampling ?
                Math.Min(minClassSize, maxSamplesPerClass) :
                Math.Min(maxClassSize, maxSamplesPerClass);

            var balancedData = new List<ClassifierModel>();

            foreach (var group in groupedData)
            {
                var samples = group.Value;
                var currentSize = samples.Count;

                if (currentSize <= targetSize)
                {
                    // If using oversampling and current size is less than target
                    if (!useUndersampling && currentSize < targetSize)
                    {
                        balancedData.AddRange(OversampleData(samples, targetSize));
                    }
                    else
                    {
                        balancedData.AddRange(samples);
                    }
                }
                else
                {
                    // Undersample if current size is greater than target
                    balancedData.AddRange(UndersampleData(samples, targetSize));
                }
            }

            return balancedData;
        }

        protected virtual IEnumerable<ClassifierModel> UndersampleData(List<ClassifierModel> samples, int targetSize)
        {
            return samples.OrderBy(x => random.Next()).Take(targetSize);
        }

        protected virtual IEnumerable<ClassifierModel> OversampleData(List<ClassifierModel> samples, int targetSize)
        {
            var result = new List<ClassifierModel>(samples);

            while (result.Count < targetSize)
            {
                // Add random samples from the original set until we reach target size
                result.Add(samples[random.Next(samples.Count)]);
            }

            return result;
        }

        protected virtual IEnumerable<string> Tokenize(string text, char separator = ' ')
        {
            return text.ToLower().Split(separator);
        }

        public Dictionary<string, int> GetClassDistribution(IEnumerable<ClassifierModel> data)
        {
            return data.GroupBy(x => x.Label)
                      .ToDictionary(g => g.Key, g => g.Count());
        }

        public abstract Dictionary<string, double> Predict(string text);
    }
}
