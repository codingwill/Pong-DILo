using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * WILLY INDRAYANA KOMARA
 * SEBELAS MARET
 */
public class GameManager : MonoBehaviour
{
    
    // Pemain 1
    public PlayerControl player1; // skrip
    private Rigidbody2D player1Rigidbody;

    // Pemain 2
    public PlayerControl player2; // skrip
    private Rigidbody2D player2Rigidbody;

    // Bola
    public BallControl ball; // skrip
    private Rigidbody2D ballRigidbody;
    private CircleCollider2D ballCollider;

    // Skor maksimal
    public int maxScore;

    // Apakah debug window ditampilkan?
    private bool isDebugWindowShown = false;

    // Objek untuk menggambar prediksi lintasan bola
    public Trajectory trajectory;

    Vector3 pos;
    Camera cam;
    public GameObject powerUp;
    public float waktu = 0;
    public float waktuPowerUp = 0;
    bool powerUpSpawned = false;
    
    public AudioSource sound;
    public AudioClip clip;
    public bool winPlayed = false;

    bool gameOver = false;

    public GameObject score1;
    public GameObject score2
        ;
    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreen = false;
        Screen.SetResolution(1280, 720, false);
        player1Rigidbody = player1.GetComponent<Rigidbody2D>();
        player2Rigidbody = player2.GetComponent<Rigidbody2D>();
        ballRigidbody = ball.GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        cam = Camera.main;
        waktu += Time.deltaTime;
        waktuPowerUp += Time.deltaTime;
        //Debug.Log(waktu);
        spawnPowerUp((int)waktu);

        if (player1.Score == maxScore || player2.Score == maxScore)
        {
            gameOver = true;
            if (!winPlayed)
            {
                sound.PlayOneShot(clip);
                winPlayed = true;
            }
        }
    }

    private void spawnPowerUp(int waktu)
    {
        if (waktuPowerUp % 20 >= 10)
        {
            if (!powerUpSpawned)
            {
                Debug.Log("Tes");
                powerUp.SetActive(true);
                float x, y;
                x = Random.Range(Screen.width - (Screen.width * 0.7f), Screen.width * 0.7f);
                y = Random.Range(Screen.width - (Screen.width * 0.7f), Screen.height * 0.7f);
                pos = cam.ScreenToWorldPoint(new Vector3(x, y, cam.nearClipPlane));
                powerUp.transform.position = pos;
                powerUpSpawned = true;
            }

        }
        else
        {
            powerUpSpawned = false;
            powerUp.SetActive(false);
        }
    }

    // Untuk menampilkan GUI
    void OnGUI()
    {
        trajectory.enabled = !trajectory.enabled;
        GUIStyle guistyle = new GUIStyle();
        guistyle.fontSize = 5;
        // Tampilkan skor pemain 1 di kiri atas dan pemain 2 di kanan atas
        //GUI.Label(new Rect(Screen.width / 2 - 150 - 12, 20, 100, 100), "" + player1.Score);
        //GUI.Label(new Rect(Screen.width / 2 + 150 + 12, 20, 100, 100), "" + player2.Score);

        // Tombol restart untuk memulai game dari awal
        if (GUI.Button(new Rect(Screen.width / 2 - 60, 35, 120, 53), "RESTART"))
        {
            if (waktu > 2 || gameOver)
            {
                // Ketika tombol restart ditekan, reset skor kedua pemain...
                player1.ResetScore();
                player2.ResetScore();
                GameObject.Find("FireBall").GetComponent<Trap>().ResetBall();
                // ...dan restart game.
                ball.SendMessage("RestartGame", 0.5f, SendMessageOptions.RequireReceiver);
                gameOver = false;
                score1.GetComponent<Text>().text = "" + player1.Score;
                score2.GetComponent<Text>().text = "" + player2.Score;
            }
            
        }

        // Jika pemain 1 menang (mencapai skor maksimal), ...
        if (player1.Score == maxScore)
        {
            
            // ...tampilkan teks "PLAYER ONE WINS" di bagian kiri layar...
            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 10, 2000, 1000), "PLAYER ONE WINS");
            waktu = 0;
            //Time.timeScale = 0;
            // ...dan kembalikan bola ke tengah.
            ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }
        // Sebaliknya, jika pemain 2 menang (mencapai skor maksimal), ...
        else if (player2.Score == maxScore)
        {
            
            // ...tampilkan teks "PLAYER TWO WINS" di bagian kanan layar... 
            GUI.Label(new Rect(Screen.width / 2 + 30, Screen.height / 2 - 10, 2000, 1000), "PLAYER TWO WINS");
            waktu = 0;
            //Time.timeScale = 0;
            // ...dan kembalikan bola ke tengah.
            ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }

        // Jika isDebugWindowShown == true, tampilkan text area untuk debug window.
        if (isDebugWindowShown)
        {
            Color oldColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;

            // Simpan variabel-variabel fisika yang akan ditampilkan. 
            float ballMass = ballRigidbody.mass;
            Vector2 ballVelocity = ballRigidbody.velocity;
            float ballSpeed = ballRigidbody.velocity.magnitude;
            Vector2 ballMomentum = ballMass * ballVelocity;
            float ballFriction = ballCollider.friction;

            float impulsePlayer1X = player1.LastContactPoint.normalImpulse;
            float impulsePlayer1Y = player1.LastContactPoint.tangentImpulse;
            float impulsePlayer2X = player2.LastContactPoint.normalImpulse;
            float impulsePlayer2Y = player2.LastContactPoint.tangentImpulse;

            // Tentukan debug text-nya
            string debugText =
                "Ball mass = " + ballMass + "\n" +
                "Ball velocity = " + ballVelocity + "\n" +
                "Ball speed = " + ballSpeed + "\n" +
                "Ball momentum = " + ballMomentum + "\n" +
                "Ball friction = " + ballFriction + "\n" +
                "Last impulse from player 1 = (" + impulsePlayer1X + ", " + impulsePlayer1Y + ")\n" +
                "Last impulse from player 2 = (" + impulsePlayer2X + ", " + impulsePlayer2Y + ")\n";

            // Tampilkan debug window
            GUIStyle guiStyle = new GUIStyle(GUI.skin.textArea);
            guiStyle.alignment = TextAnchor.UpperCenter;
            GUI.TextArea(new Rect(Screen.width / 2 - 200, Screen.height - 200, 400, 110), debugText, guiStyle);

            // Kembalikan warna lama GUI
            GUI.backgroundColor = oldColor;
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height - 73, 120, 53), "TOGGLE\nDEBUG INFO"))
        {
            isDebugWindowShown = !isDebugWindowShown;
        }
    }
}
