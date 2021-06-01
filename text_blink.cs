using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class text_blink : MonoBehaviour
{
    public Text text;
    bool text_swch = false;
    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = this.gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        // 일정 시간 동안 bar_height에 있는 문자열에서 bar가 블링크
        if (time >= 0.5f)
        {
            time = 0;
            text_swch = !text_swch;
            if (text_swch)
                text.text = "";
            else
                text.text = "터치로 시작";
        }
    }
}
