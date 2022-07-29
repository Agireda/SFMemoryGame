using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private readonly Dictionary<EFighterSelection, string> fighterSelectionDirectory = new Dictionary<EFighterSelection, string>();
    private int settings;
    public Settings gameSettings;
    private const int settingsNumber = 2; // Checks if 2 settings have been made so that the game can start
    public static GameSettings Instance;
    private bool muteSounds = false;
    public enum ECardNumber
    {
        NotSet = 0,
        E20Cards = 20,
        E30Cards = 30,
    }
    public enum EFighterSelection
    {
        NotSet,
        Ken,
        Ryu,
    }

    public struct Settings
    {
    // Settings that needs to be set before you can start the game
        public ECardNumber cardNumber;
        public EFighterSelection fighterSelection;
    }

    private void Awake()
    {
        //If there is no instance, don't destroy it on load. If there was already an instance, destroy it.
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        gameSettings = new Settings();
        ResetGameSettings();
        SetFighterSelectionDirectory();
    }

    private void SetFighterSelectionDirectory()
    {
        fighterSelectionDirectory.Add(EFighterSelection.Ryu, "Ryu");
        fighterSelectionDirectory.Add(EFighterSelection.Ken, "Ken");
    }

    public void SetCardNumber(ECardNumber Number)
    {
        //If the amount of cards or the fighter hasn't been selected, the game won't start as there are still settings to be made.
        if (gameSettings.cardNumber == ECardNumber.NotSet)
            settings++;

        gameSettings.cardNumber = Number;
    }
    public void SetFighter(EFighterSelection Fighter)
    {
        //If the amount of cards or the fighter hasn't been selected, the game won't start as there are still settings to be made.
        if (gameSettings.fighterSelection == EFighterSelection.NotSet)
            settings++;
        gameSettings.fighterSelection = Fighter;
    }

    public ECardNumber GetCardNumber()
    {
        return gameSettings.cardNumber;
    }
    public EFighterSelection GetEFighterSelection()
    {
        return gameSettings.fighterSelection;
    }

    public void ResetGameSettings()
    {
        settings = 0;
        gameSettings.fighterSelection = EFighterSelection.NotSet;
        gameSettings.cardNumber = ECardNumber.NotSet;
    }

    public bool AllSettingsReady()
    {
        return settings == settingsNumber;
    }

    public string GetMaterialDirectoryName()
    {
        return "Materials/";
    }

    public string GetFighterSelectionTextureDirectoryName()
    {
        if (fighterSelectionDirectory.ContainsKey(gameSettings.fighterSelection))
        {
            return "GameArt/FighterSelection/" + fighterSelectionDirectory[gameSettings.fighterSelection] + "/";
        }
        else
            Debug.Log("Cannot Get Directory Name");
        return "";
    }

    public void MuteSounds(bool muted)
    {
        muteSounds = muted;
    }

    public bool IsSoundMuted()
    {
        return muteSounds;
    }
}
    

