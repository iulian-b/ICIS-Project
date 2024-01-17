/*
 * Scriptul permite masinii sa 
 * evite alte masini prezente pe circuit
 * In cazul in care masina sa blocat
 * (ex. sa izbit de un obstacol)
 * scriptul va incerca sa intoarca
 * directia masinii (marsarier) pentru
 * ai permite sa se deblocheze.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class AvoidDetector : MonoBehaviour
{
    public float EVITARE_TRASEU; // DEFAULT: 0f
    public float EVITARE_TIMP; // DEFAULT: 0f
    public float EVITARE_LUNGIME; // DEFAULT: 1f
    public float WANDER_DISTANTA; // DEFAULT: 4f

    public bool MARSARIER = false;
    Rigidbody rigidBodyMasina;

    void Start() 
    {
        LoadSettings(Application.dataPath + @"\" + "Settings/AvoidDetector.ini");
        rigidBodyMasina = this.GetComponent<Rigidbody>(); 
    }

    // Caz de exit: masina nu mai intalneste un obstacol
    void OnTriggerExit(Collider col)
    {
        MARSARIER = false;
        if (col.gameObject.tag != "car") return;
        EVITARE_TIMP = 0;
    }

    // Caz de entry masina intalneste un obstacol
    void OnTriggerStay(Collider col)
    {
        Vector3 directieColisiune = this.transform.InverseTransformPoint(col.gameObject.transform.position);

        if (directieColisiune.x > 0 && directieColisiune.z > 0)
        {
            // TODO: implementare mai buna.  
            /*  NullReferenceException: Object reference not set to an instance of an object
                AvoidDetector.OnTriggerStay (UnityEngine.Collider col) (at Assets/Scripts/AvoidDetector.cs:42)
             */
            if (rigidBodyMasina.velocity.magnitude < 1) MARSARIER = true;
            else if (col.gameObject.tag == "car")
            {
                Rigidbody rigidBodyMasinaObstacol = col.GetComponent<Rigidbody>();
                EVITARE_TIMP = Time.time + EVITARE_LUNGIME;
                Vector3 directieMasinaObstacol = transform.InverseTransformPoint(rigidBodyMasinaObstacol.gameObject.transform.position);
                float unghiMasinaObstacol = Mathf.Atan2(directieMasinaObstacol.x, directieMasinaObstacol.z);
                EVITARE_TRASEU = WANDER_DISTANTA * -Mathf.Sign(unghiMasinaObstacol);
            }
        }
    }

    void LoadSettings(string F)
    {
        List<string> settings = new List<string>();
        StreamReader sr = new StreamReader(F);
        string line;
        line = sr.ReadLine();
        settings.Add(line);

        while (line != null)
        {
            //Debug.Log(line);
            line = sr.ReadLine();
            if (line != null) settings.Add(line);
        }

        sr.Close();

        EVITARE_TRASEU = float.Parse(settings[0]);
        EVITARE_TIMP = float.Parse(settings[1]);
        EVITARE_LUNGIME = float.Parse(settings[2]);
        WANDER_DISTANTA = float.Parse(settings[3]);
    }
}
