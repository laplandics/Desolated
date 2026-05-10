using System.Collections;
using Config;
using UnityEngine;
using WorldObject;

namespace Boot
{
    public class SceneBoot : MonoBehaviour
    {
        public IEnumerator Boot(SceneConfig current)
        {
            var stage = GetComponent<StageBase>();
            if (stage != null) stage.BindData(current);
            yield return null;
        }
    }
}