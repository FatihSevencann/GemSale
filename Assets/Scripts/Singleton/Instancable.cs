
using UnityEngine;
public abstract class Instancable<T> : MonoBehaviour where T : class
{
    public static T instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = GameObject.FindObjectOfType(typeof(T)) as T;
        }
    }
}