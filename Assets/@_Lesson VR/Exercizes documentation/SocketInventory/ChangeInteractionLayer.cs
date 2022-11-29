using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ChangeInteractionLayer : MonoBehaviour {

    private XRBaseInteractor _interactor;
    private XRBaseInteractable _interactable;

    [SerializeField]
    private InteractionLayerMask _newInteractionLayerMask = 0;

    private UnityEvent<XRBaseInteractable> _InteractableUpdated = new UnityEvent<XRBaseInteractable>();
    private bool _InteractableHovering;

    private XRBaseInteractable HoverInteractable {
        get { return _interactable; }
        set {
            _interactable = value;
            if (_interactable != null) {
                _InteractableUpdated.Invoke(_interactable);
            }
            else {
                Debug.Log("Null");
            }
        }
    }

    private InteractionLayerMask _oldInteractionLayerMask;
    private void Awake() {

        if (!_interactor) {
            _interactor = GetComponent<XRBaseInteractor>();
        }
        _interactor.hoverEntered.AddListener(GetInteractable);
        _interactor.hoverExited.AddListener(RemoveInteractable);
        _InteractableUpdated.AddListener(SetLayer);

    }

    #region Interactor Events
    private void GetInteractable(HoverEnterEventArgs args) {

        HoverInteractable = args.interactableObject.transform.GetComponent<XRBaseInteractable>();
        _InteractableHovering = true;
    }
    private void RemoveInteractable(HoverExitEventArgs args) {
        HoverInteractable = args.interactableObject.transform.GetComponent<XRBaseInteractable>(); ;
        _InteractableHovering = false;
    }
    #endregion

    private void SetLayer(XRBaseInteractable interactable) {

        Debug.Log($"the interactable is {interactable} the hover is {_InteractableHovering}");

        if (_InteractableHovering) {
            _oldInteractionLayerMask = _interactable.interactionLayers;
            NewLayer();
        }
        else {
            RestoreOriginalLayer();
            _interactable = null;
        }
    }

    [Button]
    public void NewLayer() {
        Debug.Log("New layer set");
        _interactable.interactionLayers = _newInteractionLayerMask;
    }

    [Button]
    public void RestoreOriginalLayer() {
        Debug.Log("Old layer restored");
        _interactable.interactionLayers = _oldInteractionLayerMask;
    }
}
