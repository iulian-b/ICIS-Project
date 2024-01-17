/*
 * Scriptul verifica la un anumit
 * interval de timp daca masina 
 * este rasturnata, si o intoarce
 * daca asta este cazul.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Flip : MonoBehaviour
{
    float ultimaVerificare;
    //float rataVerificare = 3f;
    // Intervalul de verificare
    int intervalVerificare; // DEFAULT: 3

    Rigidbody rigidBodyMasina;

    void Start()
    {
        StreamReader sr = new StreamReader(Application.dataPath + @"\" + "Settings/Flip.ini");
        intervalVerificare = Int32.Parse(sr.ReadLine());
        sr.Close();

        rigidBodyMasina = this.GetComponent<Rigidbody>(); 
    }

    // Indreapta masina
    void Indreapta()
    {
        this.transform.position += Vector3.up;
        this.transform.rotation = Quaternion.LookRotation(this.transform.forward);
    }

    void Update()
    {
        // Updatare ultima verificare
        if (transform.up.y > 0.5f || rigidBodyMasina.velocity.magnitude > 1)
            ultimaVerificare = Time.time;
        
        // Verificare dupa intervalul determinat
        if (Time.time > ultimaVerificare + intervalVerificare)
            Indreapta();
    }
}
