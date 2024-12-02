namespace DeepDiff.PerformanceTest.Entities.Simple;

internal abstract class PersistEntity
{
    public PersistChange PersistChange { get; set; }
}

internal enum PersistChange
{
    None,
    Insert,
    Update,
    Delete,
}
