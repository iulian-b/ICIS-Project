/* GameLogic */
/* Scirpt care se ocupa de generarea si distrugerea masinilor */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;

public class GameLogic : MonoBehaviour
{
    [Header("Setarile Programului")]
    // DEBUG
    public bool DEBUG = true; //DEFAULT: 0
    public bool DUMPDATA; //DEFAULT: 1
    public int TIMESCALE = 5;

    // Prefabul masinii
    public GameObject carprefab;
    // Lista ce va contine masinile clonate
    public List<GameObject> vehList;
    // Counter pentru ID-urile masinilor
    int IDcounter = 0;

    // Populatie
    public int POPULATIE; //DEFAULT: 20
    // Counterul generatiilor
    public int GENERATIE = 1; //DEFAULT: 1
    // Timpul acordat fiecarui generatie (in secunde)
    public int genTimp; //DEFAULT: 35
    float genTimpStart = 0;
    // Variabile pentru stocarea rezultatelor ultimei generatii
    int lastAvgFitness = 0;
    int lastBestFitness = 0;

    // Lista cu positiile de start a masinilor
    public List<Vector3> vehPositiiList;
    // Offsetul positiilor
    public int OFFSET_Z; //DEFAULT: 5
    public int OFFSET_X; //DEFAULT: 9
    // Marimea gridului
    public int GRID_COL; //DEFAULT: 3
    public int GRID_ROW; //DEFAULT: 10
    //ROW = Mathf.RoundToInt((POPULATIE / GRID_COL) + 0.5f)

    // Interfata
    public TMPro.TextMeshProUGUI txtGeneratie;
    public TMPro.TextMeshProUGUI txtBestFit;
    public TMPro.TextMeshProUGUI txtAvgFit;
    public TMPro.TextMeshProUGUI txtTimpParcurs;
    public TMPro.TextMeshProUGUI txtTimpMax;
    public TMPro.TextMeshProUGUI txtPopulatie;
    public TMPro.TextMeshProUGUI txtLastGen;

    void Start()
    {
        LoadSettings(Application.dataPath + @"\" + "Settings/SpawnCars.ini");
        if (DEBUG) Debug.Log("[DEBUG] Loaded Settings");

        System.IO.Directory.CreateDirectory("DataDump");

        if (DEBUG) Debug.Log("[DEBUG] GRID Size: C" + GRID_COL + " R" + GRID_ROW);

        vehPositiiList = SetareGrid(15, 0.8f, -55);
        if (DEBUG) Debug.Log("[DEBUG] vehPositiiList Size: " + vehPositiiList.Count);

        for (int i = 0; i < POPULATIE; i++)
        {
            // Clonam masina
            Color rc = GetRandColor();
            GameObject newVehClona = Instantiate(carprefab, vehPositiiList[i], this.transform.rotation);
            AIController newVehClonaAI = newVehClona.GetComponent<AIController>();

            // Adaugam parametri aleatori la noua masina clonata
            newVehClonaAI.ID = i;
            newVehClonaAI.SENSIBILITATE_DIRECTIE = UnityEngine.Random.Range(0.01f, 0.1f);
            newVehClonaAI.PREVIZIUNE = UnityEngine.Random.Range(1.0f, 50.0f);
            newVehClonaAI.CUPLU_MAX = UnityEngine.Random.Range(100.0f, 500.0f);
            newVehClonaAI.UNGHI_VIRARE_MAX = UnityEngine.Random.Range(10.0f, 100.0f);
            newVehClonaAI.CUPLU_FRANARE_MAX = UnityEngine.Random.Range(1000.0f, 9000.0f);
            newVehClonaAI.ACCELERATIE_CURBA_MAX = UnityEngine.Random.Range(1.0f, 50.0f);
            newVehClonaAI.FRANARE_CURBA_MAX = UnityEngine.Random.Range(1.0f, 10.0f);
            newVehClonaAI.THRESHOLD_VITEZA_ACCELRATIE = UnityEngine.Random.Range(1.0f, 50.0f);
            newVehClonaAI.THRESHOLD_VITEZA_FRANARE = UnityEngine.Random.Range(1.0f, 25.0f);
            newVehClonaAI.ANTIROLL = UnityEngine.Random.Range(1000.0f, 9000.0f);
            newVehClonaAI.GetComponent<TrailRenderer>().GetComponent<Renderer>().material.color = rc;
            newVehClonaAI.GetComponent<TrailRenderer>().GetComponent<Renderer>().material.SetColor("_EmissionColor", rc);
            newVehClonaAI.GetComponent<TrailRenderer>().GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            newVehClonaAI.GetComponent<TrailRenderer>().startWidth = 0.5f;
            newVehClonaAI.GetComponent<TrailRenderer>().endWidth = 0.5f;

            vehList.Add(newVehClona);
        }
        Time.timeScale = TIMESCALE;

        // Interfata initiala
        txtGeneratie.text = "GENERATIE: " + GENERATIE;
        if(DEBUG) Debug.Log(txtGeneratie.text);
        txtPopulatie.text = "populatie: " + POPULATIE;
        txtTimpParcurs.text = "timp: " + 0;
        txtTimpMax.text = "timp_gen_max: " + genTimp;
        txtLastGen.text = "";
        txtAvgFit.text = "";
        txtBestFit.text = "";
    }

    GameObject Crossover(AIController parinteX, AIController parinteY)
    {
        Color rc = GetRandColor();
        GameObject newVehClona = Instantiate(carprefab, this.transform.position, this.transform.rotation);
        AIController newVehClonaAI = newVehClona.GetComponent<AIController>();

        newVehClonaAI.ID = (IDcounter++);
        newVehClonaAI.SENSIBILITATE_DIRECTIE = (parinteX.SENSIBILITATE_DIRECTIE + parinteY.SENSIBILITATE_DIRECTIE) /2.0f;
        newVehClonaAI.PREVIZIUNE = (parinteX.PREVIZIUNE + parinteY.PREVIZIUNE) / 2.0f;
        newVehClonaAI.CUPLU_MAX = (parinteX.CUPLU_MAX + parinteY.CUPLU_MAX) / 2.0f;
        newVehClonaAI.UNGHI_VIRARE_MAX = (parinteX.UNGHI_VIRARE_MAX + parinteY.UNGHI_VIRARE_MAX) / 2.0f;
        newVehClonaAI.CUPLU_FRANARE_MAX = (parinteX.CUPLU_FRANARE_MAX + parinteY.CUPLU_FRANARE_MAX) / 2.0f;
        newVehClonaAI.ACCELERATIE_CURBA_MAX = (parinteX.ACCELERATIE_CURBA_MAX + parinteY.ACCELERATIE_CURBA_MAX) / 2.0f;
        newVehClonaAI.FRANARE_CURBA_MAX = (parinteX.FRANARE_CURBA_MAX + parinteY.FRANARE_CURBA_MAX) / 2.0f;
        newVehClonaAI.THRESHOLD_VITEZA_ACCELRATIE = (parinteX.THRESHOLD_VITEZA_ACCELRATIE + parinteY.THRESHOLD_VITEZA_ACCELRATIE) / 2.0f;
        newVehClonaAI.THRESHOLD_VITEZA_FRANARE = (parinteX.THRESHOLD_VITEZA_FRANARE + parinteY.THRESHOLD_VITEZA_FRANARE) / 2.0f;
        newVehClonaAI.ANTIROLL = (parinteX.ANTIROLL + parinteY.ANTIROLL) / 2.0f;
        newVehClonaAI.GetComponent<TrailRenderer>().GetComponent<Renderer>().material.color = rc;
        newVehClonaAI.GetComponent<TrailRenderer>().GetComponent<Renderer>().material.SetColor("_EmissionColor", rc);
        newVehClonaAI.GetComponent<TrailRenderer>().GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        newVehClonaAI.GetComponent<TrailRenderer>().startWidth = 0.5f;
        newVehClonaAI.GetComponent<TrailRenderer>().endWidth = 0.5f;

        //newVehClonaAI = Mutate(newVehClonaAI, UnityEngine.Random.Range(0,9));
        return newVehClona;
    }

    AIController Mutate(AIController V, int r) 
    {
        switch (r)
        {
            case 0: 
                V.SENSIBILITATE_DIRECTIE = UnityEngine.Random.Range(0.01f, 0.1f);
                if (DEBUG) Debug.Log("[DEBUG] Mutation on: SENSIBILITATE_DIRECTIE for veh " + V.ID);
                break;
            case 1: 
                V.PREVIZIUNE = UnityEngine.Random.Range(1.0f, 50.0f);
                if (DEBUG) Debug.Log("[DEBUG] Mutation on: PREVIZIUNE for veh " + V.ID);
                break;
            case 2: 
                V.CUPLU_MAX = UnityEngine.Random.Range(100.0f, 500.0f);
                if (DEBUG) Debug.Log("[DEBUG] Mutation on: CUPLU_MAX for veh " + V.ID);
                break;
            case 3: 
                V.UNGHI_VIRARE_MAX = UnityEngine.Random.Range(10.0f, 100.0f);
                if (DEBUG) Debug.Log("[DEBUG] Mutation on: UNGHI_VIRARE_MAX for veh " + V.ID);
                break;
            case 4: 
                V.CUPLU_FRANARE_MAX = UnityEngine.Random.Range(1000.0f, 9000.0f);
                if (DEBUG) Debug.Log("[DEBUG] Mutation on: CUPLU_FRANARE_MAX for veh " + V.ID);
                break;
            case 5:
                V.ACCELERATIE_CURBA_MAX = UnityEngine.Random.Range(1.0f, 50.0f);
                if (DEBUG) Debug.Log("[DEBUG] Mutation on: ACCELERATIE_CURBA_MAX for veh " + V.ID);
                break;
            case 6: 
                V.FRANARE_CURBA_MAX = UnityEngine.Random.Range(1.0f, 10.0f);
                if (DEBUG) Debug.Log("[DEBUG] Mutation on: FRANARE_CURBA_MAX for veh " + V.ID);
                break;
            case 7:  
                V.THRESHOLD_VITEZA_ACCELRATIE = UnityEngine.Random.Range(1.0f, 50.0f);
                if (DEBUG) Debug.Log("[DEBUG] Mutation on: THRESHOLD_VITEZA_ACCELRATIE for veh " + V.ID);
                break;
            case 8:
                V.THRESHOLD_VITEZA_FRANARE = UnityEngine.Random.Range(1.0f, 25.0f);
                if (DEBUG) Debug.Log("[DEBUG] Mutation on: THRESHOLD_VITEZA_FRANARE for veh " + V.ID);
                break;
            case 9: 
                V.ANTIROLL = UnityEngine.Random.Range(1000.0f, 9000.0f);
                if (DEBUG) Debug.Log("[DEBUG] Mutation on: ANTIROLL for veh " + V.ID);
                break;
        }

        return V;
    }

    void Selectie()
    {
        genTimpStart = Time.realtimeSinceStartup;
        List<GameObject> masiniSortate = vehList.OrderByDescending(o => o.GetComponent<AIController>().FITNESS).ToList();
        vehList.Clear();

        // Cross
        for(int i = 0; i < (int)(masiniSortate.Count / 2.0f); i++)
        {
            vehList.Add(Crossover(masiniSortate[i].GetComponent<AIController>(), masiniSortate[i + 1].GetComponent<AIController>()));
            vehList.Add(Crossover(masiniSortate[i + 1].GetComponent<AIController>(), masiniSortate[i].GetComponent<AIController>()));
        }
        for (int i = 0; i < POPULATIE; i++) vehList[i].transform.position = vehPositiiList[i];

        // Luam valorile de fitness curenta
        int bestFitness = masiniSortate[0].GetComponent<AIController>().FITNESS;
        int idCurent, fitnessCurent;
        int avgFitness = 0;

        for (int i = 0; i < vehList.Count; i++)
        {   
            fitnessCurent = masiniSortate[i].GetComponent<AIController>().FITNESS;
            idCurent = masiniSortate[i].GetComponent<AIController>().ID;
            avgFitness += fitnessCurent;

            // Cazuri rare
            if(masiniSortate.Count == 1)
            {
                avgFitness = fitnessCurent;
                break;
            }

            if(DEBUG) Debug.Log("[DEBUG] ID G" + GENERATIE + "C" + idCurent + " FITNESS "+ fitnessCurent);
        }

        avgFitness = (avgFitness / POPULATIE);
        txtBestFit.text = "best_fitness " + bestFitness;
        txtAvgFit.text = "avg_fitness " + avgFitness;

        if(DUMPDATA) DumpDatePlot(vehList, avgFitness, GENERATIE, "DataDump/plot_avg.txt");
        if(DUMPDATA) DumpDatePlot(vehList, bestFitness, GENERATIE, "DataDump/plot_bst.txt");
        
        // Curatam populatia curenta
        for (int i = 0; i < masiniSortate.Count; i++) Destroy(masiniSortate[i]);

        // Avansam generatia
        if (GENERATIE > 1)
        {
            if (DEBUG) Debug.Log("[DEBUG] BEST " + bestFitness);
            if (DEBUG) Debug.Log("[DEBUG] AVG" + avgFitness);
            if (lastBestFitness > bestFitness)
            {
                txtBestFit.color = new Color(255, 0, 0, 1f);
            }
            else
            {
                txtBestFit.color = new Color(0, 255, 0, 1f);
            }

            if (lastAvgFitness > avgFitness)
            {
                txtAvgFit.color = new Color(255, 0, 0, 1f);
            }
            else
            {
                txtAvgFit.color = new Color(0, 255, 0, 1f);
            }
        }
        if (DEBUG) Debug.Log("[DEBUG] Generation step");
        if (GENERATIE > 100) UnityEditor.EditorApplication.isPlaying = false;
        GENERATIE++;
        IDcounter = 0;
        txtGeneratie.text = "GENERATIE: " + GENERATIE;
        txtTimpParcurs.text = "timp: " + 0;
        txtLastGen.text = "gen_" + (GENERATIE - 1);
  
    }

    void Update()
    {

        txtTimpParcurs.text = "timp: " + (Time.realtimeSinceStartup - (genTimp * (GENERATIE - 1)));
        if (Time.realtimeSinceStartup > genTimpStart + genTimp)
        {
            if(DUMPDATA) DumpDateGeneratie(vehList);
            Selectie();
        }
    }

    void DumpDateGeneratie(List<GameObject> L)
    {
        List<GameObject> vehList = L.OrderByDescending(o => o.GetComponent<AIController>().FITNESS).ToList();
        if(DEBUG) for(int i = 0; i < L.Count; i++) Debug.Log(L[i].GetComponent<AIController>().FITNESS);
        int avgLocalFit = 0;
        int bstLocalFit = 0;
        for (int i = 0; i < L.Count; i++) 
        { 
            avgLocalFit += L[i].GetComponent<AIController>().FITNESS; 
            if (L[i].GetComponent<AIController>().FITNESS > bstLocalFit)
                bstLocalFit = L[i].GetComponent<AIController>().FITNESS;
        }
        avgLocalFit = (avgLocalFit / POPULATIE);

        string path = "DataDump/dump_gen_" + GENERATIE + ".txt";
        StreamWriter writer = new StreamWriter(Application.dataPath + @"\" + path, true);

        if (!File.Exists(Application.dataPath + @"\" + path))
        {
            FileStream oFileStream = null;
            oFileStream = new FileStream(Application.dataPath + @"\" + path, FileMode.Create);
            oFileStream.Close();
        }
        
        writer.WriteLine("[GENERATIE " + GENERATIE + "]");
        writer.WriteLine("[POPULATIE " + POPULATIE + "]");
        writer.WriteLine("[AVERAGE FITNESS " + avgLocalFit + "]");
        writer.WriteLine("[BEST FITNESS " + bstLocalFit + "]");

        for (int i = 0; i < vehList.Count; i++)
        {
            writer.WriteLine("ID: " + vehList[i].GetComponent<AIController>().ID);
            writer.WriteLine("FITNESS: " + vehList[i].GetComponent<AIController>().FITNESS);
            writer.WriteLine("SENSIBILITATE_DIRECTIE: " + vehList[i].GetComponent<AIController>().SENSIBILITATE_DIRECTIE);
            writer.WriteLine("PREVIZIUNE: " + vehList[i].GetComponent<AIController>().PREVIZIUNE);
            writer.WriteLine("CUPLU_MAX: " + vehList[i].GetComponent<AIController>().CUPLU_MAX);
            writer.WriteLine("UNGHI_VIRARE_MAX: " + vehList[i].GetComponent<AIController>().UNGHI_VIRARE_MAX);
            writer.WriteLine("CUPLU_FRANARE_MAX: " + vehList[i].GetComponent<AIController>().CUPLU_FRANARE_MAX);
            writer.WriteLine("ACCELERATIE_CURBA_MAX: " + vehList[i].GetComponent<AIController>().ACCELERATIE_CURBA_MAX);
            writer.WriteLine("FRANARE_CURBA_MAX: " + vehList[i].GetComponent<AIController>().FRANARE_CURBA_MAX);
            writer.WriteLine("THRESHOLD_VITEZA_ACCELRATIE: " + vehList[i].GetComponent<AIController>().THRESHOLD_VITEZA_ACCELRATIE);
            writer.WriteLine("THRESHOLD_VITEZA_FRANARE: " + vehList[i].GetComponent<AIController>().THRESHOLD_VITEZA_FRANARE);
            writer.WriteLine("ANTIROLL: " + vehList[i].GetComponent<AIController>().ANTIROLL);
            writer.WriteLine("#########################################");
        }
        writer.Close();
    }


    void DumpDatePlot(List<GameObject> vehList, int fit, int gen, string path)
    {
        if (!File.Exists(Application.dataPath + @"\" + path))
        {
            FileStream oFileStream = null;
            oFileStream = new FileStream(Application.dataPath + @"\" + path, FileMode.Create);
            oFileStream.Close();
        }
        StreamWriter writer = new StreamWriter(Application.dataPath + @"\" + path, true);
        writer.WriteLine(fit);
        writer.Close();
     
    }

    List<Vector3> SetareGrid(int startx, float starty, int startz)
    {
        int x = startx, z = startz;

        List<Vector3> positii = new List<Vector3>();

        for (int i = 0; i < GRID_ROW; i++)
        {
            for (int j = 0; j < GRID_COL; j++)
            {
                z = startz + (j * OFFSET_Z);
                x = startx + (i * OFFSET_X);
                if(DEBUG) Debug.Log(z + " " + x);
                positii.Add(new Vector3(x, starty, z));
            }
        }

        // DEBUG
        //for (int i = 0; i < positii.Count; i++) Debug.Log(positii[i]);

        return positii;
    }

    Color GetRandColor()
    {
        int r = UnityEngine.Random.Range(0, 6);
        switch (r)
        {
            case 0: return Color.blue;
            case 1: return Color.cyan;
            case 2: return Color.green;
            case 3: return Color.magenta;
            case 4: return Color.red;
            case 5: return Color.white;
            case 6: return Color.yellow;
        }
        return Color.white;
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

        if (settings[0] == "1") DEBUG = true; else DEBUG = false;
        if (settings[1] == "1") DUMPDATA = true; else DUMPDATA = false;

        POPULATIE = Int32.Parse(settings[2]);
        genTimp = Int32.Parse(settings[3]);

        OFFSET_Z = Int32.Parse(settings[4]);
        OFFSET_X = Int32.Parse(settings[5]);
        GRID_COL = Int32.Parse(settings[6]);
        GRID_ROW = Int32.Parse(settings[7]);
    }
}
