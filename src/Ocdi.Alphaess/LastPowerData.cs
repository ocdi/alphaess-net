namespace Ocdi.Alphaess;

public class LastPowerRequest
{
    public string sys_sn { get; set; }
    public bool noLoading { get; set; }
}


public class LastPowerData
{
    public float ppv1 { get; set; }
    public float ppv2 { get; set; }
    public float ppv3 { get; set; }
    public float ppv4 { get; set; }
    public float preal_l1 { get; set; }
    public float preal_l2 { get; set; }
    public float preal_l3 { get; set; }
    public float pmeter_l1 { get; set; }
    public float pmeter_l2 { get; set; }
    public float pmeter_l3 { get; set; }
    public float pmeter_dc { get; set; }
    public float soc { get; set; }
    public float pbat { get; set; }
    public float ev1_power { get; set; }
    public float ev2_power { get; set; }
    public float ev3_power { get; set; }
    public float ev4_power { get; set; }
    public string createtime { get; set; }
}
