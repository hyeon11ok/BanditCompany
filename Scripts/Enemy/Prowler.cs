using UnityEngine;
using UnityEngine.AI;

namespace EnemyEnum {
    public abstract class Prowler : Enemy {
        protected float chaseSpeed;
        protected float prowlSpeed;
        protected float prowlRange = 20;
        protected Vector3 prowlPos;
        [Range(0f, 360f)][SerializeField] float viewAngle = 0f; // 유닛 시야각 구현 변수

        public override void OnDrawGizmos() {
            if (transform.parent.GetComponent<EnemySpawner>() != null) {
                if (transform.parent.GetComponent<EnemySpawner>().IsDebug) {
                    // 시야범위
                    Gizmos.color = Color.gray;
                    Gizmos.DrawWireSphere(transform.position, sightRange);

                    // 시야각
                    Gizmos.color = Color.red;
                    float radianR = (transform.eulerAngles.y + viewAngle * 0.5f) * Mathf.Deg2Rad;
                    float radianL = (transform.eulerAngles.y - viewAngle * 0.5f) * Mathf.Deg2Rad;
                    Vector3 dirR = new Vector3(Mathf.Sin(radianR), 0f, Mathf.Cos(radianR));
                    Vector3 dirL = new Vector3(Mathf.Sin(radianL), 0f, Mathf.Cos(radianL));
                    Gizmos.DrawLine(transform.position, transform.position + (dirR * sightRange)); // 오른쪽 시야각
                    Gizmos.DrawLine(transform.position, transform.position + (dirL * sightRange)); // 왼쪽 시야각

                    // 공격 범위 표시
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireSphere(transform.position, atkRange);
                }
            }
        }

        public bool SetProwlPoint() {
            for (int i = 0; i < 30; i++) {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * prowlRange;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, prowlRange, NavMesh.AllAreas)) {

                    prowlPos = hit.position;
                    return true;
                }
            }

            return false;
        }

        public override bool TargetSearch() {
            int targetMask = 1 << 6;
            int obstacleMask = 1 << 7;

            // viewRadius를 반지름으로 한 원 영역 내 targetMask 레이어인 콜라이더를 모두 가져옴
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, sightRange, targetMask);

            for (int i = 0; i < targetsInViewRadius.Length; i++) {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                // 플레이어와 forward와 target이 이루는 각이 설정한 각도 내라면
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) {
                    float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                    // 타겟으로 가는 레이캐스트에 obstacleMask가 걸리지 않으면 visibleTargets에 Add
                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) {
                        this.target = target.gameObject;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

