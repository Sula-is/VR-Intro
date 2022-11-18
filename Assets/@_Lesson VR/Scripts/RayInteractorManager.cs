using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RayInteractorManager : MonoBehaviour {
    public static RayInteractorManager Instance;
    public static XRRayInteractor _ActiveInteractor;

    private void Awake() {
        Instance = this;
    }

    public void AddInteractor(HoverEnterEventArgs args) {
        _ActiveInteractor = args.interactorObject as XRRayInteractor;
    }
    public void RemoveInteractor(HoverExitEventArgs args) {
        _ActiveInteractor = null;
    }

}
