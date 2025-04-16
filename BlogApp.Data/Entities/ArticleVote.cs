namespace BlogApp.Data.Entities
{
    public class ArticleVote
    {
        public int Id { get; set; }
        public bool VoteValue { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
