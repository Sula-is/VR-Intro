using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChangeColorsOnInteractableEvents : MonoBehaviour {
    private XRBaseInteractable m_Interactable;
    [SerializeField]
    private Renderer[] m_Renderer;

    [SerializeField]
    private Color _DefaultColor = Color.white;
    [SerializeField]
    private Color _HoveredColor = new Color(0.929f, 0.094f, 0.278f);
    [SerializeField]
    private Color _SelectedColor = new Color(0.019f, 0.733f, 0.827f);

    protected void OnEnable() {
        m_Interactable = GetComponent<XRBaseInteractable>();

        m_Interactable.firstHoverEntered.AddListener(OnFirstHoverEntered);
        m_Interactable.lastHoverExited.AddListener(OnLastHoverExited);
        m_Interactable.firstSelectEntered.AddListener(OnFirstSelectEntered);
        m_Interactable.lastSelectExited.AddListener(OnLastSelectExited);

        UpdateColor();
    }

    protected void OnDisable() {
        m_Interactable.firstHoverEntered.RemoveListener(OnFirstHoverEntered);
        m_Interactable.lastHoverExited.RemoveListener(OnLastHoverExited);
        m_Interactable.firstSelectEntered.RemoveListener(OnFirstSelectEntered);
        m_Interactable.lastSelectExited.RemoveListener(OnLastSelectExited);
    }

    protected virtual void OnFirstHoverEntered(HoverEnterEventArgs args) => UpdateColor();

    protected virtual void OnLastHoverExited(HoverExitEventArgs args) => UpdateColor();

    protected virtual void OnFirstSelectEntered(SelectEnterEventArgs args) => UpdateColor();

    protected virtual void OnLastSelectExited(SelectExitEventArgs args) => UpdateColor();

    protected void UpdateColor() {
        if (m_Renderer == null) {
            throw new MissingReferenceException(nameof(m_Renderer));
        }
        Color color =
            m_Interactable.isSelected ? _SelectedColor :
            m_Interactable.isHovered ? _HoveredColor :
            _DefaultColor;

        for (int i = 0; i < m_Renderer.Length; i++) {
            m_Renderer[i].material.color = color;
        }
    }

}
