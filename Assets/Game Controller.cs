using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool gameOver = false;

    public GameObject deathScreen;

    // Start is called before the first frame update
    void Start()
    {
        deathScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnDeath(int finalPoints)
    {
        deathScreen.SetActive(true);
        gameOver = true;
    }

    public void respawn()
    {
        deathScreen.SetActive(false);
        gameOver = false;

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().health = 10f;
    }
}
