using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// Utility for Interastables, handles changing the interactable layer
/// </summary>
public class ChangeMyInteractionLayer : MonoBehaviour {
    [SerializeField]
    private XRBaseInteractable _interactable;
    [SerializeField]
    private InteractionLayerMask _newInteractionLayerMask = 0;

    private InteractionLayerMask _oldInteractionLayerMask;
    private void Awake() {
        if (_interactable == null) {
            _interactable = GetComponent<XRBaseInteractable>();
        }
    }

    private void Start() {
        if (_interactable == null) {
            throw new MissingComponentException("The Interactable component is missing, it needs to be on the same object.");
        }
        _oldInteractionLayerMask = _interactable.interactionLayers;
    }

    [Button]
    public void ChangeToNewLayer() {
        //Debug.Log("New layer set");
        _interactable.interactionLayers = _newInteractionLayerMask;
    }

    [Button]
    public void RestoreOriginalLayer() {
        //Debug.Log("Old layer restored");
        _interactable.interactionLayers = _oldInteractionLayerMask;
    }
}
