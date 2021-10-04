using UnityEngine;
using Utility;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (SingletonBehaviour<T>.instance == null)
            {
                SingletonBehaviour<T>.instance = Object.FindObjectOfType(typeof(T)) as T;
            }
            return SingletonBehaviour<T>.instance;
        }
    }

    public static bool Exists
    {
        get { return SingletonBehaviour<T>.Instance != null && SingletonBehaviour<T>.Instance.enabled; }
    }

    /// <summary>
    /// Throws an exception if the SingletonBehavior does not exist in the scene.
    /// </summary>
    public static void ThrowIfMissing()
    {
        ThrowIf.False(Exists, "SingletonBehaviour {0} does not exist", typeof (T).Name);
    }

    protected virtual void Awake()
    {
        instance = this as T;
    }
}