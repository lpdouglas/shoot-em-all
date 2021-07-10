using Mirror;
using UnityEngine;

namespace ShootEmAll
{
    public class Player : NetworkBehaviour
    {
        public Bullet bulletPrefab;
        public GameObject bulletPosition;
        public float speed = 10;

        float x = 0;
        float z = 0;
        bool fire;
        float fireLastTime;
        float fireInterval = 0.5f;
        [SyncVar] bool died = false;


        private void Start()
        {
            transform.position = transform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        }
        void Update()
        {
            if (!isLocalPlayer) return;
            if (died) return;

            x = Input.GetAxisRaw("Horizontal");
            z = Input.GetAxisRaw("Vertical");
            fire = Input.GetButton("Fire1");


            float speedDelta = Time.deltaTime * speed;
            if (x != 0 || z != 0)
            {
                Vector3 movement = new Vector3(x, 0, z).normalized * speedDelta;
                transform.position = transform.position + movement;
                transform.rotation = Quaternion.LookRotation(movement);
            }

            if (fire && fireLastTime < Time.time)
            {
                CmdFire();
                fireLastTime = Time.time + fireInterval;
            }
        }

        [Command]
        private void CmdFire()
        {
            if (fireLastTime < Time.time)
            {
                fireLastTime = Time.time + fireInterval;
                Bullet bullet = Instantiate(bulletPrefab, bulletPosition.transform.position, bulletPosition.transform.rotation);
                bullet.owner = netId;                
                NetworkServer.Spawn(bullet.gameObject);
            }
        }
        
        void Die()
        {
            died = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isServer) return;

            Bullet bullet = other.GetComponentInParent<Bullet>();
            if (bullet == null) return;
            if (bullet.owner == netId) return;

            Debug.Log(other);
            Die();
        }

    }

}