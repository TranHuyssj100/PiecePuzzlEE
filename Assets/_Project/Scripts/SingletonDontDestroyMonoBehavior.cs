using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonDontDestroyMonoBehavior<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    // Use this for initialization
    /// <summary>
    /// Get singleton instance.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Call from override Awake method in inheriting classes.
    /// Example : protected override void Awake () { base.Awake (); }
    /// </summary>
    protected virtual void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this);
    }
}
