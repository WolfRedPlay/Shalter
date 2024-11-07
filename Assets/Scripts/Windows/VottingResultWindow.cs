using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VottingResultWindow : MonoBehaviour
{
    [SerializeField] TMP_Text textPanel;
    [SerializeField] Transform playersPanel;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GameObject playerNamePrefab;
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

    public void SetText(string newText)
    {
        textPanel.text = newText;
    }


    public void ClearPlayerList()
    {
        int childAmount = playersPanel.transform.childCount;
        for (int i = 0; i < childAmount; i++)
        {
            Destroy(playersPanel.GetChild(i).gameObject);
        }
    }

    public void AddPlayersCards(List<GameObject> cardsToAdd)
    {
        if (cardsToAdd.Count != 0)
            if (cardsToAdd.Count <= 5)
            {
                playersPanel.GetComponent<GridLayoutGroup>().cellSize = new Vector2(384, 504);
                foreach (GameObject card in cardsToAdd)
                {
                    GameObject newCard = Instantiate(card, playersPanel);
                    newCard.GetComponent<PlayerCard>().ResizeTexts(1.5f);
                }
            }
            else
            {
                playersPanel.GetComponent<GridLayoutGroup>().cellSize = new Vector2(300, 150);
                foreach (GameObject card in cardsToAdd)
                {
                    GameObject newPlayerName = Instantiate(playerNamePrefab, playersPanel);
                    newPlayerName.GetComponent<TMP_Text>().text = card.GetComponent<PlayerCard>().Name;

                }
            }
    }

    public void SetAction(Action actionToSetUp)
    {
        blendFinishedAction = actionToSetUp;
    }
}
