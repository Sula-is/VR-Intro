using UnityEngine;
using UnityEngine.XR;

public class AccessCommonUsages : MonoBehaviour {
    [SerializeField]
    private bool _PrintInfo;

    private void Update() {
        if (_PrintInfo) {
            PrintInfo();
        }
    }

    private void PrintInfo() {
        InputFeatureUsage<float> battery = CommonUsages.batteryLevel;

        Debug.Log($"Usage = name: {battery.name} ,type: {battery.GetType()}, value: {battery}");
    }
}
