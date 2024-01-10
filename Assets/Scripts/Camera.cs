using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;
    public GameObject cam4;
    public TMPro.TextMeshProUGUI txtCamera;
    public GameObject miniMap;

    public Texture map1;
    public Texture map2;
    public Texture map3;
    public Texture map4;



    void Start()
    {
        SwitchCamera(cam1, cam2, cam3, cam4);
        txtCamera.text = "<camera: parabolica>";
    }

    void SwitchCamera(GameObject Active, GameObject inactiv1, GameObject inactiv2, GameObject inactiv3)
    {
        Active.SetActive(true);
        inactiv1.SetActive(false);
        inactiv2.SetActive(false);
        inactiv3.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow)) { SwitchCamera(cam1, cam2, cam3, cam4); txtCamera.text = "<camera: parabolica>"; miniMap.GetComponent<RawImage>().texture = map1; }
        if (Input.GetKey(KeyCode.LeftArrow)) { SwitchCamera(cam2, cam1, cam3, cam4); txtCamera.text = "<camera: rettifilo>"; miniMap.GetComponent<RawImage>().texture = map2; }
        if (Input.GetKey(KeyCode.UpArrow)) { SwitchCamera(cam3, cam2, cam1, cam4); txtCamera.text = "<camera: lesmo>"; miniMap.GetComponent<RawImage>().texture = map3; }
        if (Input.GetKey(KeyCode.RightArrow)) { SwitchCamera(cam4, cam2, cam3, cam1); txtCamera.text = "<camera: ascari>"; miniMap.GetComponent<RawImage>().texture = map4; }
    }
}
