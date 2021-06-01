using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class animating_objs
{
    public GameObject[] obj;
}

public class stage_manager : MonoBehaviour
{
    public int stage_num;
    public int max_stage_index = 0;

    public bool[] first_visit;
    public animating_objs[] anime_objs;
    public GameObject[] BG;
    public GameObject BG_obj;
    public GameObject titleUI;
    public GameObject machine;
    public GameObject etc_UI;
    public GameObject character;
    public GameObject clear_message;
    public GameObject menu;
    public GameObject board;
    public GameObject coding_interface;
    public GameObject stage_choice;
    public GameObject game_start;

    public string[] stage_names;
    public Text stage_name;

    public GameObject speech_btn;
    public Text speech;
    string temp; 
    bool isspeeching = false;
    float time = 0;
    int count_string = 0;
    bool speech_done = false;
    int count_speech_visit = 0;
    int count_speech_end = 0;

    public int[] goal_lines_three_star = { 2, 7, 9 };
    public int[] goal_lines_two_star = { 4, 9, 11 };
    public int[] goal_lines_one_star = { 6, 11, 13 };

    private Vector3 from;
    private Vector3 to;
    private float startTime;
    private float totalTime;
    private GameObject invoke_obj;

    Vector2 anime_objs_first_loc_1_1;

    public AudioSource rocket_sound;

    private void Start()
    {
        Array.Resize(ref first_visit, BG.Length);
        for (int i = 0; i < BG.Length; i++)
            first_visit[i] = true;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (isspeeching)
        {
            if (time >= 0.05f)
            {
                time = 0;
                speech.text += temp[count_string++];
                if (count_string == temp.Length)
                {
                    count_string = 0;
                    isspeeching = false;
                    speech_done = true;
                }
            }
        }

        if (time >= 0.5f && speech_done)
        {
            time = 0;
            if (speech.text[speech.text.Length - 1] == '▼')
                speech.text = temp;
            else
                speech.text += "▼";
        }

    }

    void go(GameObject obj, float x, float y, float time, float start_delay = 0)
    {
        invoke_obj = obj;
        from.Set(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
        to.Set(obj.transform.position.x + x, obj.transform.position.y + y, 0);
        totalTime = time;
        startTime = Time.time + start_delay;

        // 매끄러운 애니메이션을 위해 16ms 마다 move 메소드 invoke
        InvokeRepeating("move", start_delay, 0.0001f);
    }

    void move()
    {

        float deltaTime = Time.time - startTime;

        if (deltaTime < totalTime)
        {
            invoke_obj.transform.position = Vector3.Lerp(from, to, deltaTime / totalTime);
        }
        else
        {
            invoke_obj.transform.position = to;
            CancelInvoke("move"); // 애니메이션이 종료되면 invoke repeater 종료
        }
    }

    void text_animation(int count, string text)
    {
        count_string = 0;
        if (count == count_speech_visit)
        {
            speech.text = "";
            temp = text;
            coding_interface.SetActive(false);
            isspeeching = true;
            
        }
    }

    void text_animation_end(int count)
    { 
        if(count == count_speech_visit)
        {
            speech_btn.SetActive(false);
            isspeeching = false;
            speech_done = false;
            count_string = 0;
            count_speech_visit = -1;
            temp = "";
            speech.text = "";
            coding_interface.SetActive(true);
        }
    }

    void character_disappear()
    {
        Image img2 = character.GetComponent<Image>();
        img2.color = new Color(img2.color.r, img2.color.g, img2.color.b, 0);
    }

    void character_appear()
    {
        Image img2 = character.GetComponent<Image>();
        img2.color = new Color(img2.color.r, img2.color.g, img2.color.b, 255);
    }

    void obj_animation(int count, int obj_index)
    {
        if(count == count_speech_visit)
        {
            switch(stage_num)
            {
                case 1:
                    // 캐릭터 타기
                    Invoke("character_disappear", 0f);

                    // 로켓 이륙
                    go(anime_objs[stage_num].obj[obj_index], 0f, 2000f, 3f, 1f);
                    break;
                case 2:
                    Invoke("character_disappear", 0f);
                    // 로켓 착륙
                    go(anime_objs[stage_num].obj[obj_index], 0f, 346 - 1767, 3f);

                    // 캐릭터 점프
                    Invoke("character_appear", 3.5f);
                    Invoke("visit_animation", 3.5f);
                    break;
            }
            
            //visit_animation();
        }
    }

    public void visit_animation()
    {
        switch(stage_num)
        {
            case 0:
                text_animation(0, "긱-하!");
                text_animation(1, "반가워요, \n전 긱블러들에게\n'외계기술 발견일지'를\n가이드할\n기글즈입니다. :D");
                text_animation(2, "지금부터 외계기술을\n발견하기 위한 모험을\n떠날 예정입니다.");
                text_animation(3, "외계문명은 인간들과\n어떻게 다른 문명을\n쌓아왔을까요?");
                text_animation(4, "또 어떻게 다른 \n과학기술을\n쌓아왔을까요?");
                text_animation(5, "상상만으로도\n벌써 가슴이\n두근두근대는군요...!");
                text_animation(6, "그런데 통신 기술이\n아직 부족해 제한된\n방법으로만 기글이를\n움직여야만 해요.\nㅠㅠ");
                text_animation(7, "그래도 하나씩\n배워가다보면\n어렵지 않을 거예요!");
                text_animation(8, "우선\n다음의 안내를 보고\n방을 나가보도록\n할까요?");
                text_animation(9, "\n<-여기에 있는\nMove()버튼은\n'한칸 전진'을 의미해요.");
                text_animation(10, "초록색 ▶ 버튼을 누르면\n코드를 입력한대로 \n기글이가 움직여요!");
                text_animation(11, "빨간색 버튼을 누르면\n기글이가 원상복귀하게\n되요.");
                text_animation(12, "방을 나가서\n모험을 시작해봐요.\n건투를 빌어요!");
                text_animation_end(13);

                break; 
            case 1:
                text_animation(0, "아주 잘했어요!");
                text_animation(1, "처음치고\n엄청 잘하는데요?!");
                text_animation(2, "역시 긱블러답습니다 :)");
                text_animation(3, "이제 외계행성으로\n나가기 위한\n우주선에 타야 해요.");
                text_animation(4, "\n\n\n\n\n<-여기의 turnleft()\n버튼을 누르면\n방향을 왼쪽으로\n회전해요.");
                text_animation(5, "다른 기글이들도\n응원하러 나왔네요!");
                text_animation(6, "멋있다!!!!!!\n\n      최고다!!!\n\n   믿고 있는다구!   \n\n가즈아!!    \n\n             사랑했다!");
                text_animation(7, "당신에게 거는\n기대가 큰 듯 하네요!\n저도 그렇구요 :)");
                text_animation(8, "이번에도 잘하리라\n믿어요. 건투를 빌어요!");
                text_animation_end(9);
                break;
            case 2:
                obj_animation(0, 0);
                if(count_speech_visit == 0)
                    rocket_sound.Play();
                text_animation(1, "드디어 첫 번째\n외계행성에\n도착했어요...!");
                text_animation(2, "근데 앞에 이상한\n물건이 보이네요?");
                text_animation(3, "첫 번째 외계기술\n발견이 될 듯 한데요...?!");
                text_animation(4, "\n            여기있는->\nact()버튼은 물건을\n줍는 등의 행동을\n의미해요.");
                text_animation(5, "저 물건을 주워서\n다시 원위치로\n돌아와주세요!");
                text_animation(6, "첫 번째 성과\n기대하겠습니다 ><");
                text_animation_end(7);
                break;
        }

        count_speech_visit++;
    }

    public void ending_animation()
    {
        switch (stage_num)
        {
            case 0:
                text_animation_end(0);
                break;
            case 1:
                obj_animation(0, 0);
                if (count_speech_visit == 0)
                    rocket_sound.Play();
                text_animation_end(0);
                break;
            case 2:
                text_animation_end(0);
                break;
        }
        count_speech_visit++;
    }

    public void change_BG_right()
    {
        clear_message.SetActive(false);
        if (stage_num < max_stage_index)
        {
            BG[stage_num].SetActive(false);
            BG[++stage_num].SetActive(true);
        }
        else
        {
            BG[stage_num].SetActive(false);
            stage_num = 0;
            BG[stage_num].SetActive(true);
        }

        stage_name.text = "에피소드 " + (stage_num + 1) + "\n" + stage_names[stage_num];
        character.GetComponent<character_manage>().initialize_character();
        board.GetComponent<coding_board>().initialize_board();

        anime_objs[1].obj[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(356.2f, 311f);
        if (first_visit[stage_num] == true && titleUI.activeSelf == false)
        {
            first_visit[stage_num] = false;
            speech_btn.SetActive(true);
            visit_animation();
        }
    }

    public void change_BG_left()
    {
        if (stage_num > 0)
        {
            BG[stage_num].SetActive(false);
            BG[--stage_num].SetActive(true);
        }
        else
        {
            BG[stage_num].SetActive(false);
            stage_num = max_stage_index;
            BG[stage_num].SetActive(true);
        }

        anime_objs[1].obj[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(356.2f, 311f);
        stage_name.text = "에피소드 " + (stage_num + 1) + "\n" + stage_names[stage_num];
        character.GetComponent<character_manage>().initialize_character();
        board.GetComponent<coding_board>().initialize_board();
    }

    public void turn_off_titleUI()
    {
        titleUI.SetActive(false);
        machine.SetActive(true);
        etc_UI.SetActive(true);

        if (first_visit[stage_num] == true)
        {
            first_visit[stage_num] = false;
            speech_btn.SetActive(true);
            visit_animation();
        }
    }

    public void turn_on_titleUI()
    {
        anime_objs[1].obj[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(356.2f, 311f);
        menu.SetActive(false);
        clear_message.SetActive(false);
        titleUI.SetActive(true);
        machine.SetActive(false);
        etc_UI.SetActive(false);
    }
    
    public void turn_on_off_UI()
    {
        if(game_start.activeSelf == true)
        {
            game_start.SetActive(false);
            stage_choice.SetActive(true);
        }
        else
        {
            game_start.SetActive(true);
            stage_choice.SetActive(false);
        }
    }

    public void quit()
    {
        Application.Quit();
    }


}
