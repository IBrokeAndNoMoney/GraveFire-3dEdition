using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public float health = 5;
    public float speed = 5;
    public float damage = 5;
    public float damageCooldown = 2f;

    float nextDamageTime;

    public GameObject player;
    public GameObject bullet;

    float minDistToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        nextDamageTime = Time.time + damageCooldown;
        minDistToPlayer = (transform.localScale.z/2 + player.transform.localScale.z/2) + 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y < -5)
        {
            Destroy(gameObject);

            player.GetComponent<PlayerController>().points += 1;

            GameObject newZombie = Instantiate(gameObject, Vector3.up * 2, Quaternion.identity);
            ZombieController newZombieController = newZombie.AddComponent<ZombieController>();
            newZombieController.bullet = bullet;
            newZombieController.player = player;
        }
    }

    private void FixedUpdate()
    {
        transform.LookAt(player.transform.position);

        if ((transform.position - player.transform.position).magnitude > minDistToPlayer)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            if (Time.time > nextDamageTime) {
                nextDamageTime = Time.time + damageCooldown;
                player.GetComponent<PlayerController>().takeDamage(damage);
            }
        }
    }
}
