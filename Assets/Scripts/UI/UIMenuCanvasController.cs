using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuCanvasController : MonoBehaviour
{
    Animator menu_animator;
    string next_scene;
    bool isDone;
    // Start is called before the first frame update
    void Start()
    {
        menu_animator = GetComponent<Animator>();
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
