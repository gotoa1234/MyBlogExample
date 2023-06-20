namespace BlazorJWTLoginExample2.Repository
{
    public interface ISqliteRepository
    {
        string GetToken(string account);

        void InsertOrUpdateToken(string account, string token);

        void UpdateTokenInValid(string account, string token);
    }
}
