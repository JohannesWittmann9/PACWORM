using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    private MusicManager musicManager;

    void Start()
    {
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoaded;
        musicManager = this.GetComponent<MusicManager>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMoad)
    {
        if (scene.buildIndex == 1) GameObject.FindGameObjectWithTag("ExitButton").GetComponent<Button>().onClick.AddListener(QuitGame);
        
    }

    void Update()
    {
        
    }

    public void LoadLevel1()
    {
        StartCoroutine(LoadAsyncScene(1));
        musicManager.PlayGame();
    }

    public void LoadLevel2()
    {
        StartCoroutine(LoadAsyncScene(2));
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
        UnityEditor.EditorApplication.isPlaying = false;
    }


}
