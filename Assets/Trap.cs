using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * WILLY INDRAYANA KOMARA
 * SEBELAS MARET
 */

public class Trap : MonoBehaviour
{
    // Rigidbody 2D bola
    private Rigidbody2D rigidBody2D;
    int clock;
    float y, x, randomDirection;
    // Besarnya gaya awal yang diberikan untuk mendorong bola
    public float xInitialForce;
    public float yInitialForce;
    public float yBoundary = 9.0f;
    public float xBoundary = 17.3f;
    public float gaya = 20;
    bool moved = false;
    bool kiri = false;
    public float speed = 50;
    bool active = false;
    public GameObject score1;
    public GameObject score2;
    public BallControl ballControl;
    [SerializeField]
    private GameManager gameManager;

    public PlayerControl player1;
    public PlayerControl player2;

    public AudioSource sound;
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.waktu >= 5) active = true;
        Debug.Log(gameManager.waktu);
        clock = (int)(Time.time * 10);
        if (transform.position.x > xBoundary)
        {
            kiri = true;
        }
        else if (transform.position.x < -xBoundary)
        {
            kiri = false;
        }
        //Debug.Log("ZXC");
        if (active) PushBall();
        if (clock % 20 >= 10)
        {
            if (!moved)
            {
                moved = true;
            }

        }
        else
        {
            if (moved)
            {
                moved = false;
            }
        }


    }

    public void ResetBall()
    {
        active = false;
        GetComponent<SpriteRenderer>().enabled = false;
        // Reset posisi menjadi (0,0)
        transform.position = new Vector2(0, 11.17f);

        // Reset kecepatan menjadi (0,0)
        rigidBody2D.velocity = Vector2.zero;
        
        //Invoke("PushBall", 1);
    }

    void setActive()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        active = true;
    }

    void PushBall()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        if (clock % 20 >= 10 && !moved || clock % 20 < 10 && moved)
        {
            if (transform.position.y > yBoundary * 0.7f)
            {
                // Tentukan nilai komponen y dari gaya dorong antara -yInitialForce dan yInitialForce
                y = Random.Range(-yInitialForce, yInitialForce * 0.3f) * (speed * 0.001f);
            }
            else if (transform.position.y < -yBoundary * 0.7f)
            {
                y = Random.Range(-yInitialForce * 0.3f, yInitialForce) * (speed * 0.001f);
            }
            else
            {
                y = Random.Range(-yInitialForce, yInitialForce) * (speed * 0.001f);
            }
            

            //komponen x gaya dorong adalah hasil perhitungan vektor dari kuadrat besar gaya yang ingin diberikan dikurangi kuadrat besar gaya komponen y
            x = Mathf.Sqrt(gaya * gaya - y * y) * (speed * 0.001f);

            // Tentukan nilai acak antara 0 (inklusif) dan 2 (eksklusif)
            randomDirection = Random.Range(0, 2);
        }
        

        // Jika nilainya di bawah 1, bola bergerak ke kiri. 
        // Jika tidak, bola bergerak ke kanan.
        if (kiri)
        {
            // Gunakan gaya untuk menggerakkan bola ini.
            transform.position = new Vector2(transform.position.x -x, transform.position.y + y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x + x, transform.position.y + y);
        }

        
    }
    void OnTriggerEnter2D(Collider2D anotherCollider)
    {
        Debug.Log(anotherCollider.name);
        if (gameManager.waktu > 2)
        {
            // Jika objek tersebut bernama "Ball":
            if (anotherCollider.name == "Player1")
            {
                // Tambahkan skor ke pemain
                player2.IncrementScore();
                score2.GetComponent<Text>().text = "" + player2.Score;
                // Jika skor pemain belum mencapai skor maksimal...
                if (player2.Score < gameManager.maxScore)
                {
                    // ...restart game setelah bola mengenai dinding.
                    //Time.timeScale = 0;
                    sound.PlayOneShot(clip);
                    ResetBall();
                    ballControl.gameObject.SendMessage("RestartGame", 2.0f, SendMessageOptions.RequireReceiver);
                }
            }

            if (anotherCollider.name == "Player2")
            {
                // Tambahkan skor ke pemain
                player1.IncrementScore();
                score1.GetComponent<Text>().text = "" + player1.Score;
                // Jika skor pemain belum mencapai skor maksimal...
                if (player1.Score < gameManager.maxScore)
                {
                    // ...restart game setelah bola mengenai dinding.
                    //Time.timeScale = 0;
                    sound.PlayOneShot(clip);
                    ResetBall();
                    ballControl.gameObject.SendMessage("RestartGame", 2.0f, SendMessageOptions.RequireReceiver);
                }
            }
        }
    }
}
