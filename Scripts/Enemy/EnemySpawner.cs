using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] normalZombie;
    [SerializeField] GameObject[] uniqueZombie;
    [SerializeField] GameObject bossZombie;
    [SerializeField] private int minCnt; // 일반 좀비 최소 수
    [SerializeField] private int maxCnt; // 일반 좀비 최대 수
    [SerializeField] private bool isDebug;
    [SerializeField] private float n_spawnTime; // 일반좀비 스폰 쿨타임
    [SerializeField] private float u_spawnTime; // 유니크좀비 스폰 쿨타임
    [SerializeField] private float b_spawnTime; // 보스좀비 스폰 쿨타임

    public bool IsDebug {  get { return isDebug; } }

    // 일반, 유니크 좀비를 일정 시간마다 소환
    // 보스의 경우 제한시간이 거의 다 됐을 때 소환
    public void SpawnZombie(MapGenerator mapGen, float playTime) {
        if(playTime == 0) {
            SpawnNomalZombie(mapGen);
            SpawnUniqueZombie(mapGen);
        }
        // 일반좀비 스폰 쿨타임 체크
        if(playTime / n_spawnTime == 0)
            SpawnNomalZombie(mapGen);
        // 유니크 좀비 스폰 쿨타임 체크
        if (playTime / u_spawnTime == 0)
            SpawnUniqueZombie(mapGen);
        // 보스 좀비 스폰 쿨타임 체크
        if (playTime / b_spawnTime == 0)
            SpawnBossZombie(mapGen);
    }

    void SpawnNomalZombie(MapGenerator mapGen) {
        int totalCnt = Random.Range(minCnt, maxCnt); // 일반 좀비 수

        while (totalCnt > 0){
            NavMeshHit hit;
            if (NavMesh.SamplePosition(mapGen.GetRandomPos(), out hit, 10f, NavMesh.AllAreas)) {
                int idx = Random.Range(0, normalZombie.Length);
                GameObject tmp = Instantiate(normalZombie[idx], hit.position, Quaternion.identity);
                tmp.transform.parent = transform;
                totalCnt--;
            }
        }
    }

    void SpawnUniqueZombie(MapGenerator mapGen) {
        // 각 좀비는 맵에 1마리씩만 존재
        for(int i = 0; i < uniqueZombie.Length; i++) {
            // 두 번째 for문은 소환 가능 지점을 찾지 못할 경우를 대비하여 사용
            NavMeshHit hit;
            for (int j = 0; j < 30; j++) {
                if (NavMesh.SamplePosition(mapGen.GetRandomPos(), out hit, 10, NavMesh.AllAreas)) {
                    GameObject tmp = Instantiate(uniqueZombie[i], hit.position, Quaternion.identity);
                    tmp.transform.parent = transform;
                    break;
                }
            }
        }
    }

    public void SpawnBossZombie(MapGenerator mapGen) {
        NavMeshHit hit;
        for (int j = 0; j < 30; j++) {
            if (NavMesh.SamplePosition(mapGen.GetRandomPos(), out hit, 10, NavMesh.AllAreas)) {
                GameObject tmp = Instantiate(bossZombie, hit.position, Quaternion.identity);
                tmp.transform.parent = transform;
                break;
            }
        }
    }
}
