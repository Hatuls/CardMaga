
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
#if UNITY_EDITOR
    [UnityEditor.MenuItem("Save System/Open data path")]
    private static void OpenDataPath()
    {
        string path = Application.persistentDataPath;

        path = path.Replace(@"/", @"");
        System.Diagnostics.Process.Start("explorer.exe", "/open," + path);
    }
#endif
    public enum FileStreamType { Binary = 0, FileStream = 1 , PlayerPref =2 };

    
    public static void SaveFile<T>(T objectT, string fileName, FileStreamType fileStreamType , bool toPersistantDataPath = true, string fileType = "txt", string PathFolders = "") where T: class
    {
        switch (fileStreamType)
        {
            case FileStreamType.Binary:
                SaveToFilePathAndConvertItToBinary(objectT, fileName, fileType, PathFolders, toPersistantDataPath);
                break;
            case FileStreamType.FileStream:
                SaveToFilePathAndDontConvertItToBinary(objectT, fileName, fileType, PathFolders, toPersistantDataPath);
                break;
            case FileStreamType.PlayerPref:
                SaveToPlayerPref(objectT, fileName);
                break;
            default:
                throw new System.Exception("Save method was not assigned!");

        }
    }
    public static void SaveFile<T>(T objectT, string fileName,bool toPersistantDataPath = true, string fileType = "txt", string PathFolders = "")   where T : ISaveable
    {

        switch (objectT.FileStreamType)
        {
            case FileStreamType.Binary:
                SaveToFilePathAndConvertItToBinary(objectT, fileName, fileType, PathFolders, toPersistantDataPath);
                break;
            case FileStreamType.FileStream:
                SaveToFilePathAndDontConvertItToBinary(objectT, fileName, fileType, PathFolders,toPersistantDataPath);
                break;
            case FileStreamType.PlayerPref:
                SaveToPlayerPref(objectT, fileName);
                break;
            default:
                throw new System.Exception("Save method was not assigned!");
         
        }
    }
    private static void SaveToFilePathAndConvertItToBinary<T>(T objectT, string fileName, string fileType, string PathFolders,bool isPersistantDataPath)
    {
        string persistantDataPath = isPersistantDataPath ? Application.persistentDataPath : Application.dataPath;
        string path = string.Concat(persistantDataPath, "/", PathFolders, fileName, ".", fileType);

        if (!Directory.Exists(string.Concat(persistantDataPath, "/", PathFolders)))
            Directory.CreateDirectory(string.Concat(persistantDataPath, "/", PathFolders));


        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);
        binaryFormatter.Serialize(fileStream, objectT);
        fileStream.Close();
    }
    private static void SaveToFilePathAndDontConvertItToBinary<T>(T objectT, string fileName, string fileType, string PathFolders, bool isPersistantDataPath)
    {
        string dataPath = isPersistantDataPath ? Application.persistentDataPath : Application.dataPath;
        string windowPath = string.Concat(dataPath, "/", PathFolders, fileName, ".", fileType);

        if (!Directory.Exists(string.Concat(dataPath, "/", PathFolders)))
            Directory.CreateDirectory(string.Concat(dataPath, "/", PathFolders));

        var streamWriter = File.CreateText(windowPath);
        streamWriter.Write(JsonUtilityHandler.ConvertObjectToJson(objectT));
        streamWriter.Close();
    }
    private static void SaveToPlayerPref<T>(T objectT, string fileName)
    {
        PlayerPrefs.SetString(fileName, JsonUtilityHandler.ConvertObjectToJson(objectT));
        PlayerPrefs.Save();
    }
    public static T Load<T>(string fileName, FileStreamType fileStream, string fileType = "txt",bool fromApplicationPersistantDataPath = true, string PathFolders = "") where T : class
    {

        T objectLoaded = null;

        switch (fileStream)
        {

            case FileStreamType.Binary:
            objectLoaded = LoadFromBinaryFilePath<T>(fileName, fileType, PathFolders, fromApplicationPersistantDataPath);
                break;
            case FileStreamType.FileStream:
                objectLoaded = LoadFromFilePath<T>(fileName, fileType, PathFolders, fromApplicationPersistantDataPath);
                break;
            case FileStreamType.PlayerPref:
                objectLoaded = LoadFromPlayerPref<T>(fileName);
                break;
            default:
                throw new System.Exception("FileStreamType is not valid!");
        }

       
      
        if (objectLoaded == null)
            Debug.Log("No Save File");

        return objectLoaded;

    }
    private static T LoadFromPlayerPref<T>(string fileName) where T : class
    {
        
        if (PlayerPrefs.HasKey(fileName))
            return JsonUtilityHandler.LoadFromJson<T>(PlayerPrefs.GetString(fileName));
        
        return null;
    }
    private static T LoadFromFilePath<T>(string fileName, string fileType, string PathFolders , bool fromPersistantDataPath) where T : class
    {

        string dataPAth = fromPersistantDataPath ? Application.persistentDataPath : Application.dataPath;
        string path = string.Concat(dataPAth, "/", PathFolders, fileName, ".", fileType);

        if (File.Exists(path))
        {
           string txt =  File.ReadAllText (path);

            T loadedObject = JsonUtilityHandler.LoadFromJson<T>(txt);
   
            return loadedObject;
        }
        return null;
    }
    private static T LoadFromBinaryFilePath<T>(string fileName,  string fileType, string PathFolders, bool fromPersistantDataPath) where T : class
    {
        string dataPath = fromPersistantDataPath ? Application.persistentDataPath : Application.dataPath;
        string path = string.Concat(dataPath, "/", PathFolders, fileName, ".", fileType);

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);
            T loadedObject =(T)formatter.Deserialize(fileStream) ;
            fileStream.Close();
            return loadedObject;
        }
        return null;
    }
}
public interface ISaveable
{
   SaveManager.FileStreamType FileStreamType { get; }
}