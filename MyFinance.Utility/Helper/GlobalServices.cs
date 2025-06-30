using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Utility.Helper;

public class GlobalServices : IGlobalServices
{
    JsSession _jsSession = new JsSession();
    public GlobalServices(JsSession session)
    {
        _jsSession = session;
    }

    public string GetAuthToken()
    {
        return _jsSession.AccessToken ?? string.Empty;
    }

    public JsSession GetSession()
    {
        return _jsSession;
    }
    public UserInfo GetUserInfo()
    {
        return new UserInfo
        {
            Id = _jsSession.User?.Id,
            Email = _jsSession.User?.Email,
            Name = _jsSession.GetUserMetadataString("name") ?? string.Empty
        };
    }   

    public static string AuthToken { get; set; } = string.Empty;
    public static UserInfo UserInfo { get; set; } = new UserInfo();
    public static JsSession JsSession { get; set; } = new JsSession();
}

public interface IGlobalServices
{
    string GetAuthToken();
    JsSession GetSession();
    UserInfo GetUserInfo();
}
