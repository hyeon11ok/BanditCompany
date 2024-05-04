using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    [SerializeField] MapGenerator mapGenerator;
    [SerializeField] EnemySpawner enemySpawner;

    private float playTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        mapGenerator.MapGenStart();
    }

    // Update is called once per frame
    void Update()
    {
        enemySpawner.SpawnZombie(mapGenerator, playTime);
        playTime += Time.deltaTime;
    }
}
