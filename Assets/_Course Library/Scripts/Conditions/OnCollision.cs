using System;

using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Calls functionality when a collision occurs
/// </summary>
public class OnCollision : MonoBehaviour {
    public string requiredTag = string.Empty;
    [Serializable] public class CollisionEvent : UnityEvent<Collision> { }

    // When the object enters a collision
    public CollisionEvent OnEnter = new CollisionEvent();

    // When the object exits a collision
    public CollisionEvent OnExit = new CollisionEvent();

    // When the object enters a collision and matches the required tag
    public CollisionEvent OnEnterAction = new CollisionEvent();

    // When the object exits a collision and matches the required tag
    public CollisionEvent OnExitAction = new CollisionEvent();

    private void OnCollisionEnter(Collision collision) {
        OnEnter.Invoke(collision);
        if (CanTriggerActionTag(collision.gameObject)) {
            OnEnterAction.Invoke(collision);
        }
    }

    private void OnCollisionExit(Collision collision) {
        OnExit.Invoke(collision);
        if (CanTriggerActionTag(collision.gameObject)) {
            OnExitAction.Invoke(collision);
        }
    }

    private void OnValidate() {
        if (TryGetComponent(out Collider collider))
            collider.isTrigger = false;
    }

    private bool CanTriggerActionTag(GameObject otherGameObject) {
        if (requiredTag != string.Empty) {
            return otherGameObject.CompareTag(requiredTag);
        }
        else {
            return true;
        }
    }


}
