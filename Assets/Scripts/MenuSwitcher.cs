using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuSwitcher : MonoBehaviour
{
    TMPro.TMP_Dropdown Drop;
    public GameObject menu1;
    public GameObject menu2;
    public GameObject menu3;
    public GameObject menu4;

    void Start()
    {
        Drop = this.GetComponent<TMP_Dropdown>();
        Drop.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged();
        });
    }

    void DropdownValueChanged()
    {
        int val = this.GetComponent<TMP_Dropdown>().value;
        switch (val)
        {
            case 0:
                menu1.SetActive(true);
                menu2.SetActive(false);
                menu3.SetActive(false);
                menu4.SetActive(false);
                break;
            case 1:
                menu1.SetActive(false);
                menu2.SetActive(false);
                menu3.SetActive(true);
                menu4.SetActive(false);
                break;
            case 2:
                menu1.SetActive(false);
                menu2.SetActive(false);
                menu3.SetActive(false);
                menu4.SetActive(true);
                break;
            /*case 3:
                menu1.SetActive(false);
                menu2.SetActive(false);
                menu3.SetActive(false);
                menu4.SetActive(true);
                break;*/
        }
    }
}
