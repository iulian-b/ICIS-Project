using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMainScene : MonoBehaviour
{
    public Button startbutton;

    void Start()
    {
        Button btnStart = startbutton.GetComponent<Button>();
        btnStart.onClick.AddListener(StartScene);
    }

    void StartScene()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
