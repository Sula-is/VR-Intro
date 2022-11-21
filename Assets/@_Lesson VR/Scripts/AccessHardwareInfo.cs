using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR;

public class AccessHardwareInfo : MonoBehaviour {
    [SerializeField]
    private bool _PrintInfo;

    [SerializeField]
    private List<InputDevice> _inputDevices = new List<InputDevice>();

    private void Start() {
        InputDevices.GetDevices(_inputDevices);

        for (int i = 0; i < _inputDevices.Count; i++) {

            Debug.Log($"The device {_inputDevices[i].name} is connected.");
        }
    }

    private void Update() {
        if (_PrintInfo) {
            PrintInfo();
        }
    }

    private void PrintInfo() {


        for (int i = 0; i < _inputDevices.Count; i++) {
            if (!_inputDevices[i].isValid) {
                return;
            }

            float batteryLevelDevice = 0f;
            _inputDevices[i].TryGetFeatureValue(CommonUsages.batteryLevel, out batteryLevelDevice);
            Debug.Log($"The device {_inputDevices[i].name} has a battery level of {batteryLevelDevice}");
        }


        float batteryLevel = SystemInfo.batteryLevel;
        BatteryStatus batteryStatus = SystemInfo.batteryStatus;

        Debug.Log($"SystemInfo Battery = level: {batteryLevel}, status: {batteryStatus}.");
    }
}
