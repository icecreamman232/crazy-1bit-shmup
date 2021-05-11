using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMonsterController : MonoBehaviour
{
    public List<MonsterWithSplineMove> monsterList;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Run()
    {
        StartCoroutine(OnMonsterRunning());
    }
    IEnumerator OnMonsterRunning()
    {
        for(int i  = 0; i < monsterList.Count; i++)
        {
            monsterList[i].Run(); 
            yield return new WaitForSeconds(0.5f);
        }
    }
}
