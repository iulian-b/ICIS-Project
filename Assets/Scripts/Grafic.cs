using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Grafic : MonoBehaviour
{
    public Sprite cercSprite;
    private RectTransform containerGrafic;
    string path = "Assets/plot.txt";
    List<int> Yval;
    float timeDelta = 0;

    private void Awake()
    {
        containerGrafic = transform.Find("container grafic").GetComponent<RectTransform>();
    }

    private void PutCerc(Vector2 anchoredPos)
    {
        GameObject cerc = new GameObject("punct", typeof(Image));
        cerc.transform.SetParent(containerGrafic, false);
        cerc.GetComponent<Image>().sprite = cercSprite;
        RectTransform rt = cerc.GetComponent<RectTransform>();
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = new Vector2(11, 11);
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(0, 0);
    }

    private void ShowGraph(List<int> yList)
    {
        float graphH = containerGrafic.sizeDelta.y;
        float graphMaxY = 100f;
        float xSize = 50f;

        for (int i = 0; i < yList.Count; i++)
        {
            float x = i * xSize;
            float y = (yList[i] / graphMaxY) * graphH;
            PutCerc(new Vector2(x, y));
        }

        timeDelta += 15;
    }

    private void Update()
    {
        if (Time.realtimeSinceStartup > (timeDelta + 15))
        {
            StreamReader reader = new StreamReader(path);
            int y = 10;
            Yval.Add(y);
            PutCerc(new Vector2(timeDelta, y));
            timeDelta += 15;
            reader.Close();
        }
    }
}
