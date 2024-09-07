namespace AXitUnityTemplate.Networking.FirebaseNetwork.Database.UserProfile.Data
{
    using System.Collections.Generic;

    public class User
    {
        public string                   UserId;
        public string                   UserName;
        public string                   FacebookId;
        public Dictionary<string, Item> Inventory;
        public List<Friend>             Friends;
        public int                      HighScore;

        public User() { }

        public User(string userId, string userName, string facebookId, Dictionary<string, Item> inventory, List<Friend> friends, int highScore)
        {
            this.UserId     = userId;
            this.UserName   = userName;
            this.FacebookId = facebookId;
            this.Inventory  = inventory;
            this.Friends    = friends;
            this.HighScore  = highScore;
        }
    }
}