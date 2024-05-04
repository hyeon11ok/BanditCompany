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
                        // �÷��̾� üũ
                        if (TargetSearch()) {
                            eState = EnemyState.Chase;
                            break;
                        }

                        // �÷��̾ �� ã���� ���
                        if (SetProwlPoint()) {
                            eState = EnemyState.Prowl;
                            break;
                        }
                    }
                    break;
                case EnemyState.Prowl: {
                        // �÷��̾� üũ
                        if (TargetSearch()) {
                            eState = EnemyState.Chase;
                            break;
                        }

                        // �÷��̾ �� ã���� ���
                        moveSpeed = prowlSpeed;
                        Move(targetPos);
                        // ��ǥ������ �������� ���
                        if (GetDist(targetPos) < 0.2f) {
                            eState = EnemyState.Idle;
                        }
                    }
                    break;
                case EnemyState.Chase: {
                        moveSpeed = chaseSpeed;
                        Move(targetPos);

                        // ���̻� �÷��̾ ������ �������
                        if (!TargetSearch()) {
                            // ������ ����������� �̵� �Ŀ��� ������ ������ �ٽ� Idle��
                            if (GetDist(targetPos) < atkRange) {
                                eState = EnemyState.Idle;
                            }
                        } else {
                            if (GetDist(targetPos) < atkRange) {
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

        public override void Attack() {
            Debug.Log("Attack");
        }
    }
}
