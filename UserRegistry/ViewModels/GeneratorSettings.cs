namespace UserRegistry.ViewModels;

public class GeneratorSettings
{
    public string Region { get; set; }
    public double ErrorCount { get; set; } = 0;
    public int Count { get; set; }
    public int Seed { get; set; }
    public int Page { get; set; }
}