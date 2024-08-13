namespace AXitUnityTemplate.Networking.Firebase.Database.UserProfile.Data
{
    public class Friend
    {
        public string       FriendId   { get; set; }
        public string       FriendName { get; set; }
        public FriendStatus Status     { get; set; }

        public Friend(string friendId, string friendName, FriendStatus status)
        {
            this.FriendId   = friendId;
            this.FriendName = friendName;
            this.Status     = status;
        }
    }

    public enum FriendStatus
    {
        Default  = -1,
        Pending  = 0,
        Accepted = 1,
        Rejected = 2,
    }
}