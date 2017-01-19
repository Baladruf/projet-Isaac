using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public int hp;
    public int vitesse;
    public int portée;
    public int cadence;
    public float vitesseTir;
    public int degat;
    private string tirTag;

    public GameObject tir;

    private float timerD, timerDRef;

    private Rigidbody2D rigi;
    private Vector2 deplac, posActu;
    private int PX, PY;
    private float posX, posY;

    private float timer;
    private bool time;
    private Animator animator;
    private string blink;
    public GameObject child;
    private bool isDeplaceF;
    private bool isDeplaceR;

    //lier au HUD
    public Text[] listeText;
    private const string c_hp = "hp:";
    private const string c_vitesse = "vitesse:";
    private const string c_porte = "portée:";
    private const string c_cadence = "cadence:";
    private const string c_vitesseTir = "vitesse tir:";
    private const string c_degat = "degat:";

    //lier au game over
    private float timerInvin;
    public int tempsInvin;

    // Use this for initialization
    void Start () {
        tirTag = "tirSimple";
        afficheCadence();
        afficheDegat();
        afficheHp();
        affichePortee();
        afficheVitesse();
        afficheVitesseTir();
        posActu = new Vector2();
        animator = gameObject.GetComponent<Animator>();
        rigi = gameObject.GetComponent<Rigidbody2D>();
        deplac = new Vector2(0, 0);
        timer = 0;
        time = true;
        PX = 1;
        PY = 0;
        posX = 0.5f;
        posY = 0;
        isDeplaceF = false;
        isDeplaceR = false;
}
	
	// Update is called once per frame
	void Update () {

        if (timerInvin < tempsInvin)
        {
            timerInvin += Time.deltaTime;
        }

        
        //Debug.Log("apres" + deplac);
        

        if (Input.GetKey(KeyCode.Space) && time)
        {
            GameObject v_tir;
            switch (tirTag)
            {
                case "tirSimple":
                    v_tir = Instantiate(tir, new Vector3(transform.position.x + PX, transform.position.y + PY, 0), Quaternion.identity);
                    v_tir.GetComponent<Tir>().setAttribut(portée, vitesseTir, degat, PX, PY);
                    break;
                case "tirDiag":
                    tirDiag();
                    v_tir = Instantiate(tir, new Vector3(transform.position.x + PX, transform.position.y + PY, 0), Quaternion.identity);
                    v_tir.GetComponent<Tir>().setAttribut(portée, vitesseTir, degat, PX, PY);
                    break;
                case "tirCible":
                    v_tir = Instantiate(tir, new Vector3(transform.position.x + PX, transform.position.y + PY, 0), Quaternion.identity);
                    v_tir.GetComponent<Tir>().setAttributTirCibler(portée, vitesseTir / 200, degat, 3);
                    break;
                case "tirSize":
                    v_tir = Instantiate(tir, new Vector3(transform.position.x + PX, transform.position.y + PY, 0), Quaternion.identity);
                    v_tir.GetComponent<Tir>().setDeplacementSize(portée, vitesseTir, degat, PX, PY, 0.3f, 2);
                    break;
            }
            /*tirDiag();
            GameObject v_tir = Instantiate(tir, new Vector3(transform.position.x + PX, transform.position.y + PY, 0), Quaternion.identity);
            v_tir.GetComponent<Tir>().setAttribut(portée, vitesseTir, degat, PX, PY);*/

            /*
            GameObject v_tir = Instantiate(tir, new Vector3(transform.position.x + PX, transform.position.y + PY, 0), Quaternion.identity);
            v_tir.GetComponent<Tir>().setAttributTirCibler(portée, vitesseTir / 200, degat, 3);
            */

            


            animator.SetTrigger(blink);

          //tir.SetActive(true);
            time = false;
        }

        if (!time)
        {
            timer += Time.deltaTime;
            if(timer >= cadence)
            {
                timer = 0;
                time = true;
            }
        }

        //rigi.velocity -= rigi.velocity;
        
    }

    void FixedUpdate()
    {
        deplacement();
        deplacementTete();
    }


    private void deplacement()
    {

        bool isdeplace = false;
        deplac.Set(transform.position.x, transform.position.y);

        //Debug.Log("avant" + deplac);

        if (Input.GetKey(KeyCode.UpArrow))
        {


            deplac.Set(deplac.x, deplac.y + (vitesse * Time.deltaTime));
            //animator.SetTrigger("move back");
            if(!isDeplaceF)
                child.GetComponent<Animator>().SetTrigger("f");
            if(isDeplaceF)
                child.GetComponent<Animator>().SetTrigger("m_avant");
            isDeplaceF = true;
            isDeplaceR = false;
            isdeplace = true;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {

            deplac.Set(deplac.x, deplac.y + (-1 * vitesse * Time.deltaTime));
            //animator.SetTrigger("move forward");
            if (!isDeplaceF)
                child.GetComponent<Animator>().SetTrigger("f");
            if(isDeplaceF)
                child.GetComponent<Animator>().SetTrigger("m_avant");
            isDeplaceF = true;
            isDeplaceR = false;
            isdeplace = true;
        }


        if (Input.GetKey(KeyCode.RightArrow))
        {

            deplac.Set(deplac.x + (vitesse * Time.deltaTime), deplac.y);
            child.GetComponent<SpriteRenderer>().flipX = false;
            //animator.SetTrigger("move right");
            if(!isDeplaceR)
                child.GetComponent<Animator>().SetTrigger("r");
            if(isDeplaceR)
                child.GetComponent<Animator>().SetTrigger("m_droite");
            isDeplaceF = false;
            isDeplaceR = true;
            isdeplace = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {

            deplac.Set(deplac.x + (-1 * vitesse * Time.deltaTime), deplac.y);
            child.GetComponent<SpriteRenderer>().flipX = true;
            //animator.SetTrigger("move left");
            if(!isDeplaceR)
                child.GetComponent<Animator>().SetTrigger("r");
            if(isDeplaceR)
                child.GetComponent<Animator>().SetTrigger("m_droite");
            isDeplaceF = false;
            isDeplaceR = true;
            isdeplace = true;
        }

        if (!isdeplace && isDeplaceF)
        {
            child.GetComponent<Animator>().SetTrigger("f");
        }else if (!isdeplace && isDeplaceR)
        {
            child.GetComponent<Animator>().SetTrigger("r");
        }

        rigi.MovePosition(deplac);
    }

    private void deplacementTete()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            PY = 1;
            PX = 0;
            animator.SetTrigger("move back");
            blink = "blink";
        }else if (Input.GetKey(KeyCode.S))
        {
            PY = -1;
            PX = 0;
            animator.SetTrigger("move forward");
            blink = "blink back";
        }
        if (Input.GetKey(KeyCode.D))
        {
            PY = 0;
            PX = 1;
            animator.SetTrigger("move right");
            blink = "blink right";
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            PY = 0;
            PX = -1;
            animator.SetTrigger("move left");
            blink = "blink left";
        }
    }

    private void tirDiag()
    {
        switch (PY)
        {
            case 1:
                tirDiagSpawn(Mathf.PI / 4);
                tirDiagSpawn(3 * Mathf.PI / 4);
                break;
            case -1:
                tirDiagSpawn(5 * Mathf.PI / 4);
                tirDiagSpawn(7 * Mathf.PI / 4);
                break;
            case 0:
                switch (PX)
                {
                    case 1:
                        tirDiagSpawn(Mathf.PI / 4);
                        tirDiagSpawn(-1 * Mathf.PI / 4);
                        break;
                    case -1:
                        tirDiagSpawn(5 * Mathf.PI / 4);
                        tirDiagSpawn(3 * Mathf.PI / 4);
                        break;
                }
                break;
        }
    }

    private void tirDiagSpawn(float p_angle)
    {
        GameObject v_tir = Instantiate(tir, new Vector3(transform.position.x + Mathf.Cos(p_angle), transform.position.y + Mathf.Sin(p_angle), 0), Quaternion.identity);
        v_tir.GetComponent<Tir>().setAttribut(portée, vitesseTir, degat, Mathf.Cos(p_angle), Mathf.Sin(p_angle));
    }

    private void afficheHp()
    {
        listeText[0].text = c_hp + hp;
    }
    private void afficheVitesse()
    {
        listeText[1].text = c_vitesse + vitesse;
    }
    private void affichePortee()
    {
        listeText[2].text = c_porte + portée;
    }
    private void afficheCadence()
    {
        listeText[3].text = c_cadence + cadence;
    }
    private void afficheVitesseTir()
    {
        listeText[4].text = c_vitesseTir +  (int)vitesseTir;
    }
    private void afficheDegat()
    {
        listeText[5].text = c_degat + degat;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "hpP":
                hp++;
                afficheHp();
                Destroy(other.gameObject);
                break;
            case "vitesseP":
                vitesse++;
                afficheVitesse();
                Destroy(other.gameObject);
                break;
            case "porteeP":
                portée++;
                affichePortee();
                Destroy(other.gameObject);
                break;
            case "cadenceP":
                cadence++;
                afficheCadence();
                Destroy(other.gameObject);
                break;
            case "vitesseTirP":
                vitesseTir++;
                afficheVitesseTir();
                Destroy(other.gameObject);
                break;
            case "degatP":
                degat++;
                afficheDegat();
                Destroy(other.gameObject);
                break;
            case "tirDiag":
                tirTag = "tirDiag";
                Destroy(other.gameObject);
                break;
            case "tirCible":
                tirTag = "tirCible";
                Destroy(other.gameObject);
                break;
            case "tirSize":
                tirTag = "tirSize";
                Destroy(other.gameObject);
                break;
            case "ennemi":
                gameOver();
                break;
            case "tirEnnemi":
                gameOver();
                break;
        }
    }

    private void gameOver()
    {
        if(timerInvin >= tempsInvin && hp > 0)
        {
            hp--;
            timerInvin = 0;
        }else if(hp == 0)
        {
            child.SetActive(false);
            timerD = 0;
            timerDRef = 1000;
            animator.SetTrigger("death");
            
            Destroy(gameObject);
            Application.LoadLevel("MENU");
            //code sortie game
        }
    }

}