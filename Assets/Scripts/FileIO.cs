
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class FileIO
{
    public static void SaveDataToFile(string relatedFolder, string fileName,string extension, object content)
    {
        string url = Path.Combine(Application.persistentDataPath, relatedFolder, fileName+extension).Replace("\\", "/");
        Directory.CreateDirectory(Path.GetDirectoryName(url));
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream file = File.Create(url))
        {
            bf.Serialize(file, content);
            file.Close();
        }

    }

    public static CameraStateData[] LoadDataFromFile(string relatedFolder, string fileName,string extension)
    {
        string url = Path.Combine(Application.persistentDataPath, relatedFolder, fileName+extension).Replace("\\", "/");
        Debug.Log(url);
        BinaryFormatter bf = new BinaryFormatter();
        CameraStateData[] result;
        if (File.Exists(url))
        {
            using (FileStream file = File.Open(url, FileMode.Open))
            {
                result = (CameraStateData[])bf.Deserialize(file);
                file.Close();
            }
        }
        else { result = null; }
        return result;
    }

    public static string[] GetDataListInfo(string relatedFolder,string extension)
    {
        string directoryUrl = Path.Combine(Application.persistentDataPath, relatedFolder).Replace("\\", "/");
        if (!Directory.Exists(directoryUrl))
        {
            Directory.CreateDirectory(directoryUrl);
        }
        string[] FileNames= Directory.GetFiles(directoryUrl, "*" + extension, SearchOption.TopDirectoryOnly);
        if (FileNames.Length == 0)
        {
            Debug.Log("Should copy");
            string exampleFileUrl = Path.Combine(Application.streamingAssetsPath,"example" + extension).Replace("\\", "/");
            string destinationFileUrl= Path.Combine(directoryUrl, "example" + extension).Replace("\\", "/");
            File.Copy(exampleFileUrl, destinationFileUrl);
            FileNames=new string[1] {"example"} ;
        }
        else
        {
            for (int i = 0; i < FileNames.Length; i++)
            {
                FileNames[i] = Path.GetFileNameWithoutExtension(FileNames[i]);
            }
        }
        return FileNames;
    }
}
