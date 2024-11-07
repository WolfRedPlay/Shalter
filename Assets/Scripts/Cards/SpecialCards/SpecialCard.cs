using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCard : Card
{
    protected bool isUsed = false;

    GameManager manager;

    public bool Used
    {
        get { return isUsed; }
    }

    protected PlayerData owner;

    public PlayerData Owner
    {
        set { owner = value; }
    }


    protected virtual void OnEnable()
    {
        cardType = CardType.SPECIAL;
        isUsed = false;
    }

    public virtual void Use()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        manager.RewritePlayersCards();
        isUsed = true;
        
        if (OnUsed != null)
        {
            OnUsed.Invoke();
            OnUsed = null;
        }  
    }

    public string description;

    public event Action OnUsed;

    
    public virtual bool CheckPossibilityToUse()
    {
        if (!isUsed) return true;
        else return false;
    }

}
