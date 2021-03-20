using UnityEngine;

namespace Game.Scripts
{
    /// <summary>
    /// Base singleton class. Should be used only for entities that persist throughout game lifecycle.
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        private static bool _cantBeActive = false;

        public static T Instance
        {
            get
            {
                if (_cantBeActive)
                    return null;

                if (_instance != null)
                    return _instance;

                _instance = new GameObject(typeof(T).Name).AddComponent<T>();

                return _instance;
            }
        }

        public static bool CantBeActive => _cantBeActive;

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);


                Application.quitting += HandleApplicationQuitting;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance != this)
                return;

            Application.quitting -= HandleApplicationQuitting;
        }

        private void HandleApplicationQuitting()
        {
            _cantBeActive = true;
        }
    }
}
