using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class AnimatedNumbers : MonoBehaviour
{
    public RectTransform uiElement;
    public float appearDuration = 0.5f;
    public float fallDuration = 2f;
    public float fadeDuration = 0.5f;
    public float fallDistance = 100f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = uiElement.GetComponent<CanvasGroup>();
     
    }
    private void Start()
    {
        AnimateElement();
        Destroy(gameObject, 3f);
    }

    public void AnimateElement()
    {
        canvasGroup.alpha = 0f;
        Vector2 startPosition = uiElement.anchoredPosition;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(canvasGroup.DOFade(1f, appearDuration));

        sequence.Append(uiElement.DOAnchorPos(new Vector2(startPosition.x, startPosition.y - fallDistance), fallDuration)
            .SetEase(Ease.OutQuad));

        sequence.Append(canvasGroup.DOFade(0f, fadeDuration));

        sequence.OnComplete(() => Destroy(gameObject));
    }

    private void OnDestroy()
    {
        DOTween.Kill(uiElement);
        DOTween.Kill(canvasGroup);
    }
}
