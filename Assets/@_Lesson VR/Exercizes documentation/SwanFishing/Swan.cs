using UnityEngine;
using UnityEngine.Events;

public class Swan : MonoBehaviour {


    public int Value { get { return _value; } private set { _value = value; ValueUpdated.Invoke(value); } }
    public UnityEvent<int> ValueUpdated = new UnityEvent<int>();

    private int _value;

    private void AssignValue(int valueMin, int valueMax) {
        System.Random rnd = new System.Random();
        Value = rnd.Next(valueMin, valueMax);
    }

    public Swan() {
        // TO DO Remove the magic numbers, a "SwanGenerator" is needed
        AssignValue(0, 10);
    }

    private void OnEnable() {
        ValueUpdated.Invoke(_value);
    }


}
