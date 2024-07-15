using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.Android;

public class SaveManager : MonoBehaviour
{
    #region Singleton
    public static SaveManager Instance;
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    #endregion
    private void Load() 
    {
        Debug.Log("LOAD");
        string[] file = File.ReadAllLines(Path.Combine(Application.persistentDataPath, "save/save.txt"));
        ResourceManager.Instance.Load(file[0]);
        QuestManager.Instance.Load(file[1]);
        Factory[] factories = FindObjectsOfType<Factory>();
        for (int i = 0; i < factories.Length; ++i)
        {
            factories[i].Load(file[2 + i]);
        }
    }
    private void Save() 
    {
        Debug.Log("SAVE");
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(ResourceManager.Instance.Save());
        sb.AppendLine(QuestManager.Instance.Save());
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
            Load();
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
