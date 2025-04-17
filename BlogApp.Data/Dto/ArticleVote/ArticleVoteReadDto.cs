namespace BlogApp.Data.Dto.ArticleVote
{
    public class ArticleVoteReadDto
    {
        public int ArticleId { get; set; }
        public bool? VoteValue { get; set; }

        public int PositiveVotes { get; set; }
        public int NegativeVotes { get; set; }
    }
}
