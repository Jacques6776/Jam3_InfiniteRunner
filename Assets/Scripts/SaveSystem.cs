using UnityEngine;
//add using system.io - NOTE this tends to work more on PC
using System.IO;

//static class to create a save system that operates beyond the game being closed
public static class SaveSystem
{
    //generates a Save folder within the game directory
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/Saves/";
    public static readonly string FILE_EXT = ".json";

    public static void Save(string fileName, string dataToSave)
    {
        if (!Directory.Exists(SAVE_FOLDER)) // if directory doesn't exist this will make it
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }

        File.WriteAllText(SAVE_FOLDER + fileName + FILE_EXT, dataToSave); //This will write the save data into the folder
    }

    //loads the data from the folder
    public static string Load (string fileName)
    {
        string fileLocation = SAVE_FOLDER + fileName + FILE_EXT;//string to simplify the file location

        if (File.Exists(fileLocation))
        {
            string loadedData = File.ReadAllText(fileLocation);

            //once loaded it return the data
            return loadedData;
        }
        else //if file doesn't exist it will retunr to create a new file
        {
            return null;
        }
    }
}
