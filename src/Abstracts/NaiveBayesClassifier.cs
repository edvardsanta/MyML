using MyML.Interfaces;

namespace MyML.Abstracts
{
    public abstract class NaiveBayesClassifier : INaiveBayesClassifier
    {
        protected Dictionary<string, Dictionary<string, int>> wordCountsPerLabel;
        protected Dictionary<string, int> totalWordsPerLabel;

        public NaiveBayesClassifier()
        {
            wordCountsPerLabel = new Dictionary<string, Dictionary<string, int>>();
            totalWordsPerLabel = new Dictionary<string, int>();
        }

        public virtual void Train(IEnumerable<ClassifierModel> trainingData)
        {
            foreach (var data in trainingData)
            {
                // Initialize dictionary for new labels
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

        protected IEnumerable<string> Tokenize(string text, char separator = ' ')
        {
            return text.ToLower().Split(separator);
        }
       
        public abstract string Predict(string text);    
    }
}
