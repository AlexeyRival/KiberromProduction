using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestWindow : BaseWindow
{
    [Header("UI")]
    [SerializeField] private Transform questList;
    [SerializeField] private Text requiringsText;
    [SerializeField] private Button nextLevelButton;

    [SerializeField] private Button closeButton;

    [Header("Prefabs")]
    [SerializeField] private QuestRequiringUI questPrefab;

    private QuestManager questManager;

    public void SetContext(QuestManager questManager) 
    {
        this.questManager = questManager;
    }

    public override void Show()
    {
        closeButton.onClick.AddListener(Hide);

        RectTransform UIRect = ((RectTransform)transform);

        Vector2 targetPosition = UIRect.anchoredPosition;
        UIRect.anchoredPosition += new Vector2(-Screen.width, 0);
        UIRect.DOAnchorPos(targetPosition, .5f).SetEase(Ease.OutBack);

        Draw();
    }
    public override void Draw()
    {
        questList.ClearRoot();

        int counter = 0;

        Quest[] quests = questManager.GetQuests();
        Debug.Log(quests.Length);
        for (int i = 0; i < quests.Length; ++i)
        {
            QuestRequiringUI ob = Instantiate(questPrefab, questList);
            ob.Fill(i, quests[i].text, quests[i].complete);
            if (quests[i].complete) ++counter;
        }

        requiringsText.text = $"{counter}/{quests.Length}";

        nextLevelButton.gameObject.SetActive(counter == quests.Length);
        
        if (counter == quests.Length)
        {
            nextLevelButton.onClick.AddListener(delegate { questManager.NextLevel(); Hide(); });
        }
    }
    public override void Hide()
    {
        nextLevelButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();

        RectTransform UIRect = ((RectTransform)transform);

        Vector2 bufferPos = UIRect.anchoredPosition;
        Vector2 targetPosition = UIRect.anchoredPosition + new Vector2(-Screen.width,0);

        UIRect.DOAnchorPos(targetPosition, .5f).SetEase(Ease.OutBack).OnComplete(() => {
            Destroy(gameObject);
            GameManager.GetManager<TouchManager>().SetIgnoreMode(false);
        });
    }
}
