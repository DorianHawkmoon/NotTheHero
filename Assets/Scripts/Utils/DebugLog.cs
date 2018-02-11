using UnityEngine;

/// <summary>
/// Little class to help with logger
/// </summary>
public class DebugLog {
    /// <summary>
    /// Name of the class
    /// </summary>
    private string header;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="header">Header for the logs</param>
    public DebugLog(string header) {
        this.header = header;
    }

    /// <summary>
    /// Write a message log with the given header and the given message
    /// </summary>
    /// <param name="message">message to append to header</param>
    public void Log(string message) {
            Debug.Log(header + ": " + message);
    }
}
