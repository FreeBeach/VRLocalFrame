using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// </summary>

namespace Waha.Common
{
    public class RootMonoBehaviour : MonoBehaviour
    {
        private static GameObject s_RootObj = null;
        public static Transform Root
        {
            get
            {
                if (null == s_RootObj)
                {
                    s_RootObj = new GameObject("[RootManager]");
                    s_RootObj.AddComponent<RootMonoBehaviour>();
                    if (Application.isPlaying)
                        GameObject.DontDestroyOnLoad(s_RootObj);
                }

                return s_RootObj.transform;
            }
        }
    }

    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        private static object _lock = new object();

        private static GameObject s_RootObj = null;

        public static T GetInstance()
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                                 "' already destroyed on application quit." +
                                 " Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                                       " - there should never be more than 1 singleton!" +
                                       " Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = typeof(T).ToString();
                        singleton.transform.SetParent(RootMonoBehaviour.Root);
                        var flags = HideFlags.DontSaveInBuild | HideFlags.NotEditable;
                        singleton.hideFlags = flags;

                        Debug.Log("[Singleton] An instance of " + typeof(T) +
                                  " is needed in the scene, so '" + singleton +
                                  "' was created.");
                    }
                    else
                    {
                        //Debug.Log("[Singleton] Using instance already created: " + _instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }

        private static bool IsDontDestroyOnLoad()
        {
            if (_instance == null)
            {
                return false;
            }
            // Object exists independent of Scene lifecycle, assume that means it has DontDestroyOnLoad set
            if ((_instance.gameObject.hideFlags & HideFlags.DontSave) == HideFlags.DontSave)
            {
                return true;
            }
            return false;
        }

        private static bool applicationIsQuitting = false;
        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed, 
        ///   it will create a buggy ghost object that will stay on the Editor scene
        ///   even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (IsDontDestroyOnLoad())
            {
                applicationIsQuitting = true;
            }
        }
    }
}
