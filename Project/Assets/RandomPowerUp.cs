using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * WILLY INDRAYANA KOMARA
 * SEBELAS MARET
 */
public class RandomPowerUp : MonoBehaviour
{
    public GameObject ball;
    private BallControl ballControl;
    public GameObject player1;
    public GameObject player2;
    public GameObject manager;

    public GameObject boostText1;
    public GameObject boostText2;

    public AudioSource sound;
    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        ballControl = ball.GetComponent<BallControl>();
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name != "FireBall")
        {
            if (ballControl.thrownBy == player1)
            {
                sound.PlayOneShot(clip);
                Debug.Log("AHAHAH");
                player1.transform.localScale = new Vector3(2, 3, 1);
                Invoke("backToOriSize1", 10);
                boostText1.SetActive(true);
            }
            else if (ballControl.thrownBy == player2)
            {
                sound.PlayOneShot(clip);
                player2.transform.localScale = new Vector3(2, 3, 1);
                Invoke("backToOriSize2", 10);
                boostText2.SetActive(true);
            }
            manager.GetComponent<GameManager>().waktuPowerUp = 0;
        }

    }

    private void backToOriSize1()
    {
        player1.transform.localScale = new Vector3(1f, 1f, 1f);
        boostText1.SetActive(false);
    }

    private void backToOriSize2()
    {
        player2.transform.localScale = new Vector3(1f, 1f, 1f);
        boostText2.SetActive(false);
    }
}
