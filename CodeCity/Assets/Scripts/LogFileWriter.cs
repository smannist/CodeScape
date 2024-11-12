using System;
using System.IO;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

public static class LogFileWriter
{
    // Set a default log file path
    private static string logFilePath = Path.Combine("Logs", "log.txt");

    // Static constructor to clear log at start
    public static void CreateLog()
    {
        // Start with a fresh log file each time the game runs
        string message = "Log Start: " + System.DateTime.Now + "\n";
        
        #if UNITY_WEBGL //WebGL has no filesystem
            Debug.Log(message);
        #else
            File.WriteAllText(logFilePath, message);
        #endif
    }



    private static void LogInfo(string message)
    {
        string logMessage = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + message + "\n";
        #if UNITY_WEBGL //WebGL has no filesystem
            Debug.Log(message);
        #else
            File.AppendAllText(logFilePath, logMessage);
        #endif
    }

    public static void WriteLog(string message1, object message2 = null, object message3 = null, object message4 = null)
    {
        string message = $"{message1} {message2} {message3} {message4}";
        LogInfo(message);
    }
}
