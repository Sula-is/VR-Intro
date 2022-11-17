using UnityEngine;

/// <summary>
/// Destroys this object
/// </summary>
public class DestroyObject : MonoBehaviour {
    [Tooltip("If enabled calls destroy on start")]
    public bool _destroyImmediate = true;

    [Tooltip("Time before destroying in seconds, can be set to 0")]
    public float _destroyDelay = 5.0f;

    private void Start() {
        if (_destroyImmediate) {
            Destroy(gameObject, _destroyDelay);
        }
    }

    /// <summary>
    /// Calls the destroy function
    /// </summary>
    public void DestroyThisObject() {
        Destroy(gameObject, _destroyDelay);
    }
}

