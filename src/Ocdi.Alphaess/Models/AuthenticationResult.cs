namespace Ocdi.Alphaess.Models;

public partial class ApiResult<T>
{

    public long Code { get; set; }


    public string? Info { get; set; }


    public T? Data { get; set; }
}

public partial class AccessData
{
    public string AccessToken { get; set; } = null!;

    public float ExpiresIn { get; set; }

    public string? TokenCreateTime { get; set; }

    public string RefreshTokenKey { get; set; } = null!;
}

