using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
            }

            return instance;
        }
    }

    public static bool HasInstance
    {
        get
        {
            return instance != null;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
            instance = this as T;
    }

    public void OnDestroy()
    {
        Destroy();
    }

    public void Destroy()
    {
        if (instance == this)
            instance = null;
    }
}