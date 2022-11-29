using System.Collections.Generic;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabbedValidTargets : MonoBehaviour {
    //prendo dalle mani gli oggetti grabbati e li sostituisco ai valid target per la cintura

    [SerializeField]
    private XRSocketInteractor _socketInteractor;
    [SerializeField]
    private List<IXRInteractable> targetsList = new List<IXRInteractable>();

    private void Awake() {
        if (!_socketInteractor) {
            _socketInteractor = GetComponent<XRSocketInteractor>();
        }
    }
    [ShowInInspector]
    private void InitTargets() {
        _socketInteractor.GetValidTargets(targetsList);
    }
    [Button]
    private void AddTarget(IXRInteractable interactable) {
        targetsList.Add(interactable);
    }
    [Button]
    private void RemoveTarget(IXRInteractable interactable) {
        targetsList.Remove(interactable);
    }

}
