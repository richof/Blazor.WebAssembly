namespace Blazor.WebAsembly.Services.Configurations
{
    public class TokenSettings
    {
        public string Secret { get; set; }
        public int Expire { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
