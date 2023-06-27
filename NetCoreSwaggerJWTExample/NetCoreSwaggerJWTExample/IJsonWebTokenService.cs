namespace NetCoreSwaggerJWTExample
{
    public interface IJsonWebTokenService
    {
        public string GenerateToken(string user);
    }
}
