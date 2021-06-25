using UnityEngine;
public interface ISingleton {
    void Init();
}
public abstract class MonoSingleton<T> : MonoBehaviour , ISingleton where T : Component
{
    public static T Instance;

    public virtual void Awake() {

        if (isActiveAndEnabled)
        {
            if (Instance == null)
                Instance = this as T;
            else if (Instance != this as T)
                Destroy(this);

        }
    
    }




    public abstract void Init();
}
