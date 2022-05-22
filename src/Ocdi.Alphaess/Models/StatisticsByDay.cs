namespace Ocdi.Alphaess.Models;

/// <summary>
/// Each period in an array covers 5 minutes, with 288 elements in the array
/// The times are specified in the Time array
/// </summary>
public class StatisticsByDay
{
    public string[] Time { get; set; } = Array.Empty<string>();
    public float[] Ppv { get; set; } = Array.Empty<float>();
    public float[] DieselPv { get; set; } = Array.Empty<float>();
    public float[] UsePower { get; set; } = Array.Empty<float>();
    public float[] HomePower { get; set; } = Array.Empty<float>();
    public float[] ChargingPile { get; set; } = Array.Empty<float>();
    public float[] Cbat { get; set; } = Array.Empty<float>();
    public float[] FeedIn { get; set; } = Array.Empty<float>();
    public float[] GridCharge { get; set; } = Array.Empty<float>();
    public float Ebat { get; set; }
    public float Epvtoday { get; set; }
    public float ELoad { get; set; }
    public float EHomeLoad { get; set; }
    public float EChargingPile { get; set; }
    public float Echarge { get; set; }
    public float EGridCharge { get; set; }
    public float EInput { get; set; }
    public float EFeedIn { get; set; }
    public float EDiesel { get; set; }
    public float Soc { get; set; }
    public int LasIdx { get; set; }
    public string? PowerSource { get; set; }
    public float MaximumPower { get; set; }
    public float MaxPpv { get; set; }
    public float MaxUsePower { get; set; }
    public float MaxFeedIn { get; set; }
    public float MaxGridCharge { get; set; }
    public float MaxDieselPv { get; set; }
    public float MaxHomePile { get; set; }
    public float MaxChargingPile { get; set; }
    public bool hasChargingPile { get; set; }
    public float[] DispatchSwitch { get; set; } = Array.Empty<float>();
    public object? DispatchSwitchArr { get; set; }


}
