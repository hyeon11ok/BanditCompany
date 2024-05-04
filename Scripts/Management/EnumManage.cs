using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyEnum {
    public enum EnemyState {
        Idle = 0,
        Prowl,
        Chase,
        Attack,
        Death
    }
}
