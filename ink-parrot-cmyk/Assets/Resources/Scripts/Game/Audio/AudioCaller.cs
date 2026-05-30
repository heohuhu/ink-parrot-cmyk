using UnityEngine;

public class AudioCaller : MonoBehaviour
{
    public void ButtonOnClicked()
    {
        AudioManager.Instance.PlayUI("버튼");
    }

    public void ButtonOnClicked(string Name)
    {
        AudioManager.Instance.Play(Name);
    }

    public void PlayUI(string Name)
    {
        AudioManager.Instance.PlayUI(Name);
    }

    public void PlaySFX(string Name)
    {
        AudioManager.Instance.PlaySFX(Name);
    }
}
