using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetGameButton : MonoBehaviour
{
    public enum EButtonType
    {
        NotSet,
        CardNumberButton,
        FighterButton,
    };
    [SerializeField] public EButtonType ButtonType = EButtonType.NotSet;
    [HideInInspector] public GameSettings.ECardNumber CardNumber = GameSettings.ECardNumber.NotSet;
    [HideInInspector] public GameSettings.EFighterSelection Fighter = GameSettings.EFighterSelection.NotSet;
    
    public void SetGameOption(string GameSceneName)
    {
     //Checks that all settings have been made, if they have, change scene to Game
        var comp = gameObject.GetComponent<SetGameButton>();

        switch (comp.ButtonType)
        {
            case SetGameButton.EButtonType.CardNumberButton:
                GameSettings.Instance.SetCardNumber(comp.CardNumber);
                break;
            case SetGameButton.EButtonType.FighterButton:
                GameSettings.Instance.SetFighter(comp.Fighter);
                break;
        }

        if(GameSettings.Instance.AllSettingsReady())
        {
            SceneManager.LoadScene(GameSceneName);
        }
    }
}
