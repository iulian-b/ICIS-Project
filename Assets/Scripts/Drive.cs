/*
 * Scriptul se ocupa de logica 
 * de conducere a masini.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    public WheelCollider colliderRoti;
    public GameObject modelRoti;

    public float CUPLU_MAX; //DEFAULT: 200
    public float UNGHI_VIRARE_MAX; //DEFAULT: 60
    public float CUPLU_FRANARE_MAX; //DEFAULT: 500
    // Pentru rotiile din fata, unde virajul este posibil
    public bool VIRARE_POSIBILA = false; //DEFAULT: 0

    void Start(){ colliderRoti = this.GetComponent<WheelCollider>();}

    public void Avanseaza(float acceleratie, float viraj, float franare)
    {
        Quaternion quat;
        Vector3 position;
        acceleratie = Mathf.Clamp(acceleratie, -1, 1);
        float cupluImpingere = acceleratie * CUPLU_MAX;
        colliderRoti.motorTorque = cupluImpingere;

        if (VIRARE_POSIBILA)
        {
            viraj = Mathf.Clamp(viraj, -1, 1) * UNGHI_VIRARE_MAX;
            colliderRoti.steerAngle = viraj;
        }
        else
        {
            franare = Mathf.Clamp(franare, -1, 1) * CUPLU_FRANARE_MAX;
            colliderRoti.brakeTorque = franare;
        }

        colliderRoti.GetWorldPose(out position, out quat);
        modelRoti.transform.position = position;
        modelRoti.transform.rotation = quat;
    }
}