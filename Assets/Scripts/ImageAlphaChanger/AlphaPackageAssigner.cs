using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.ImageAlpha
{
    public class AlphaPackageAssigner : MonoBehaviour
    {
        [SerializeField] private AlphaPackageGroup[] _alphaPackages;

        public void SetAlphas(int alphaID)
        {
            AlphaPackage[] _alphaPackageGroup = GetAlphaPackageFromID(alphaID);
            ImageAlphaHandler.Instance.SetAlpha(_alphaPackageGroup);
        }

        private AlphaPackage[] GetAlphaPackageFromID(int alphaID)
        {
            for (int i = 0; i < _alphaPackages.Length; i++)
            {
                if (_alphaPackages[i].PackageID == alphaID)
                    return _alphaPackages[i].AlphaPackages;
            }

            throw new System.Exception("AlphaPackageAssigner: AlphaPackageGroup Was not found");
        }
    }
    
    [System.Serializable]
    public class AlphaPackageGroup
    {
#if UNITY_EDITOR
        [SerializeField] private string _alphaPackageName;
#endif
       [SerializeField] private int _packageID;
       [SerializeField] private AlphaPackage[] _alphaPackages;

        public AlphaPackage[] AlphaPackages { get { return _alphaPackages; } }
        public int PackageID { get { return _packageID; } }
    }

}
