using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;

    private static GameObject manager;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (manager == null)
        {
            manager = gameObject;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        Button level1 = GameObject.Find("Level1Button").GetComponent<Button>();
        level1.onClick.AddListener(LoadLevel1);

        GameObject managers = GameObject.Find("Managers");
        gameManager = managers.GetComponent<GameManager>();

        SetHighscore();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMoad)
    {
        if (scene.buildIndex == 1)
        {
            gameManager.enabled = true;
            GameObject.FindGameObjectWithTag("ExitButton").GetComponent<Button>().onClick.AddListener(QuitGame);
        }
        if (scene.buildIndex == 0)
        {
            gameManager.enabled = false;
            GameObject.Find("BackgroundMusic").GetComponent<MusicManager>().PlayIntro();
            Button level1 = GameObject.Find("Level1Button").GetComponent<Button>();
            if(level1.onClick.GetPersistentEventCount() == 0) level1.onClick.AddListener(LoadLevel1);
            SetHighscore();
        }
        
    }

    void Update()
    {
        
    }

    public void LoadLevel1()
    {
        StartCoroutine(LoadAsyncScene(1));
    }

    public void LoadLevel2()
    {
        StartCoroutine(LoadAsyncScene(2));
    } 

    public void LoadStartScreen()
    {
        StartCoroutine(LoadAsyncScene(0));
    }

    public IEnumerator LoadAsyncScene(int scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void QuitGame()
    {
    	#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit(); // This will exit the app in a built version
        #endif
    }

    private void SetHighscore()
    {
        int score = PlayerPrefs.GetInt("score");
        string time = PlayerPrefs.GetString("time");
        if (time.Length == 0) time = "00:00:00";
        GameObject.Find("Highscore").GetComponent<TMPro.TextMeshProUGUI>().text = "Highscore: " + score;
        GameObject.Find("Time").GetComponent<TMPro.TextMeshProUGUI>().text = "Time: " + time;
    }

}
