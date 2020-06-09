using Newtonsoft.Json;//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\created by stanley.df//\\//\\//\\//\\//\\//\\//\\//\\//\\

namespace SkyCinema
{
    public class JsonData
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("total_results")]//
        public int TotalResults { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("results")]
        public Results[] Results { get; set; }

    }

    public class Results
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("vote_average")]
        public float Vote { get; set; }
        
        [JsonProperty("genre_ids")]
        public int[] Ganre { get; set; }

        [JsonProperty("release_date")]
        public string Release { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("poster_path")]//
        public string ImgLink { get; set; }
    }

    public class Solo
    {
        [JsonProperty("budget")]
        public string Budget { get; set; }

        [JsonProperty("genres")]
        public Ganre[] Ganre { get; set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("popularity")]
        public double Popularity { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("vote_average")]
        public float Vote { get; set; }

        [JsonProperty("poster_path")]
        public string Poster { get; set; }
    }
    public class Ganre
    {
        [JsonProperty("name")]
        public string Name { get; set; }

    }
}
