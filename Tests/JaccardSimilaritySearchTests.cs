using FluentAssertions;
using MyML;

public class JaccardSimilaritySearchTests
{
    private readonly JaccardSimilaritySearch _jaccardSimilaritySearch;

    public JaccardSimilaritySearchTests()
    {
        // Inicializar a instância da classe de similaridade de Jaccard
        _jaccardSimilaritySearch = new JaccardSimilaritySearch();
    }

    [Fact]
    public void JaccardSimilarity_BasicTest()
    {
        // Arrange
        string query = "cachorro brinca parque";
        var documents = new List<string>
        {
            "O cachorro está brincando no parque.",
            "O gato está no jardim.",
            "A árvore é alta e verde."
        };

        // Act
        var results = _jaccardSimilaritySearch.Search(query, documents);

        // Assert
        results.Should().HaveCount(3, "deve haver 3 documentos na lista de resultados");
        results[0].Score.Should().BeGreaterThan(results[1].Score, "o documento mais relevante deve ter uma pontuação maior");
    }

    [Fact]
    public void JaccardSimilarity_EmptyQueryTest()
    {
        // Arrange
        string query = "";
        var documents = new List<string>
        {
            "O gato está no jardim.",
            "O cachorro brinca no parque."
        };

        // Act
        var results = _jaccardSimilaritySearch.Search(query, documents);

        // Assert
        results.Should().OnlyContain(result => result.Score == 0, "a pontuação deve ser 0 para uma consulta vazia");
    }

    [Fact]
    public void JaccardSimilarity_IdenticalDocumentsTest()
    {
        // Arrange
        string query = "cachorro no parque";
        var documents = new List<string>
        {
            "cachorro no parque",
            "cachorro no parque"
        };

        // Act
        var results = _jaccardSimilaritySearch.Search(query, documents);

        // Assert
        results[0].Score.Should().BeApproximately(1.0, 0.01, "a pontuação deve ser 1.0 para documentos idênticos");
        results[1].Score.Should().BeApproximately(1.0, 0.01, "a pontuação deve ser 1.0 para documentos idênticos");
    }
}
