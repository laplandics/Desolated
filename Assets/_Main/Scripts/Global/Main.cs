using Config;
using Constant;
using R3;
using UnityEngine;

public static class Main
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Reset()
    {
        CurrentMainCamera = new ReactiveProperty<Camera>();
        CurrentSceneConfig = new ReactiveProperty<SceneConfig>();
        CurrentPlayerConfig = new ReactiveProperty<PlayerConfig>();
        CurrentEncounterState = new ReactiveProperty<EncounterStates>();
    }
    
    public static ReactiveProperty<Camera> CurrentMainCamera { get; private set; }
    public static ReactiveProperty<SceneConfig> CurrentSceneConfig { get; private set; }
    public static ReactiveProperty<PlayerConfig> CurrentPlayerConfig { get; private set; }
    public static ReactiveProperty<EncounterStates> CurrentEncounterState { get; private set; }
}