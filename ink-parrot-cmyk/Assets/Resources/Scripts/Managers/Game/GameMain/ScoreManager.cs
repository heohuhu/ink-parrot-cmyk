using TMPro;
using UnityEngine;
public class ScoreManager : MonoBehaviour
{
    static public ScoreManager Instance;
    public int score = 0, combo = 0;
    public TextMeshProUGUI ScoreText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ScoreUpdate();
    }

    void GetScore(int num)
    {
        score += num;
        ScoreUpdate();
    }

    public void ScoreUpdate()
    {
        ScoreText.text = "Score\n" + score.ToString();
    }
}
