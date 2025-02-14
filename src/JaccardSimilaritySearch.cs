using MyML.Abstracts;
using System.Text.RegularExpressions;

namespace MyML
{
    public class JaccardSimilaritySearch : SimilaritySearch
    {
        protected override List<string> PreprocessText(string text)
        {
            text = text.ToLower();
            text = Regex.Replace(text, @"[^\w\s]", "");
            return text.Split(' ').Where(word => !string.IsNullOrWhiteSpace(word)).ToList();
        }

        protected override double CalculateSimilarity(List<string> tokens1, List<string> tokens2)
        {
            var set1 = new HashSet<string>(tokens1);
            var set2 = new HashSet<string>(tokens2);

            int intersectionSize = set1.Intersect(set2).Count();
            int unionSize = set1.Union(set2).Count();
            return (double)intersectionSize / unionSize;
        }
    }
}
