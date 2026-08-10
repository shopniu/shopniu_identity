


using Shopniu_identity.Aplication.Authentication.Models;

namespace Shopniu_identity.Aplication.Authentication.Account;

public class AccountHandler
{
    public LoginViewModel GetLogin(string returnUrl)
    {
        return new LoginViewModel { ReturnUrl = returnUrl };
    }
}