namespace MyML.Interfaces
{
    public interface INaiveBayesClassifier
    {
        public void Train(IEnumerable<ClassifierModel> trainingData);
        public string Predict(string text);
    }
}
