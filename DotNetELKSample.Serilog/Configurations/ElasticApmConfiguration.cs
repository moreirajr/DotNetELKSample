namespace DotNetELKSample.Serilog.Configurations
{
    public class ElasticApmConfiguration
    {
        public string SecretToken { get; set; }
        public string ServerUrls { get; set; }
        public string ServiceName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}