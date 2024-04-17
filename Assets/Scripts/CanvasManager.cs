using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public GameObject overPanel;
    public GameObject tintPanel;

    public GameObject winPanel;
    public TextMeshProUGUI levelNameText;
    public GameObject starsPanel;
    public TextMeshProUGUI dotsCountText;
    public TextMeshProUGUI coinCountText;

    public GameObject loosePanel;
    public TextMeshProUGUI looseLevelNameText;

    private Scene currentScene;

    public GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentScene = SceneManager.GetActiveScene();

    }

    public void WinScreen(int starsCollected, int dotsCollected, int coinsCollected, int dotCount, int coinCount)
    {

        overPanel.active = true;
        winPanel.active = true;

        tintPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0.9f);

        levelNameText.text = currentScene.name;

        foreach (Transform star in starsPanel.transform)
        {
            if (starsCollected == 0) break;
            starsCollected--;
            star.GetComponent<RawImage>().color = new Color(255, 255, 0);
        }

        dotsCountText.text = $"{dotsCollected}/{dotCount}";

        coinCountText.text = $"{coinsCollected}/{coinCount}";
    }

    public void LooseScreen()
    {

        tintPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0.9f);

        overPanel.active = true;
        loosePanel.active = true;

        looseLevelNameText.text = currentScene.name;
    }

    public void NextLevelTrigger()
    {
        gameManager.NextLevel();
    }

    public void ResetLevelTrigger()
    {
        gameManager.PlayAgain();
    }
}
