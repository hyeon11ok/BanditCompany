using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyEnum {
    public class BossZombie : Chaser {
        

        // Start is called before the first frame update
        void Start() {
            InitStatus();
            if (TargetSearch()) {
                eState = EnemyState.Chase;
            }
        }

        // Update is called once per frame
        void Update() {
            switch (eState) {
                case EnemyState.Chase: {
                        Move(target.transform.position);

                        if (GetDist(target.transform.position) < atkRange) {
                            eState = EnemyState.Attack;
                        }
                    }
                    break;
            }
        }

        public override void InitStatus() {
            hp = 999999999;
            curHp = hp;
            atkDmg = 50;
            moveSpeed = 2f;
            atkRange = 1;
            sightRange = 8;
            agent = GetComponent<NavMeshAgent>();
        }
    }
}

