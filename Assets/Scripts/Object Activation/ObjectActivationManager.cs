using CardMaga.Battle.Players;
using UnityEngine;
namespace CardMaga.UI.Visuals
{

    public class ObjectActivationManager : MonoBehaviour
    {
        [SerializeField]
        private ActivatedObject[] _activatedObjects;

        private ActivatedObject _currentObject;
        public void Activate(TagSO tagSO)
        {
            if (_currentObject != null)
                _currentObject.DeActivate();

            _currentObject = GetObject(tagSO);

            _currentObject.Activate();
        }

        private ActivatedObject GetObject(TagSO tagSO)
        {
            for (int i = 0; i < _activatedObjects.Length; i++)
            {
                if (_activatedObjects[i].TagSO == tagSO)
                    return _activatedObjects[i];
            }
            throw new System.Exception("Tag was not found!\nTag Name: " + tagSO.name);
        }

        public void CloseAll()
        {
            for (int i = 0; i < _activatedObjects.Length; i++)
                _activatedObjects[i].DeActivate();
        }

        private void OnDestroy()
        {
            CloseAll();
        }
    }



    [System.Serializable]
    public class ActivatedObject
    {
        public TagSO TagSO;
        [SerializeField]
        private GameObject GameObject;


        public void Activate() => GameObject.SetActive(true);
        public void DeActivate() => GameObject.SetActive(false);
    }
}