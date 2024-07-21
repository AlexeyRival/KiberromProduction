using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : Manager
{
    public Level[] levels;
    public int currentLevel = 0;
    private float timer=1;
    private int prevCompleted;

    [Header("UI")]
    [SerializeField] private Button openQuestButton;
    [Header("Prefabs")]
    [SerializeField] private QuestWindow questWindow;
    [SerializeField] private GameObject droppedStar;


    private void Start()
    {
        for (int i = 0; i < levels.Length; ++i) 
        {
            levels[i].Start();
        }
        openQuestButton.onClick.AddListener(OpenUI);
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

    private void OpenUI() 
    {
        QuestWindow qw =  Instantiate(questWindow, GameManager.GetUIRoot().transform);
        qw.SetContext(this);
        qw.Show();
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
            QuestCompleted();
        }
    }
    private void QuestCompleted()
    {
        Instantiate(droppedStar, openQuestButton.transform.position, Quaternion.identity, GameManager.GetUIRoot());
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
