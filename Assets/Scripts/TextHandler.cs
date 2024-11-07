using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TextHandler : MonoBehaviour
{
    List<string> playersNames = new List<string>() 
    {
        "Серёжа",
        "Лиза",
        "Оксана",
        "Вова",
        "Миша",
        "Граф",
        "Player7",
        "Player8",
        "Player9",
        "Player10",
        "Player11",
        "Player12",
        "Player13",
        "Player14",
        "Player15",

    };


    public string GetPlayerName(int index)
    {
        return playersNames[index];
    }
}
