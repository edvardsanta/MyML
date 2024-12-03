using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyML.Abstracts
{
    public abstract class SimilaritySearch
    {
        protected abstract List<string> PreprocessText(string text);

        // Método abstrato para calcular a similaridade
        protected abstract double CalculateSimilarity(List<string> tokens1, List<string> tokens2);

        public List<(string Document, double Score)> Search(string query, List<string> documents)
        {
            // Pré-processar a consulta
            var queryTokens = new List<string>(PreprocessText(query));

            // Armazenar os documentos e seus escores
            var results = new List<(string Document, double Score)>();

            // Calcular a similaridade para cada documento
            foreach (var doc in documents)
            {
                var docTokens = new List<string>(PreprocessText(doc));
                double score = CalculateSimilarity(queryTokens, docTokens);
                results.Add((doc, score));
            }

            // Classificar os documentos com base no escore (do maior para o menor)
            results.Sort((x, y) => y.Score.CompareTo(x.Score));

            return results;
        }
    }
}
