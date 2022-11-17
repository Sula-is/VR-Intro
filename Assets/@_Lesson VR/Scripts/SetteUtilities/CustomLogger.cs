using System;

using UnityEngine;


[Flags]
public enum CustomLogType {
    None = 0,
    Everything = 7,
    Error = 1,
    Warning = 2,
    Log = 4
}

public class CustomLogger : Singleton<CustomLogger> {

    [SerializeField] private CustomLogType logType = CustomLogType.Everything;

    private void Awake() { if (!Debug.isDebugBuild) logType = CustomLogType.Error; }

    /// <summary>
    /// Print a Debug Log into the console
    /// </summary>
    /// <param name="sender">The Component who wants to send a message</param>
    /// <param name="printSenderName">True = print add to the string "Sender = {gameObject.name} + message" || This only works if the sender is given as GameObject</param>
    /// <param name="message">The message to print on Console</param>
    public void LogMessage<T>(T sender, bool printSenderName, string message) where T : class {
        if (!logType.HasFlag(CustomLogType.Log))
            return;

        if (printSenderName)
            message = BuildStringWithGameobjectName(sender, message);

        Debug.Log(message);
    }

    /// <summary>
    /// Print a Debug Log Error into the console
    /// </summary>
    /// <param name="sender">The Component who wants to send a message</param>
    /// <param name="printSenderName">True = print add to the string "Sender = {gameObject.name} + message" || This only works if the sender is given as GameObject</param>
    /// <param name="message">The message to print on Console</param>
    public void ErrorMessage<T>(T sender, bool printSenderName, string message) where T : class {
        if (!logType.HasFlag(CustomLogType.Error))
            return;

        if (printSenderName)
            message = BuildStringWithGameobjectName(sender, message);

        Debug.LogError(message);
    }

    /// <summary>
    /// Print a Debug Log Warning into the console
    /// </summary>
    /// <param name="sender">The Component who wants to send a message</param>
    /// <param name="printSenderName">True = print add to the string "Sender = {gameObject.name} + message" || This only works if the sender is given as GameObject</param>
    /// <param name="message">The message to print on Console</param>
    public void WarningMessage<T>(T sender, bool printSenderName, string message) where T : class {
        if (!logType.HasFlag(CustomLogType.Warning))
            return;

        if (printSenderName)
            message = BuildStringWithGameobjectName(sender, message);


        Debug.LogWarning(message);
    }

    /// <summary>
    /// Print a Debug Log into the console
    /// </summary>
    /// <param name="message">The message to print on Console</param>
    public void LogMessage(string message) {
        if (!logType.HasFlag(CustomLogType.Log))
            return;

        Debug.Log(message);
    }

    /// <summary>
    /// Print a Debug Log Error into the console
    /// </summary>
    /// <param name="message">The message to print on Console</param>
    public void ErrorMessage(string message) {
        if (!logType.HasFlag(CustomLogType.Error))
            return;

        Debug.LogError(message);
    }

    /// <summary>
    /// Print a Debug Log Warning into the console
    /// </summary>
    /// <param name="message">The message to print on Console</param>
    public void WarningMessage(string message) {
        if (!logType.HasFlag(CustomLogType.Warning))
            return;

        Debug.LogWarning(message);
    }

    private string BuildStringWithGameobjectName<T>(T sender, string startMessage) where T : class {
        string message = string.Empty;
        GameObject go = sender as GameObject;

        if (go != null)
            message = $"Sender = {go.name} || {startMessage}";
        else
            message = $"Sender = {sender.GetType()} || {startMessage}";

        return message;
    }

    //[ShowInInspector]
    private void DEBUG_TestLog(CustomLogType type, string message, bool printGoName) {
        switch (type) {
            case CustomLogType.Error:
                ErrorMessage(this, printGoName, message);
                break;
            case CustomLogType.Warning:
                WarningMessage(this.gameObject, printGoName, message);
                break;
            case CustomLogType.Log:
                LogMessage(this.gameObject, printGoName, message);
                break;
            case CustomLogType.None:
            default:
                break;
        }
    }
}
