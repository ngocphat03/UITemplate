namespace UITemplate.Scripts.Extension.StateMachine.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UITemplate.Scripts.Extension.StateMachine.Interface;
    using UITemplate.Scripts.Extension.StateMachine.Signal;
    using Zenject;

    public abstract class StateMachine : IStateMachine, ITickable
    {
        private readonly   SignalBus                signalBus;
        protected readonly Dictionary<Type, IState> TypeToState;

        protected StateMachine(
            List<IState> listState,
            SignalBus signalBus
        )
        {
            this.signalBus   = signalBus;
            this.TypeToState = listState.ToDictionary(state => state.GetType(), state => state);
        }

        public IState CurrentState { get; private set; }

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
            if (this.CurrentState != null)
            {
                this.CurrentState.Exit();
                this.signalBus.Fire(new OnStateExitSignal(this.CurrentState));
            }

            this.CurrentState = nextState;
            this.signalBus.Fire(new OnStateEnterSignal(this.CurrentState));
            nextState.Enter();
        }

        public void Tick()
        {
            if (this.CurrentState is not ITickable tickableState) return;
            tickableState.Tick();
        }
    }
}