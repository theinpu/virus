using UnityEngine;

public class GameGlobalScript : MonoBehaviour
{

    public const int MaxCells = 5;

	public int PlayerCount = 2;
    public PlayerSetting[] PlayerSetting;
    public GameSettings GameSettings = new GameSettings();

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

    public void InitPlayerSettings()
    {
        PlayerSetting = new PlayerSetting[PlayerCount];
      
        for (var i = 0; i < PlayerCount; i++)
        {
            PlayerSetting[i] = new PlayerSetting();
        }
    }
}