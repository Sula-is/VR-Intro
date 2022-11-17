using UnityEngine;
/// <summary>
/// Inherit a code from this, givin as Type T the code itself, and it magically become a singleton (if someone require his instance and it doesn't exists or it is disable, this code will create a new Instace of the code)
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour
    where T : Component {

    private static T _instance;

    public static T Instance {
        get {
            if (_instance == null) {

                T[] objs = FindObjectsOfType(typeof(T)) as T[];

                if (objs.Length > 0)
                    _instance = objs[0];

                if (objs.Length > 1)
                    throw new System.Exception($"There's more than one {typeof(T).Name} inside this scene");

                if (_instance == null) {
                    GameObject obj = new GameObject();
                    obj.name = string.Format("_{0}", typeof(T).Name);
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }
}