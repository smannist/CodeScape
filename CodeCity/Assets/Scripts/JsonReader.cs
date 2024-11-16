using Assets.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using static BuildingGenerator;
using File = System.IO.File;
//using SourceFile = Assets.Entities.File;

public class JsonReader : MonoBehaviour
{
    private string filePath;
    public FileData fileData;
    public bool fileLoaded = false;

    public void StartReadingJson()
    {
        // Assuming the .sln file is in the root project directory, we access it directly.
        filePath = Path.Combine(Directory.GetCurrentDirectory(), "output.json");
        LogFileWriter.WriteLog("filePath:" , filePath);
        #if UNITY_WEBGL
			StartCoroutine(LoadJsonOverHttp(filePath));
		#else
			LoadJsonFile(filePath);
		#endif
    }

	private void LoadJsonFile(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            fileData = JsonConvert.DeserializeObject<FileData>(json);
            Debug.Log("Floor Count: " + fileData?.files?.Count());
            fileLoaded = true;
        }
        else
        {
            Debug.LogError("JSON file not found at " + filePath);
        }
	}

    private IEnumerator LoadJsonOverHttp(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError($"{uri}: Error: {webRequest.error}");
                    break;
                case UnityWebRequest.Result.Success:
                    bool success = false;

                    try
                    {
                        fileData = JsonConvert.DeserializeObject<FileData>(webRequest.downloadHandler.text);
                        fileLoaded = true;
                        success = true;
                        Debug.Log($"Successfully loaded JSON data from {uri}");
                    }
                    catch (JsonException ex)
                    {
                        Debug.LogError($"Failed to deserialize JSON from {uri}");
                    }

                    // Fallback to example json instead
                    if (!success)
                    {
                        Debug.LogWarning($"Attempting to load example JSON as a fallback");
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "examples", "example_output.json");
                        yield return LoadJsonOverHttp(filePath);
                    }

                    break;
            }
        }
    }

}
