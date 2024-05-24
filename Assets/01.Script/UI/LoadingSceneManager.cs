using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public Image progressBar;
    private string[] tip;
    public Text tipText;

    void Start()
    {
        tip = new string[] {
        "���� ���°� �Ǹ� �ڵ����� ���� �����ϴ�.",
        "�ɾ�ٴϼ���! ������ �����մϴ�.",
        "������ �ִ� �������� ���� �Ƿθ� ȸ���ϰ� ������ �̿��ϼ���.",
        "����ϸ� ���ĵ����� ���� �ϼ���.",
        "���ĵ����� ���̴� ����ϼ��� �ֽ��ϴ�."
    };

        StartCoroutine(LoadSceneAsync());
    }

    // �� �ε�
    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");

        // �ε��� ���� ������ ������� ����
        asyncLoad.allowSceneActivation = false;
        // �� �迭���� �����ϰ� �ϳ��� ���� �����Ͽ� ǥ��
        int randomIndex = Random.Range(0, tip.Length);
        tipText.text = tip[randomIndex]; // ���õ� ���� Text ������Ʈ�� ����

        // �ε� ���� ��Ȳ�� ���� UI ������Ʈ
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            progressBar.fillAmount = progress;

            // �ε� �Ϸ� �������� �� Ȱ��ȭ
            if (asyncLoad.progress >= 0.9f)
            {
                // ����ڰ� �غ�Ǿ��� �� �� Ȱ��ȭ�ϱ� ���� Ư�� �Է��� ��ٸ�
                tipText.text = "�ƹ�Ű�� �����ּ���!";

                if (Input.anyKeyDown)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
