using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tir : MonoBehaviour {

    private int portée, degat;
    private Vector2 initPos, deplac;
    private Rigidbody2D rigi;
    private float PX, PY, vTir;
    private bool init;
    private Transform trans;

    //lier a tir cibler
    public Transform listeEnnemi;
    private bool isTirCible, initCible;
    private Vector2 vectForward, vectRef, deplacCible;
    private float dis;
    private GameObject ob;
    private float timerCible, timerCibleRef;

    //lier a tir size
    private float tailleMin, tailleMax;
    private float timerSize, timerSizeRef;
    private Vector3 sizeV;
    private bool isSize;


    // Use this for initialization
    void Awake () {
        initPos = new Vector2(transform.position.x, transform.position.y);
        trans = gameObject.transform;
        rigi = gameObject.GetComponent<Rigidbody2D>();
        init = false;
        isTirCible = false;
        initCible = true;
        sizeV = new Vector3();
        isSize = false;
	}


    void FixedUpdate()
    {
        if (isTirCible && initCible)
        {
            vectForward = new Vector2();
            vectRef = new Vector2(3, 3);
            deplacCible = new Vector2();
            timerCible = 0;
            initCible = false;
        }

        if (init) {
            if (!isTirCible && (trans.position.x >= initPos.x + portée || trans.position.y >= initPos.y + portée || trans.position.x <= initPos.x - portée || trans.position.y <= initPos.y - portée))
            {
                Debug.Log("destroy 1");
                Destroy(gameObject);

            }else if (isTirCible)
            {
                deplacementCibler();
                if (timerCible < timerCibleRef)
                    timerCible += Time.deltaTime;
                else
                {
                    Debug.Log("destroy 2");
                    Destroy(gameObject);

                }
            }
            if (isSize)
            {
                deplacementSize();
            }
            /*else
            {
                rigi.velocity = deplac;

                Debug.Log(deplac);
            }*/
        }
    }

    public void setAttribut(int p_porte, float p_vtir, int p_degat, float p_x, float p_y)
    {
        portée = p_porte;
        vTir = p_vtir;
        degat = p_degat;
        PX = p_x;
        PY = p_y;
        deplac = new Vector2(PX * vTir * Time.deltaTime, PY * vTir * Time.deltaTime);
        init = true;
        rigi.velocity = deplac;
        
        //Debug.Log(deplac);
    }

    public void setAttributTirCibler(int p_porte, float p_vtir, int p_degat, int p_time)
    {
        portée = p_porte;
        vTir = p_vtir;
        degat = p_degat;
        timerCibleRef = p_time;
        isTirCible = true;
        init = true;
    }

    private void deplacementCibler()
    {
        if(listeEnnemi.childCount > 0)
        {
            vectForward.Set(-transform.position.x + listeEnnemi.GetChild(0).transform.position.x, -transform.position.y + listeEnnemi.GetChild(0).transform.position.y);
            //dis = vectForward.sqrMagnitude / vectRef.sqrMagnitude;
            dis =Vector2.Distance(transform.position, listeEnnemi.GetChild(0).transform.position) /Vector2.Distance(Vector2.zero, vectRef);


            Debug.Log("dis = " + dis);
            Debug.Log("vctuer = "+ (vectForward.x / dis));

            deplacCible.Set(transform.position.x + ((vectForward.x / dis) * vTir * Time.deltaTime), transform.position.y + ((vectForward.y / dis) * vTir * Time.deltaTime));
            rigi.MovePosition(deplacCible);
            
        }else
        {
            deplacCible.Set(transform.position.x + (vTir * Time.deltaTime), transform.position.y + (vTir * Time.deltaTime));
        }
    }

    public void setDeplacementSize(int p_porte, float p_vtir, int p_degat, float p_x, float p_y, float p_min, float p_max)
    {
        tailleMin = p_min;
        tailleMax = p_max;
        timerSize = p_min;
        sizeV.Set(tailleMin, tailleMin, 1);
        transform.localScale = sizeV;
        portée = p_porte;
        vTir = p_vtir;
        degat = p_degat;
        PX = p_x;
        PY = p_y;
        deplac = new Vector2(PX * vTir * Time.deltaTime, PY * vTir * Time.deltaTime);
        init = true;
        rigi.velocity = deplac;
        isSize = true;
    }

    private void deplacementSize()
    {
        if (timerSize < tailleMax)
        {
            timerSize += Time.deltaTime;
            sizeV.Set(timerSize/2, timerSize/2, 1);
            transform.localScale = sizeV;
            degat += (int)(timerSize * 1.2);
        }

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag.Equals("ennemi"))
        {
            //coll.gameObject.GetComponent<>();
            Destroy(coll.gameObject);
            Destroy(gameObject);
        }
        else if (coll.gameObject.tag.Equals("mur")){
            Destroy(gameObject);
        }
    }


}
