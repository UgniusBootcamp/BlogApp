namespace BlogApp.Data.Helpers.Roles
{
    public class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string BlogUser = nameof(BlogUser);
        public const string Voter = nameof(Voter);
        public const string Author = nameof(Author);
        public const string Commentator = nameof(Commentator);

        public static readonly IReadOnlyCollection<string> All = [
            Admin,
            BlogUser,
            Voter,
            Author,
            Commentator,
            ];
    }
}
