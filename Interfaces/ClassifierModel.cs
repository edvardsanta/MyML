namespace MyML.Interfaces
{
    public record ClassifierModel
    {
        public string Text { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }
}
