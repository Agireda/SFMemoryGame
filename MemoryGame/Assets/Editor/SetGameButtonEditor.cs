using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SetGameButton))]
[CanEditMultipleObjects]
[System.Serializable]
public class SetGameButtonEditor : Editor
    //Got help from a Unity Developer for this one. Basically it changes the editor to let me select several different options for the buttons in the Editor.
    //If I add additional enums they too should show up
{
    public override void OnInspectorGUI()
        //This function has to have this exact name, since that's the name Unity has for it's inspector GUI
    {
        DrawDefaultInspector();

        SetGameButton myScript = target as SetGameButton;

        switch (myScript.ButtonType)
        {
            case SetGameButton.EButtonType.CardNumberButton:
                myScript.CardNumber = (GameSettings.ECardNumber)EditorGUILayout.EnumPopup("Number of Cards", myScript.CardNumber);
                break;

            case SetGameButton.EButtonType.FighterButton:
                myScript.Fighter = (GameSettings.EFighterSelection)EditorGUILayout.EnumPopup("Fighter", myScript.Fighter);
                break;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
