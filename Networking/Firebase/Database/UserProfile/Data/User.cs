namespace AXitUnityTemplate.Networking.Firebase.Database.UserProfile.Data
{
    using System.Collections.Generic;

    public class User
    {
        public string       UserId ;
        public string       UserName ;
        public List<Item>   Inventory ;
        public List<Friend> Friends ;
        public int          HighScore ;
        
        public User()
        {
        }
        public User(string userId, string userName, int highScore)
        {
            this.UserId    = userId;
            this.UserName  = userName;
            this.HighScore = highScore;
        }
    }
}