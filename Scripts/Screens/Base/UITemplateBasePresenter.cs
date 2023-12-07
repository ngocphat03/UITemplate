﻿namespace UITemplate.Scripts.Screens.Base
{
    using UITemplate.Scripts.Interface;

    public abstract class UITemplateBaseScreenPresenter<TView> : BaseScreenPresenter<TView> where TView : IScreenView
    {
        protected override void OnViewReady() { base.OnViewReady(); }
    }

    public abstract class UITemplateBaseScreenPresenter<TView, TModel> : BaseScreenPresenter<TView, TModel> where TView : IScreenView
    {
        protected override void OnViewReady() { base.OnViewReady(); }
    }
}