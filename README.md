# MyML
Just a machine learning lib written in Dotnet 8 framework

## Features

- **Naive Bayes Classifier (Multinomial):** designed for text classification tasks. The Multinomial variant is suitable for handling text data with multiple classes and word occurrences.

- **Similarity Search**
   - Jaccard Similarity Search: Measures similarity between sets, ideal for categorical or keyword-based data.
   - Cosine Similarity Search: Measures the cosine similarity between text/vector pairs, commonly used in recommendation systems.

- **Generic Design:** The library is designed to be generic and adaptable, allowing you to train classifiers for various types of data and labels.

- **Easy Training:** Training a classifier is straightforward, and the library handles essential tasks such as feature extraction and Laplace (add-one) smoothing automatically.


## Usage
### **Naive Bayes Classifier**
``` csharp
// Create an instance of the classifier
var classifier = new MultinomialNaiveBayesClassifier();

// Train the classifier with your training data (ClassifierModel)
classifier.Train(trainingData);

// Make predictions
var prediction = classifier.Predict(textToClassify);
```
### Similarity Search
#### Jaccard Similarity Search
``` csharp
// Create an instance of the Jaccard similarity search
SimilaritySearch search = new JaccardSimilaritySearch();

// Query and documents
string query = "gato no jardim";
var documents = new List<string>
{
    "O gato está no jardim.",
    "O cachorro brinca no parque.",
    "A árvore é alta e verde."
};

// Perform the search
var results = search.Search(query, documents);
```

#### Cosine Similarity Search
``` csharp
// Create an instance of the Cosine similarity search
SimilaritySearch search = new CosineSimilaritySearch();

// Query and documents
string query = "gato no jardim";
var documents = new List<string>
{
    "O gato está no jardim.",
    "O cachorro brinca no parque.",
    "A árvore é alta e verde."
};

// Perform the search
var results = search.Search(query, documents);
```


## TODO

- [ ] Do unit tests with possible use cases
- [ ] Try other Supervised Learning (Decision Trees,Logistic Regression)
- [ ] Integrate these features into the classifier (e.g., similarity-based predictions or clustering).
- [ ] Explore ranking, leveraging these similarity metrics.

