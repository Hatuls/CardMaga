using Battles;
using Cards;
using UnityEngine;
namespace Rewards
{

    [CreateAssetMenu(fileName = "Reward", menuName = "ScriptableObjects/Rewards/Battle Reward")]
    public class BattleRewardSO :ScriptableObject 
    {
        [SerializeField]
        CharacterTypeEnum _characterDifficultyEnum;
        public CharacterTypeEnum CharacterDifficultyEnum => _characterDifficultyEnum;


        [SerializeField]
        ushort _minMoneyReward;
        [SerializeField]
        ushort _maxMoneyReward;
    
        public ushort MoneyReward { get { return (ushort)Random.Range(_minMoneyReward, _maxMoneyReward); } }

        [SerializeField]
        BattleDropChance _cardChances;    
        [SerializeField]
        BattleDropChance _recipesChances;










        public bool Init(string[] row)
        {
            const int ID = 0;
            const int Gold = 1;
            const int DropChance = 2;
            const int Common = 3;
            const int UnCommon = 4;
            const int Rare = 5;
            const int Epic = 6;
            const int Legendrei = 7;

            const int hasRecipe = 8;

            const int Recipe= 7;


            // character difficultyenum
            if (int.TryParse(row[ID], out int outcome))
                _characterDifficultyEnum = (CharacterTypeEnum)outcome;
            else
                return false;

            //gold
            string[] gold = row[Gold].Split('&');
            for (int i = 0; i < gold.Length; i++)
            {
                if (ushort.TryParse(gold[i], out ushort amount))
                {
                    if (i == 0)
                        _minMoneyReward = amount;
                    else
                        _maxMoneyReward = amount;
                }
            }

            //card Drop Chances

          var dropChances = SplitData(row[DropChance]);
          var commonChanceLevels = SplitData(row[Common]);
          var uncommonChanceLevels = SplitData(row[UnCommon]);
          var rareChanceLevels = SplitData(row[Rare]);
          var epicChanceLevels = SplitData(row[Epic]);
          var legendreiLevels = SplitData(row[Legendrei]);

          _cardChances = new BattleDropChance(dropChances, commonChanceLevels, uncommonChanceLevels, rareChanceLevels, epicChanceLevels, legendreiLevels);


            if (int.TryParse(row[hasRecipe], out int result))
            {
                if (result == 1)
                {
                    dropChances = SplitData(row[DropChance + Recipe]);
                    commonChanceLevels = SplitData(row[Common + Recipe]);
                    uncommonChanceLevels = SplitData(row[UnCommon + Recipe]);
                    rareChanceLevels = SplitData(row[Rare + Recipe]);
                    epicChanceLevels = SplitData(row[Epic + Recipe]);
                    legendreiLevels = SplitData(row[Legendrei + Recipe]);

                    _recipesChances = new BattleDropChance(dropChances, commonChanceLevels, uncommonChanceLevels, rareChanceLevels, epicChanceLevels, legendreiLevels);
                }
            }

            return true;

            byte[] SplitData(string tab)
            {
                string[] data = tab.Split('&');
                byte[] array = new byte[data.Length];

                for (int i = 0; i < data.Length; i++)
                {
                    if (byte.TryParse(data[i], out byte amount))
                        array[i] = amount;
                    else
                        throw new System.Exception(" Could not be converted to number (byte)!\n" + data[i]); ;
                }
                return array;
            }
        }


        public BattleReward CreateReward()
        {
            /*
             *   להגריל 3 מספרים 
             * להוציא את הרמה של הרריתי 
             * להגריל רמה 
             * להוסיף לרשימה
             * 
             * לעשות אותו הדבר גם לCSV
             */

            const byte CardAmount = 3;
            Card[] rewardCards = new Card[CardAmount];

            var cardChances = _cardChances.DropChances;
            byte random;
            var cardFactoryHandler = Factory.GameFactory.Instance.CardFactoryHandler;
            var cardCollection = cardFactoryHandler.CardCollection;

            const int OneHundred = 100;



            for (int i = 0; i < CardAmount; i++)
            {
                int index = -1;
                byte addition = 0;
                random = (byte)Random.Range(0, OneHundred);

                // find  BattleDropChance.RarityDropChance from the random number
                for (int j = 0; j < cardChances.Length; j++)
                {
                    if (j > 0)
                        addition += cardChances[j - 1].DropChance;

                    if (random <  (addition + cardChances[j].DropChance))
                    {
                        index = j;
                        break;
                    }
                }

                if (index == -1)
                    throw new System.Exception("Index Was Not Found - Random Chance was not in any of the rarity chances");


                var rarityCardCollection = cardCollection.GetCardByRarity((RarityEnum)(index + 1));
                
                int randomID = Random.Range(0, rarityCardCollection.CardsID.Length);
                if (randomID >= rarityCardCollection.CardsID.Length)
              Debug.Log($"Rarity is : {(RarityEnum)(index + 1)}\nrarityCardCollection.CardsID.Length = {rarityCardCollection.CardsID.Length}\nRandom ID is : {randomID}");
            
                
                ushort CardId = rarityCardCollection.CardsID[randomID];
                // get cards level;
                var DropChance = _cardChances.DropChances[index];
                var levelChances = DropChance.LevelChances;
                addition = 0;
                random = (byte)Random.Range(0, OneHundred);

                for (int j = 0; j < levelChances.Length; j++)
                {
                    if (j > 0)
                        addition += levelChances[j - 1];

                    if (random < (addition + levelChances[j]))
                    {
                        index = j;
                        break;
                    }
                }
                var card = cardFactoryHandler.CardCollection.GetCard(CardId);
                
                rewardCards[i] = cardFactoryHandler.CreateCard(card, card.CardsMaxLevel > (byte)index ? (byte)0:(byte) index);

                if (rewardCards[i] == null)
                    throw new System.Exception("Card Created is Null!"); 
            }


            BattleReward reward = new BattleReward(MoneyReward, rewardCards);

            return reward;
        } 
        
        
        
        
        
        
        [System.Serializable]
        public class BattleDropChance
        {

        [System.Serializable]
            public class RarityDropChance
            {
                [SerializeField]
                RarityEnum _rarityEnum;
                public RarityEnum RarityEnumType => _rarityEnum;

                [SerializeField]
                byte _dropChance;
                public byte DropChance => _dropChance;

                [SerializeField]
                byte[] _levelChances;
                public byte[] LevelChances => _levelChances;
                public RarityDropChance(RarityEnum rare ,byte chance, byte[] levelChances)
                {
                    _rarityEnum = rare;
                    _dropChance = chance;
                    _levelChances = levelChances;
                }
            }
        [SerializeField]
            RarityDropChance[] _dropChances;

        //[SerializeField] 
        //    byte[] _commonChanceLevels;

        //[SerializeField] 
        //    byte[] _uncommonChanceLevels;

        //[SerializeField] 
        //    byte[] _rareChanceLevels;

        //[SerializeField]
        //    byte[] _epicChanceLevels;

        //[SerializeField]
        //    byte[] _legendreiLevels;

            public RarityDropChance[] DropChances => _dropChances;
            //public byte[] CommonChanceLevels => _commonChanceLevels;
            //public byte[] UncommonChanceLevels => _uncommonChanceLevels;
            //public byte[] RareChanceLevels => _rareChanceLevels;
            //public byte[] EpicChanceLevels => _epicChanceLevels;
            //public byte[] LegendreiLevels => _legendreiLevels;

            public BattleDropChance(params byte[][] chances)
            {
                _dropChances = new RarityDropChance[chances[0].Length];

                for (int i = 0; i < chances[0].Length; i++)
                    _dropChances[i] = new RarityDropChance((RarityEnum)(i+1), chances[0][i], chances[i+1]);
                
             
                //_commonChanceLevels = chances[1];
                //_uncommonChanceLevels = chances[2];
                //_rareChanceLevels = chances[3];
                //_epicChanceLevels = chances[4];
                //_legendreiLevels = chances[5];


            }
        }
   
    }


}

