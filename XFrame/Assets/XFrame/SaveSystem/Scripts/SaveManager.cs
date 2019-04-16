using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveManager : Singleton<SaveManager>
{
    void Awake()
    {

    }

    private void LoadAutoSave()
    {

        //autoSaveFile = (AutoSave)SaveSystem.Load ("autosave");


        //if (autoSaveFile == null)
        //{
        //	Debug.Log ("Creating new savefile");

        //	autoSaveFile = new AutoSave ();

        //	SaveSystem.Save (autoSaveFile, "autosave");
        //}
    }

    public void SaveDataToFile<T>(T file, string fileName)
        where T : SaveFile
    {
        SaveSystem.Save(file, fileName);
    }
    public SaveFile LoadDataFromFile(string fileName)
    {
        return SaveSystem.Load(fileName);
    }
}
