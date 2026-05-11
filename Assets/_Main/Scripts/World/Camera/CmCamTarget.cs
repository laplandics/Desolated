using ObservableCollections;
using R3;
using UnityEngine;
using Utils;
using WorldObject;

public class CmCamTarget : MonoBehaviour
{
    private void Awake()
    {
        G.Resolve<Entities>().Storage.ObserveAll.ObserveAdd().Subscribe(e =>
        { if (e.Value.Key.Contains("Player")) GoToPlayer(e.Value.Value); });
    }

    private void GoToPlayer(Entity playerEntity)
    {
        transform.position = playerEntity.transform.position;
    }
}
