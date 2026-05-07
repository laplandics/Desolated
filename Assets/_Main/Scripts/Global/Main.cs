using Boot;
using Constant;
using R3;
using UnityEngine;

public static class Main
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Reset()
    {
        CurrentMainCamera = new ReactiveProperty<Camera>();
        CurrentSceneParams = new ReactiveProperty<SceneParams>();
        CurrentEncounterPhase = new ReactiveProperty<EncounterPhases>();
    }
    
    public static ReactiveProperty<Camera> CurrentMainCamera { get; private set; }
    public static ReactiveProperty<SceneParams> CurrentSceneParams { get; private set; }
    public static ReactiveProperty<EncounterPhases> CurrentEncounterPhase { get; private set; }
}