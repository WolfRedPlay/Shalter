using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsedSpecialCardWindow : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] CardData specialCard;
    [SerializeField] TMP_Text header;
    Action blendFinishedAction;

    public void StartBlendOut(float duration)
    {
        canvasGroup.alpha = 1f;
        gameObject.SetActive(true);
        StartCoroutine(BlendOut(duration));
    }

    private IEnumerator BlendOut(float duration)
    {
        float startAlha = 1f;
        float elapsedTime = 0f;

        float blendTime = 0f;
        float blendDuration = Times.BlendOutDuration;

        float fullDuration = duration + blendDuration;

        while (elapsedTime < fullDuration)
        {
            elapsedTime += Time.deltaTime;
            if (duration - elapsedTime <= 0f)
            {
                blendTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(startAlha, 0f, blendTime / blendDuration);
                canvasGroup.alpha = newAlpha;
            }

            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
        blendFinishedAction?.Invoke();
    }

    public void SetHeader(string text)
    {
        header.text = text;
    }

    public void SetSpecilaCard(SpecialCard newSpecialCard)
    {
        specialCard.SetCardData(newSpecialCard);
    }

    public void SetAction(Action actionToSetUp)
    {
        blendFinishedAction = actionToSetUp;
    }

}
