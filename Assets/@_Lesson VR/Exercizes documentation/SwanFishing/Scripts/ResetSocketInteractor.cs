//using UnityEngine;

///// <summary>
///// Handles the activation and deactivation of a </><see cref="XRSocketInteractor"/>
///// It's used to disable the socket for a fixed timeframe to enable the detachment of the Interactable
///// </summary>
//[DisallowMultipleComponent]
//[RequireComponent(typeof(XRSocketInteractor))]
//internal class ResetSocketInteractor : MonoBehaviour {

//    /// <summary>
//    /// The delay need to set up to avoid the immediate reattachment of the Interactable
//    /// </summary>
//    [SerializeField]
//    [Tooltip("How long it's the delay before reactivating the Socket Interactor")]
//    private float _deactivateDelay;
//    private XRSocketInteractor _socketInteractor;

//    private void Awake() {
//        _socketInteractor = GetComponent<XRSocketInteractor>();
//    }

//    public void ActivateSocket() {
//        _socketInteractor.socketActive = true;
//    }
//    public void DeactivateSocket() {
//        _socketInteractor.socketActive = false;
//    }

//    /// <summary>
//    /// FixedUpdate
//    /// FixedUpdate has the frequency of the physics system; it is called every fixed frame-rate frame. 
//    /// Compute Physics system calculations after FixedUpdate. 0.02 seconds (50 calls per second) is the default time between calls.
//    /// </summary>
//    private void FixedUpdate() {
//        if (_socketInteractor.socketActive) {
//            return;
//        }

//        Invoke("ActivateSocket", _deactivateDelay);
//    }


//}
