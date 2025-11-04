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
    [Header("Готовность головоломок")]
    public static bool PuzzleOneReady = false;
    public static bool PuzzleTwoReady = false;
    public static bool PuzzleTreeReady = false;
    [Header("Настройки головоломок")]
    public static string CodePanelCode = "1234";
}
