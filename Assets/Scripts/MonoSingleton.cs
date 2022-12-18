
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                }
            }
            else
            {
            }
            return instance;
        }
    }
    protected void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            Init();
        }
    }
    public virtual void Init()
    {
    }
}

