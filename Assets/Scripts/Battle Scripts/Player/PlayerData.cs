
using UnityEngine;

public class PlayerData 
{
    private static PlayerData _instance;

    public static PlayerData Instance
    {
        get
        {
            if (_instance == null)
                _instance = new PlayerData();

            return _instance;
        }
    }
    public PlayerData()
    {







    }
    



}
