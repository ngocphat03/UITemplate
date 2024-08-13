#if LEADERBOARD
namespace UITemplate.Leaderboard
{
    using Zenject;

    public class LeaderboardInstall : Installer<LeaderboardInstall>
    {
        public override void InstallBindings() { this.Container.BindInterfacesAndSelfTo<LeaderboardService>().AsSingle().NonLazy(); }
    }
}
#endif