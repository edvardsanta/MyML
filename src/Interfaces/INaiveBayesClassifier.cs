namespace MyML.Interfaces
{
    public interface INaiveBayesClassifier
    {
        public void Train(IEnumerable<ClassifierModel> trainingData);
        public Dictionary<string, double> Predict(string text);
    }
}
