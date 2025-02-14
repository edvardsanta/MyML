namespace MyML.Abstracts
{
    public abstract class SimilaritySearch
    {
        protected abstract List<string> PreprocessText(string text);

        protected abstract double CalculateSimilarity(List<string> tokens1, List<string> tokens2);

        public List<(string Document, double Score)> Search(string query, List<string> documents)
        {
            var queryTokens = new List<string>(PreprocessText(query));

            var results = new List<(string Document, double Score)>();
            foreach (var doc in documents)
            {
                var docTokens = new List<string>(PreprocessText(doc));
                double score = CalculateSimilarity(queryTokens, docTokens);
                results.Add((doc, score));
            }

            results.Sort((x, y) => y.Score.CompareTo(x.Score));

            return results;
        }
    }
}
