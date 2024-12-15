#if ZENJECT
namespace AXitUnityTemplate.StateMachine.Runtime.Base.ZenjectDI
{
    using System;
    using Zenject;
    using System.Linq;
    using System.Collections.Generic;
    using AXitUnityTemplate.StateMachine.Runtime.Interfaces;

    public abstract class StateMachineBase : ITickable, IStateMachine
    {
        protected Dictionary<Type, IState> TypeToState;
        
        public IState CurrentState { get; private set; }
        
        public virtual void Initialize(List<IState> listState)
        {
            this.TypeToState = listState.ToDictionary(state => state.GetType(), state => state);
        }

        public void TransitionTo<T>() where T : class, IState { this.TransitionTo(typeof(T)); }

        public void TransitionTo<TState, TModel>(TModel model) where TState : class, IState<TModel>
        {
            var stateType = typeof(TState);

            if (!this.TypeToState.TryGetValue(stateType, out var nextState)) return;

            if (nextState is not TState nextStateT) return;
            nextStateT.Model = model;

            this.InternalStateTransition(nextState);
        }

        public virtual void TransitionTo(Type stateType)
        {
            if (!this.TypeToState.TryGetValue(stateType, out var nextState)) return;

            this.InternalStateTransition(nextState);
        }

        private void InternalStateTransition(IState nextState)
        {
            this.CurrentState?.Exit();
            this.CurrentState = nextState;
            nextState.Enter();
        }
        
        public void Tick() { this.CurrentState?.Update(); }
    }
}
#endif
