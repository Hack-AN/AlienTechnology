using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class locating_obj : MonoBehaviour
{
    public Canvas canvas;
    public bool isup;

    // Start is called before the first frame update
    void Start()
    {
        if(isup)
            this.transform.position = new Vector3(canvas.transform.position.x, canvas.transform.position.y + canvas.GetComponent<RectTransform>().rect.height /2, 0);
        else
            this.transform.position = new Vector3(canvas.transform.position.x, canvas.transform.position.y - canvas.GetComponent<RectTransform>().rect.height/2 , 0);
    }

}
