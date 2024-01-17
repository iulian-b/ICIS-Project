using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;

public class InitSettings : MonoBehaviour
{

    string SpawnCarsPath = "Assets/Settings/SpawnCars.ini";
    string AIControllerPath = "Assets/Settings/AIController.ini";
    string FlipPath = "Assets/Settings/Flip.ini";
    string AvoidDetectorPath = "Assets/Settings/AvoidDetector.ini";
    string line;

    List<string> SpawnCars = new List<string>();
    List<string> Flip = new List<string>();
    List<string> AIController = new List<string>();
    List<string> AvoidDetector = new List<string>();

    public Button loadbuttton;
    public Button resetbutton;

    public TMP_InputField S1Populatie;
    public TMP_InputField S1Timp;
    public TMP_InputField S1Offsetz;
    public TMP_InputField S1Offsetx;
    public TMP_InputField S1Gridcol;
    public TMP_InputField S1Gridrow;
    public Toggle S1Debug;
    public Toggle S1Dump;

    public TMP_InputField S2Sensibilitate;
    public TMP_InputField S2Previziune;
    public TMP_InputField S2Cuplumax;
    public TMP_InputField S2Unghi;
    public TMP_InputField S2Cuplufranare;
    public TMP_InputField S2Acceleratie;
    public TMP_InputField S2Franare;
    public TMP_InputField S2Thresholdacc;
    public TMP_InputField S2Thresholdfrn;
    public TMP_InputField S2Antiroll;

    public TMP_InputField S3Interval;

    public TMP_InputField S4Traseu;
    public TMP_InputField S4Timp;
    public TMP_InputField S4Lungime;
    public TMP_InputField S4Wander;

    void Start()
    {
        // Initial Load
        SpawnCars = LoadSettingsFile(SpawnCarsPath);
        Flip = LoadSettingsFile(FlipPath);
        AIController = LoadSettingsFile(AIControllerPath);
        AvoidDetector = LoadSettingsFile(AvoidDetectorPath);

        //ReadList(SpawnCars);
        DisplaySettings(SpawnCars, Flip, AIController, AvoidDetector);
        
        Button btnReset = resetbutton.GetComponent<Button>();
        Button btnWrite = loadbuttton.GetComponent<Button>();
        btnReset.onClick.AddListener(ResetSettings);
        btnWrite.onClick.AddListener(WriteNewSettings);
    }

    List<string> LoadSettingsFile(string F)
    {
        List<string> output = new List<string>();
        StreamReader sr = new StreamReader(F);
       
        line = sr.ReadLine();
        output.Add(line);
        
        while (line != null)
        {
            //Debug.Log(line);
            line = sr.ReadLine();
            if(line != null) output.Add(line);
        }

        sr.Close();
        return output;
    }

    void ReadList(List<string> L)
    {
        foreach (string i in L) Debug.Log(i);
    }

    void ClearFile(string F)
    {
        FileStream fileStream = File.Open(F, FileMode.Open);
        fileStream.SetLength(0);
        fileStream.Close();
    }

    void WriteSettings(string F, List<string> L)
    {
        StreamWriter writer = new StreamWriter(F);
        foreach (string s in L) writer.WriteLine(s);
        writer.Close();
    }

    void DisplaySettings(List<string> spawn, List<string> flip, List<string> ctrl, List<string> avdt)
    {
        // SpawnCars
        if (spawn[0] == "1") S1Debug.isOn = true; else S1Debug.isOn = false;
        if (spawn[1] == "1") S1Dump.isOn = true; else S1Dump.isOn = false;
        S1Populatie.text = spawn[2];
        S1Timp.text = spawn[3];
        S1Offsetz.text = spawn[4];
        S1Offsetx.text = spawn[5];
        S1Gridcol.text = spawn[6];
        S1Gridrow.text = spawn[7];

        // AIController
        S2Sensibilitate.text = ctrl[0];
        S2Previziune.text = ctrl[1];
        S2Cuplumax.text = ctrl[2];
        S2Unghi.text = ctrl[3];
        S2Cuplufranare.text = ctrl[4];
        S2Acceleratie.text = ctrl[5];
        S2Franare.text = ctrl[6];
        S2Thresholdacc.text = ctrl[7];
        S2Thresholdfrn.text = ctrl[8];
        S2Antiroll.text = ctrl[9];

        // Flip
        S3Interval.text = flip[0];
    
        // AvoidDetect
        S4Traseu.text = avdt[0];
        S4Timp.text = avdt[1];
        S4Lungime.text = avdt[2];
        S4Wander.text = avdt[3];
    }

    void WriteNewSettings()
    {
        // Spawncars
        List<string> NEWspawncars = new List<string>();
        if (S1Debug.isOn == true) NEWspawncars.Add("1"); else NEWspawncars.Add("0");
        if (S1Dump.isOn == true) NEWspawncars.Add("1"); else NEWspawncars.Add("0");
        NEWspawncars.Add(S1Populatie.text);
        NEWspawncars.Add(S1Timp.text);
        NEWspawncars.Add(S1Offsetz.text);
        NEWspawncars.Add(S1Offsetx.text);
        NEWspawncars.Add(S1Gridcol.text);
        NEWspawncars.Add(S1Gridrow.text);
        SpawnCars = NEWspawncars;
        WriteSettings(SpawnCarsPath, NEWspawncars);

        // AIController
        List<string> NEWaicontroller = new List<string>();
        NEWaicontroller.Add(S2Sensibilitate.text);
        NEWaicontroller.Add(S2Previziune.text);
        NEWaicontroller.Add(S2Cuplumax.text);
        NEWaicontroller.Add(S2Unghi.text);
        NEWaicontroller.Add(S2Cuplufranare.text);
        NEWaicontroller.Add(S2Acceleratie.text);
        NEWaicontroller.Add(S2Franare.text);
        NEWaicontroller.Add(S2Thresholdacc.text);
        NEWaicontroller.Add(S2Thresholdfrn.text);
        NEWaicontroller.Add(S2Antiroll.text);
        AIController = NEWaicontroller;
        WriteSettings(AIControllerPath, NEWaicontroller);

        // Flip
        List<string> NEWflip = new List<string>();
        NEWflip.Add(S3Interval.text);
        Flip = NEWflip;
        WriteSettings(FlipPath, NEWflip);

        // AvoidDetector
        List<string> NEWavoiddetector = new List<string>();
        NEWavoiddetector.Add(S4Traseu.text);
        NEWavoiddetector.Add(S4Timp.text);
        NEWavoiddetector.Add(S4Lungime.text);
        NEWavoiddetector.Add(S4Wander.text);
        AvoidDetector = NEWavoiddetector;
        WriteSettings(AvoidDetectorPath, NEWavoiddetector);

        // Reset Menu
        DisplaySettings(SpawnCars, Flip, AIController, AvoidDetector);
    }

    void ResetSettings()
    {
        // Valori default
        List<string> DFLTspawncars = new List<string>() { "0", "1", "20", "35", "4", "10", "3", "7" };
        List<string> DFLTaicontroller = new List<string>() { "0.01", "30.0", "200.0", "60.0", "500.0", "20.0", "10.0", "20.0", "10.0", "5000.0" };
        List<string> DFLTavoiddetector = new List<string>() { "0.0", "0.0", "1.0", "4.0" };
        List<string> DFLTflip = new List<string>() { "3" };

        // Clear
        ClearFile(SpawnCarsPath);
        ClearFile(AIControllerPath);
        ClearFile(FlipPath);
        ClearFile(AvoidDetectorPath);

        // Write
        WriteSettings(SpawnCarsPath, DFLTspawncars);
        WriteSettings(AIControllerPath, DFLTaicontroller);
        WriteSettings(FlipPath, DFLTflip);
        WriteSettings(AvoidDetectorPath, DFLTavoiddetector);

        // Reset Menu
        SpawnCars = DFLTspawncars;
        AIController = DFLTaicontroller;
        AvoidDetector = DFLTavoiddetector;
        Flip = DFLTflip;
        DisplaySettings(SpawnCars, Flip, AIController, AvoidDetector);
    }


}
