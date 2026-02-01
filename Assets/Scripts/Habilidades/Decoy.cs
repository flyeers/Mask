using AI.Perception;
using UnityEngine;

namespace Habilidades
{
    public class Decoy : MonoBehaviour
    {
        [SerializeField] 
        private GameObject decoyPrefab;
        [SerializeField]
        private Perceivable perceivable;
        [SerializeField] 
        private Perceivable.EEntityType maskWhenDecoy;

        private Perceivable.EEntityType originalMask;
    }
}
