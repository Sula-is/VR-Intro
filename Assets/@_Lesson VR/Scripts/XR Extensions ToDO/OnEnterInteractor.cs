using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
public class OnEnterInteractor : XRBaseInteractor, IXRActivateInteractor {

    private XRBaseInteractable _selectedInteractable;

    #region Interface
    public bool shouldActivate => throw new System.NotImplementedException();
    public bool shouldDeactivate => throw new System.NotImplementedException();
    public void GetActivateTargets(List<IXRActivateInteractable> targets) {
        throw new System.NotImplementedException();
    }
    #endregion

    //private void Awake() {
    //    GetComponent<Collider>();//set trigger
    //}

    //call Activate when enters the collider
    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.TryGetComponent<XRBaseInteractable>(out XRBaseInteractable interactable)) {
            if (_selectedInteractable != null) {
                return;
            }
            _selectedInteractable = interactable;
            SendActivateEvent(_selectedInteractable);
        }
    }
    private void OnTriggerExit(Collider collider) {
        if (collider.gameObject.TryGetComponent<XRBaseInteractable>(out XRBaseInteractable interactable)) {
            if (_selectedInteractable == interactable) {
                SendDeactivateEvent(_selectedInteractable);
                _selectedInteractable = null;
            }
        }
    }

    private readonly LinkedPool<ActivateEventArgs> m_ActivateEventArgs = new LinkedPool<ActivateEventArgs>(() => new ActivateEventArgs(), collectionCheck: false);
    private readonly LinkedPool<DeactivateEventArgs> m_DeactivateEventArgs = new LinkedPool<DeactivateEventArgs>(() => new DeactivateEventArgs(), collectionCheck: false);

    private void SendActivateEvent(IXRActivateInteractable target) {
        if (target == null || target as Object == null) {

            using (m_ActivateEventArgs.Get(out ActivateEventArgs args)) {
                args.interactorObject = this;
                args.interactableObject = target;
                target.OnActivated(args);
            }
        }
    }

    private void SendDeactivateEvent(IXRActivateInteractable target) {
        if (target == null || target as Object == null) {

            using (m_DeactivateEventArgs.Get(out DeactivateEventArgs args)) {
                args.interactorObject = this;
                args.interactableObject = target;
                target.OnDeactivated(args);
            }
        }
    }

}
//XRTriggerInteractor
