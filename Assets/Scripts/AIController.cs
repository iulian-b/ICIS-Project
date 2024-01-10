using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [Header("Setarile Vehicului")]
    public float SENSIBILITATE_DIRECTIE; //DEFAULT: 0.01
    public float PREVIZIUNE; //DEFAULT: 30
    public float CUPLU_MAX; //DEFAULT: 200
    public float UNGHI_VIRARE_MAX; //DEFAULT: 60
    public float CUPLU_FRANARE_MAX; //DEFAULT: 500
    public float ACCELERATIE_CURBA_MAX; //DEFAULT: 20
    public float FRANARE_CURBA_MAX; //DEFAULT: 10
    public float THRESHOLD_VITEZA_ACCELRATIE; //DEFAULT: 20
    public float THRESHOLD_VITEZA_FRANARE; //DEFAULT: 10
    public float ANTIROLL; //DEFAULT: 5000
    public int ID; //DEFAULT: NONE 
    public int FITNESS; //DEFAULT: 0
    //public TMPro.TextMeshProUGUI lblID = this.transform.position.y + 1.5f;

    Drive[] listaMasini;
    Rigidbody rigidBodyMasina;
    public GameObject brakelight;

    public Circuit Traseu;
    Vector3 Obiectiv;
    AvoidDetector avoid;

    int wpCurent = 0;
    GameObject tracker;
    int currentTrackerWP = 0;

    void Start()
    {
        listaMasini = this.GetComponentsInChildren<Drive>();
        Traseu = GameObject.FindGameObjectWithTag("circuit").GetComponent<Circuit>();
        Obiectiv = Traseu.wpList[wpCurent].transform.position;
        rigidBodyMasina = this.GetComponent<Rigidbody>();

        // Creaza un tracker
        tracker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        DestroyImmediate(tracker.GetComponent<Collider>());
        tracker.GetComponent<MeshRenderer>().enabled = false;
        tracker.transform.position = this.transform.position;
        tracker.transform.rotation = this.transform.rotation;

        avoid = this.GetComponent<AvoidDetector>();
        this.GetComponent<AntiRoll>().ANTIROLL = ANTIROLL;

        foreach (Drive drive in listaMasini)
        {
            drive.CUPLU_MAX = CUPLU_MAX;
            drive.UNGHI_VIRARE_MAX = UNGHI_VIRARE_MAX;
            drive.CUPLU_FRANARE_MAX = CUPLU_FRANARE_MAX;
        }


    }


    float trackerSpeed = 15.0f;
    void ProgressTracker()
    {
        Debug.DrawLine(this.transform.position, tracker.transform.position);
        if (Vector3.Distance(this.transform.position, tracker.transform.position) > PREVIZIUNE)
        {
            trackerSpeed -= 1.0f;
            if (trackerSpeed < 2) trackerSpeed = 2;
            return;
        }

        if (Vector3.Distance(this.transform.position, tracker.transform.position) < PREVIZIUNE / 2.0f)
        {
            trackerSpeed += 1.0f;
            if (trackerSpeed > 15) trackerSpeed = 15;
        }

        tracker.transform.LookAt(Traseu.wpList[currentTrackerWP].transform.position);
        tracker.transform.Translate(0, 0, trackerSpeed * Time.deltaTime);

        if (Vector3.Distance(tracker.transform.position, Traseu.wpList[currentTrackerWP].transform.position) < 1)
        {
            currentTrackerWP++;
            FITNESS++;
            // Loopback
            if (currentTrackerWP >= Traseu.wpList.Length) currentTrackerWP = 0;
        }
    }

    void Update()
    {
        ProgressTracker();
        Obiectiv = tracker.transform.position;

        Vector3 localTarget;

        if (Time.time < avoid.EVITARE_TIMP)
        {
            localTarget = tracker.transform.right * avoid.EVITARE_TRASEU;
        }
        else
        {
            localTarget = this.transform.InverseTransformPoint(Obiectiv);
        }

        //float distanceToTarget = Vector3.Distance(target, this.transform.position);
        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        float s = Mathf.Clamp(targetAngle * SENSIBILITATE_DIRECTIE, -1, 1) * Mathf.Sign(rigidBodyMasina.velocity.magnitude);

        float corner = Mathf.Clamp(Mathf.Abs(targetAngle), 0, 90);
        float cornerFactor = corner / 90.0f;

        float a = 1;
        if(corner > ACCELERATIE_CURBA_MAX && rigidBodyMasina.velocity.magnitude > THRESHOLD_VITEZA_ACCELRATIE)
            a = Mathf.Lerp(0, 1, 1 - cornerFactor);

        float b = 0;
        if (corner > FRANARE_CURBA_MAX && rigidBodyMasina.velocity.magnitude > THRESHOLD_VITEZA_FRANARE)
            b = Mathf.Lerp(0, 1, cornerFactor);

        if (avoid.MARSARIER)
        {
            a = -1 * a;
            s = -1 * s;
        }

        for (int i = 0; i < listaMasini.Length; i++)
        {
            listaMasini[i].Avanseaza(a, s, b);
        }

        if (b > 0)
        {
            brakelight.SetActive(true);
        }
        else
        {
            brakelight.SetActive(false);
        }

        /*if (distanceToTarget < 4)
        {
            currentWP++;
            if (currentWP >= circuit.waypoints.Length)
                currentWP = 0;

            target = circuit.waypoints[currentWP].transform.position;
        }*/
    }
}

