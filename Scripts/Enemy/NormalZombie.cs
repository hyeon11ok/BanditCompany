using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyEnum {
    public class NormalZombie : Prowler {
        // Start is called before the first frame update
        void Start() {
            InitStatus();
        }

        // Update is called once per frame
        void Update() {
            switch (eState) {
                case EnemyState.Idle: {
                        moveSpeed = prowlSpeed;
                        // 플레이어 체크
                        if (TargetSearch()) {
                            eState = EnemyState.Chase;
                            break;
                        }

                        // 플레이어를 못 찾았을 경우
                        if (SetProwlPoint()) {
                            eState = EnemyState.Prowl;
                            break;
                        }
                    }
                    break;
                case EnemyState.Prowl: {
                        // 플레이어 체크
                        if (TargetSearch()) {
                            eState = EnemyState.Chase;
                            break;
                        }

                        // 플레이어를 못 찾았을 경우
                        moveSpeed = prowlSpeed;
                        Move(prowlPos);
                        // 목표지점에 도착했을 경우
                        if (GetDist(prowlPos) < 0.2f) {
                            eState = EnemyState.Idle;
                        }
                    }
                    break;
                case EnemyState.Chase: {
                        moveSpeed = chaseSpeed;
                        Move(target.transform.position);

                        // 더이상 플레이어가 보이지 않을경우
                        if (!TargetSearch()) {
                            // 마지막 목격지점까지 이동 후에도 보이지 않으면 다시 Idle로
                            if (GetDist(target.transform.position) < atkRange) {
                                eState = EnemyState.Idle;
                            }
                        } else {
                            if (GetDist(target.transform.position) < atkRange) {
                                eState = EnemyState.Attack;
                            }
                        }
                    }
                    break;
            }
        }

        public override void InitStatus() {
            hp = 100;
            curHp = hp;
            atkDmg = 7;
            prowlSpeed = 1;
            chaseSpeed = 1.5f;
            atkRange = 3;
            sightRange = 7;
            agent = GetComponent<NavMeshAgent>();
        }
    }
}
