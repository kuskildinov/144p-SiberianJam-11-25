using UnityEngine;

public static class GlobalVars
{
    [Header("Настройки управления")]
    public static KeyCode RunKey = KeyCode.LeftShift;
    public static KeyCode JumpKey = KeyCode.Space;
    public static KeyCode InteractionKeyPrimary = KeyCode.E;
    public static KeyCode InteractionKeySecondary = KeyCode.Mouse0;
    [Header("Сцены")]
    public static string MainMenuSceneName = "MainMenu";
    public static string GameLevelSceneName = "Level_2";
    [Header("Комната игрока")]
    public static bool PlayerLeftRoom = false;
    [Header("Готовность головоломок")]
    public static bool PuzzleOneReady = false;
    public static bool PuzzleTwoReady = false;
    public static bool PuzzleTreeReady = false;
    public static bool PuzzleFourReady = false;
    [Header("Настройки головоломок")]
    public static string CodePanelCode = "4221";    
    [Header("Настройки главной башни")]
    public static float ShowTowerIndicatorsTime = 3f;
    [Header("Настройка камер")]
    public static int PlayerCamPriority = 10;
    public static int MainTowerIndicatorsCamPriority = 11;
}
