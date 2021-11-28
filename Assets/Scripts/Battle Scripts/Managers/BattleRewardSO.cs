using Battles;
using Cards;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Rewards
{
    public enum ResourceEnum
    {
        None = 0,
        Credits = 1,
        EXP = 2,
        Diamonds = 3,
        Gold = 4,
        Energy = 5,
        Chips = 65,
    }
    public enum ActsEnum
    {
        None = 0,
        ActOne = 1,
        ActTwo = 2,
        ActThree = 3
    }
    [CreateAssetMenu(fileName = "Reward", menuName = "ScriptableObjects/Rewards/Battle Reward")]
    public class BattleRewardSO : ScriptableObject
    {
        [SerializeField]
        CharacterTypeEnum _characterDifficultyEnum;
        public CharacterTypeEnum CharacterDifficultyEnum => _characterDifficultyEnum;



        [SerializeField]
        BattleDropChance[] _cardChances;
        [SerializeField]
        BattleDropChance[] _recipesChances;

        [SerializeField]
        List<CurrencyRewardAmount> _rewardTypes;

        [System.Serializable]
        public class CurrencyRewardAmount
        {

            [SerializeField]
            ResourceEnum _currencyType;

            [SerializeField]
            ushort[] _minValue;
            [SerializeField]
            ushort[] _maxValue;
            public ResourceEnum CurrencyType => _currencyType;

            public CurrencyRewardAmount(ushort[] minValue, ushort[] maxValue, ResourceEnum currencyType)
            {
                _minValue = minValue;
                _maxValue = maxValue;
                _currencyType = currencyType;

            }
            public ushort RandomizeAmount(ActsEnum _actType)
            {
                int index = (int)_actType;
                return (ushort)Random.Range(_minValue[index], _maxValue[index]);
            }
        }

        public ushort RandomAmount(ResourceEnum currencyEnum, ActsEnum _actType)
        {
            for (int i = 0; i < _rewardTypes.Count; i++)
            {
                if (_rewardTypes[i].CurrencyType == currencyEnum)
                    return _rewardTypes[i].RandomizeAmount(_actType);
            }
            throw new System.Exception($"BattleRewardSO: CurrencyEnum is not a valid type {currencyEnum}!");
        }

        public BattleReward CreateReward(ActsEnum actsEnum)
        {
            Card[] rewardCards = GenerateCardsRewards(actsEnum);

            Combo.Combo[] combo = null;
            if (_characterDifficultyEnum >= CharacterTypeEnum.Elite_Enemy)
            {
                combo = GenerateComboReward(actsEnum);

            }

            ushort EXP = RandomAmount(ResourceEnum.EXP, actsEnum);
            ushort Credits = RandomAmount(ResourceEnum.Credits, actsEnum);
            ushort Gold = RandomAmount(ResourceEnum.Gold, actsEnum);
            ushort Diamonds = RandomAmount(ResourceEnum.Diamonds, actsEnum);


            BattleReward reward = new BattleReward(Credits, EXP, Gold, Diamonds, rewardCards, combo);

            return reward;
        }

        private Card[] GenerateCardsRewards(ActsEnum actsEnum)
        {
            var actCardChance = _cardChances.First(x => x.ActEnum == actsEnum);
            const byte CardAmount = 3;
            Card[] rewardCards = new Card[CardAmount];

            byte random;
            var cardFactoryHandler = Factory.GameFactory.Instance.CardFactoryHandler;

            var cardCollection = cardFactoryHandler.CardCollection;
            var cardChances = actCardChance.DropChances;

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

                    if (random < (addition + cardChances[j].DropChance))
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

                if (randomID >= rarityCardCollection.CardsID.Length)
                    throw new System.Exception(
                        $"BattleRewardSO: CardID Was bigger than the reward collection for: {rarityCardCollection.CardsID.Length}\nCardID: {randomID}\n Rarity: {(RarityEnum)(index + 1)}");
                ushort CardId = rarityCardCollection.CardsID[randomID];
                // get cards level;
                var DropChance = actCardChance.DropChances[index];
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

                rewardCards[i] = cardFactoryHandler.CreateCard(card, (card.CardsMaxLevel < (byte)index) ? (byte)(card.CardsMaxLevel - 1) : (byte)index);


                if (rewardCards[i] == null)
                    throw new System.Exception("Card Created is Null!");
            }

            return rewardCards;
        }

        private Combo.Combo[] GenerateComboReward(ActsEnum actsEnum)
        {
            Combo.Combo[] combo = new Combo.Combo[1];
            int tryTimes = 6;
            var recipesChances = _recipesChances.First(x => x.ActEnum == actsEnum);
            int addition = 0;
            int random;
            int OneHundred = 100;
            var playerCombo = BattleData.Player.CharacterData.ComboRecipe.ToList();
            var comboIDs = playerCombo.Select(x => new { ID = x.ComboSO.ID });
            var comboFactoryHandler = Factory.GameFactory.Instance.ComboFactoryHandler;
            var comboCollection = comboFactoryHandler.ComboCollection;
            var comboChances = recipesChances.DropChances;
            int index = -1;
            ushort id=0;
            do
            {
                if (tryTimes <= 0)
                {
                    combo = null;
                    break;
                }
                tryTimes--;

                do
                {


                    addition = 0;
                    random = Random.Range(0, OneHundred);


                    for (int j = 0; j < comboChances.Length; j++)
                    {
                        if (j > 0)
                            addition += comboChances[j - 1].DropChance;

                        if (random <= (addition + comboChances[j].DropChance))
                        {
                            index = j;
                            break;
                        }
                    }

                    if (index == -1)
                        throw new System.Exception("Index Was Not Found - Random Chance was not in any of the rarity chances");

                   
                } while (comboCollection.GetComboByRarity((RarityEnum)(index + 1)).ComboID.Length == 0);
                var rarityComboCollection = comboCollection.GetComboByRarity((RarityEnum)(index + 1));


                ushort[] playerID = new ushort[comboIDs.Count()];
                for (int i = 0; i < playerID.Length; i++)
                    playerID[i] = comboIDs.ElementAt(i).ID;

                playerID.ToList();
                var comboID = rarityComboCollection.ComboID.Union(playerID).Distinct();
                comboID = comboID.Where((x) => !playerID.Contains(x));

                if (comboID.Count() == 0)
                    continue;
                 id = comboID.ElementAt(Random.Range(0, comboID.Count()));



            } while (playerCombo.Contains(combo[0]) || (combo[0] == null && tryTimes>0));



            var DropChance = recipesChances.DropChances[index];
            var levelChances = DropChance.LevelChances;

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
            if (id>0)
            combo[0] = comboFactoryHandler.CreateCombo(comboFactoryHandler.ComboCollection.GetCombo(id), (byte)index);


            if (combo[0] == null)
            {
                Debug.Log("Tried Few Times But There Was No Combo!");

            }

            return combo;
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
                public RarityDropChance(RarityEnum rare, byte chance, byte[] levelChances)
                {
                    _rarityEnum = rare;
                    _dropChance = chance;
                    _levelChances = levelChances;
                }
              
            }
            [SerializeField]
            RarityDropChance[] _dropChances;

            [SerializeField]
            private ActsEnum _act;

            public RarityDropChance[] DropChances => _dropChances;
            public ActsEnum ActEnum => _act;
            public BattleDropChance(ActsEnum act, params byte[][] chances)
            {
                _dropChances = new RarityDropChance[chances[0].Length];

                for (int i = 0; i < chances[0].Length; i++)
                    _dropChances[i] = new RarityDropChance((RarityEnum)(i + 1), chances[0][i], chances[i + 1]);

                _act = act;
                //_commonChanceLevels = chances[1];
                //_uncommonChanceLevels = chances[2];
                //_rareChanceLevels = chances[3];
                //_epicChanceLevels = chances[4];
                //_legendreiLevels = chances[5];


            }
        }



        public bool Init(string[] row)
        {
            const int ID = 0;
            const int Credits = 1;

            const int DropChance = 5;
            const int Common = 6;
            const int UnCommon = 7;
            const int Rare = 8;
            const int Epic = 9;
            const int Legendrei = 10;

            const int hasRecipe = 11;

            const int Recipe = 7;


            // character difficultyenum
            if (int.TryParse(row[ID], out int outcome))
                _characterDifficultyEnum = (CharacterTypeEnum)outcome;
            else
                return false;

            //gold
            List<CurrencyRewardAmount> _currencyList = new List<CurrencyRewardAmount>();
            for (int i = 0; i < 4; i++)
            {
                string[] amount = row[Credits + i].Split('^');
                List<ushort> minAmount = new List<ushort>();
                List<ushort> maxAmount = new List<ushort>();
                for (int j = 1; j <= amount.Length; j++)
                {
                    amount[j - 1].Trim();
                    string[] perActs = amount[j - 1].Split('&');

                    if (ushort.TryParse(perActs[0], out ushort min))
                    {
                        minAmount.Add(min);
                    }
                    else
                        throw new System.Exception($"Battle RewardSO: Min Value is not a valid number {(ResourceEnum)Credits + i} -  {perActs[0]}");

                    if (ushort.TryParse(perActs[1], out ushort max))
                    {
                        maxAmount.Add(max);
                    }
                    else
                        throw new System.Exception($"Battle RewardSO: Max Value is not a valid number {(ResourceEnum)Credits + i} -  {perActs[1]}");

                }
                _currencyList.Add(new CurrencyRewardAmount(minAmount.ToArray(), maxAmount.ToArray(), (ResourceEnum)Credits + i));
            }

            _rewardTypes = _currencyList;



            //card Drop Chances
            List<BattleDropChance> _battleDropChacnes = new List<BattleDropChance>();
            {
                string[] dropChanceString = row[DropChance].Split('^');
                string[] commonChanceLevelsString = row[Common].Split('^');
                string[] uncommonChanceLevelsString = row[UnCommon].Split('^');
                string[] rareChanceLevelsString = row[Rare].Split('^');
                string[] epicChanceLevelsString = row[Epic].Split('^');
                string[] legendreiLevelsString = row[Legendrei].Split('^');

                for (int i = 0; i < dropChanceString.Length; i++)
                {

                    var dropChances = SplitData(dropChanceString[i]);
                    var commonChanceLevels = SplitData(commonChanceLevelsString[i]);
                    var uncommonChanceLevels = SplitData(uncommonChanceLevelsString[i]);
                    var rareChanceLevels = SplitData(rareChanceLevelsString[i]);
                    var epicChanceLevels = SplitData(epicChanceLevelsString[i]);
                    var legendreiLevels = SplitData(legendreiLevelsString[i]);

                    _battleDropChacnes.Add(new BattleDropChance((ActsEnum)(i + 1), dropChances, commonChanceLevels, uncommonChanceLevels, rareChanceLevels, epicChanceLevels, legendreiLevels));
                }
                _cardChances = _battleDropChacnes.ToArray();
            }

            if (int.TryParse(row[hasRecipe], out int result))
            {
                if (result == 1)
                {
                    _battleDropChacnes = new List<BattleDropChance>();
                    string[] dropChanceString = row[DropChance + Recipe].Split('^');
                    string[] commonChanceLevelsString = row[Common + Recipe].Split('^');
                    string[] uncommonChanceLevelsString = row[UnCommon + Recipe].Split('^');
                    string[] rareChanceLevelsString = row[Rare + Recipe].Split('^');
                    string[] epicChanceLevelsString = row[Epic + Recipe].Split('^');
                    string[] legendreiLevelsString = row[Legendrei + Recipe].Split('^');

                    for (int i = 0; i < dropChanceString.Length; i++)
                    {
                        var dropChances = SplitData(dropChanceString[i]);
                        var commonChanceLevels = SplitData(commonChanceLevelsString[i]);
                        var uncommonChanceLevels = SplitData(uncommonChanceLevelsString[i]);
                        var rareChanceLevels = SplitData(rareChanceLevelsString[i]);
                        var epicChanceLevels = SplitData(epicChanceLevelsString[i]);
                        var legendreiLevels = SplitData(legendreiLevelsString[i]);

                        _battleDropChacnes.Add(new BattleDropChance((ActsEnum)(i + 1), dropChances, commonChanceLevels, uncommonChanceLevels, rareChanceLevels, epicChanceLevels, legendreiLevels));
                    }
                    _recipesChances = _battleDropChacnes.ToArray();
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


    }


}

