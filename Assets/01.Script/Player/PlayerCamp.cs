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
            // ���콺 Ŀ���� ȭ�� �ȿ� ����
            Cursor.lockState = CursorLockMode.None;
            // ���콺 Ŀ�� ����
            Cursor.visible = true;
        }
    }

}
