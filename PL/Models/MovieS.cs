namespace PL.Models
{
    public class MovieS
    {
        public int page { get; set; }
        public int id { get; set; }
        public string backdrop_path { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public decimal popularity { get; set; }
        public string release_date { get; set; }
        public string title { get; set; }
     
        public List<object> Movies { get; set; }
    }
}
