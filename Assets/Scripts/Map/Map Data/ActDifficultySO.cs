using Rewards;
using UnityEngine;
namespace CardMaga
{
    [CreateAssetMenu(fileName = "Act Difficulty SO", menuName = "ScriptableObjects/Map/Act Difficulty")]
    public class ActDifficultySO : ScriptableObject
    {
        [SerializeField]
        private ActsEnum _act;
        public ActsEnum Act { get => _act; private set => _act = value; }
        [SerializeField]
        NodeLevel[] _nodeLevels;
        public NodeLevel this[int index]
        {
            get
            {
                return _nodeLevels[index];

            }
        }



        [System.Serializable]
        public class NodeLevel
        {
            [SerializeField]
            MinMaxRatio[] _minMaxCharacters;
            public NodeLevel(params MinMaxRatio[] minMaxRatios)
            {
                _minMaxCharacters = minMaxRatios;

            }

            public MinMaxRatio[] MinMaxCharacters { get => _minMaxCharacters; }

            [System.Serializable]
            public class MinMaxRatio
            {

                [SerializeField]
                byte _minDiffculty;

                public MinMaxRatio(byte minDiffculty, byte maxDiffculty)
                {
                    _minDiffculty = minDiffculty;
                    _maxDiffculty = maxDiffculty;
                }

                [SerializeField]
                byte _maxDiffculty;
                public byte MinDiffculty { get => _minDiffculty; }
                public byte MaxDiffculty { get => _maxDiffculty; }
            }
        }


#if UNITY_EDITOR
        public void Init(string[] row)
        {
            const int ActEnumIndex = 0;
            const int Floor = 1;
            const int MaxFloor = 13;

            if (int.TryParse(row[ActEnumIndex], out int actNum))
                _act = (ActsEnum)actNum;
            else
                throw new System.Exception($"Act DiffucltySO: Act number is not a valid number!");

            _nodeLevels = new NodeLevel[MaxFloor];
            for (int i = 0; i < MaxFloor; i++)
            {

                string[] basicBoss = row[i + Floor].Split('&');
                int count = basicBoss.Length == 0 ? 1 : basicBoss.Length;

                NodeLevel.MinMaxRatio[] ratio = new NodeLevel.MinMaxRatio[count];
                for (int j = 0; j < count; j++)
                {
                    string[] minMaxString = basicBoss[j].Split('^');

                    if (byte.TryParse(minMaxString[0], out byte min) == false)
                        throw new System.Exception($"ActDiffucltySO: Min value is not a valid number\n Act: {_act}\nValue: {minMaxString[0]}");

                    if (byte.TryParse(minMaxString[1], out byte max) == false)
                        throw new System.Exception($"ActDiffucltySO: Max value is not a valid number\n Act: {_act}\nValue: {minMaxString[1]}\nCoulmne: {i+1}");
                    ratio[j] = new NodeLevel.MinMaxRatio(min, max);
                }

                _nodeLevels[i] = new NodeLevel(ratio);
            }
        }
#endif
    }



}