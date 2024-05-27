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
                    // �ν� ���� ǥ��
                    Gizmos.color = Color.gray;
                    Gizmos.DrawWireSphere(spawnPos, sightRange);

                    // ���� ���� ǥ��
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireSphere(transform.position, atkRange);
                }
            }
        }

        public override bool TargetSearch() {
            int playerLayermask = 1 << 6; // 6�� ���̾��ũ�� �÷��̾�� ����
            Collider[] colls = Physics.OverlapSphere(spawnPos, sightRange, playerLayermask); // ������ �������� ���� ���� �ȿ� ������ �÷��̾� Ž��

            if (colls.Length > 0) {
                target = colls[0].gameObject;
                return true;
            } else {
                return false;
            }
        }
    }
}

