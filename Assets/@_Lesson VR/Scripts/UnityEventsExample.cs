using UnityEngine;
using UnityEngine.Events;

public class UnityEventsExample : MonoBehaviour {
    public UnityEvent _simpleEvent = new UnityEvent();
    public UnityEvent<int> _intEvent = new UnityEvent<int>();
    private int _value = 5;

    private void OnEnable() {
        _simpleEvent.AddListener(SimpleBehaviour);
        _intEvent.AddListener(IntBehaviour);
    }
    private void OnDisable() {
        _simpleEvent.RemoveListener(SimpleBehaviour);
        _intEvent.RemoveAllListeners();
    }

    private void SimpleBehaviour() {

    }
    private void IntBehaviour(int value) {

    }

    private void CallEvents() {
        _simpleEvent.Invoke();
        _intEvent.Invoke(_value);

    }
}
