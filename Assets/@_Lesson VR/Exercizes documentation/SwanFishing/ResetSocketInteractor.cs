using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSocketInteractor))]
public class ResetSocketInteractor : MonoBehaviour {

    [SerializeField]
    private float _deactivateDelay;
    private XRSocketInteractor _socketInteractor;

    private void Awake() {
        _socketInteractor = GetComponent<XRSocketInteractor>();
    }

    public void ActivateSocket() {
        _socketInteractor.socketActive = true;
    }
    public void DeactivateSocket() {
        _socketInteractor.socketActive = false;
    }

    private void FixedUpdate() {
        if (_socketInteractor.socketActive) {
            return;
        }

        Invoke("ActivateSocket", _deactivateDelay);
    }


}
