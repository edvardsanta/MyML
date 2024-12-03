using MyML.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyML
{
    public class CosineSimilaritySearch : SimilaritySearch
    {
        protected override List<string> PreprocessText(string text)
        {
            text = text.ToLower();
            text = Regex.Replace(text, @"[^\w\s]", "");
            return text.Split(' ').Where(word => !string.IsNullOrWhiteSpace(word)).ToList();
        }

        protected override double CalculateSimilarity(List<string> tokens1, List<string> tokens2)
        {
            var allTokens = tokens1.Concat(tokens2).Distinct().ToList();
            var vector1 = allTokens.Select(token => tokens1.Count(t => t == token)).ToArray();
            var vector2 = allTokens.Select(token => tokens2.Count(t => t == token)).ToArray();

            double dotProduct = vector1.Zip(vector2, (a, b) => a * b).Sum();
            double magnitude1 = Math.Sqrt(vector1.Sum(v => v * v));
            double magnitude2 = Math.Sqrt(vector2.Sum(v => v * v));

            return magnitude1 > 0 && magnitude2 > 0 ? dotProduct / (magnitude1 * magnitude2) : 0.0;
        }
    }
}
