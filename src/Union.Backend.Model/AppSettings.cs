namespace Union.Backend.Model
{
    public class AppSettings
    {
        public string Host { get; set; }
        public string ClientSecret { get; set; }
        public string BackSecret { get; set; }

        public static string GEOCAL_API_URL = "https://api-adresse.data.gouv.fr/search";
    }
}
