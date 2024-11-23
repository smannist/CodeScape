using Assets.Entities;
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
        //StartCoroutine(LoadJsonOverHttp(filePath));
        LoadJsonFile(filePath);
#else
			LoadJsonFile(filePath);
#endif
    }

	
	private void LoadJsonFile(string path){
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
                    Debug.LogError(uri + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    fileData = JsonConvert.DeserializeObject<FileData>(webRequest.downloadHandler.text);
                    Globals.docs = fileData;
                    fileLoaded = true;
                    break;
            }
        }
    }

}
