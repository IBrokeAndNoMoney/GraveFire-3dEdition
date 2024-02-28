using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Transform cameraT;
    Rigidbody rb;

    public GameObject bazookaBullet;
    public GameObject Bullets;

    public GameController gameController;

    public float sensitivityX = 250f;
    public float sensitivityY = 250f;

    public float walkspeed = 6f;
    public float jumpPower = 300f;

    float verticalCamRotation = 0f;
    Vector3 moveAmount;
    Vector3 smoothVelocity;

    bool grounded = false;

    public float health = 10f;
    public int points = 0;

    public GameObject ouchParticles;
    float timeSinceLastDamaged;
    float ouchParticlesDisplaytime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        cameraT = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.gameOver)
        {
            transform.Rotate(Vector3.up * sensitivityX * Input.GetAxisRaw("Mouse X") * Time.deltaTime);

            verticalCamRotation += Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;
            verticalCamRotation = Mathf.Clamp(verticalCamRotation, -80, 80);
            cameraT.localEulerAngles = Vector3.left * verticalCamRotation;

            Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            Vector3 targetMove = moveDir * walkspeed;
            moveAmount = Vector3.SmoothDamp(moveAmount, targetMove, ref smoothVelocity, .15f);

            if (Input.GetButtonDown("Jump"))
            {
                if (grounded)
                {
                    rb.AddForce(Vector3.up * jumpPower);
                }
            }

            Ray jumpRay = new Ray(transform.position, Vector3.down);

            grounded = Physics.Raycast(jumpRay, .6f);

            if (Input.GetMouseButtonDown(0))
            {
                GameObject bullet = Instantiate(bazookaBullet, transform.position, Quaternion.LookRotation(cameraT.up, cameraT.forward), Bullets.transform);
                bullet.AddComponent<Rigidbody>();
                bullet.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
                bullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 50f, ForceMode.VelocityChange);
            }

            if (Time.time > timeSinceLastDamaged + ouchParticlesDisplaytime)
            {
                ouchParticles.SetActive(false);
            }

            if (transform.position.y < -5)
            {
                transform.position = new Vector3(0, 1, 0);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.deltaTime);
    }
    public void takeDamage(float damageAmount)
    {
        timeSinceLastDamaged = Time.time;
        ouchParticles.SetActive(true);

        health -= damageAmount;
        if (health == 0f)
        {
            gameController.OnDeath(points);
        }
    }
}
