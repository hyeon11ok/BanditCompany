using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyEnum {
    public abstract class Enemy : MonoBehaviour {
        protected float hp;
        protected float curHp;
        protected float atkDmg;
        protected float moveSpeed;
        protected float atkRange;
        protected float sightRange;
        protected GameObject target;
        protected NavMeshAgent agent;
        protected EnemyState eState = EnemyState.Idle;

        public void GetDamaged(float damage) {
            if (curHp > damage) {
                curHp -= damage;
            } else {
                curHp = 0;
                // 사망 체크
                eState = EnemyState.Death;
            }
        }
        public abstract void InitStatus();
        public abstract bool TargetSearch();
        public void Move(Vector3 pos) {
            agent.speed = moveSpeed;
            agent.SetDestination(pos);
            Vector3 dir = pos - transform.position;
            dir.Normalize();
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 5 * Time.deltaTime);
        }
        public void Attack() {
            // 플레이어 매니저에 접근하여 공격
        }
        public abstract void OnDrawGizmos();
        public float GetDist(Vector3 target) {
            return Vector3.Distance(transform.position, target);
        }
    }
}