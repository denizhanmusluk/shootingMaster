using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;


public class GameManager : MonoBehaviour, IWinObserver, ILoseObserver
{
    public bool gameActive;
    public static GameManager Instance;
    [SerializeField] public GameObject startButton;
    [SerializeField] GameObject failPanel;
    [SerializeField] GameObject successPanel;
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject ProgressBar;
    [SerializeField] GameObject bulletButton, shotgunBulletButton, bombButton;
    [SerializeField] Transform bulletTargetUI;
    [SerializeField] GameObject player;
    [SerializeField] GameObject bulletPrefab, shotgunBulletPrefab, bombPrefab;
    [SerializeField] TextMeshProUGUI scoreText;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        winObservers = new List<IWinObserver>();
        loseObservers = new List<ILoseObserver>();
        startObservers = new List<IStartGameObserver>();
    }

    void Start()
    {
        startButton.SetActive(true);
        successPanel.SetActive(false);
        failPanel.SetActive(false);
        ProgressBar.SetActive(true);
        Add_WinObserver(this);
        Add_LoseObserver(this);
        scoreText.text = Globals.score.ToString();
        bulletButton.SetActive(true);
        shotgunBulletButton.SetActive(true);
        bombButton.SetActive(true);

    }

    public void scoreSet()
    {
        Globals.score++;
        scoreText.text = Globals.score.ToString();

    }


    public void StartButton()
    {
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(startDelay());
            bulletButton.SetActive(false);
            shotgunBulletButton.SetActive(false);
            bombButton.SetActive(false);

        }
    }
    IEnumerator startDelay()
    {
        yield return new WaitForSeconds(0.1f);
        Globals.isGameActive = true;
        startButton.SetActive(false);
        Notify_GameStartObservers();
    }
    public void RestartButton()
    {
        PlayerPrefs.SetInt("cost", 0);
        PlayerPrefs.SetInt("diamond", 0);
        PlayerPrefs.SetInt("level", 0);
        PlayerPrefs.SetInt("heart", 0);
        Globals.isGameActive = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void NextButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator bulletSelection(Transform selectedBullet)
    {
        while (Vector2.Distance(bulletTargetUI.position, selectedBullet.position) > 1)
        {
            selectedBullet.position = Vector2.MoveTowards(selectedBullet.position, bulletTargetUI.position, 3000 * Time.deltaTime);
            yield return null;
        }
        bulletTargetUI.GetComponent<Image>().sprite = selectedBullet.GetComponent<Image>().sprite;

        bulletButton.SetActive(false);
        shotgunBulletButton.SetActive(false);
        bombButton.SetActive(false);
    }

    public void bullet()
    {
        StartCoroutine(bulletSelection(bulletButton.transform.GetChild(0).transform));
        player.GetComponent<Shooting>().bulletPrefab = bulletPrefab;

    }
    public void shotgunBullet()
    {
        StartCoroutine(bulletSelection(shotgunBulletButton.transform.GetChild(0).transform));
        player.GetComponent<Shooting>().bulletPrefab = shotgunBulletPrefab;

    }
    public void bomb()
    {
        StartCoroutine(bulletSelection(bombButton.transform.GetChild(0).transform));
        player.GetComponent<Shooting>().bulletPrefab = bombPrefab;
        player.GetComponent<Shooting>().raycastActive = false;

    }

    public void LoseScenario()
    {
        Globals.isGameActive = false;
        ProgressBar.SetActive(false);


        StartCoroutine(Fail_Delay());
    }
    IEnumerator Fail_Delay()
    {
        yield return new WaitForSeconds(2);

        failPanel.SetActive(true);

    }
    public void WinScenario()
    {


        ProgressBar.SetActive(false);

        Globals.isGameActive = false;

        StartCoroutine(win_Delay());

        Globals.currentLevel++;
        PlayerPrefs.SetInt("level", Globals.currentLevel);

    }
    IEnumerator win_Delay()
    {
        yield return new WaitForSeconds(0.1f);

        successPanel.SetActive(true);

    }
    public void GameEnd()
    {

    }





    #region Observer Funcs

    private List<IWinObserver> winObservers;
    private List<ILoseObserver> loseObservers;
    private List<IStartGameObserver> startObservers;


    #region Start Observer
    public void Add_StartObserver(IStartGameObserver observer)
    {
        startObservers.Add(observer);
    }

    public void Remove_StartObserver(IStartGameObserver observer)
    {
        startObservers.Remove(observer);
    }

    public void Notify_GameStartObservers()
    {
        foreach (IStartGameObserver observer in startObservers.ToArray())
        {
            if (startObservers.Contains(observer))
                observer.StartGame();
        }
    }
    #endregion



    #region Win Observer
    public void Add_WinObserver(IWinObserver observer)
    {
        winObservers.Add(observer);
    }

    public void Remove_WinObserver(IWinObserver observer)
    {
        winObservers.Remove(observer);
    }

    public void Notify_WinObservers()
    {
        foreach (IWinObserver observer in winObservers.ToArray())
        {
            if (winObservers.Contains(observer))
                observer.WinScenario();
        }
    }
    #endregion

    #region Lose Observer
    public void Add_LoseObserver(ILoseObserver observer)
    {
        loseObservers.Add(observer);
    }

    public void Remove_LoseObserver(ILoseObserver observer)
    {
        loseObservers.Remove(observer);
    }

    public void Notify_LoseObservers()
    {
        foreach (ILoseObserver observer in loseObservers.ToArray())
        {
            if (loseObservers.Contains(observer))
                observer.LoseScenario();
        }
    }
    #endregion
    #endregion
}
