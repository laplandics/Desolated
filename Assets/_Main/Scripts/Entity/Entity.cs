using NaughtyAttributes;
using UnityEngine;

namespace Entity
{
    public class Entity : MonoBehaviour
    {
        [ReadOnly] public string id;

        public void BindData(Data.Proxy.Entity entity)
        {
            transform.position = entity.Position.Value;
            transform.rotation = Quaternion.Euler(entity.Rotation.Value);
        }
    }
}