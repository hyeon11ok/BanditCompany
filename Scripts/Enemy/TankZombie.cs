using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyEnum {
    public class TankZombie : Sentinel {
        // Start is called before the first frame update
        void Start() {
            InitStatus();
        }

        // Update is called once per frame
        void Update() {
            switch (eState) {
                case EnemyState.Idle: {
                        // 플레이어 체크
                        if (TargetSearch()) {
                            eState = EnemyState.Chase;
                            break;
                        }
                    }
                    break;
                case EnemyState.Chase: {
                        Move(target_tr.position);

                        if (GetDist(target_tr.position) < atkRange) {
                            eState = EnemyState.Attack;
                        }
                    }
                    break;
            }
        }

        public override void Attack() {
            throw new System.NotImplementedException();
        }

        public override void InitStatus() {
            hp = 150;
            curHp = hp;
            atkDmg = 10;
            moveSpeed = 1.8f;
            atkRange = 1;
            sightRange = 8;
            agent = GetComponent<NavMeshAgent>();
            spawnPos = transform.position;
        }
    }
}

