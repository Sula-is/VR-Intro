using TMPro;

using UnityEngine;

public class UpdateText : MonoBehaviour {
    [SerializeField]
    private TextMeshPro _txt;

    public void UpdateValue(string value) {
        _txt.text = value;
    }
    public void UpdateValue(int value) {
        _txt.text = value.ToString();
    }
}
