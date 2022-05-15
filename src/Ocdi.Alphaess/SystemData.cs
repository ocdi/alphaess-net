using System.Text.Json.Serialization;

namespace Ocdi.Alphaess;

public class SystemData
{
    [JsonPropertyName("sys_sn")]
    public string SystemSerialNumber { get; set; }

    [JsonPropertyName("sys_name")]
    public string SystemName {get;set;}

    [JsonPropertyName("popv")]
    public float SolarPowerKW { get; set; }

    [JsonPropertyName("minv")]
    public string InverterModel { get; set; }
    [JsonPropertyName("poinv")]
    public float InverterPowerKW { get; set; }

    [JsonPropertyName("cobat")]
    public float BatteryCapacity { get; set; }

    [JsonPropertyName("mbat")]
    public string BatteryModel { get; set; }

    [JsonPropertyName("uscapacity")]
    public float UsuableCapacity { get; set; }

    [JsonPropertyName("ems_status")]
    public string SystemStatus { get; set; }

}