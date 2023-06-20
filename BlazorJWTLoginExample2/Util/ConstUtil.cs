namespace BlazorJWTLoginExample2.Util
{
    public class ConstUtil
    {
        /// <summary>
        /// 發行者
        /// </summary>
        public const string Issuer = "JwtLoginIssuer";

        /// <summary>
        /// 加密金鑰
        /// </summary>
        public const string SignKey = "this_is_a_secure_key_with_length_greater_than_32";

        /// <summary>
        /// 使用者
        /// </summary>
        public const string Audience = "JwtLoginAudience";
    }
}
