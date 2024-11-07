using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButton : MonoBehaviour
{
    PlayerData playerData;

    Color notSelectedColor = Color.white;
    Color selectedColor = Color.green;

    public PlayerData PlayerData
    {
        set { 
            playerData = value; 
            playerName.text = value.PlayerName;
        }
        get { return playerData; }
    }

    [SerializeField] TMP_Text playerName;
    [SerializeField] Image buttonImage;

    public void SelectPlayer()
    {
       buttonImage.color = selectedColor;
    }

    public void DeselectPlayer()
    {
        buttonImage.color = notSelectedColor;
    }
}
