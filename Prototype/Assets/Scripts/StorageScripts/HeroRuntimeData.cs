using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class HeroRuntimeData
{
    public int heroIndex;
    public int slotIndex;
    public int level;
}

public class RuntimeStorage
{
    #region HeroStorage
    private static string heroPath =  Application.persistentDataPath + "/heros.data";

    public void SavingHero()
    {
        if (File.Exists(heroPath))
        {

        }
        else
            Debug.LogError("File hero storage can't find!");
    }

    public void ReadHeroData()
    {
        if (File.Exists(heroPath))
        {

        }
        else
            Debug.LogError("File hero storage can't find!");
    }

    public void DeleteHeroData()
    {

    }
    #endregion

    #region SystemConfig



    #endregion
}