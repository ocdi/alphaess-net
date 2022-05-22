using System.Text.Json.Serialization;

namespace Ocdi.Alphaess.Models;


public class LastPowerData
{
    public float ppv1 { get; set; }
    public float ppv2 { get; set; }
    public float ppv3 { get; set; }
    public float ppv4 { get; set; }
    public float preal_l1 { get; set; }
    public float preal_l2 { get; set; }
    public float preal_l3 { get; set; }

    [JsonPropertyName("pmeter_l1")]
    public float PowerMeterAC { get; set; }
    public float pmeter_l2 { get; set; }
    public float pmeter_l3 { get; set; }
    [JsonPropertyName("pmeter_dc")]
    public float PowerMeterDC { get; set; }

    [JsonPropertyName("soc")]
    public float StateOfCharge { get; set; }

    [JsonPropertyName("pbat")]
    public float PowerBattery { get; set; }
    public float ev1_power { get; set; }
    public float ev2_power { get; set; }
    public float ev3_power { get; set; }
    public float ev4_power { get; set; }

    [JsonPropertyName("createtime")]
    public string CreateTime { get; set; }

    public float Load => PowerMeterDC + PowerBattery + PowerMeterAC;
    public string Description => PowerBattery < 0 ? "Charing" : "Discharging";

    public override string ToString() => $"{StateOfCharge,4:0.0} {PowerMeterDC,8:0.00} {PowerMeterAC,8:0.0} {PowerBattery,8:0.00} {Load,8:0.0} {Description,13}";
}
