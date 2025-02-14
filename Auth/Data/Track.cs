namespace Auth.Data
{
    public class Track
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public  ICollection<UserTrackRecommendation> Recommendations { get; set; }
    }


    public class UserTrackRecommendation
    {
        public int Id { get; set; }
        public string UserId { get; set; }  
        public int TrackId { get; set; }
        public DateTime CreatedAt { get; set; }
        public  Track Track { get; set; }
    }
}
