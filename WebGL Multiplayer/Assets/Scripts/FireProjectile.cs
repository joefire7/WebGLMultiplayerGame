using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FireProjectile : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject serverProjectilePrefab;
    [SerializeField] private GameObject clientProjectilePrefab;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private Collider2D playerCollider;

    [Header("Setting")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float muzzleFlashDuration;
    public NetworkVariable<int> bulletCount = new NetworkVariable<int>(5, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    //public int bulletCount = 5;

    private bool shouldFire;
    private float previousFireTime;
    private float muzzleFlashTimer;
    public override void OnNetworkSpawn()
    {
       if(!IsOwner) return;
        inputReader.PrimaryFireEvent += HandlePrimaryFire;
        UIController.instance.fireProjectile = this;
    }

    public override void OnNetworkDespawn()
    {
        if(!IsOwner) return;
        inputReader.PrimaryFireEvent -= HandlePrimaryFire;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R)) { bulletCount.Value += 1; }

        if (muzzleFlashTimer > 0)
        {
            muzzleFlashTimer -= Time.deltaTime;
            if(muzzleFlashTimer <= 0)
            {
                muzzleFlash.SetActive(false);
            }
        }
        if (!IsOwner) return;

        if (!shouldFire) return;

        if (UIController.instance.isViewsActivate) return;

        if (bulletCount.Value <= 0) return;
        
        
        // Fire the bullet more slow
        if (Time.time < (1 / fireRate) + previousFireTime) return;

        // Server Spawn Projectile RPC
        PrimaryFireServerRpc(projectileSpawnPoint.position, projectileSpawnPoint.up);
        // Dummy Local Spawn Projectile
        SpawnDummyProjectile(projectileSpawnPoint.position, projectileSpawnPoint.up);

        previousFireTime = Time.time;
    }

    private void HandlePrimaryFire(bool shouldFire)
    {
        this.shouldFire = shouldFire;
    }

    [ServerRpc] 
    private void PrimaryFireServerRpc(Vector3 spawnPos, Vector3 direction)
    {
        if(bulletCount.Value <= 0) return;

        GameObject projectileInstance = Instantiate(serverProjectilePrefab, spawnPos, Quaternion.identity);
        projectileInstance.transform.up = direction;
        Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());

        if(projectileInstance.TryGetComponent<DealDamageOnContact>(out DealDamageOnContact dealDamage))
        {
            dealDamage.SetOwner(OwnerClientId);
        }

        if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }

        bulletCount.Value -= 1;
        SpawnDummyProjectileClientRpc(spawnPos, direction);
    }

    [ClientRpc]
    private void SpawnDummyProjectileClientRpc(Vector3 spawnPos, Vector3 direction)
    {
        if (IsOwner) return;
        if (IsOwner && bulletCount.Value <= 0) return;
        SpawnDummyProjectile(spawnPos, direction);
    }

    private void SpawnDummyProjectile(Vector3 spawnPos, Vector3 direction)
    {

        if (bulletCount.Value <= 0) return;

        if(IsOwner && bulletCount.Value == 0)
        {
            Debug.Log("Dummy Projectile: 0");
            return;
        }else
        {
            muzzleFlash.SetActive(true);
            muzzleFlashTimer = muzzleFlashDuration;
            GameObject projectileInstance = Instantiate(clientProjectilePrefab, spawnPos, Quaternion.identity);
            projectileInstance.transform.up = direction;
            Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());

            if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb.velocity = rb.transform.up * projectileSpeed;
            }
        }
      
    }
    [ServerRpc(RequireOwnership = false)]
    public void AddedBulletServerRpc(int bullet)
    {
        bulletCount.Value += bullet;
    }

}
