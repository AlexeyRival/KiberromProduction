using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public Level[] levels;
    public int currentLevel = 0;
    private float timer=1;
    private int prevCompleted;

    #region Singleton
    public static QuestManager Instance;
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    #endregion

    private void Start()
    {
        for (int i = 0; i < levels.Length; ++i) 
        {
            levels[i].Start();
        }
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) 
        {
            timer = 1f;
            CheckQuests();
        }
    }
    private void CheckQuests() 
    {
        int completeCount = 0;
        for (int i = 0; i < levels[currentLevel].GetQuests().Length; ++i) 
        {
            levels[currentLevel].GetQuests()[i].Check();
            if (levels[currentLevel].GetQuests()[i].complete) { ++completeCount; }
        }
        if (completeCount > prevCompleted) 
        {
            prevCompleted = completeCount;
            UIManager.Instance.QuestCompleted();
        }
    }
    public Quest[] GetQuests() 
    {
        return levels[currentLevel].GetQuests();
    }
    public void ResourceProduced(Resource resource) 
    {
        foreach (Quest quest in levels[currentLevel].GetQuests())
        {
            if (quest is ResourceProduceQuest) 
            {
                if (((ResourceProduceQuest)quest).resource == resource) { quest.Progress(1); }
            }
        }
    }
    public void NextLevel() 
    {
        ++currentLevel;
        prevCompleted = 0;
        Application.LoadLevel(currentLevel);
    }

    public string Save() 
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(currentLevel);
        return sb.ToString();
    }
    public void Load(string data) 
    {
        int oldlevel = currentLevel;
        currentLevel = int.Parse(data);
        if(oldlevel!=currentLevel)Application.LoadLevel(currentLevel);
    }
}
