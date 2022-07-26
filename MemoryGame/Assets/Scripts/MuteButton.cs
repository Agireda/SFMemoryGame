using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public Sprite unmutedSoundsSprite;
    public Sprite mutedSoundsSprite;

    private Button button;
    SpriteState state;
    void Start()
    {
        button = GetComponent<Button>();

        if (GameSettings.Instance.IsSoundMuted())
        {
            state.pressedSprite = mutedSoundsSprite;
            state.highlightedSprite = mutedSoundsSprite;
            button.GetComponent<Image>().sprite = mutedSoundsSprite;
        }
        else
        {
            state.pressedSprite = unmutedSoundsSprite;
            state.highlightedSprite = unmutedSoundsSprite;
            button.GetComponent<Image>().sprite = unmutedSoundsSprite;
        }
    }

    private void OnGUI()
    {
        if(GameSettings.Instance.IsSoundMuted())
        {
            button.GetComponent<Image>().sprite = mutedSoundsSprite;
        }
        else
        {
            button.GetComponent<Image>().sprite = unmutedSoundsSprite;
        }
    }

    public void ToggleSoundsIcon()
    {
        if(GameSettings.Instance.IsSoundMuted())
        {
            state.pressedSprite = unmutedSoundsSprite;
            state.highlightedSprite = unmutedSoundsSprite;
            GameSettings.Instance.MuteSounds(false);
        }
        else
        {
            state.pressedSprite = mutedSoundsSprite;
            state.highlightedSprite = mutedSoundsSprite;
            GameSettings.Instance.MuteSounds(true);
        }

        button.spriteState = state;
    }
}
