namespace Vestis.Core.Failures
{
    public class UserHasNoWardrobeFailure : IFailure
    {
        public string FailureKey => "UserHasNoWardrobe";

        public string InternalData => $"User {_user} has no wardrobe.";

        private readonly string _user;

        internal UserHasNoWardrobeFailure(string user) => _user = user;
    }
}
