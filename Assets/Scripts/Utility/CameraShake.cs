using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Vector3 origin_position;

    public float duration;
    public float distance;
    float timer;
    public bool isFinished;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Setup()
    {
        origin_position = transform.position;
        timer = 0;
        isFinished = false;
    }
    public void Shake()
    {
        StartCoroutine(OnShaking());
    }
    IEnumerator OnShaking()
    {
        timer = 0;
        isFinished = false;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            var rand_pos = origin_position + Random.insideUnitSphere * distance;
            transform.position = rand_pos;
            yield return null;
        }
        transform.position = origin_position;
        isFinished = true;
    }
}
