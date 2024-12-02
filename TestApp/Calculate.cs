using DeepDiff;
using Serilog;
using TestApp.Entities;
using TestApp.Entities.ActivationControl;

namespace TestApp;

public class Calculate : ICalculate
{
    private ILogger Logger { get; }
    private IDeepDiff DeepDiff { get; }

    public Calculate(ILogger logger, IDeepDiff deepDiff)
    {
        Logger = logger;
        DeepDiff = deepDiff;
    }

    public void Perform(Date deliveryDate)
    {
        Logger.Information($"Start calculation for {deliveryDate}");

        var existing = Generate(deliveryDate, ActivationControlStatus.Validated, "INTERNAL", "TSO");
        var calculated = Generate(deliveryDate, ActivationControlStatus.Calculated, null!, null!);
        calculated.TotalEnergyToBeSupplied = 5m;
        calculated.ActivationControlDetails[5].DpDetails[2].TimestampDetails[7].EnergySupplied = -7m;

        var result =  DeepDiff.MergeSingle(existing, calculated);

        Logger.Information($"result?: {result.Entity != null}");
    }

    private static ActivationControl Generate(Date deliveryDate, ActivationControlStatus status, string internalComment, string tsoComment)
        => new ()
        {
            Day = deliveryDate,
            ContractReference = "CREF",

            TotalEnergyRequested = 1,
            TotalDiscrepancy = 2,
            TotalEnergyToBeSupplied = 3,

            Status = status,
            InternalComment = internalComment,
            TsoComment = tsoComment,

            ActivationControlDetails = Enumerable.Range(0, 96)
                .Select(x => new ActivationControlDetail
                {
                    StartsOn = deliveryDate.UtcDateTime.AddMinutes(15 * x),

                    OfferedVolumeUp = x,
                    OfferedVolumeDown = 2 * x,
                    OfferedVolumeForRedispatchingUp = 3 * x,
                    OfferedVolumeForRedispatchingDown = 4 * x,
                    PermittedDeviationUp = 5 * x,
                    PermittedDeviationDown = 6 * x,
                    RampingRate = 7 * x,
                    HasJump = x % 2 == 0,

                    TimestampDetails = Enumerable.Range(0, 255)
                        .Select(y => new ActivationControlTimestampDetail
                        {
                            Timestamp = deliveryDate.UtcDateTime.AddMinutes(15 * x).AddSeconds(4 * y),

                            PowerMeasured = x * y,
                            PowerBaseline = 2 * x * y,
                            FcrCorrection = 3 * x * y,
                            EnergyRequested = 4 * x * y,
                            EnergyRequestedForRedispatching = 5 * x * y,
                            EnergySupplied = 6 * x * y,
                            EnergyToBeSupplied = 7 * x * y,
                            Deviation = 8 * x * y,
                            PermittedDeviation = 9 * x * y,
                            MaxDeviation = 10 * x * y,
                            Discrepancy = 11 * x * y,
                            IsJumpExcluded = (x + y) % 2 == 0,
                            IsMeasurementExcluded = false,
                        }).ToList(),

                    DpDetails = Enumerable.Range(0, 10)
                        .Select(y => new ActivationControlDpDetail
                        {
                            DeliveryPointEan = $"DPEAN_{x * y}",

                            DeliveryPointName = $"DPNAME_{x * y}",
                            Direction = (x * y) % 2 == 0 ? Direction.Up : Direction.Down,
                            DeliveryPointType = (2 * x * y) % 2 == 0 ? DeliveryPointType.SingleUnit : DeliveryPointType.ProvidingGroup,
                            TotalEnergySupplied = 3 * x * y,

                            TimestampDetails = Enumerable.Range(0, 255)
                                .Select(z => new ActivationControlDpTimestampDetail
                                {
                                    Timestamp = deliveryDate.UtcDateTime.AddMinutes(15 * x).AddSeconds(4 * z),

                                    PowerMeasured = x * y * z,
                                    PowerBaseline = 2 * x * y * z,
                                    FcrCorrection = 3 * x * y * z,
                                    EnergySupplied = 6 * x * y * z,
                                }).ToList(),
                        }).ToList(),

                }).ToList()
        };
}
