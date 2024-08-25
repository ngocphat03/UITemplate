#if LEADERBOARD
namespace UITemplate.Leaderboard
{
    using AXitUnityTemplate.Networking.FirebaseNetwork.Leaderboard;
    using Zenject;

    public class LeaderboardInstall : Installer<LeaderboardInstall>
    {
        public override void InstallBindings() { this.Container.BindInterfacesAndSelfTo<LeaderboardService>().AsSingle().NonLazy(); }
    }
}
#endif