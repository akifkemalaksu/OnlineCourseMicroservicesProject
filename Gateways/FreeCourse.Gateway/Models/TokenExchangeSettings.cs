namespace FreeCourse.Gateway.Models
{
    public class TokenExchangeSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TokenGrantType { get; set; }
        public string SubjectTokenType { get; set; }
        public string Scope { get; set; }
    }
}
