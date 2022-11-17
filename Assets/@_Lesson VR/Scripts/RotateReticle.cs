using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RotateReticle : MonoBehaviour {
    private XRInteractorLineVisual _xRInteractorLineVisual;
    private XRRayInteractor _xRRayInteractor;

    private void Awake() {
        _xRInteractorLineVisual = GetComponent<XRInteractorLineVisual>();
        _xRRayInteractor = GetComponent<XRRayInteractor>();
    }

    private void UpdateReticle() {
        GameObject reticleInstance = _xRInteractorLineVisual.reticle;
        _xRRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hitInfo);

        //Se Ancora ruotato a seconda della Z
        if (hitInfo.transform.TryGetComponent<TeleportationAnchor>(out TeleportationAnchor tpAnchor)) {
            float yValue = tpAnchor.transform.rotation.y;
            reticleInstance.transform.Rotate(Vector3.up, yValue);
        }

        //Se Area ruotato a seconda della direzione

    }
}
