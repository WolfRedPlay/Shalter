using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class ScreenEffect : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    private RectTransform Back_Transform;

    private float Back_Pos_x;
    private float Back_Pos_y;
    private float Back_Size_x;
    private float Back_Size_y;

    private void Start()
    {
        Back_Transform = GetComponent<RectTransform>();

        Back_Size_x = 1920f;
        Back_Size_y = 1280f;
        Back_Pos_x = 0f;
        Back_Pos_y = 0f;
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        Back_Pos_x += speed * Time.deltaTime;
        Back_Pos_x = Mathf.Repeat(Back_Pos_x, Back_Size_x);
        Back_Pos_y += speed * Time.deltaTime;
        Back_Pos_y = Mathf.Repeat(Back_Pos_y, Back_Size_y);

        Back_Transform.anchoredPosition = new Vector3(Back_Pos_x, Back_Pos_y, 0);
    }

}
