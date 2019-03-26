namespace Liberator.RESTore.Access
{
    public interface IToken
    {
        string UserName { get; set; }

        string Password { get; set; }

        string GetAccessToken(string userName, string password, string[] scopes, string clientId, string authority);
    }
}
