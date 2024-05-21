using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamp : MonoBehaviour
{
    public GameObject campUI;

    public void Camp()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            campUI.SetActive(true);
            // 마우스 커서를 화면 안에 고정
            Cursor.lockState = CursorLockMode.None;
            // 마우스 커서 숨김
            Cursor.visible = true;
        }
    }

}
