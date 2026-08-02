using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    static public TutorialManager Instance;

    private int Dialogue_x, Dialogue_y;
    private List<List<string>> Dialogues = new List<List<string>>()
    {
        new List<string>()
        {
            "안녕! 내가 이 게임을 안내해 줄게.",
            "CMYK라는게 익숙하지 않지? 하지만 어려울 거 없어!",
            "내가 말하는 대로 한번 해볼래?"
        },

        new List<string>()
        {
            "이 이미지만 기억하면 다양한 색깔을 만들 수 있어!",
            "미술시간이나 과학시간 때 보던 느낌이지?",
            "우리 새들이 그걸 게임으로 만들었어!",
            "다 읽으면 아무 공간이나 클릭해줘!"
        },

        new List<string>()
        {
            "우선 같이 앵무새 한 마리를 만들어보자!",
            "모르겠더라도 괜찮아. 튜토리얼은 언제든 다시 할 수 있어!",
            "내가 환경설정에 늘 있으니까 안심해.",
            "아까 읽었던 C, M, Y 각각의 앵무새가 보이지?",
            "내가 여기서 어떤 앵무새를 만들지 제시해줄거야!"
        },

        new List<string>()
        {
            "이 앵무새를 같이 만들어볼까?",
            "우선 마젠타 색깔의 앵무새를 클릭해봐!"
        },

        new List<string>()
        {
            "잘했어! 여기서 색깔을 조정할 수 있어.",
            "왼쪽에 버튼들이 많지?"
        },

        new List<string>()
        {
            "각 버튼마다 이 곳을 수정 할 수 있어!",
            "우선 Head1을 터치해봐!"
        },

        new List<string>()
        {
            "좋아! 오른쪽에 게이지와 버튼이 떴지?",
            "이걸로 밝기를 조절할 수 있어.",
            "직접 조작해봐!"
        },

        new List<string>()
        {
            "게이지와 버튼을 조작해봐!"
        },

        new List<string>()
        {
            "잘했어! 지금은 튜토리얼이니까, 정답을 알려줄게.",
            "해보면서 감을 잡아나가보자."
        },

        new List<string>()
        {
            "아주 좋아! 다른 색상의 앵무새들도 이렇게 해서 합치면,",
            "우리가 원하는 색상의 앵무새가 나올거야!",
            "옐로우 색상의 앵무새를 클릭해보자."
        },

        new List<string>()
        {
            "이제 마지막이야! 거의 다왔어!",
            "이제 시안 색상 앵무새를 클릭해보자."
        },

        new List<string>()
        {
            "이제 완성이야! 축하해.",
            "각각의 앵무새들을 합치면?!"
        },

        new List<string>()
        {
            "완성!",
            "첫 앵무새 완성이야!",
            "이런 식으로, 점수를 많이 얻는 게임이야.",
            "자, 이제 진짜 앵무새들을 만들러 떠나보자!"
        },

        new List<string>()
        {
            "여기는 커스텀 앵무새를 제작할 수 있는 곳이야!",
            "자신만의 색깔인 담긴 앵무새를 마음대로 제작할 수 있어.",
            "다 만들면 이름도 지어줄 수 있다구!",
            "게임 플레이하면서 낮은 확률로 등장하기도 해.",
            "너의 창의력을 마음껏 발휘해봐"
        }
    };

    void Awake()
    {
        Instance = this;
    }

    public void TutorialStart()
    {
        TutorialUIManager.Instance.Init();
        GameManager.Instance.StopTime();
        GameUiManager.Instance.DisableEveryThing();


        Dialogue_x = 0;
        Dialogue_y = -1;

        NextDialogue();
    }


    public void NextDialogue()
    {
        {
            int next_x = Dialogue_x;
            int next_y = Dialogue_y + 1;

            if(next_x >= Dialogues.Count)
                return;

            if(Dialogues[next_x].Count <= next_y) // 다음 이벤트로
            {
                next_x += 1;
                next_y = 0;
                CallEvent(next_x);
            }

            Dialogue_x = next_x;
            Dialogue_y = next_y;
        }

        TutorialUIManager.Instance.PrintDialogue(GetDialogue(Dialogue_x, Dialogue_y));
    }

    public void CallEvent(int x)
    {
        switch (x)
        {
            case 1:

            break;

            default:

            break;
        }
    }

    private string GetDialogue(int x, int y)
    {
        if (x < 0 || x >= Dialogues.Count)
            return "배열 크기 초과";

        if (y < 0 || y >= Dialogues[x].Count)
            return "배열 크기 초과";

        return Dialogues[x][y];
    }
}