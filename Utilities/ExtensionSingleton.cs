namespace AXitUnityTemplate.Utilities
{
    using UnityEngine;
    using JetBrains.Annotations;

    public abstract class Singleton<T> : Singleton where T : MonoBehaviour
    {
        #region Fields

        [CanBeNull] private static T _instance;

        [NotNull]
        // ReSharper disable once StaticMemberInGenericType
        private static readonly object Lock = new object();

        private bool _persistent = true;

        #endregion

        #region Properties

        [NotNull]
        public static T Instance
        {
            get
            {
                if (Singleton.Quitting)
                {
                    Debug.LogWarning($"[{nameof(Singleton)}<{typeof(T)}>] Instance will not be returned because the application is quitting.");

                    // ReSharper disable once AssignNullToNotNullAttribute
                    return null;
                }

                lock (Singleton<T>.Lock)
                {
                    if (Singleton<T>._instance != null)
                        return Singleton<T>._instance;
                    var instances = Object.FindObjectsOfType<T>();
                    var count     = instances.Length;
                    if (count > 0)
                    {
                        if (count == 1)
                            return Singleton<T>._instance = instances[0];
                        Debug.LogWarning(
                            $"[{nameof(Singleton)}<{typeof(T)}>] There should never be more than one {nameof(Singleton)} of type {typeof(T)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");
                        for (var i = 1; i < instances.Length; i++)
                            Object.Destroy(instances[i]);

                        return Singleton<T>._instance = instances[0];
                    }

                    Debug.Log($"[{nameof(Singleton)}<{typeof(T)}>] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");

                    return Singleton<T>._instance = new GameObject($"({nameof(Singleton)}){typeof(T)}")
                        .AddComponent<T>();
                }
            }
        }

        #endregion

        #region Methods

        private void Awake()
        {
            if (this._persistent)
                Object.DontDestroyOnLoad(this.gameObject);
            this.OnAwake();
        }

        protected virtual void OnAwake() { }

        #endregion
    }

    public abstract class Singleton : MonoBehaviour
    {
        #region Properties

        public static bool Quitting { get; private set; }

        #endregion

        #region Methods

        private void OnApplicationQuit() { Singleton.Quitting = true; }

        #endregion
    }
}