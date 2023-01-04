using CardMaga.Battle.Players;
using CardMaga.Tools.Pools;
using Sirenix.OdinInspector;
using UnityEngine;
namespace CardMaga.ObjectPool
{
    [CreateAssetMenu(fileName = "New BasePool SO", menuName = "ScriptableObjects/Pool/New Base Pool SO")]
    public class BasePoolSO<T> : TagSO where T : MonoBehaviour, ITaggable, IPoolableMB<T>
    {
        [PreviewField(100f)]
        [SerializeField]
        private T _poolPrefab;

        public T PullPrefab => _poolPrefab;
    }
}