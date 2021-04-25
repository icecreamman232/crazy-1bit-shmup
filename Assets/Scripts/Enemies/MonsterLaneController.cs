using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLaneController : MonoBehaviour
{
    public float min_spawn_delay;
    public float max_spawn_delay;

    float spawn_delay;


    public int wave_index;
    public List<GameObject> list_monsters;


    // Start is called before the first frame update
    void Start()
    {
    }
    public void StartMonsterLane()
    {
        wave_index = 0;
        StopAllCoroutines();
        StartCoroutine(OnSpawnNextMonster());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void  GetRandomDelay()
    {
        spawn_delay = Random.Range(min_spawn_delay, max_spawn_delay);
    }
    public IEnumerator OnSpawnNextMonster()
    {
        while(true)
        {
            GetRandomDelay();
            yield return new WaitForSeconds(spawn_delay);
            SpawnMonster();
            
        }
    }
    void SpawnMonster()
    {
        //Can switch to other monster here?
        var rand = Random.Range(0, 2);
        var monster = Lean.Pool.LeanPool.Spawn(list_monsters[rand], this.transform);
        monster.GetComponent<BaseMonster>().Run(GameManager.Instance.GetCurrentLevelSpeed(GameManager.Instance.wave_index));
        wave_index += 1;
    }
}
