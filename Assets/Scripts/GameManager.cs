using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool gameGoing = true;

    private int dotCount = 0;
    private int coinCount = 0;
    private int starCount = 0;

    public int dotsCollected = 0;
    public int coinsCollected = 0;
    public int starsCollected = 0;
    
    public List<string> sceneNames = new List<string>();
    public int currentSceneIndex = 0;

    private Scene currentScene;

    public CanvasManager canvasManager;

    public AudioClip winAudio;
    public AudioClip spawnAudio;
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }
    private void Awake()
    {
        if (Instance == null) 
        {
            DontDestroyOnLoad(gameObject); 
            Instance = this;
        }
        else if (Instance != this) 
        {
            Destroy(gameObject); 
        }
        SoundManager.Play(spawnAudio, 1f, 0.5f);
    }

    async void Update()
    {

        canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();
        
        if(currentScene.name == null) 
        {
            gameGoing = true;
            currentScene = SceneManager.GetActiveScene();
        }
        await new WaitForSeconds(1);
        if (dotCount == 0 && coinCount == 0 && starCount == 0)
        {
            CountCollectables();
        }
            
            
    }

    public void CollectDot()
    {
        dotsCollected++;
    }

    public void CollectCoin()
    {
        coinsCollected++;
    }

    public void CollectStar()
    {
        starsCollected++;
    }

    public void Win()
    {
        SoundManager.Play(winAudio, 1f, 1f);
        gameGoing = false;

        canvasManager.WinScreen(starsCollected, dotsCollected, coinsCollected, dotCount, coinCount);
    }

    public void Die()
    {
        gameGoing = false;

        canvasManager.LooseScreen();
    }

    public void NextLevel()
    {
        currentSceneIndex++;
        if (currentSceneIndex >= sceneNames.Count) currentSceneIndex = 0;
        SceneManager.LoadScene(sceneNames[currentSceneIndex]);
        ResetCollectables();
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(currentScene.name);

        ResetCollectables();

    }

    private void ResetCollectables()
    {
        dotCount = 0;
        coinCount = 0;
        starCount = 0;

        dotsCollected = 0;
        coinsCollected = 0;
        starsCollected = 0;
    }

    public void CountCollectables()
    {
        var dots = GameObject.FindGameObjectsWithTag("Dot");
        var coins = GameObject.FindGameObjectsWithTag("Coin");
        var stars = GameObject.FindGameObjectsWithTag("Star");

        foreach (GameObject dot in dots)
        {
            print(1);
            dotCount++;
        }

        foreach (GameObject coin in coins)
        {
            coinCount++;
        }

        foreach (GameObject star in stars)
        {
            starCount++;
        }
    }
}
