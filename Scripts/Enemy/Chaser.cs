using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace EnemyEnum {
    public abstract class Chaser : Enemy {

        public override void OnDrawGizmos() {
            if(transform.parent.GetComponent<EnemySpawner>() != null) {
                if (transform.parent.GetComponent<EnemySpawner>().IsDebug) {
                    // ���� ���� ǥ��
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireSphere(transform.position, atkRange);
                }
            }
        }

        public override bool TargetSearch() {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            if (players.Length != 0) {
                int idx = Random.Range(0, players.Length);
                target = players[idx].gameObject;
                return true;
            }
            return false;
        }
    }
}

