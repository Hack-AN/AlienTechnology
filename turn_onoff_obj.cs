using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turn_onoff_obj : MonoBehaviour
{
    public GameObject obj;


    public void turn_onoff()
    {
        if (obj.activeSelf == true)
            obj.SetActive(false);
        else
            obj.SetActive(true);
    }
}
