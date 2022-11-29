using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabbedValidTargets : MonoBehaviour {
    //prendo dalle mani gli oggetti grabbati e li sostituisco ai valid target per la cintura

    [SerializeField]
    private XRSocketInteractor _socketInteractor;
    [SerializeField]
    private List<IXRInteractable> targetsList = new List<IXRInteractable>();

    private void Awake() {
        _socketInteractor = GetComponent<XRSocketInteractor>();
    }

    private void InitTargets() {
        _socketInteractor.GetValidTargets(targetsList);
    }
    private void AddTarget(IXRInteractable interactable) {
        targetsList.Add(interactable);
    }
    private void RemoveTarget(IXRInteractable interactable) {
        targetsList.Remove(interactable);
    }

}
