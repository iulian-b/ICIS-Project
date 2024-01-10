/*
 * Scriptul se ocupa cu crearea 
 * unui traseu de waypointuri
 * pentru ca masinile sa le urmareasca.
 * 
 * https://www.youtube.com/watch?v=eAtEvUKKFEA
 * Bazat pe acest material
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circuit : MonoBehaviour
{
    public GameObject[] wpList;

    void OnDrawGizmos(){ DrawGizmos(false); }

    void OnDrawGizmosSelected(){ DrawGizmos(true); }

    void DrawGizmos(bool sel)
    {
        if (!sel) return;
        if (wpList.Length > 1)
        {
            Vector3 prv = wpList[0].transform.position;
            for (int i = 1; i < wpList.Length; i++)
            {
                Vector3 nxt = wpList[i].transform.position;
                Gizmos.DrawLine(prv, nxt);
                prv = nxt;
            }
            Gizmos.DrawLine(prv, wpList[0].transform.position);
        }
    }

}
