using System;
using UnityEngine;

namespace Amanotes.Utils
{

	//========================================================
	// class BaseSingleton StrongD Add 2016-02-19
	//========================================================
	// - for making singleton object
	// - usage
	//		+ declare class(derived )	
	//			public class OnlyOne : BaseSingleton< OnlyOne >
	//		+ client
	//			OnlyOne.Instance.[method]
	//========================================================
	public abstract class Singleton<T> where T : new()
	{
		private static T singleton;
		public static T Instance
		{
			get
			{
				if (singleton == null)
				{
					singleton = new T();
				}
				return singleton;
			}
		}
		public static T instance
		{
			get
			{
				if (singleton == null)
				{
					singleton = new T();
				}
				return singleton;
			}
		}
	}

	//========================================================
	// class BaseSingleton StrongD Add 2016-02-19
	//========================================================
	// - for making singleton object mono in scene
	// - usage
	//		+ declare class(derived )	
	//			public class OnlyOne : BaseSingleton< OnlyOne >
	//		+ client
	//			OnlyOne.Instance.[method]
	//		+ if scene don't have animation instance, it will be auto generate with name prefix '@'
	//		+ if have 2 instance, new instance will auto remove
	//		+ singleton mono just exist in one scene, call don't detroy on load to keep it forever
	//========================================================
	public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T singleton;

		public static bool IsInstanceValid() { return singleton != null; }

        private static bool isApplicationQuitting = false;

		void Reset()
		{
			// gameObject.name = typeof(T).Name;
		}

		public static T Instance
		{
			get
			{
				if (SingletonMono<T>.singleton != null)
				{
					return SingletonMono<T>.singleton;
				}
				if (SingletonMono<T>.singleton == null)
				{
					SingletonMono<T>.singleton = (T)FindObjectOfType(typeof(T));
					if (SingletonMono<T>.singleton == null)
					{
						GameObject obj = new GameObject();
						obj.name = "[@" + typeof(T).Name + "]";
						SingletonMono<T>.singleton = obj.AddComponent<T>();
					}
				}

				return SingletonMono<T>.singleton;
			}
		}
		public static T instance
		{
			get
			{
				if (!Application.isPlaying)
				{
					return null;
				}

                if (isApplicationQuitting)
                {
                    return null;
                }

				if (SingletonMono<T>.singleton == null)
				{
					SingletonMono<T>.singleton = (T)FindObjectOfType(typeof(T));
					if (SingletonMono<T>.singleton == null)
					{
						GameObject obj = new GameObject();
						obj.name = "[@" + typeof(T).Name + "]";
						SingletonMono<T>.singleton = obj.AddComponent<T>();
					}
				}

				return SingletonMono<T>.singleton;
			}
		}

        private void OnApplicationQuit()
        {
            isApplicationQuitting = true;
        }
    }
	public class LazySingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static readonly Lazy<T> lazy = new Lazy<T>(CreateSingleton);
        public static T instance => lazy.Value;
        private static T CreateSingleton()
        {
            GameObject obj = new GameObject();
            obj.name = "[@" + typeof(T).Name + "]";
            return obj.AddComponent<T>();
        }

    }


	public class SingletonMonoNoAutoGen<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T singleton;

		public static bool IsInstanceValid() { return singleton != null; }

		private static bool isApplicationQuitting = false;

		void Reset()
		{
			gameObject.name = typeof(T).Name;
		}

		public static T Instance
		{
			get
			{
				if (SingletonMonoNoAutoGen<T>.singleton != null)
				{
					return SingletonMonoNoAutoGen<T>.singleton;
				}
				if (SingletonMonoNoAutoGen<T>.singleton == null)
				{
					SingletonMonoNoAutoGen<T>.singleton = (T)FindObjectOfType(typeof(T));
				}

				return SingletonMonoNoAutoGen<T>.singleton;
			}
		}
		public static T instance
		{
			get
			{
				if (!Application.isPlaying)
				{
					return null;
				}

				if (isApplicationQuitting)
				{
					return null;
				}

				if (SingletonMonoNoAutoGen<T>.singleton == null)
				{
					SingletonMonoNoAutoGen<T>.singleton = (T)FindObjectOfType(typeof(T));
				}

				return SingletonMonoNoAutoGen<T>.singleton;
			}
		}

		private void OnApplicationQuit()
		{
			isApplicationQuitting = true;
		}
	}

}