using UnityEngine;

namespace ShootEmAll
{
    public class Bullet : MonoBehaviour
    {

        float speed = 20;
        float startTime;
        float lifeTime = 1;
        public uint owner { get; set; }

        private void Start()
        {
            startTime = Time.time;
        }

        void Update()
        {
            transform.position += transform.forward * Time.deltaTime * speed;

            if (Time.time > startTime + lifeTime)
            {
                Destroy(this.gameObject);
            }
        }

    }
}