using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DebugPrints : MonoBehaviour {

    /// <summary>
    /// Prints the info from the input arguments
    /// </summary>
    /// <param name="args"></param>
    public void Print(HoverEnterEventArgs args) {
        Debug.Log($"The interactable is : {args.interactableObject.transform.name}. The event is : {args.GetType()}");
    }

    /// <summary>
    /// Prints the input string
    /// </summary>
    /// <param name="text">String to print</param>
    public void Print(string text) {
        Debug.Log(text);
    }
}
