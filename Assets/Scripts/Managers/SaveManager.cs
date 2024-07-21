using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.Android;

public class SaveManager : Manager
{
    private IEnumerator Load() 
    {
        Debug.Log("LOAD");
        if (File.Exists(Path.Combine(Application.persistentDataPath, "save/save.txt")))
        {
            string[] file = File.ReadAllLines(Path.Combine(Application.persistentDataPath, "save/save.txt"));
            GameManager.GetManager<ResourceManager>().Load(file[0]);
            GameManager.GetManager<QuestManager>().Load(file[1]);
            yield return null;
            Factory[] factories = FindObjectsOfType<Factory>();
            for (int i = 0; i < factories.Length; ++i)
            {
                factories[i].Load(file[2 + i]);
            }
        }
    }
    private void Save() 
    {
        Debug.Log("SAVE");
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(GameManager.GetManager<ResourceManager>().Save());
        sb.AppendLine(GameManager.GetManager<QuestManager>().Save());
        Factory[] factories = FindObjectsOfType<Factory>();
        for (int i = 0; i < factories.Length; ++i) 
        {
            sb.AppendLine(factories[i].Save());
        }
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "save"))) Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "save"));
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "save/save.txt"), sb.ToString());
    }
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
                Permission.RequestUserPermission(Permission.ExternalStorageRead);
            }
        }
        if (File.Exists(Path.Combine(Application.persistentDataPath, "save/save.txt"))) 
        {
            StartCoroutine(Load());
        }
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            Debug.Log("Выходим...");
            Save();
        }
    }
}
