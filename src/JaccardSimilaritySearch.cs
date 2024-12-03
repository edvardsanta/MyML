using MyML.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyML
{
    public class JaccardSimilaritySearch : SimilaritySearch
    {
        // Implementação do método de pré-processamento de texto
        protected override List<string> PreprocessText(string text)
        {
            // Converter o texto para minúsculas e remover pontuação
            text = text.ToLower();
            text = Regex.Replace(text, @"[^\w\s]", "");
            return text.Split(' ').Where(word => !string.IsNullOrWhiteSpace(word)).ToList();
        }

        // Implementação do método de cálculo do Índice de Jaccard
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
