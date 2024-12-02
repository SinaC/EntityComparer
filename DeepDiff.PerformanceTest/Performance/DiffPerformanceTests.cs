using DeepDiff.Configuration;
using DeepDiff.PerformanceTest.Entities;
using DeepDiff.PerformanceTest.Entities.Simple;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace DeepDiff.PerformanceTest;

public class DiffPerformanceTests
{
    private ITestOutputHelper Output { get; }

    public DiffPerformanceTests(ITestOutputHelper output)
    {
        Output = output;
    }

    [Fact]
    public void Merge()
    {
        var sw = new Stopwatch();
        sw.Start();
        var (existingEntities, newEntities) = GenerateEntities(500);
        sw.Stop();
        Output.WriteLine("Generation: {0} ms", sw.ElapsedMilliseconds);

        var deepDiff = CreateDeepDiff();
        sw.Restart();
        var diff = deepDiff.MergeMany(existingEntities, newEntities);
        var results = diff.Entities.ToArray();
        sw.Stop();

        Output.WriteLine("Diff: {0} ms", sw.ElapsedMilliseconds);
    }

    [Fact]
    public void Merge_NoHashtable()
    {
        var sw = new Stopwatch();
        sw.Start();
        var (existingEntities, newEntities) = GenerateEntities(500);
        sw.Stop();
        Output.WriteLine("Generation: {0} ms", sw.ElapsedMilliseconds);

        var deepDiff = CreateDeepDiff();
        sw.Restart();
        var diff = deepDiff.MergeMany(existingEntities, newEntities, cfg => cfg.UseHashtable(false));
        var results = diff.Entities.ToArray();
        sw.Stop();

        Output.WriteLine("Diff: {0} ms", sw.ElapsedMilliseconds);
    }

    [Fact]
    public void Merge_NaiveComparer()
    {
        var sw = new Stopwatch();
        sw.Start();
        var (existingEntities, newEntities) = GenerateEntities(500);
        sw.Stop();
        Output.WriteLine("Generation: {0} ms", sw.ElapsedMilliseconds);

        var deepDiff = CreateDeepDiff();
        sw.Restart();
        var diff = deepDiff.MergeMany(existingEntities, newEntities, cfg => cfg.UsePrecompiledEqualityComparer(false));
        var results = diff.Entities.ToArray();
        sw.Stop();

        Output.WriteLine("Diff: {0} ms", sw.ElapsedMilliseconds);
    }

    [Fact]
    public void Merge_NoHashtable_NaiveComparer()
    {
        var sw = new Stopwatch();
        sw.Start();
        var (existingEntities, newEntities) = GenerateEntities(500);
        sw.Stop();
        Output.WriteLine("Generation: {0} ms", sw.ElapsedMilliseconds);

        var deepDiff = CreateDeepDiff();
        sw.Restart();
        var diff = deepDiff.MergeMany(existingEntities, newEntities, cfg => cfg.UseHashtable(false).UsePrecompiledEqualityComparer(false));
        var results = diff.Entities.ToArray();
        sw.Stop();

        Output.WriteLine("Diff: {0} ms", sw.ElapsedMilliseconds);
    }

    private static IDeepDiff CreateDeepDiff()
    {
        var diffConfiguration = new DeepDiffConfiguration();
        diffConfiguration.PersistEntity<EntityLevel0>()
            .HasKey(x => new { x.StartsOn, x.Direction })
            .HasValues(x => new { x.RequestedPower, x.Penalty })
            .OnUpdate(cfg => cfg.CopyValues(x => x.AdditionalValueToCopy))
            .HasOne(x => x.SubEntity)
            .HasMany(x => x.SubEntities);
        diffConfiguration.PersistEntity<EntityLevel1>()
            .HasKey(x => x.Timestamp)
            .HasValues(x => new { x.Power, x.Price })
            .HasOne(x => x.SubEntity)
            .HasMany(x => x.SubEntities);
        diffConfiguration.PersistEntity<EntityLevel2>()
            .HasKey(x => x.DeliveryPointEan)
            .HasValues(x => new { x.Value1, x.Value2 });
        var diff = diffConfiguration.CreateDeepDiff();
        return diff;
    }

    private static (EntityLevel0[] existingEntities, EntityLevel0[] newEntities) GenerateEntities(int n)
    {
        var now = DateTime.Now;
        var existingEntities = GenerateEntities(now, n).ToArray();
        var newEntities = GenerateEntities(now, n).ToArray();
        for (var entity0Index = 0; entity0Index < existingEntities.Length; entity0Index++)
        {
            var entity0 = existingEntities[entity0Index];
            if (entity0Index % 3 == 0)
                entity0.StartsOn = entity0.StartsOn.AddMonths(1);
            for (var entity1Index = 0; entity1Index < entity0.SubEntities.Count; entity1Index++)
            {
                var entity1 = entity0.SubEntities[entity1Index];
                if (entity1Index % 4 == 0)
                    entity1.Timestamp = entity1.Timestamp.AddMonths(1);
                for (var entity2Index = 0; entity2Index < entity1.SubEntities.Count; entity2Index++)
                {
                    var entity2 = entity1.SubEntities[entity2Index];
                    if (entity2Index % 5 == 0)
                        entity2.DeliveryPointEan += "_MOD";
                }
            }
        }

        return (existingEntities, newEntities);
    }

    private static IEnumerable<EntityLevel0> GenerateEntities(DateTime? now, int n)
    {
        return Enumerable.Range(0, n)
            .Select(x => new EntityLevel0
            {
                Id = Guid.NewGuid(),

                StartsOn = (now ?? DateTime.Now).AddMinutes(x),
                Direction = Direction.Up,

                RequestedPower = x,
                Penalty = x,

                SubEntity = new EntityLevel1
                {
                    Id = Guid.NewGuid(),

                    Timestamp = now ?? DateTime.Now,

                    Power = x + 500,
                    Price = x * 500,

                    SubEntities = Enumerable.Range(0, 255)
                            .Select(z => new EntityLevel2
                            {
                                Id = Guid.NewGuid(),

                                DeliveryPointEan = $"DP_{x}_{500}_{z}",

                                Value1 = x + 500 + z,
                                Value2 = x * 500 * z,
                            }).ToList()
                },

                SubEntities = Enumerable.Range(0, 96)
                    .Select(y => new EntityLevel1
                    {
                        Id = Guid.NewGuid(),

                        Timestamp = (now ?? DateTime.Now).AddMinutes(x).AddSeconds(y),

                        Power = x + y,
                        Price = x * y,

                        SubEntity = new EntityLevel2
                        {
                            Id = Guid.NewGuid(),

                            DeliveryPointEan = $"DP_{x}_{y}_{500}",

                            Value1 = x + y + 500,
                            Value2 = x * y * 500,
                        },

                        SubEntities = Enumerable.Range(0, 255)
                            .Select(z => new EntityLevel2
                            {
                                Id = Guid.NewGuid(),

                                DeliveryPointEan = $"DP_{x}_{y}_{z}",

                                Value1 = x + y + z,
                                Value2 = x * y * z,
                            }).ToList()
                    }).ToList()
            }).ToList();
    }
}
