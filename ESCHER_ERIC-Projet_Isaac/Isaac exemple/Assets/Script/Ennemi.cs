using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemi : MonoBehaviour {

    public int hp, typeEnnemi;
    public float vitesse;
    private float timer, timerRef;
    private Rigidbody2D rigi;
    private Vector2 pos;

    //deplacement cibler
    public Transform Player;
    private Vector2 moveCalcul, move, vectRef;
    private float distance;


    private void Awake()
    {
        rigi = gameObject.GetComponent<Rigidbody2D>();
        pos = new Vector2();
        moveCalcul = new Vector2();
        vectRef = new Vector2(3, 3);
        move = new Vector2();
    }

    // Use this for initialization
    void Start () {
        timer = 2;
        timerRef = timer;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        switch (typeEnnemi)
        {
            case 1:
                deplacementAlea();
                break;
            case 2:
                deplacementCible();
                break;
        }
    }

    private void deplacementAlea()
    {
        float speed = vitesse * Time.deltaTime;
        if(timer < timerRef)
        {
            timer += Time.deltaTime;
        }else
        {
            float angle = Random.Range(0f, 2 * Mathf.PI);
            pos.Set(Mathf.Cos(angle) * speed /*+ transform.position.x*/, Mathf.Sin(angle) * speed /*+ transform.position.y*/);
            timer = 0;
        }
        rigi.velocity = pos;
    }


    private void deplacementCible()
    {
        if(Player != null)
        {
            moveCalcul.Set(Player.position.x - transform.position.x, Player.position.y - transform.position.y);
            distance = Vector2.Distance(Player.position, transform.position) / Vector2.Distance(vectRef, Vector2.zero);

            move.Set(transform.position.x + ((moveCalcul.x / distance) * (vitesse / 200) * Time.deltaTime), transform.position.y + ((moveCalcul.y / distance) * (vitesse / 200) * Time.deltaTime));
            rigi.MovePosition(move);
        }
    }

}
