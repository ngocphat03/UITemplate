namespace AXitUnityTemplate.MVP.Core
{
    using Zenject;
    using AXitUnityTemplate.MVP.Interface;

    public class MvpFactory 
    {
        private readonly DiContainer diContainer;

        public MvpFactory(DiContainer diContainer) { this.diContainer = diContainer; }

        public T Create<T>(IModel model) where T : IPresenter
        {
            var presenter = this.diContainer.Instantiate<T>();
            presenter.Initialize(model);

            return presenter;
        }
    }
}