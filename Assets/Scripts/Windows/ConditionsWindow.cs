using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsWindow : MonoBehaviour
{
    [SerializeField] List<GameObject> panelItems;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ShowItems()
    {
        foreach (GameObject item in panelItems)
        {
            item.SetActive(true);
        }
    }
    
    public void HideItems()
    {
        foreach (GameObject item in panelItems)
        {
            item.SetActive(false);
        }
    }

    public void HideWindow()
    {
        gameObject.SetActive(false);
    }

    public void OpenWindow()
    {
        animator.SetBool("Open", true);
    }
    
    public void CloseWindow()
    {
        animator.SetBool("Open", false);
    }
}
