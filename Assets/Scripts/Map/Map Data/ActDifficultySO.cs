using UnityEngine;
using Rewards;
namespace Map
{
    public class ActDifficultySO : ScriptableObject
    {
        [SerializeField]
        private ActsEnum _act;
        public ActsEnum Act { get => _act; private set => _act = value; }
        [SerializeField]
        NodeLevel[] _nodeLevels;
      public NodeLevel this[int index]
        => _nodeLevels[index];
        


        [System.Serializable]
        public class NodeLevel
        {
            [SerializeField]
            byte _minDiffculty;
            [SerializeField]
            byte _maxDiffculty;

            public NodeLevel(byte maxDiffculty, byte minDiffculty)
            {
                _maxDiffculty = maxDiffculty;
                _minDiffculty = minDiffculty;
            
            }

            public byte MinDiffculty { get => _minDiffculty;}
            public byte MaxDiffculty { get => _maxDiffculty;}
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
                string[] minMaxString = row[i + Floor].Split('^');

                if (byte.TryParse(minMaxString[0], out byte min) == false)
                    throw new System.Exception($"ActDiffucltySO: Min value is not a valid number\n Act: {_act}\nValue: {minMaxString[0]}");

                if (byte.TryParse(minMaxString[1], out byte max) == false)
                    throw new System.Exception($"ActDiffucltySO: Max value is not a valid number\n Act: {_act}\nValue: {minMaxString[1]}");


                _nodeLevels[i] = new NodeLevel(max, min);
            }
        }
#endif
    }



}