using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] normalZombie;
    [SerializeField] GameObject[] uniqueZombie;
    [SerializeField] GameObject bossZombie;
    [SerializeField] private int minCnt; // �Ϲ� ���� �ּ� ��
    [SerializeField] private int maxCnt; // �Ϲ� ���� �ִ� ��
    [SerializeField] private bool isDebug;
    [SerializeField] private float n_spawnTime; // �Ϲ����� ���� ��Ÿ��
    [SerializeField] private float u_spawnTime; // ����ũ���� ���� ��Ÿ��
    [SerializeField] private float b_spawnTime; // �������� ���� ��Ÿ��

    public bool IsDebug {  get { return isDebug; } }

    // �Ϲ�, ����ũ ���� ���� �ð����� ��ȯ
    // ������ ��� ���ѽð��� ���� �� ���� �� ��ȯ
    public void SpawnZombie(MapGenerator mapGen, float playTime) {
        if(playTime == 0) {
            SpawnNomalZombie(mapGen);
            SpawnUniqueZombie(mapGen);
        }
        // �Ϲ����� ���� ��Ÿ�� üũ
        if(playTime / n_spawnTime == 0)
            SpawnNomalZombie(mapGen);
        // ����ũ ���� ���� ��Ÿ�� üũ
        if (playTime / u_spawnTime == 0)
            SpawnUniqueZombie(mapGen);
        // ���� ���� ���� ��Ÿ�� üũ
        if (playTime / b_spawnTime == 0)
            SpawnBossZombie(mapGen);
    }

    void SpawnNomalZombie(MapGenerator mapGen) {
        int totalCnt = Random.Range(minCnt, maxCnt); // �Ϲ� ���� ��

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
        // �� ����� �ʿ� 1�������� ����
        for(int i = 0; i < uniqueZombie.Length; i++) {
            // �� ��° for���� ��ȯ ���� ������ ã�� ���� ��츦 ����Ͽ� ���
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
