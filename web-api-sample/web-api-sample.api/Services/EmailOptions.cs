namespace web_api_sample.api.Services
{
    public class EmailOptions
    {
        public string PrimaryDomain { get; set; }

        public int PrimaryPort { get; set; }

        public string UsernameEmail { get; set; }

        public string UsernamePassword { get; set; }
    }
}