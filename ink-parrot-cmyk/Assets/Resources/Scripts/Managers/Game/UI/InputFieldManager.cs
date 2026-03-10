using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HangulSafeInputLimit : MonoBehaviour
{
    public TMP_InputField inputField;
    private int maxLength;

    void Start()
    {
        this.maxLength = inputField.characterLimit;

        inputField.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(string text)
    {
        // 한글 조합 중이면 처리하지 않음
        if (!string.IsNullOrEmpty(Input.compositionString))
        return;

        if (text.Length > maxLength)
        {
            inputField.SetTextWithoutNotify(text.Substring(0, maxLength));
            inputField.ForceLabelUpdate();
        }
    }
}