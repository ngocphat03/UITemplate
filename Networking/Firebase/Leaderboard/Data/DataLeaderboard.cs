#if LEADERBOARD
namespace AXitUnityTemplate.Networking.Firebase.Leaderboard.Data
{
    public class DataLeaderboard
    {
        public string UserName;
        public int    TotalWin;

        public DataLeaderboard(string userName, int totalWin)
        {
            this.UserName = userName;
            this.TotalWin = totalWin;
        }
    }
}
#endif