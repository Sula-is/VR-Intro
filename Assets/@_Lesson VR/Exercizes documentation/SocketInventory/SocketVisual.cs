using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketVisual : MonoBehaviour {
    private XRBaseInteractor _interactor;

    //change the sprite color
    public SpriteRenderer spriteRenderer;

    //change the sprite color
    public Color validColor = Color.green;
    public Color invalidColor = Color.red;

    private void ChangeRendererColor(SelectEnterEventArgs eventArgs) {
        spriteRenderer.color = invalidColor;
    }
    private void ChangeRendererColor(SelectExitEventArgs eventArgs) {
        spriteRenderer.color = validColor;
    }

    private void Awake() {

        if (!_interactor) {
            _interactor = GetComponent<XRBaseInteractor>();
        }

        _interactor.selectEntered.AddListener(ChangeRendererColor);
        _interactor.selectExited.AddListener(ChangeRendererColor);

        spriteRenderer.color = validColor;
    }





}
