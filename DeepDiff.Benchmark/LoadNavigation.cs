using BenchmarkDotNet.Attributes;
using DeepDiff.Benchmark.Entities;
using DeepDiff.Configuration;

namespace DeepDiff.Benchmark;

public class LoadNavigation
{
    private Random Random { get; }

    private IReadOnlyCollection<NavigationEntityLevel0> ExistingEntities { get; set; } = null!;
    private IReadOnlyCollection<NavigationEntityLevel0> NewEntities { get; set; } = null!;

    private IDeepDiff NoHashtableNaiveComparerDeepDiff { get; }
    private IDeepDiff NoHastablePrecompiledComparerDeepDiff { get; }
    private IDeepDiff HastableNaiveComparerDeepDiff { get; }
    private IDeepDiff HashtablePrecompiledComparerDeepDiff { get; }

    public LoadNavigation()
    {
        Random = new Random();

        var noHashtableNaiveComparerDiffConfiguration = new DeepDiffConfiguration();
        noHashtableNaiveComparerDiffConfiguration
           .Entity<NavigationEntityLevel0>()
           .HasKey(x => new { x.StartsOn, x.Direction }, opt => opt.UsePrecompiledEqualityComparer(false))
           .HasValues(x => new { x.RequestedPower, x.Penalty }, opt => opt.UsePrecompiledEqualityComparer(false))
           .OnUpdate(cfg => cfg.CopyValues(x => x.AdditionalValueToCopy))
           .HasOne(x => x.SubEntity)
           .HasMany(x => x.SubEntities)
           .OnInsert(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Insert))
           .OnDelete(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete))
           .OnUpdate(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete));
        noHashtableNaiveComparerDiffConfiguration
            .Entity<NavigationEntityLevel1>()
            .HasKey(x => x.Timestamp, opt => opt.UsePrecompiledEqualityComparer(false))
            .HasValues(x => new { x.Power, x.Price }, opt => opt.UsePrecompiledEqualityComparer(false))
            .HasOne(x => x.SubEntity)
            .HasMany(x => x.SubEntities)
            .OnInsert(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Insert))
            .OnDelete(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete))
            .OnUpdate(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete));
        noHashtableNaiveComparerDiffConfiguration
           .Entity<NavigationEntityLevel2>()
           .HasKey(x => x.DeliveryPointEan, opt => opt.UsePrecompiledEqualityComparer(false))
           .HasValues(x => new { x.Value1, x.Value2 }, opt => opt.UsePrecompiledEqualityComparer(false))
           .OnInsert(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Insert))
           .OnDelete(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete))
           .OnUpdate(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete));
        NoHashtableNaiveComparerDeepDiff = noHashtableNaiveComparerDiffConfiguration.CreateDeepDiff();

        var noHastablePrecompiledComparerDiffConfiguration = new DeepDiffConfiguration();
        noHastablePrecompiledComparerDiffConfiguration
            .Entity<NavigationEntityLevel0>()
            .HasKey(x => new { x.StartsOn, x.Direction })
            .HasValues(x => new { x.RequestedPower, x.Penalty })
            .OnUpdate(cfg => cfg.CopyValues(x => x.AdditionalValueToCopy))
            .HasOne(x => x.SubEntity)
            .HasMany(x => x.SubEntities)
            .OnInsert(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Insert))
            .OnDelete(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete))
            .OnUpdate(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete));
        noHastablePrecompiledComparerDiffConfiguration
            .Entity<NavigationEntityLevel1>()
            .HasKey(x => x.Timestamp)
            .HasValues(x => new { x.Power, x.Price })
            .HasOne(x => x.SubEntity)
            .HasMany(x => x.SubEntities)
            .OnInsert(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Insert))
            .OnDelete(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete))
            .OnUpdate(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete));
        noHastablePrecompiledComparerDiffConfiguration
           .Entity<NavigationEntityLevel2>()
           .HasKey(x => x.DeliveryPointEan)
           .HasValues(x => new { x.Value1, x.Value2 })
           .OnInsert(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Insert))
           .OnDelete(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete))
           .OnUpdate(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete));
        NoHastablePrecompiledComparerDeepDiff = noHastablePrecompiledComparerDiffConfiguration.CreateDeepDiff();

        var hastableNaiveComparerDiffConfiguration = new DeepDiffConfiguration();
        hastableNaiveComparerDiffConfiguration
           .Entity<NavigationEntityLevel0>()
           .HasKey(x => new { x.StartsOn, x.Direction }, opt => opt.UsePrecompiledEqualityComparer(false))
           .HasValues(x => new { x.RequestedPower, x.Penalty }, opt => opt.UsePrecompiledEqualityComparer(false))
           .OnUpdate(cfg => cfg.CopyValues(x => x.AdditionalValueToCopy))
           .HasOne(x => x.SubEntity)
           .HasMany(x => x.SubEntities)
           .OnInsert(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Insert))
           .OnDelete(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete))
           .OnUpdate(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete));
        hastableNaiveComparerDiffConfiguration
            .Entity<NavigationEntityLevel1>()
            .HasKey(x => x.Timestamp, opt => opt.UsePrecompiledEqualityComparer(false))
            .HasValues(x => new { x.Power, x.Price }, opt => opt.UsePrecompiledEqualityComparer(false))
            .HasOne(x => x.SubEntity)
            .HasMany(x => x.SubEntities)
            .OnInsert(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Insert))
            .OnDelete(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete))
            .OnUpdate(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete));
        hastableNaiveComparerDiffConfiguration
           .Entity<NavigationEntityLevel2>()
           .HasKey(x => x.DeliveryPointEan, opt => opt.UsePrecompiledEqualityComparer(false))
           .HasValues(x => new { x.Value1, x.Value2 }, opt => opt.UsePrecompiledEqualityComparer(false))
           .OnInsert(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Insert))
           .OnDelete(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete))
           .OnUpdate(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete));
        HastableNaiveComparerDeepDiff = hastableNaiveComparerDiffConfiguration.CreateDeepDiff();

        var hashtableDiffConfiguration = new DeepDiffConfiguration();
        hashtableDiffConfiguration
            .Entity<NavigationEntityLevel0>()
            .HasKey(x => new { x.StartsOn, x.Direction })
            .HasValues(x => new { x.RequestedPower, x.Penalty })
            .OnUpdate(cfg => cfg.CopyValues(x => x.AdditionalValueToCopy))
            .HasOne(x => x.SubEntity)
            .HasMany(x => x.SubEntities)
            .OnInsert(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Insert))
            .OnDelete(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete))
            .OnUpdate(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete));
        hashtableDiffConfiguration
            .Entity<NavigationEntityLevel1>()
            .HasKey(x => x.Timestamp)
            .HasValues(x => new { x.Power, x.Price })
            .HasOne(x => x.SubEntity)
            .HasMany(x => x.SubEntities)
            .OnInsert(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Insert))
            .OnDelete(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete))
            .OnUpdate(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete));
        hashtableDiffConfiguration
           .Entity<NavigationEntityLevel2>()
           .HasKey(x => x.DeliveryPointEan)
           .HasValues(x => new { x.Value1, x.Value2 })
           .OnInsert(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Insert))
           .OnDelete(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete))
           .OnUpdate(cfg => cfg.SetValue(x => x.PersistChange, PersistChange.Delete));
        HashtablePrecompiledComparerDeepDiff = hashtableDiffConfiguration.CreateDeepDiff();
    }

    [Params(10, 100)]
    public int N { get; set; }

    [Params(DataGenerationOptions.Identical, DataGenerationOptions.NoExisting, DataGenerationOptions.NoNew, DataGenerationOptions.Random)]
    public DataGenerationOptions Option { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        switch (Option)
        {
            case DataGenerationOptions.Identical:
                GenerateIdentical();
                break;
            case DataGenerationOptions.NoExisting:
                GenerateNoExisting();
                break;
            case DataGenerationOptions.NoNew:
                GenerateNoNew();
                break;
            case DataGenerationOptions.Random:
                GenerateRandom();
                break;
        }
    }

    [Benchmark]
    public void NoHashtableNaiveComparerDiff()
    {
        var results = NoHashtableNaiveComparerDeepDiff.MergeMany(ExistingEntities, NewEntities, cfg => cfg.UseHashtable(false).GenerateOperations(false)).Entities.ToList();
    }

    [Benchmark]
    public void NoHastablePrecompileComparerDiff()
    {
        var results = NoHastablePrecompiledComparerDeepDiff.MergeMany(ExistingEntities, NewEntities, cfg => cfg.UseHashtable(false).GenerateOperations(false)).Entities.ToList();
    }

    [Benchmark]
    public void HastableNaiveComparerDiff()
    {
        var results = HastableNaiveComparerDeepDiff.MergeMany(ExistingEntities, NewEntities, cfg => cfg.GenerateOperations(false)).Entities.ToList();
    }

    [Benchmark]
    public void HashtablePrecompileComparerDiff()
    {
        var results = HashtablePrecompiledComparerDeepDiff.MergeMany(ExistingEntities, NewEntities, cfg => cfg.GenerateOperations(false)).Entities.ToList();
    }

    private void GenerateIdentical()
    {
        var now = DateTime.Now;
        ExistingEntities = GenerateEntities(now, N).ToArray();
        NewEntities = GenerateEntities(now, N).ToArray();
    }

    private void GenerateNoExisting()
    {
        ExistingEntities = Array.Empty<NavigationEntityLevel0>();
        NewEntities = GenerateEntities(DateTime.Now, N).ToArray();
    }

    private void GenerateNoNew()
    {
        ExistingEntities = GenerateEntities(DateTime.Now, N).ToArray();
        NewEntities = Array.Empty<NavigationEntityLevel0>();
    }

    private void GenerateRandom()
    {
        var now = DateTime.Now;
        ExistingEntities = GenerateEntities(now, N).ToArray();
        NewEntities = GenerateEntities(now, N).ToArray();
        foreach (var entity0 in NewEntities)
        {
            if (Random.Next(3) == 0)
                entity0.StartsOn = entity0.StartsOn.AddMonths(-1); // substract to ensure no conflict with other data
            foreach (var entity1 in entity0.SubEntities)
            {
                if (Random.Next(4) == 0)
                    entity1.Timestamp = entity1.Timestamp.AddMonths(-1); // substract to ensure no conflict with other data
                foreach (var entity2 in entity1.SubEntities)
                {
                    if (Random.Next(5) == 0)
                        entity2.DeliveryPointEan = entity2.DeliveryPointEan + "_MOD";
                }
            }
        }
    }

    private static IEnumerable<NavigationEntityLevel0> GenerateEntities(DateTime? now, int n)
    {
        return Enumerable.Range(0, n)
            .Select(x => new NavigationEntityLevel0
            {
                Id = Guid.NewGuid(),

                StartsOn = (now ?? DateTime.Now).AddHours(x),
                Direction = Direction.Up,

                RequestedPower = x,
                Penalty = x,

                SubEntity = new NavigationEntityLevel1
                {
                    Id = Guid.NewGuid(),

                    Timestamp = now ?? DateTime.Now,

                    Power = x + 1000,
                    Price = x * 1000,

                    SubEntities = Enumerable.Range(0, 255)
                            .Select(z => new NavigationEntityLevel2
                            {
                                Id = Guid.NewGuid(),

                                DeliveryPointEan = $"DP_{x}_{1000}_{z}",

                                Value1 = x + 1000 + z,
                                Value2 = x * 1000 * z,
                            }).ToList()
                },

                SubEntities = Enumerable.Range(0, 96)
                    .Select(y => new NavigationEntityLevel1
                    {
                        Id = Guid.NewGuid(),

                        Timestamp = (now ?? DateTime.Now).AddHours(x).AddMinutes(y),

                        Power = x + y,
                        Price = x * y,

                        SubEntity = new NavigationEntityLevel2
                        {
                            Id = Guid.NewGuid(),

                            DeliveryPointEan = $"DP_{x}_{y}_{1000}",

                            Value1 = x + y + 1000,
                            Value2 = x * y * 1000,
                        },

                        SubEntities = Enumerable.Range(0, 255)
                            .Select(z => new NavigationEntityLevel2
                            {
                                Id = Guid.NewGuid(),

                                DeliveryPointEan = $"DP_{x}_{y}_{z}",

                                Value1 = x + y + z,
                                Value2 = x * y * z,
                            }).ToList()
                    }).ToList()
            }).ToList();
    }

    public enum DataGenerationOptions
    {
        Identical,
        NoExisting,
        NoNew,
        Random
    }
}
