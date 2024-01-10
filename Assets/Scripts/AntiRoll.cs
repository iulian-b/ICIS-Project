/*
 * Scriptul verifica daca toate rotile masinii 
 * ating pamantul, si se asigura ca masina  
 * ramane lipita de sol.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRoll : MonoBehaviour
{ 
    // ANTIROLL
    public float ANTIROLL; // DEFAULT: 5000.0

    // Collidere roti
    public WheelCollider roataLF; // Stanga Fata
    public WheelCollider roataLB; // Stanga Spate
    public WheelCollider roataRF; // Dreapta Fata
    public WheelCollider roataRB; // Dreapta Spate

    Rigidbody rigidBodyMasina;

    void Start() { rigidBodyMasina = this.GetComponent<Rigidbody>(); }

    void ImpanteazaRoti(WheelCollider roataSx, WheelCollider roataDx)
    {
        WheelHit contact;
        float deplasareLx = 1.0f;
        float deplasareDx = 1.0f;

        // Roata Stanga
        bool impamantatLx = roataSx.GetGroundHit(out contact);
        if (impamantatLx)
            deplasareLx = (-roataSx.transform.InverseTransformPoint(contact.point).y - roataSx.radius) / roataSx.suspensionDistance;

        // Roata Dreapta
        bool impamantatDx = roataDx.GetGroundHit(out contact);
        if (impamantatDx)
            deplasareDx = (-roataDx.transform.InverseTransformPoint(contact.point).y - roataDx.radius) / roataDx.suspensionDistance;

        // Aplicare antiroll
        float antiRollForce = (deplasareLx - deplasareDx) * ANTIROLL;
        if (impamantatLx) rigidBodyMasina.AddForceAtPosition(roataSx.transform.up * -antiRollForce, roataSx.transform.position);
        if (impamantatDx) rigidBodyMasina.AddForceAtPosition(roataDx.transform.up * -antiRollForce, roataDx.transform.position);
    }

    void Update()
    {
        // Impamantare roti fata
        ImpanteazaRoti(roataLF, roataRF);
        // Impamantare roti spate
        ImpanteazaRoti(roataLB, roataRB);
    }
}
