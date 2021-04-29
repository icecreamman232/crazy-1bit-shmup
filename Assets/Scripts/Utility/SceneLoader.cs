using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    #region Singleton
    public static SceneLoader Instance;
    #endregion

    public Animator scene_loader_animator;
    public Image loading_bg;
    public Image loading_logo;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(loading_bg.gameObject.transform.parent);
    }
    void Start()
    {
        RectTransform rt = loading_bg.rectTransform;
        rt.sizeDelta = new Vector2(Screen.width, Screen.height);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadScene(string scene_name)
    {
        StartCoroutine(LoadAsyncScene(scene_name));
    }
    IEnumerator LoadAsyncScene(string scene_name)
    {
        AsyncOperation asyncload = SceneManager.LoadSceneAsync(scene_name);
        loading_bg.gameObject.SetActive(true);
        loading_logo.gameObject.SetActive(true);
        scene_loader_animator.Play("loading_logo_anim");
        while (!asyncload.isDone)
        {
            scene_loader_animator.Play("New State");
            loading_bg.gameObject.SetActive(false);
            loading_logo.gameObject.SetActive(false);
            yield return null;
        }
    }
}
