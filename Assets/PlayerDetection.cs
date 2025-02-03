using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public Transform origin, end, player;
    public float radarSpeed;
    public bool playerDetected;


    public static bool playerIsDetected;

    private int playerLayer = 1 << 8;
    private Rigidbody2D enemyRb;
    private Vector3 facePlayer;

    private void Start()
    {
        enemyRb = GetComponentInParent<Rigidbody2D>();
        playerIsDetected = false;
    }
    private void Update()
    {
        PlayerDetector();
        if (playerDetected == false)
        {
            Radar();
        }
        else { PlayerIsDetected(); }

    }

    void PlayerDetector()
    {
        Debug.DrawLine(origin.position, end.position, Color.red);
        playerDetected = Physics2D.Linecast(origin.position, end.position, playerLayer);
    }

    void Radar()
    {
        end.RotateAround(origin.position, Vector3.forward, radarSpeed * Time.deltaTime);
        playerIsDetected = false;
    }

    void PlayersPosition()
    {
        facePlayer = player.position - enemyRb.transform.GetChild(0).GetChild(0).position;
        enemyRb.transform.GetChild(0).GetChild(0).up = -facePlayer;
    }

    void PlayerIsDetected()
    {
        if (playerDetected == true)
        {
            playerIsDetected = true;
            end.position = player.position;
            PlayersPosition();
        }
    }
}

