using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyEnum {
    public abstract class Sentinel : Enemy {
        protected Vector3 spawnPos;

        public Vector3 SpawnPos { set { spawnPos = value; } }

        public override void OnDrawGizmos() {
            if (transform.parent.GetComponent<EnemySpawner>() != null) {
                if (transform.parent.GetComponent<EnemySpawner>().IsDebug) {
                    // 인식 범위 표시
                    Gizmos.color = Color.gray;
                    Gizmos.DrawWireSphere(spawnPos, sightRange);

                    // 공격 범위 표시
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireSphere(transform.position, atkRange);
                }
            }
        }

        public override bool TargetSearch() {
            int playerLayermask = 1 << 6; // 6번 레이어마스크를 플레이어로 설정
            Collider[] colls = Physics.OverlapSphere(spawnPos, sightRange, playerLayermask); // 스포너 기준으로 일정 범위 안에 들어오는 플레이어 탐색

            if (colls.Length > 0) {
                target = colls[0].gameObject;
                return true;
            } else {
                return false;
            }
        }
    }
}

