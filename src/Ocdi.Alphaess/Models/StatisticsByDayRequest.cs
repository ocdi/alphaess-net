namespace Ocdi.Alphaess.Models;

public class StatisticsByDayRequest
{
    public string sn { get; set; }
    /// <summary>
    /// Seems to be the SN
    /// </summary>
    public string userId { get; set; }

    /// <summary>
    /// Seems to be zero
    /// </summary>
    public int isOEM { get; set; }

    /// <summary>
    /// Date in yyyy-mm-dd format, for the day to get the data requested
    /// </summary>
    public string szDay { get; set; }
    /// <summary>
    /// Date in yyyy-mm-dd format, seems to be today's date
    /// </summary>
    public string sDate { get; set; }
}
