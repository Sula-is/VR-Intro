using TMPro;

using UnityEngine;
/// <summary>
/// Utility for text mesh pro component, need to be on the same object.
/// Updates the text value
/// </summary>
public class UpdateText : MonoBehaviour {
    private TextMeshPro _txt;

    private void Awake() {
        _txt = GetComponentInChildren<TextMeshPro>();
        if (!_txt) {
            throw new MissingReferenceException("TextMeshPro");
        }
    }

    public void UpdateValue(string value) {
        _txt.text = value;
    }
    public void UpdateValue(int value) {
        _txt.text = value.ToString();
    }
}
