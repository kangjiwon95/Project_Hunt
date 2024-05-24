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
        "조준 상태가 되면 자동으로 숨을 참습니다.",
        "걸어다니세요! 동물은 예민합니다.",
        "지도에 있는 별장으로 가서 피로를 회복하고 상점을 이용하세요.",
        "사냥하면 육식동물을 주의 하세요.",
        "육식동물의 먹이는 당신일수도 있습니다."
    };

        StartCoroutine(LoadSceneAsync());
    }

    // 씬 로딩
    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");

        // 로딩이 끝날 때까지 대기하지 않음
        asyncLoad.allowSceneActivation = false;
        // 팁 배열에서 랜덤하게 하나의 팁을 선택하여 표시
        int randomIndex = Random.Range(0, tip.Length);
        tipText.text = tip[randomIndex]; // 선택된 팁을 Text 컴포넌트에 설정

        // 로딩 진행 상황에 따라 UI 업데이트
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            progressBar.fillAmount = progress;

            // 로딩 완료 시점에서 씬 활성화
            if (asyncLoad.progress >= 0.9f)
            {
                // 사용자가 준비되었을 때 씬 활성화하기 위해 특정 입력을 기다림
                tipText.text = "아무키나 눌러주세요!";

                if (Input.anyKeyDown)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
