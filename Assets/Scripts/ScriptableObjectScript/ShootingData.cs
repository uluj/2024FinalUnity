using UnityEngine;

namespace ScriptableObjectScript
{
    [CreateAssetMenu(fileName = "ShootingData", menuName = "ScriptableObjects/ShootingData", order = 1)]
    public class ShootingData : ScriptableObject
    {
        [Header("Gun Properties")] public float range = 100f;
        public int damageAmount = 10;
        public bool automatic;
        public float fireRate = 1f;
        
        [Header("Visual Effects")] 
        public GameObject muzzleFlashPrefab;
        public GameObject hitEffectPrefab;
        
        [Header("Audio")] 
        public AudioClip sfx; // Sound effect for shooting
    }
}