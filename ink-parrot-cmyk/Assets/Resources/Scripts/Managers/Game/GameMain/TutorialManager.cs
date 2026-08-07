using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    static public TutorialManager Instance;

    private int Dialogue_x, Dialogue_y;
    public string current_question = "none";
    public bool is_event_fulfilled = false;

    public List<GameObject> ActivateTarget = new List<GameObject>();

    private List<List<string>> Dialogues = new List<List<string>>()
    {
        new List<string>() //0
        {
            "안녕! 내가 이 게임을 안내해 줄게.",
            "CMYK라는게 익숙하지 않지? 하지만 어려울 거 없어!",
            "내가 말하는 대로 한번 해볼래?"
        },

        new List<string>() //1
        {
            "이 이미지만 기억하면 다양한 색깔을 만들 수 있어!",
            "미술시간이나 과학시간 때 보던 느낌이지?",
            "우리 새들이 그걸 게임으로 만들었어!",
            "다 읽으면 아무 공간이나 클릭해줘!"
        },

        new List<string>() //2
        {
            "우선 같이 앵무새 한 마리를 만들어보자!",
            "모르겠더라도 괜찮아. 튜토리얼은 언제든 다시 할 수 있어!",
            "내가 환경설정에 늘 있으니까 안심해.",
            "아까 읽었던 C, M, Y 각각의 앵무새가 보이지?",
            "내가 여기서 어떤 앵무새를 만들지 제시해줄거야!"
        },

        new List<string>() //3
        {
            "이 앵무새를 같이 만들어볼까?",
            "우선 마젠타 색깔의 앵무새를 클릭해봐!"
        },

        new List<string>() //4
        {
            "",
            "잘했어! 여기서 색깔을 조정할 수 있어.",
            "왼쪽에 버튼들이 많지?"
        },

        new List<string>() //5
        {
            "각 버튼마다 이 곳을 수정 할 수 있어!",
            "우선 Head1을 터치해봐!"
        },

        new List<string>() //6
        {
            "",
            "좋아! 오른쪽에 게이지와 버튼이 떴지?",
            "이걸로 밝기를 조절할 수 있어.",
            "직접 조작해봐!"
        },

        new List<string>() //7
        {
            "",
            "잘했어! 지금은 튜토리얼이니까, 정답을 알려줄게.",
            "해보면서 감을 잡아나가보자."
        },

        new List<string>() //8
        {
            "Head1 : 0"
        },

        new List<string>()
        {
            "Head2 : 3"
        },

        new List<string>()
        {
            "Head3 : 3"
        },

        new List<string>()
        {
            "Body1 : 3"
        },

        new List<string>()
        {
            "Body2 : 3"
        },

        new List<string>()
        {
            "Wing1 : 0"
        },

        new List<string>() //14
        {
            "Wing2 : 0"
        },

        new List<string>() //15
        {
            "아주 좋아! 다른 색상의 앵무새들도 이렇게 해서 합치면,",
            "우리가 원하는 색상의 앵무새가 나올거야!",
            "옐로우 색상의 앵무새를 클릭해보자."
        },

        new List<string>() //16
        {
            ""
        },

        new  List<string>() //17
        {
            "Head1 : 3"
        },

        new  List<string>()
        {
            "Head2 : 0"
        },

        new  List<string>()
        {
            "Head3 : 3"
        },

        new  List<string>()
        {
            "Body1 : 3"
        },

        new  List<string>()
        {
            "Body2 : 0"
        },

        new  List<string>()
        {
            "Wing1 : 3"
        },

        new  List<string>() //23
        {
            "Wing2 : 3"
        },

        new List<string>() //24
        {
            "이제 마지막이야! 거의 다왔어!",
            "이제 시안 색상 앵무새를 클릭해보자."
        },

        new List<string>() //25
        {
            ""
        },

        new  List<string>() //26
        {
            "Head1 : 3"
        },

        new  List<string>()
        {
            "Head2 : 3"
        },

        new  List<string>()
        {
            "Head3 : 0"
        },

        new  List<string>()
        {
            "Body1 : 3"
        },

        new  List<string>()
        {
            "Body2 : 0"
        },

        new  List<string>()
        {
            "Wing1 : 3"
        },

        new  List<string>() //32
        {
            "Wing2 : 3"
        },

        new List<string>() //33
        {
            "이제 완성이야! 축하해.",
            "각각의 앵무새들을 합치면?!"
        },

        new List<string>() //34 - 여기에서 정답 제출을 처리하고 다음 텍스트를 출력하기 위해 대기합니다.
        {
            ""  
        },

        new List<string>() //35
        {
            "완성!",
            "첫 앵무새 완성이야!",
            "이런 식으로, 점수를 많이 얻는 게임이야.",
            "자, 이제 진짜 앵무새들을 만들러 떠나보자!"
        },

        new List<string>() //36 종료
        {
            ""
        }

        //new List<string>() //
        //{
        //    "여기는 커스텀 앵무새를 제작할 수 있는 곳이야!",
        //    "자신만의 색깔인 담긴 앵무새를 마음대로 제작할 수 있어.",
        //    "다 만들면 이름도 지어줄 수 있다구!",
        //    "게임 플레이하면서 낮은 확률로 등장하기도 해.",
        //    "너의 창의력을 마음껏 발휘해봐"
        //}
    };

    void Awake()
    {
        Instance = this;
    }

    public void TutorialStart()
    {
        TutorialUIManager.Instance.Init();
        //GameManager.Instance.StopTime();
        //GameUiManager.Instance.DisableEveryThing();
        //HighlightManager.Instance.AddException(TutorialUIManager.Instance.TutorialCanvas);
        

        Dialogue_x = 0;
        Dialogue_y = -1;
        is_event_fulfilled = true;
        current_question = "none";

        NextDialogue();
    }

    private Coroutine coroutine_event = null;
    public void NextDialogue()
    {
        if(is_event_fulfilled == false || coroutine_event != null)
            return;
        {
            int next_x = Dialogue_x;
            int next_y = Dialogue_y + 1;

            if(next_x >= Dialogues.Count)
                return;

            if(Dialogues[next_x].Count <= next_y) // 다음 이벤트로
            {
                next_x += 1;
                next_y = 0;
            }

            Dialogue_x = next_x;
            Dialogue_y = next_y;
        }

        coroutine_event = StartCoroutine(PlayEvent(Dialogue_x, Dialogue_y));
    }

    private IEnumerator PlayEvent(int x, int y)
    {
        yield return CallEvent(x, y);
        yield return TutorialUIManager.Instance.PrintDialogue(GetDialogue(x, y));
        coroutine_event = null;
    }

    private IEnumerator CallEvent(int x, int y)
    {
        string text = x.ToString() + "-" + y.ToString();
        switch (text)
        {
            case "1-0":
                //ActivateTarget[0].SetActive(true);

            break;

            case "2-0":
                //ActivateTarget[0].SetActive(false);

            break;

            case "3-0":
                //로리킷 앵무새 제시
                AnswerSheet.Instance.MakeAnswer(6);
            break;

            case "4-0":
                current_question = "Color-" + ((int)Constants.ColorType.Magenta).ToString();
                is_event_fulfilled = false;

                //하이라이트 조절
            break;

            case "5-0":
                //ActivateTarget[1].SetActive(true); // 버튼 아트 에셋 표시
            break;

            case "6-0":
                current_question = "Template-0";
                is_event_fulfilled = false;
            break;
            
            case "7-0":
                current_question = "색상조작-0-0";
                is_event_fulfilled = false;
            break;

            case "8-0":
            case "9-0":
            case "10-0":
            case "11-0":
            case "12-0":
            case "13-0":
            case "14-0":

                TutorialUIManager.Instance.SetDialogueButton(false);
                current_question = "Answer-" + (x - 8).ToString() + "-" + Dialogues[x][0].Split(" ")[2];
                is_event_fulfilled = false;

            break;

            case "15-0":
                TutorialUIManager.Instance.SetDialogueButton(true);
            break;

            case "15-1":
                current_question = "tmp";
                GameManager.Instance.unSelectColor();
                current_question = "none";
            break;

            case "16-0":
                current_question = "Color-" + ((int)Constants.ColorType.Yellow).ToString();
                is_event_fulfilled = false;
            break;

            case "17-0":
            case "18-0":
            case "19-0":
            case "20-0":
            case "21-0":
            case "22-0":
            case "23-0":

                TutorialUIManager.Instance.SetDialogueButton(false);
                current_question = "Answer-" + (x - 17).ToString() + "-" + Dialogues[x][0].Split(" ")[2];
                is_event_fulfilled = false;

            break;

            case "24-0":
                TutorialUIManager.Instance.SetDialogueButton(true);
            break;

            case "24-1":
                current_question = "tmp";
                GameManager.Instance.unSelectColor();
                current_question = "none";
            break;

            case "25-0":
                current_question = "Color-" + ((int)Constants.ColorType.Cyan).ToString();
                is_event_fulfilled = false;
            break;

            case "26-0":
            case "27-0":
            case "28-0":
            case "29-0":
            case "30-0":
            case "31-0":
            case "32-0":

                TutorialUIManager.Instance.SetDialogueButton(false);
                current_question = "Answer-" + (x - 26).ToString() + "-" + Dialogues[x][0].Split(" ")[2];
                is_event_fulfilled = false;

            break;

            case "33-0":
                TutorialUIManager.Instance.SetDialogueButton(true);
            break;

            case "33-1":
                current_question = "tmp";
                GameManager.Instance.unSelectColor();
                current_question = "none";
            break;

            case "34-0":
                while(GameManager.Instance.processing == 1)
                    yield return null;
                current_question = "tmp";
                GameManager.Instance.AnswerSubmit();
                current_question = "none";
                yield return new WaitForSeconds(2f);
            break;

            case "36-0":
                SettingManager.Instance.setting.isTutorial = false;
                SettingManager.Instance.SettingSave();
                GameManager.Instance.ReturnStartMenu();
            break;

            default:

            break;
        }

        yield return null;
    }

    private string GetDialogue(int x, int y)
    {
        if (x < 0 || x >= Dialogues.Count)
            return "배열 크기 초과";

        if (y < 0 || y >= Dialogues[x].Count)
            return "배열 크기 초과";

        return Dialogues[x][y];
    }

    public void ButtonClicked()
    {

        NextDialogue();
    }
}