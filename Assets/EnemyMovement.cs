using System.Collections;
using UnityEngine;

public class Enemy01Move : MonoBehaviour
{
    [Header("Enemy Weapon Field")]
    public GameObject eBul;
    public Transform eBulSpawn;

    private Rigidbody2D enemyRb;
    private float enemySpd = 5f;
    private float bulSpd = 20f;
    private float shootTimer = 1.0f;
    private int moveDir;
    private int curDir; // You can store your current direction here but I'm not using it in this code. This is how you would do it though
    private bool canChange;
    private bool canFire;
    private bool readyTofire;
    private Vector2 moveDirection;

    private void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        moveDir = Random.Range(1, 5); // 1-4  -- 1=down 2=left 3=right 4=up
        if (moveDir < 1 || moveDir > 4)
        {
            while (moveDir < 1 || moveDir > 4) { moveDir = Random.Range(1, 5); }
        }

        canChange = true; // Allows the enemy to pick a random direction when true
        canFire = false; // Enemy can only fire if this is true
        readyTofire = false; // When true, it will set canFire true. To properly set the fire rate
    }

    private void Update()
    {
        if (PlayerDetection.playerIsDetected == false)
        {
            MovementHandler();
            readyTofire = false;
            shootTimer = 1.0f;
        }
        else
        {
            StopMoving();
        }

        if (readyTofire == true)
        {
            shootTimer -= Time.deltaTime;
            // Debug.Log(shootTimer);
            if (shootTimer > 0f && canFire == true)
            {
                StartCoroutine(FireRate());
            }

            if (shootTimer < -1f)
            {
                shootTimer = 1.0f;
            }
        }
    }

    private void FixedUpdate()
    {
        enemyRb.MovePosition(enemyRb.position + (moveDirection * enemySpd) * Time.fixedDeltaTime);
    }

    void ChangeDirection()
    {
        // Can move in any direction
        if (EnemyDetection.pathOpenDown == true && EnemyDetection.pathOpenLeft == true && EnemyDetection.pathOpenRight == true &&
                                                                                                 EnemyDetection.pathOpenUp == true)
        {
            moveDir = Random.Range(1, 5); // 1-4  -- 1=down 2=left 3=right 4=up
            if (moveDir < 1 || moveDir > 4)
            {
                while (moveDir < 1 || moveDir > 4)
                {
                    moveDir = Random.Range(1, 5);
                    if (moveDir == curDir)
                    {
                        moveDir = Random.Range(1, 5);
                    }
                }// End of While loop
            }// End of IF moveDir statement
        } // End of all paths open IF statement

        // Cannot move right
        else if (EnemyDetection.pathOpenDown == true && EnemyDetection.pathOpenLeft == true && EnemyDetection.pathOpenRight == false &&
                                                                                                     EnemyDetection.pathOpenUp == true)
        {
            moveDir = Random.Range(1, 5); // 1-4  -- 1=down 2=left 3=right 4=up
            while (moveDir < 1 || moveDir > 4 || moveDir == 3)
            {
                moveDir = Random.Range(1, 5);
            }// End of While loop
        }// End of Right path closed all other paths open Statement

        // Cannot move Left or Right
        else if (EnemyDetection.pathOpenDown == true && EnemyDetection.pathOpenLeft == false && EnemyDetection.pathOpenRight == false &&
                                                                                                      EnemyDetection.pathOpenUp == true)
        {
            moveDir = Random.Range(1, 5);
            while (moveDir < 1 || moveDir > 4 || moveDir == 2 || moveDir == 3)
            {
                moveDir = Random.Range(1, 5);
            }// End of While Loop
        }// End of Up or Down path open IF Statement

        // Can only move up
        else if (EnemyDetection.pathOpenDown == false && EnemyDetection.pathOpenLeft == false && EnemyDetection.pathOpenRight == false &&
                                                                                                       EnemyDetection.pathOpenUp == true)
        {
            moveDir = 4;
        } // End of Up movement only IF statement

        // Can only move Up or Right
        else if (EnemyDetection.pathOpenDown == false && EnemyDetection.pathOpenLeft == false && EnemyDetection.pathOpenRight == true &&
                                                                                                      EnemyDetection.pathOpenUp == true)
        {
            moveDir = Random.Range(1, 5);// 1-4  -- 1=down 2=left 3=right 4=up
            while (moveDir < 3 || moveDir > 4) // can only be 3 or 4
            {
                moveDir = Random.Range(1, 5);
            }
        }// End of Up or Right Movement IF statement

        // Can move Left, Right or Up
        else if (EnemyDetection.pathOpenDown == false && EnemyDetection.pathOpenLeft == true && EnemyDetection.pathOpenRight == true &&
                                                                                                     EnemyDetection.pathOpenUp == true)
        {
            moveDir = Random.Range(1, 5);
            while (moveDir > 4 || moveDir < 2) // Cannot be 1
            {
                moveDir = Random.Range(1, 5);
            }
        }// End of Left,Right,Up IF statement

        // Can only move Left or Up
        else if (EnemyDetection.pathOpenDown == false && EnemyDetection.pathOpenLeft == true && EnemyDetection.pathOpenRight == false &&
                                                                                                      EnemyDetection.pathOpenUp == true)
        {
            moveDir = Random.Range(1, 5); // 1 = down 2 = left 3 = right 4 = up
            while (moveDir < 1 || moveDir > 4 || moveDir == 1 || moveDir == 3)
            {
                moveDir = Random.Range(1, 5);
            }
        }// End of Left/Up IF statement

        // Cannot move left
        else if (EnemyDetection.pathOpenDown == true && EnemyDetection.pathOpenLeft == false && EnemyDetection.pathOpenRight == true &&
                                                                                                     EnemyDetection.pathOpenUp == true)
        {
            moveDir = Random.Range(1, 5);
            while (moveDir < 1 || moveDir > 4 || moveDir == 2)
            {
                moveDir = Random.Range(1, 5);
            }
        }// End of Down, Right, Up IF statement

        // Can move any direction except Up
        else if (EnemyDetection.pathOpenDown == true && EnemyDetection.pathOpenLeft == true && EnemyDetection.pathOpenRight == true &&
                                                                                                   EnemyDetection.pathOpenUp == false)
        {
            moveDir = Random.Range(1, 5);
            while (moveDir < 1 || moveDir > 3)
            {
                moveDir = Random.Range(1, 5);
            }
        }// End of any direction BUT Up statement

        // Cannot move Right or Up
        else if (EnemyDetection.pathOpenDown == true && EnemyDetection.pathOpenLeft == true && EnemyDetection.pathOpenRight == false &&
                                                                                                   EnemyDetection.pathOpenUp == false)
        {
            moveDir = Random.Range(1, 5); // 1 = down 2 = left 3 = right 4 = up
            while (moveDir < 1 || moveDir > 2)
            {
                moveDir = Random.Range(1, 5);
            }
        }// End of can't move Right or Up statement

        // Cannot move Left or Up
        else if (EnemyDetection.pathOpenDown == true && EnemyDetection.pathOpenLeft == false && EnemyDetection.pathOpenRight == true &&
                                                                                                   EnemyDetection.pathOpenUp == false)
        {
            moveDir = Random.Range(1, 5);
            while (moveDir < 1 || moveDir > 3)
            {
                moveDir = Random.Range(1, 5);
            }
        }// End of Can't move Left or Up statement

        // Can only move Down
        else if (EnemyDetection.pathOpenDown == true && EnemyDetection.pathOpenLeft == false && EnemyDetection.pathOpenRight == false &&
                                                                                                   EnemyDetection.pathOpenUp == false)
        {
            moveDir = 1;
        } // End of can only move Down statement

        // Can only move Left
        else if (EnemyDetection.pathOpenDown == false && EnemyDetection.pathOpenLeft == true && EnemyDetection.pathOpenRight == false &&
                                                                                                   EnemyDetection.pathOpenUp == false)
        {
            moveDir = 2;
        } // End of can only move Left statement

        // Can only move Right
        else if (EnemyDetection.pathOpenDown == false && EnemyDetection.pathOpenLeft == false && EnemyDetection.pathOpenRight == true &&
                                                                                                   EnemyDetection.pathOpenUp == false)
        {
            moveDir = 3;
        } // End of can only move Right

        else { moveDir = 4; } // In case we forgot a scenario or since we didn't add move up only, we'll move Up by default
    }

    void MoveLeft()
    {
        curDir = 2;
        enemyRb.transform.GetChild(0).GetChild(0).eulerAngles = new Vector3(0, 0, -90);
        moveDirection = Vector2.left;
    }

    void MoveRight()
    {
        curDir = 3;
        enemyRb.transform.GetChild(0).GetChild(0).eulerAngles = new Vector3(0, 0, 90);
        moveDirection = Vector2.right;
    }

    void MoveUp()
    {
        curDir = 4;
        enemyRb.transform.GetChild(0).GetChild(0).eulerAngles = new Vector3(0, 0, 180);
        moveDirection = Vector2.up;
    }

    void MoveDown()
    {
        curDir = 1;
        enemyRb.transform.GetChild(0).GetChild(0).eulerAngles = new Vector3(0, 0, 0);
        moveDirection = Vector2.down;
    }

    void StopMoving()
    {
        moveDirection = Vector2.zero;
        if (shootTimer >= 1.0f)
        {
            readyTofire = true;
            canFire = true;
        }
    }

    void StartShooting()
    {
        GameObject eBull = Instantiate(eBul, eBulSpawn.position, eBulSpawn.rotation);
        Rigidbody2D eBulRb = eBull.GetComponent<Rigidbody2D>();
        eBulRb.AddForce(eBulSpawn.up * bulSpd, ForceMode2D.Impulse);
        Destroy(eBull, 3f);
    }

    void MovementHandler()
    {
        if (moveDir == 1)
        {
            if (EnemyDetection.pathOpenDown == true)
            {
                MoveDown();
            }
            else { ChangeDirection(); }
        }
        else if (moveDir == 2)
        {
            if (EnemyDetection.pathOpenLeft == true)
            {
                MoveLeft();
            }
            else { ChangeDirection(); }
        }
        else if (moveDir == 3)
        {
            if (EnemyDetection.pathOpenRight == true)
            {
                MoveRight();
            }
            else { ChangeDirection(); }
        }
        else if (moveDir == 4)
        {
            if (EnemyDetection.pathOpenUp == true)
            {
                MoveUp();
            }
            else { ChangeDirection(); }
        }

        if (canChange == true)
        {
            StartCoroutine(RandomMovement());
        }
    }

    IEnumerator RandomMovement()
    {
        canChange = false;
        var timer = Random.Range(0.5f, 2.5f);
        yield return new WaitForSeconds(timer);
        ChangeDirection();
        yield return new WaitForSeconds(0.5f);
        canChange = true;
    }

    IEnumerator FireRate()
    {
        canFire = false;
        float fireRate = 0.25f;
        yield return new WaitForSeconds(fireRate);
        StartShooting();
        yield return new WaitForSeconds(fireRate);
        canFire = true;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            ChangeDirection();
        }
    }
}

