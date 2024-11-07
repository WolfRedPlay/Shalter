using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] Menu menu;
    [SerializeField] GameObject roller;
    [SerializeField] PlayersAmountWindow playersAmountWindow;

    Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void GatesClose()
    {
        animator.SetBool("GameStarted", false);

    }

    public void OnGaetsClosed()
    {
        UnityEngine.Debug.Log("SceneReloaded");
        SceneManager.LoadScene(0);
    }

    public void ShowRoller()
    {
        roller.SetActive(true);
    }
    
    public void HideRoller()
    {
        roller.SetActive(false);
    }

    public void FinishTransition()
    {
        animator.SetBool("Transition", false);
    }

    public void MiddleTransition()
    {
        playersAmountWindow.MiddleTransition();
    }
    
    public void StartTransition()
    {
        animator.SetBool("Transition", true);
    }

}
