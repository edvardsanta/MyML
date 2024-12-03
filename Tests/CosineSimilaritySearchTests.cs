using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using MyML;

public class CosineSimilaritySearchTests
{
    private readonly CosineSimilaritySearch _cosineSimilaritySearch;

    public CosineSimilaritySearchTests()
    {
        // Inicializar a instância da classe de similaridade de cosseno
        _cosineSimilaritySearch = new CosineSimilaritySearch();
    }

    [Fact]
    public void CosineSimilarity_BasicTest()
    {
        // Arrange
        string query = "gato no jardim";
        var documents = new List<string>
        {
            "O gato está no jardim.",
            "O cachorro brinca no parque.",
            "A árvore é alta e verde."
        };

        // Act
        var results = _cosineSimilaritySearch.Search(query, documents);

        // Assert
        results.Should().HaveCount(3, "deve haver 3 documentos na lista de resultados");
        results[0].Score.Should().BeGreaterThan(results[1].Score, "o documento mais relevante deve ter uma pontuação maior");
    }

    [Fact]
    public void CosineSimilarity_EmptyQueryTest()
    {
        // Arrange
        string query = "";
        var documents = new List<string>
        {
            "O gato está no jardim.",
            "O cachorro brinca no parque."
        };

        // Act
        var results = _cosineSimilaritySearch.Search(query, documents);

        // Assert
        results.Should().OnlyContain(result => result.Score == 0, "a pontuação deve ser 0 para uma consulta vazia");
    }

    [Fact]
    public void CosineSimilarity_IdenticalDocumentsTest()
    {
        // Arrange
        string query = "gato no jardim";
        var documents = new List<string>
        {
            "gato no jardim",
            "gato no jardim"
        };

        // Act
        var results = _cosineSimilaritySearch.Search(query, documents);

        // Assert
        results[0].Score.Should().BeApproximately(1.0, 0.01, "a pontuação deve ser 1.0 para documentos idênticos");
        results[1].Score.Should().BeApproximately(1.0, 0.01, "a pontuação deve ser 1.0 para documentos idênticos");
    }
}
