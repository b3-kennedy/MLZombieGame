using Assets.SimpleSpinner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance;

    public GameObject loadScreen;
    public GameObject loadingBar;
    public SimpleSpinner spinner;

    float loadingbarScale;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }


    public void SwitchToScene(int sceneIndex)
    {
        var rect = loadingBar.GetComponent<RectTransform>().rect;
        rect.width = 0;
        loadScreen.SetActive(true);
        StartCoroutine(SwitchToSceneAsync(sceneIndex));
    }

    IEnumerator SwitchToSceneAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            Debug.Log(operation.progress);
            var rect = loadingBar.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2((0.9f * operation.progress) * 100, rect.sizeDelta.y);
            spinner.Spin();
            //loadingBar.GetComponent<Image>().color = new Color(operation.progress * 255, 0, 0);
            yield return null;
        }
        var loadingRect = loadingBar.GetComponent<RectTransform>();
        loadingRect.sizeDelta = new Vector2(100, loadingRect.sizeDelta.y);
        yield return new WaitForSeconds(0.2f);
        loadScreen.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
