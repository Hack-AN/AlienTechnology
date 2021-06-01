using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

static class Constants
{
    public const float oneblksize = 104;
}

[Serializable]
public class line_btn
{
    public GameObject btn;
}

public class coding_board : MonoBehaviour
{
    public line_btn[] line_btns;
    string bar = "|";

    int bar_height = 0;
    float time = 0;
    bool bar_swch = false;

    public GameObject character;

    private Vector3 from;
    private Vector3 to;
    private float startTime;
    private const float totalTime = 0.7f;

    public bool iskit = false;
    public GameObject kit;

    IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        //line_btns[0].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text += bar;
        coroutine = timer();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        // 일정 시간 동안 bar_height에 있는 문자열에서 bar가 블링크
        if (time >= 0.5f)
        {
            time = 0;
            bar_swch = !bar_swch;
            if (bar_swch)
                line_btns[bar_height].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text += bar;
            else
            {
                string temp = line_btns[bar_height].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
                if (temp.Length == 0)
                    return;
                else if (temp[temp.Length - 1] == bar[0])
                    temp = temp.Substring(0, temp.Length - 1);
                else
                    return;

                line_btns[bar_height].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = temp;
            }
        }
    }

    public void initialize_board()
    {
        locate_bar(0);
        revert();
        for (int i = 0; i < line_btns.Length; i++)
        {
            line_btns[i].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            Image img2 = line_btns[i].btn.GetComponent<Image>();
            img2.color = new Color(img2.color.r, img2.color.g, img2.color.b, 0);
        }
    }

    public void locate_bar(int blk_num)
    {
        if (bar_height == blk_num)
            return;

        string temp = line_btns[bar_height].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;

        if (temp.Length != 0 && temp[temp.Length - 1] == bar[0])
            temp = temp.Substring(0, temp.Length - 1);
        line_btns[bar_height].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = temp;
        bar_height = blk_num;


    }

    public void input_code(string command)
    {
        if (bar_height < line_btns.Length - 1)
        {
            if (line_btns[bar_height].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Length == 0 || line_btns[bar_height].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == bar)
                line_btns[bar_height].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = command;
            else // 나중에 while, if를 위해 들여쓰기에 해당하는 기능도 추가하기.
            {
                for (int i = line_btns.Length - 1; i > bar_height + 1; i--)
                {
                    line_btns[i].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = line_btns[i - 1].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
                }
                line_btns[bar_height + 1].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = command;
            }

            bar_height++;
        }


    }

    public void input_backspace()
    {
        if (line_btns[bar_height].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Length == 0 && bar_height > 0)
            line_btns[--bar_height].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        else if (line_btns[bar_height].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == bar && bar_height > 0)
        {
            line_btns[bar_height].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            line_btns[--bar_height].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";

        }
        else if (line_btns[bar_height].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Length != 0)
            line_btns[bar_height].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
    }

    void go()
    {

        float x = character.GetComponent<character_manage>().cur_vector.x * Constants.oneblksize;
        float y = character.GetComponent<character_manage>().cur_vector.y * Constants.oneblksize;

        to.Set(character.transform.position.x + x, character.transform.position.y + y, 0);

        startTime = Time.time;

        // 매끄러운 애니메이션을 위해 16ms 마다 move 메소드 invoke
        InvokeRepeating("move", 0, 0.0001f);
    }

    void move()
    {

        float deltaTime = Time.time - startTime;

        if (deltaTime < totalTime)
        {
            character.transform.position = Vector3.Lerp(from, to, deltaTime / totalTime);
        }
        else
        {
            character.transform.position = to;
            CancelInvoke("move"); // 애니메이션이 종료되면 invoke repeater 종료
        }
    }
    public void play()
    {
        revert();
        StartCoroutine(timer());
        
    }

    public void revert()
    {
        if(kit != null)
            kit.SetActive(true);
        iskit = false;
        Image img2 = line_btns[line_btns.Length - 1].btn.GetComponent<Image>();
        img2.color = new Color(img2.color.r, img2.color.g, img2.color.b, 0);
        character.GetComponent<character_manage>().initialize_character();
    }

    IEnumerator timer()
    {
        int line = 0;

        yield return new WaitForSeconds(1f);

        while (line <  line_btns.Length)
        {
            string cur = line_btns[line].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            Image img1 = line_btns[line].btn.GetComponent<Image>();
            img1.color = new Color(img1.color.r, img1.color.g, img1.color.b, 0.3f);
            if (line > 0)
            {
                Image img2 = line_btns[line-1].btn.GetComponent<Image>();
                img2.color = new Color(img2.color.r, img2.color.g, img2.color.b, 0);
            }


            if (cur == "Move();" || cur == "Move();|")
            {
                character.GetComponent<character_manage>().change_vector(true);
                from.Set(character.transform.position.x, character.transform.position.y, character.transform.position.z);
                Invoke("go", 0);
            }
            else if(cur == "turnleft();" || cur == "turnleft();|")
            {

                float x = character.GetComponent<character_manage>().cur_vector.x;
                float y = character.GetComponent<character_manage>().cur_vector.y;

                character.GetComponent<character_manage>().cur_vector.x = -y;
                character.GetComponent<character_manage>().cur_vector.y = x;

                character.GetComponent<character_manage>().change_vector(true);

            }
            else if(cur == "turnright();" || cur == "turnright();|")
            {
                float x = character.GetComponent<character_manage>().cur_vector.x;
                float y = character.GetComponent<character_manage>().cur_vector.y;

                character.GetComponent<character_manage>().cur_vector.x = y;
                character.GetComponent<character_manage>().cur_vector.y = -x;

                character.GetComponent<character_manage>().change_vector(true);
            }
            else if(cur == "act();" || cur == "act();|")
            {
                character.GetComponent<character_manage>().change_vector(false);
                if (iskit == true)
                {
                    kit.SetActive(false);
                }
            }

            line++;

            if(cur != "")
                yield return new WaitForSeconds(1f);

        }


        
    }

    public int coded_lines()
    {
        int count = 0;

        for (int i = 0; i < line_btns.Length; i++)
            if (line_btns[i].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text != "" && line_btns[i].btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text != bar)
                count++;

        return count;
    }

}
