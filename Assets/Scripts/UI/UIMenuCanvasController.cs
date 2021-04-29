using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuCanvasController : MonoBehaviour
{
    public Text version_text;
    Animator menu_animator;
    string next_scene;
    bool isDone;
    // Start is called before the first frame update
    void Start()
    {
        menu_animator = GetComponent<Animator>();
        version_text.text = "Version " + Application.version;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AnimationFinished()
    {
        SceneLoader.Instance.LoadScene(next_scene);
    }
    public void CallbackToTransition(string scene_name)
    {
        next_scene = scene_name;
        menu_animator.Play("menu_outtro");
    }
    
}
