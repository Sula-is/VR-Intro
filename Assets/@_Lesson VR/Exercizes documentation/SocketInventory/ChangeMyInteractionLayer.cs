using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChangeMyInteractionLayer : MonoBehaviour {
    [SerializeField]
    private XRBaseInteractable _interactable;
    [SerializeField]
    private InteractionLayerMask _newInteractionLayerMask = 0;

    private InteractionLayerMask _oldInteractionLayerMask;
    private void Awake() {
        _oldInteractionLayerMask = _interactable.interactionLayers;
    }

    [Button]
    public void ChangeLayer() {
        //Debug.Log("New layer set");
        _interactable.interactionLayers = _newInteractionLayerMask;
    }

    [Button]
    public void RestoreOriginalLayer() {
        //Debug.Log("Old layer restored");
        _interactable.interactionLayers = _oldInteractionLayerMask;
    }
}
