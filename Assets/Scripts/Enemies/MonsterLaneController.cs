using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLaneController : MonoBehaviour
{
    public float min_spawn_delay;
    public float max_spawn_delay;

    float min_limit;
    float max_limit;



    float spawn_delay;


    public int wave_index;
    public List<GameObject> list_monsters;


    // Start is called before the first frame update
    void Start()
    {
    }
    public void StartMonsterLane(float MinLimit, float MaxLimit)
    {
        wave_index = 0;
        min_limit = MinLimit;
        max_limit = MaxLimit;
        StopAllCoroutines();
        StartCoroutine(OnSpawnNextMonster());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateSpawnRate(float min_decrease_rate, float max_decrease_rate )
    {
        min_spawn_delay -= min_decrease_rate;
        if(min_spawn_delay < min_limit) { min_spawn_delay = min_limit; }

        max_spawn_delay -= max_decrease_rate;
        if (max_spawn_delay < max_limit) { max_spawn_delay = max_limit; }
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
        monster.GetComponent<BaseMonster>().Run();
        wave_index += 1;
    }
}
