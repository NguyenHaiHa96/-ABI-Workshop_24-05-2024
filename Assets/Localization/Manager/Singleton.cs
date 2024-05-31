using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Ins;
    public bool m_DontDestroyOnLoad = true;

    /// <summary>
    /// Create the singleton instance if needed and call OnSingletonAwake().
    /// </summary>
    private void Awake ()
    {
        if (Ins == null) {
            //If I am the first instance, make me the Singleton
            Ins = this as T;

            if (transform.parent == null && m_DontDestroyOnLoad) {
                DontDestroyOnLoad (this.gameObject);
            }
        } else {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if (this != Ins) {
                DestroyImmediate (this.gameObject);
            }
            return;
        }

        OnSingletonAwake ();
    }

    void OnDestroy ()
    {
        if (Ins == this) {
            Ins = null;
        }
    }

    /// <summary>
    /// This method is called just after the singleton construction.
    /// Override it to perform the initial setup.
    /// </summary>
    protected virtual void OnSingletonAwake ()
    {
    }
}
