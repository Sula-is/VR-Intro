using UnityEngine;
using UnityEngine.Events;

internal class Swan : MonoBehaviour {

    public UnityEvent<int> ValueUpdated = new UnityEvent<int>();

    /// <summary>
    /// When the value is updated I'm going to call the Unity Event <see cref="ValueUpdated"/> and send the value.
    /// </summary>
    public int Value { get { return _value; } private set { _value = value; ValueUpdated.Invoke(value); } }
    private int _value;

    /// <summary>
    /// Creates a random value for this Swan between a range
    /// </summary>
    /// <param name="valueMin"></param>
    /// <param name="valueMax"></param>
    private void AssignValue(int valueMin, int valueMax) { // TO DO It needs to be moved on the "SwanGenerator"
        System.Random rnd = new System.Random();
        Value = rnd.Next(valueMin, valueMax);
    }

    /// <summary>
    /// I'm initializing the swan with a value
    /// </summary>
    public void Start() {
        // TO DO Remove the magic numbers, use a "SwanGenerator" to instantiate all the swans
        AssignValue(0, 10);
    }
}
