using UnityEngine;
using UnityEngine.Events;

namespace Actions{
    public static class CharacterActions{
        public static UnityAction<GameObject, int> Damaged;
        public static UnityAction<GameObject, int> Healed;
    }
}