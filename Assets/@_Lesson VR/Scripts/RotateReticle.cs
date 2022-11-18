using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RotateReticle : MonoBehaviour {

    private XRRayInteractor _xRRayInteractor;
    private bool _rotateWithView;
    private bool _initialized;


    /// <summary>
    /// Updates the reticle rotation
    /// </summary>
    private void UpdateReticle() {
        GameObject reticleInstance = this.gameObject;
        if (reticleInstance == null) { return; }

        _xRRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hitInfo);

        //Se Ancora ruotato a seconda della Z
        if (hitInfo.transform.TryGetComponent<TeleportationAnchor>(out TeleportationAnchor tpAnchor)) {
            float yValue = tpAnchor.transform.rotation.y;
            reticleInstance.transform.Rotate(Vector3.up, yValue);
            return;
        }

        //Se Area ruotato a seconda della direzione della camera
        if (hitInfo.transform.TryGetComponent<TeleportationArea>(out TeleportationArea tpArea)) {
            _rotateWithView = true;
        }

    }

    private void Update() {
        if (_xRRayInteractor == null) {
            _xRRayInteractor = RayInteractorManager._ActiveInteractor;
        }
        else {
            _initialized = true;
            UpdateReticle();
        }

        if (!_initialized) {
            return;
        }
        //update the rotation
        if (_rotateWithView) {
            // se la target rot è diversa da quella attuale
            Quaternion rot = Camera.main.transform.rotation;
            if (this.transform.rotation != rot) {
                Vector3 newRot = new Vector3(0, rot.eulerAngles.y, 0);
                this.transform.rotation = Quaternion.Euler(newRot);
            }
        }

    }
}
