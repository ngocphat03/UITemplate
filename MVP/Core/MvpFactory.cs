namespace AXitUnityTemplate.MVP.Core
{
    using AXitUnityTemplate.MVP.Interface;

#if ZENJECT
    using Zenject;

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
#elif VCONTAINER
    using VContainer;

    public class MvpFactory
    {
        private readonly IObjectResolver resolver;

        public MvpFactory(IObjectResolver resolver) { this.resolver = resolver; }

        public T Create<T>(IModel model) where T : IPresenter
        {
            var presenter = this.resolver.Resolve<T>();
            presenter.Initialize(model);

            return presenter;
        }
    }

#else
    public class MvpFactory
    {
        public T Create<T>(IModel model) where T : IPresenter
        {
            var presenter = System.Activator.CreateInstance<T>();
            presenter.Initialize(model);

            return presenter;
        }
    }
#endif
}