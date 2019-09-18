using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanelController : MonoBehaviour
{

    public GameObject[] Button; // 0; Menu, 1: Home, 2: GameInfo 3: Cancel
    public GameObject PausePanel;
    public GameObject GameInfoPanel;
    public GameObject[] Guide; // Size = 2

    public bool isActive = false;


    private int CurrentGuideNum; // Size = 2

    private void Start()
    {
        CurrentGuideNum = 0;
    }





    public void OnMenuButtonClicked()
    {
        if (GameManager.instance.gameState != GameManager.GameState.GamePlay) return;

        if (!isActive)
        {
            isActive = true;

            GameManager.instance.GameWait = true;

            Time.timeScale = 0f;

            PausePanel.SetActive(true);
        }

        else OnCancelButtonClicked();

    }
    public void OnHomeButtonClicked()
    {
        if (GameManager.instance.gameState != GameManager.GameState.GamePlay) return;

        Time.timeScale = 1.0f;

        SoundManager.instance.SkillNum = new int[6];

        SoundManager.instance.Depth = 0;

        SoundManager.instance.SkillLeft = 0;

        SoundManager.instance.Score = 0;

        Destroy(MapGenerator.instance.gameObject);

        SoundManager.instance.BGM[SoundManager.instance.NowPlaying].Stop();

        SoundManager.instance.main();

        SceneManager.LoadScene("Title");
    }

    public void OnGameInfoButtonClicked()
    {
        if (GameManager.instance.gameState != GameManager.GameState.GamePlay) return;

        GameInfoPanel.SetActive(true);

        PausePanel.SetActive(false);
    }

    public void OnCancelButtonClicked()
    {
        if (GameManager.instance.gameState != GameManager.GameState.GamePlay) return;

        isActive = false;

        GameManager.instance.GameWait = false;

        Time.timeScale = 1.0f;

        PausePanel.SetActive(false);

        GameInfoPanel.SetActive(false);
    }

    public void OnNextButtonClicked(int dir)
    {
        switch (dir)
        {
            // Previous Guide
            case 0:

                Guide[CurrentGuideNum--].SetActive(false);

                if (CurrentGuideNum == -1) CurrentGuideNum = Guide.Length - 1;

                Guide[CurrentGuideNum].SetActive(true);

                break;

            // Next Guide
            case 1:

                Guide[CurrentGuideNum++].SetActive(false);

                if (CurrentGuideNum == Guide.Length) CurrentGuideNum = 0;

                Guide[CurrentGuideNum].SetActive(true);

                break;
        }
    }
}