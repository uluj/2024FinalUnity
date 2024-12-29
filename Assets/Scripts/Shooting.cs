using ScriptableObjectScript;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private EnemyHealth target; // The target to be shot
    [SerializeField] private ShootingData shootingData; // ScriptableObject for shooting data
    [SerializeField] private Camera fpCamera; // First-person camera
    [SerializeField] private Transform muzzleFlashRefPoint; // Muzzle flash reference point
    [SerializeField] private ParticleSystem muzzleParticleSystem;
    private float _nextFireTime; // To manage fire rate for automatic weapons

    void Start()
    {
        InitializeReferences();
    }

    void Update()
    {
        HandleInput();
    }

    #region Initialization

    private void InitializeReferences()
    {
        AssignDefaultCamera();
        ValidateReferences();
    }

    private void AssignDefaultCamera()
    {
        if (fpCamera == null)
        {
            fpCamera = Camera.main;
            Debug.LogWarning("First-person camera not assigned. Defaulting to Camera.main.");
        }
    }

    private void ValidateReferences()
    {
        if (shootingData.muzzleFlashPrefab == null)
            Debug.LogError("Muzzle Flash Prefab is not assigned in ShootingData.");
        if (shootingData.hitEffectPrefab == null)
            Debug.LogError("Hit Effect Prefab is not assigned in ShootingData.");
        if (muzzleFlashRefPoint == null)
            Debug.LogError("Muzzle Flash Reference Point is not assigned in the Shooting script.");
    }

    #endregion

    #region Input Handling

    private void HandleInput()
    {
        if (shootingData.automatic)
        {
            HandleAutomaticFire();
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void HandleAutomaticFire()
    {
        if (Input.GetButton("Fire1") && Time.time >= _nextFireTime)
        {
            Shoot();
            _nextFireTime = Time.time + (1f / shootingData.fireRate);
        }
    }

    #endregion

    #region Shooting Logic

    private void Shoot()
    {
        if (PerformRaycast(out RaycastHit hit))
        {
            HandleHit(hit);
        }
        else
        {
            Debug.Log("No target hit.");
        }

        PlayMuzzleFlashEffect();
        PlayShootingSound();
    }

    private bool PerformRaycast(out RaycastHit hit)
    {
        Vector3 shootDirection = fpCamera.transform.forward;
        bool hitDetected = Physics.Raycast(fpCamera.transform.position, shootDirection, out hit, shootingData.range);

        DebugRaycast(shootDirection, hitDetected, hit);
        return hitDetected;
    }

    private void DebugRaycast(Vector3 direction, bool hitDetected, RaycastHit hit)
    {
        Debug.DrawRay(fpCamera.transform.position, direction * shootingData.range, Color.red, 2f);

        Debug.Log(hitDetected
            ? $"Raycast hit: {hit.transform.name} at position {hit.point}, distance: {hit.distance}."
            : "Raycast did not hit any object.");
    }

    #endregion

    #region Hit Handling

    private void HandleHit(RaycastHit hit)
    {
        Debug.Log($"Hit object: {hit.transform.name} at position {hit.point}.");

        AssignTarget(hit);
        if (target != null)
        {
            ApplyDamageToTarget();
        }

        PlayHitEffect(hit);
    }

    private void AssignTarget(RaycastHit hit)
    {
        target = hit.transform.GetComponent<EnemyHealth>();
        if (target == null)
        {
            Debug.Log("Hit object is not an EnemyHealth target.");
        }
    }

    private void ApplyDamageToTarget()
    {
        target.TakeDamage(shootingData.damageAmount);
        Debug.Log($"Dealt {shootingData.damageAmount} damage to {target.name}.");
    }

    #endregion

    #region Visual and Audio Effects

    private void PlayMuzzleFlashEffect()
    {
        if (shootingData.muzzleFlashPrefab != null && muzzleFlashRefPoint != null)
        {
            GameObject muzzleFlash = Instantiate(shootingData.muzzleFlashPrefab, muzzleFlashRefPoint.position, muzzleFlashRefPoint.rotation);
            Debug.Log("Played muzzle flash effect.");
            DestroyAfterDelay(muzzleFlash, 0.2f);
        }
        else
        {
            Debug.LogWarning("Cannot play muzzle flash effect due to missing references.");
        }
    }

// Destroys the instantiated object after a given delay
    private void DestroyAfterDelay(GameObject obj, float delay)
    {
        Destroy(obj, delay);
        Debug.Log($"Scheduled destruction of {obj.name} after {delay} seconds.");
    }


    private void PlayHitEffect(RaycastHit hit)
    {
        if (shootingData.hitEffectPrefab != null)
        {
            Instantiate(shootingData.hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            Debug.Log($"Created hit effect at {hit.point} with normal {hit.normal}.");
        }
        else
        {
            Debug.LogWarning("Hit effect prefab is not assigned in ShootingData.");
        }
    }

    private void PlayShootingSound()
    {
        if (shootingData.sfx != null)
        {
            AudioSource.PlayClipAtPoint(shootingData.sfx, muzzleFlashRefPoint.position);
            Debug.Log("Played shooting sound effect.");
        }
        else
        {
            Debug.LogWarning("Shooting sound effect is not assigned in ShootingData.");
        }
    }

    #endregion
}
