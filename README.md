# MyML
Just a machine learning lib written in Dotnet 8 framework

## Features

- **Naive Bayes Classifier (Multinomial):** designed for text classification tasks. The Multinomial variant is suitable for handling text data with multiple classes and word occurrences.

- **Generic Design:** The library is designed to be generic and adaptable, allowing you to train classifiers for various types of data and labels.

- **Easy Training:** Training a classifier is straightforward, and the library handles essential tasks such as feature extraction and Laplace (add-one) smoothing automatically.


## Usage
``` csharp
// Create an instance of the classifier
var classifier = new MultinomialNaiveBayesClassifier();

// Train the classifier with your training data (ClassifierModel)
classifier.Train(trainingData);

// Make predictions
var prediction = classifier.Predict(textToClassify);
```

## TODO

- Do unit tests with possible use cases
- Try other Supervised Learning (Decision Trees,Logistic Regression)
