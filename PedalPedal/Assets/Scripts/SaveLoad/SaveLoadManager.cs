using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public string DataFileName = "data.json";

    public static SaveLoadManager instance;
    private void Awake() { instance = this; }  

    public void Save<T>(T data, string fileName, string folderName = "Data")
    {
        string path = Path.Combine(Application.persistentDataPath, folderName, fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, JsonUtility.ToJson(data, true));
    }

    public void Load<T>(T data, string fileName, string folderName = "Data")
    {
        string path = Path.Combine(Application.persistentDataPath, folderName, fileName);
        if (File.Exists(path))
        {
            string loadDataString = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(loadDataString, data);
        }
    }

    public static bool CheckFileExists(string fileName, string folderName = "Data")
    {
        string path = Path.Combine(Application.persistentDataPath, folderName, fileName);
        if (File.Exists(path)) return true;
        return false;
    }
}
