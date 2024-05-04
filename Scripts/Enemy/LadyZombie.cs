using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyEnum {
    public class LadyZombie : Sentinel {
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
            hp = 80;
            curHp = hp;
            atkDmg = 10;
            moveSpeed = 2;
            atkRange = 1;
            sightRange = 10;
            agent = GetComponent<NavMeshAgent>();
            spawnPos = transform.position;
        }
    }
}

