using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class character_manage : MonoBehaviour
{
    public Sprite[] forward_standing;
    public Sprite[] forward_walking;
    public Sprite[] back_standing;
    public Sprite[] back_walking;
    public Sprite[] left_standing;
    public Sprite[] left_walking;
    public Sprite[] right_standing;
    public Sprite[] right_walking;

    public GameObject stage_num;
    public GameObject coding_board;
    public Text score;
    public Text goal;
    public GameObject[] stars;

    public GameObject[] start_location_obj;

    public Vector2[] start_vector;
    /* (-1,0) : 서쪽
     * (1, 0) : 동쪽
     * (0,-1) : 남쪽
     * (0, 1) : 북쪽
     */

    public Vector2 cur_vector;

    public GameObject clear_message;


    int animation = 0;
    const int const_forward_standing = 0;
    const int const_forward_walking = 1;
    const int const_back_standing = 2;
    const int const_back_walking = 3;
    const int const_left_standing = 4;
    const int const_left_walking = 5;
    const int const_right_standing = 6;
    const int const_right_walking = 7;

    float time = 0;
    int animation_index = 0;
    bool animating = false;

    // Start is called before the first frame update
    void Start()
    {
        initialize_character();
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time >= 0.5f)
        {
            time = 0;
            animating = true;
        }

        if(animating)
        {
            animation_index = 1 - animation_index;
            switch (animation)
            {
                case 0:
                    this.GetComponent<Image>().sprite = forward_standing[animation_index];
                    break;
                case 1:
                    this.GetComponent<Image>().sprite = forward_walking[animation_index];
                    break;
                case 2:
                    this.GetComponent<Image>().sprite = back_standing[animation_index];
                    break;
                case 3:
                    this.GetComponent<Image>().sprite = back_walking[animation_index];
                    break;
                case 4:
                    this.GetComponent<Image>().sprite = left_standing[animation_index];
                    break;
                case 5:
                    this.GetComponent<Image>().sprite = left_walking[animation_index];
                    break;
                case 6:
                    this.GetComponent<Image>().sprite = right_standing[animation_index];
                    break;
                case 7:
                    this.GetComponent<Image>().sprite = right_walking[animation_index];
                    break;
            }
            animating = false;
        }
            
    }

    public void initialize_character()
    {
        // 스테이지 별로 start_location과 start_vector에 설정된 값에서 시작하기
        switch (stage_num.GetComponent<stage_manager>().stage_num)
        {
            case 0:
                start_vector[0] = new Vector2(0, -1);
                break;
            case 1:
                start_vector[1] = new Vector2(0, -1);
                break;
            case 2:
                start_vector[2] = new Vector2(1, 0);
                break;
        }

        this.transform.position = start_location_obj[stage_num.GetComponent<stage_manager>().stage_num].transform.position;
        cur_vector = start_vector[stage_num.GetComponent<stage_manager>().stage_num];
        change_vector(false);
    }

    public void change_vector(bool iswalking)
    {
        if (cur_vector.x == 1)
        {
            animation = iswalking ? const_right_walking : const_right_standing;
        }
        else if (cur_vector.x == 0 && cur_vector.y == 1)
        {
            animation = iswalking ? const_back_walking : const_back_standing;
        }
        else if (cur_vector.x == 0 && cur_vector.y == -1)
        {
            animation = iswalking ? const_forward_walking : const_forward_standing;
        }
        else if (cur_vector.x == -1)
        {
            animation = iswalking ? const_left_walking : const_left_standing;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Finish")
        {
            switch(stage_num.GetComponent<stage_manager>().stage_num)
            {
                case 0:
                    success();
                    break;
                case 1:
                    stage_num.GetComponent<stage_manager>().ending_animation();
                    //success에 대해 invoke 넣기
                    Invoke("success", 4.5f);
                    this.gameObject.SetActive(true);
                    break;
                case 2:
                    if (coding_board.GetComponent<coding_board>().kit != null && coding_board.GetComponent<coding_board>().kit.activeSelf == false)
                        success();
                    break;
            }
            
        }

        if(other.gameObject.tag == "kit")
        {
            coding_board.GetComponent<coding_board>().iskit = true;
            coding_board.GetComponent<coding_board>().kit = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "kit")
        {
            coding_board.GetComponent<coding_board>().iskit = false;
            coding_board.GetComponent<coding_board>().kit = null;
        }
    }



    public void success()
    {
        int _stage_num = stage_num.GetComponent<stage_manager>().stage_num;
        int lines = coding_board.GetComponent<coding_board>().coded_lines();

        

        score.text = "Lines : ";
        goal.text = "Your goal : ";

        for(int i = 0; i <3; i++)
            stars[i].SetActive(false);

        if (stage_num.GetComponent<stage_manager>().max_stage_index == stage_num.GetComponent<stage_manager>().stage_num && stage_num.GetComponent<stage_manager>().max_stage_index < stage_num.GetComponent<stage_manager>().BG.Length - 1)
            stage_num.GetComponent<stage_manager>().max_stage_index++;

        if (stage_num.GetComponent<stage_manager>().goal_lines_one_star[_stage_num] >= lines)
            stars[0].SetActive(true);
        if (stage_num.GetComponent<stage_manager>().goal_lines_two_star[_stage_num] >= lines)
            stars[1].SetActive(true);
        if (stage_num.GetComponent<stage_manager>().goal_lines_three_star[_stage_num] >= lines)
            stars[2].SetActive(true);

        Image img2 = this.GetComponent<Image>();
        img2.color = new Color(img2.color.r, img2.color.g, img2.color.b, 1);


        score.text += lines.ToString();
        goal.text += stage_num.GetComponent<stage_manager>().goal_lines_three_star[_stage_num].ToString();

        clear_message.SetActive(true);

    }
}
