using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * WILLY INDRAYANA KOMARA
 * SEBELAS MARET
 */
public class BallControl : MonoBehaviour
{
    // Rigidbody 2D bola
    private Rigidbody2D rigidBody2D;

    // Besarnya gaya awal yang diberikan untuk mendorong bola
    public float xInitialForce;
    public float yInitialForce;
    public GameObject thrownBy;
    //menentukan gaya awal yang diberikan pada bola
    public float gaya = 100;
    // Titik asal lintasan bola saat ini
    private Vector2 trajectoryOrigin;
    // Untuk mengakses informasi titik asal lintasan
    public GameManager gameManager;

    public AudioSource sound;
    public AudioClip clip;

    bool canPush = true;
    public Vector2 TrajectoryOrigin
    {
        get { return trajectoryOrigin; }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        trajectoryOrigin = transform.position;
        // Mulai game
        RestartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void OnCollisionExit2D(Collision2D collision)
    {
        trajectoryOrigin = transform.position;
        sound.PlayOneShot(clip);
        if (!(collision.gameObject == GameObject.Find("TopWall") || collision.gameObject == GameObject.Find("BottomWall")))
            thrownBy = collision.gameObject;
    }

    void ResetBall()
    {
        // Reset posisi menjadi (0,0)
        transform.position = Vector2.zero;

        // Reset kecepatan menjadi (0,0)
        rigidBody2D.velocity = Vector2.zero;

        gameManager.waktu = 0;
        gameManager.waktuPowerUp = 0;
    }

    void PushBall()
    {
        

        // Tentukan nilai komponen y dari gaya dorong antara -yInitialForce dan yInitialForce
        float y = Random.Range(-yInitialForce, yInitialForce);
        
        //komponen x gaya dorong adalah hasil perhitungan vektor dari kuadrat besar gaya yang ingin diberikan dikurangi kuadrat besar gaya komponen y
        float x = Mathf.Sqrt(gaya*gaya - y*y);

        // Tentukan nilai acak antara 0 (inklusif) dan 2 (eksklusif)
        float randomDirection = Random.Range(0, 2);

        // Jika nilainya di bawah 1, bola bergerak ke kiri. 
        // Jika tidak, bola bergerak ke kanan.
        if (randomDirection < 1.0f)
        {
            // Gunakan gaya untuk menggerakkan bola ini.
            rigidBody2D.AddForce(new Vector2(-x, y));
        }
        else
        {
            rigidBody2D.AddForce(new Vector2(x, y));
        }
    }

    /*
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Player"))
        {
            Vector2 vel;
            vel.x = rigidBody2D.velocity.x;
            vel.y = (rigidBody2D.velocity.y / 2) + (coll.collider.attachedRigidbody.velocity.y / 3);
            rigidBody2D.velocity = vel;
        }
    }
    */
    

    void RestartGame()
    {
        // Kembalikan bola ke posisi semula
        ResetBall();
        gameManager.winPlayed = false;
        if (gameManager.waktu < 1e9)
        {
            Time.timeScale = 1;
            //gameManager.waktu = 0;
            // Setelah 2 detik, berikan gaya ke bola
            Invoke("PushBall", 2);
        }
        
    }
}
