//using Battle;
//using CardMaga.Card;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//namespace Rewards
//{
//    public enum ResourceEnum
//    {
//        None = 0,
//        Credits = 1,
//        EXP = 2,
//        Diamonds = 3,
//        Gold = 4,
//        Energy = 5,
//        Chips = 65,
//    }
//    public enum ActsEnum
//    {
//        None = 0,
//        ActOne = 1,
//        ActTwo = 2,
//        ActThree = 3
//    }
//    [CreateAssetMenu(fileName = "Reward", menuName = "ScriptableObjects/Rewards/Battle Reward")]
//    public class BattleRewardSO : ScriptableObject
//    {
//        [SerializeField]
//        CharacterTypeEnum _characterDifficultyEnum;
//        public CharacterTypeEnum CharacterDifficultyEnum => _characterDifficultyEnum;



//        [SerializeField]
//        BattleDropChance[] _cardChances;
//        [SerializeField]
//        BattleDropChance[] _recipesChances;

//        [SerializeField]
//        List<CurrencyRewardAmount> _rewardTypes;

//        [System.Serializable]
//        public class CurrencyRewardAmount
//        {

//            [SerializeField]
//            ResourceEnum _currencyType;

//            [SerializeField]
//            ushort[] _minValue;
//            [SerializeField]
//            ushort[] _maxValue;
//            public ResourceEnum CurrencyType => _currencyType;

//            public CurrencyRewardAmount(ushort[] minValue, ushort[] maxValue, ResourceEnum currencyType)
//            {
//                _minValue = minValue;
//                _maxValue = maxValue;
//                _currencyType = currencyType;

//            }
//            public ushort RandomizeAmount(ActsEnum _actType)
//            {
//                int index = (int)_actType;

//                return (ushort)Random.Range(_minValue[index], _maxValue[index]);
//            }
//        }

//        public ushort RandomAmount(ResourceEnum currencyEnum, ActsEnum _actType)
//        {
//            for (int i = 0; i < _rewardTypes.Count; i++)
//            {
//                if (_rewardTypes[i].CurrencyType == currencyEnum)
//                    return _rewardTypes[i].RandomizeAmount((_characterDifficultyEnum == CharacterTypeEnum.Tutorial) ? ActsEnum.None : _actType);
//            }
//            throw new System.Exception($"BattleRewardSO: CurrencyEnum is not a valid type {currencyEnum}!");
//        }

//        public BattleReward CreateReward(ActsEnum actsEnum, IEnumerable<Battle.Combo.ComboData> workOnCombo)
//        {
//            CardData[] rewardCards = GenerateCardsRewards(actsEnum);

//            Battle.Combo.ComboData[] combo = null;
//            if (_characterDifficultyEnum >= CharacterTypeEnum.Elite_Enemy)
//            {
//                combo = GenerateComboReward(actsEnum, workOnCombo);
//            }


//            ushort Credits = RandomAmount(ResourceEnum.Credits, actsEnum);
//            ushort Gold = RandomAmount(ResourceEnum.Gold, actsEnum);


//            BattleReward reward = new BattleReward(Credits, Gold, rewardCards, combo);

//            return reward;
//        }
//        public RunReward CreateRunReward(ActsEnum act)
//        {
//            ushort EXP = RandomAmount(ResourceEnum.EXP, act);
//            ushort Diamonds = RandomAmount(ResourceEnum.Diamonds, act);
//            return new RunReward(EXP, Diamonds);
//        }
//        //refactor this method
//        public CardData[] GenerateCardsRewards(ActsEnum actsEnum, byte CardAmount = 3)
//        {
//            var actCardChance = _cardChances.First(x => x.ActEnum == actsEnum);

//            CardData[] rewardCards = new CardData[CardAmount];

//            byte random;
//            var cardFactoryHandler = Factory.GameFactory.Instance.CardFactoryHandler;

//            var cardCollection = cardFactoryHandler.CardCollection;
//            var cardChances = actCardChance.DropChances;

//            const int OneHundred = 100;



//            for (int i = 0; i < CardAmount; i++)
//            {
//                int index = -1;
//                byte addition = 0;
//                random = (byte)Random.Range(0, OneHundred);

//                // find  BattleDropChance.RarityDropChance from the random number
//                for (int j = 0; j < cardChances.Length; j++)
//                {
//                    if (j > 0)
//                        addition += cardChances[j - 1].DropChance;

//                    if (random < (addition + cardChances[j].DropChance))
//                    {
//                        index = j;
//                        break;
//                    }
//                }

//                if (index == -1)
//                    throw new System.Exception("Index Was Not Found - Random Chance was not in any of the rarity chances");


//                var rarityCardCollection = cardCollection.GetCardByRarity((RarityEnum)(index + 1));

//                int randomID = Random.Range(0, rarityCardCollection.CardsID.Length);
//                if (randomID >= rarityCardCollection.CardsID.Length)
//                    Debug.Log($"Rarity is : {(RarityEnum)(index + 1)}\nrarityCardCollection.CardsID.Length = {rarityCardCollection.CardsID.Length}\nRandom ID is : {randomID}");

//                if (randomID >= rarityCardCollection.CardsID.Length)
//                    throw new System.Exception(
//                        $"BattleRewardSO: CardID Was bigger than the reward collection for: {rarityCardCollection.CardsID.Length}\nCardID: {randomID}\n Rarity: {(RarityEnum)(index + 1)}");
//                ushort CardId = rarityCardCollection.CardsID[randomID];
//                // get cards level;
//                var DropChance = actCardChance.DropChances[index];
//                var levelChances = DropChance.LevelChances;
//                addition = 0;
//                random = (byte)Random.Range(0, OneHundred);

//                for (int j = 0; j < levelChances.Length; j++)
//                {
//                    if (j > 0)
//                        addition += levelChances[j - 1];

//                    if (random < (addition + levelChances[j]))
//                    {
//                        index = j;
//                        break;
//                    }
//                }
//                var card = cardFactoryHandler.GetCard(CardId);

//                rewardCards[i] = cardFactoryHandler.CreateCard(card, (card.CardsMaxLevel < (byte)index) ? (byte)(card.CardsMaxLevel - 1) : (byte)index);


//                if (rewardCards[i] == null)
//                    throw new System.Exception("Card Created is Null!");
//            }

//            return rewardCards;
//        }
//        //refactor this method
//        public Battle.Combo.ComboData[] GenerateComboReward(ActsEnum actsEnum, IEnumerable<Battle.Combo.ComboData> workOnCombo, byte amount = 1)
//        {
//            // roll combos from rarity 
//            // check if the combo is optional to be reward based on interface
//            // if no -> reduce the rarity level
//            // if yes -> try highest rarity level
//            // 

//            int index = -1;
//            var recipesChances = _recipesChances.First(x => x.ActEnum == actsEnum);
//            var comboChances = recipesChances.DropChances;
//            var comboFactoryHandler = Factory.GameFactory.Instance.ComboFactoryHandler;
//            var comboCollection = comboFactoryHandler.ComboCollection;

//            var comboIDs = workOnCombo.Select(x => new { ID = x.ID });

//            Battle.Combo.ComboData[] combo = new Battle.Combo.ComboData[amount];
//            List<int> allPossibleCombosIDFromChances = new List<int>();
//            byte[] chances = new byte[comboChances.Length];
//            for (int j = 0; j < chances.Length; j++)
//            {
//                chances[j] = comboChances[j].DropChance;
//                if (chances[j] > 0)
//                {
//                    var ids = comboCollection.GetComboByRarity((RarityEnum)(j + 1)).ComboID;
//                    allPossibleCombosIDFromChances.AddRange(ids);
//                }
//            }
//            allPossibleCombosIDFromChances = allPossibleCombosIDFromChances.Except(workOnCombo.Select(x => x.ID)).ToList();

//            if (allPossibleCombosIDFromChances.Count == 0)
//                return combo;

//            var factory = Factory.GameFactory.Instance.ComboFactoryHandler;
//            var possibleCombos = factory.GetComboSOFromIDs(allPossibleCombosIDFromChances).ToList();

//            for (int i = 0; i < amount; i++)
//            {

//                IEnumerable<Battle.Combo.ComboSO> combosFromThisRarity;
//                do
//                {
//                    RarityEnum rarity = GetRandomRarity();
//                    if (rarity == RarityEnum.None)
//                        throw new System.Exception();

//                    combosFromThisRarity = possibleCombos.Where(x => x.GetRarityEnum == rarity);
//                } while (combosFromThisRarity.Count() == 0);

//                BattleDropChance.RarityDropChance DropChance = recipesChances.DropChances[index];
//                byte[] levelChances = DropChance.LevelChances;
//                int level = ChanceHelper.GetRandomIndexByChances(levelChances);
//                var randomCombo = combosFromThisRarity.ElementAt(Random.Range(0, combosFromThisRarity.Count()));
//                combo[i] = comboFactoryHandler.CreateCombo(randomCombo, (byte)level);
//                allPossibleCombosIDFromChances.Remove(randomCombo.ID);
//                possibleCombos.Remove(randomCombo);
//                if (possibleCombos.Count == 0 || allPossibleCombosIDFromChances.Count == 0)
//                    return combo;
//            }

//            //for (int i = 0; i < amount; i++)
//            //{
//            //    if (allPossibleCombosIDFromChances.Count == 0)
//            //        break;
//            //    do
//            //    {

//            //        index = ChanceHelper.GetRandomIndexByChances(chances);
//            //        if (index == -1)
//            //        {
//            //            Debug.LogError("Index Was Not Found - Random Chance was not in any of the rarity chances");
//            //            combo[i] = null;
//            //        }
//            //        ushort[] comboID = comboCollection.GetComboByRarity((RarityEnum)(index + 1)).ComboID;
//            //        if (comboID.Length == 0)
//            //            continue;

//            //        int length = comboID.Length;
//            //        List<ushort> combos = new List<ushort>(length);
//            //        combos.AddRange(comboID);


//            //        List<ushort> combosSortByRarity = new List<ushort>(length);
//            //        for (int j = 0; j < length; j++)
//            //        {
//            //            int randomindex = Random.Range(0, combos.Count);
//            //            combosSortByRarity.Add(combos.ElementAt(randomindex));
//            //            combos.RemoveAt(randomindex);
//            //        }

//            //        for (int j = 0; j < length; j++)
//            //        {
//            //            if (!workOnCombo.Any(x => x.ComboSO.ID == combosSortByRarity[j]))
//            //            {
//            //                var DropChance = recipesChances.DropChances[index];
//            //                var levelChances = DropChance.LevelChances;
//            //                int level = ChanceHelper.GetRandomIndexByChances(levelChances);
//            //                combo[i] = comboFactoryHandler.CreateCombo(comboFactoryHandler.ComboCollection.GetCombo(combosSortByRarity[j]), (byte)level);
//            //                break;
//            //            }
//            //        }

//            //        if (combo[i] == null)
//            //        {
//            //            // check if he has all the possibles combos
//            //            bool hasAll = false;
//            //            for (int j = 0; j < allPossibleCombosIDFromChances.Count; j++)
//            //            {
//            //                hasAll = false;
//            //                for (int z = 0; z < comboIDs.Count(); z++)
//            //                {
//            //                    if (comboIDs.ElementAt(z).ID == allPossibleCombosIDFromChances[i])
//            //                    {
//            //                        hasAll = true;
//            //                        break;

//            //                    }
//            //                }
//            //                if (!hasAll)
//            //                    break;
//            //            }
//            //            if (hasAll)
//            //            {
//            //                Debug.LogWarning("he got all the combos that he can get from this chances returning null");
//            //                return combo;
//            //            }


//            //        }
//            //    } while (combo[i] == null);
//            //}
//            RarityEnum GetRandomRarity()
//            {
//                index = ChanceHelper.GetRandomIndexByChances(chances);
//                if (index == -1)
//                {
//                    Debug.LogError("Index Was Not Found - Random Chance was not in any of the rarity chances");
//                    return RarityEnum.None;
//                }
//                return (RarityEnum)(index + 1);
//            }
//            List<T> RandomizeList<T>(IEnumerable<T> list)
//            {
//                int length = list.Count();
//                List<T> copyList = new List<T>(length);
//                copyList.AddRange(list);


//                List<T> newList = new List<T>(length);
//                for (int j = 0; j < length; j++)
//                {
//                    int randomindex = Random.Range(0, copyList.Count);
//                    newList.Add(list.ElementAt(randomindex));
//                    copyList.RemoveAt(randomindex);
//                }
//                return newList;
//            }



//            return combo;
//        }


//        [System.Serializable]
//        public class BattleDropChance
//        {

//            [System.Serializable]
//            public class RarityDropChance
//            {
//                [SerializeField]
//                RarityEnum _rarityEnum;
//                public RarityEnum RarityEnumType => _rarityEnum;

//                [SerializeField]
//                byte _dropChance;
//                public byte DropChance => _dropChance;

//                [SerializeField]
//                byte[] _levelChances;
//                public byte[] LevelChances => _levelChances;
//                public RarityDropChance(RarityEnum rare, byte chance, byte[] levelChances)
//                {
//                    _rarityEnum = rare;
//                    _dropChance = chance;
//                    _levelChances = levelChances;
//                }

//            }
//            [SerializeField]
//            RarityDropChance[] _dropChances;

//            [SerializeField]
//            private ActsEnum _act;

//            public RarityDropChance[] DropChances => _dropChances;
//            public ActsEnum ActEnum => _act;
//            public BattleDropChance(ActsEnum act, params byte[][] chances)
//            {
//                _dropChances = new RarityDropChance[chances[0].Length];

//                for (int i = 0; i < chances[0].Length; i++)
//                    _dropChances[i] = new RarityDropChance((RarityEnum)(i + 1), chances[0][i], chances[i + 1]);

//                _act = act;
//                //_commonChanceLevels = chances[1];
//                //_uncommonChanceLevels = chances[2];
//                //_rareChanceLevels = chances[3];
//                //_epicChanceLevels = chances[4];
//                //_legendreiLevels = chances[5];


//            }
//        }


//#if UNITY_EDITOR
//        public bool Init(string[] row)
//        {
//            const int ID = 0;
//            const int Credits = 1;

//            const int DropChance = 5;
//            const int Common = 6;
//            const int UnCommon = 7;
//            const int Rare = 8;
//            const int Epic = 9;
//            const int Legendrei = 10;

//            const int hasRecipe = 11;

//            const int Recipe = 7;


//            // character difficultyenum
//            if (int.TryParse(row[ID], out int outcome))
//                _characterDifficultyEnum = (CharacterTypeEnum)outcome;
//            else
//                return false;

//            //gold
//            List<CurrencyRewardAmount> _currencyList = new List<CurrencyRewardAmount>();
//            for (int i = 0; i < 4; i++)
//            {
//                string[] amount = row[Credits + i].Split('^');
//                List<ushort> minAmount = new List<ushort>();
//                List<ushort> maxAmount = new List<ushort>();
//                for (int j = 1; j <= amount.Length; j++)
//                {
//                    amount[j - 1].Trim();
//                    string[] perActs = amount[j - 1].Split('&');

//                    if (ushort.TryParse(perActs[0], out ushort min))
//                    {
//                        minAmount.Add(min);
//                    }
//                    else
//                        throw new System.Exception($"Battle RewardSO: Min Value is not a valid number {(ResourceEnum)Credits + i} -  {perActs[0]}");

//                    if (ushort.TryParse(perActs[1], out ushort max))
//                    {
//                        maxAmount.Add(max);
//                    }
//                    else
//                        throw new System.Exception($"Battle RewardSO: Max Value is not a valid number {(ResourceEnum)Credits + i} -  {perActs[1]}");

//                }

//                var minVal = minAmount.ToArray();
//                var maxVal = maxAmount.ToArray();
//                var rarity = (ResourceEnum)(Credits + i);
//                _currencyList.Add(new CurrencyRewardAmount(minVal, maxVal, rarity));
//            }

//            _rewardTypes = _currencyList;



//            //card Drop Chances
//            List<BattleDropChance> _battleDropChacnes = new List<BattleDropChance>();
//            {
//                string[] dropChanceString = row[DropChance].Split('^');
//                string[] commonChanceLevelsString = row[Common].Split('^');
//                string[] uncommonChanceLevelsString = row[UnCommon].Split('^');
//                string[] rareChanceLevelsString = row[Rare].Split('^');
//                string[] epicChanceLevelsString = row[Epic].Split('^');
//                string[] legendreiLevelsString = row[Legendrei].Split('^');

//                for (int i = 0; i < dropChanceString.Length; i++)
//                {

//                    var dropChances = SplitData(dropChanceString[i]);
//                    var commonChanceLevels = SplitData(commonChanceLevelsString[i]);
//                    var uncommonChanceLevels = SplitData(uncommonChanceLevelsString[i]);
//                    var rareChanceLevels = SplitData(rareChanceLevelsString[i]);
//                    var epicChanceLevels = SplitData(epicChanceLevelsString[i]);
//                    var legendreiLevels = SplitData(legendreiLevelsString[i]);

//                    _battleDropChacnes.Add(new BattleDropChance((ActsEnum)(i + 1), dropChances, commonChanceLevels, uncommonChanceLevels, rareChanceLevels, epicChanceLevels, legendreiLevels));
//                }
//                _cardChances = _battleDropChacnes.ToArray();
//            }

//            if (int.TryParse(row[hasRecipe], out int result))
//            {
//                if (result == 1)
//                {
//                    _battleDropChacnes = new List<BattleDropChance>();
//                    string[] dropChanceString = row[DropChance + Recipe].Split('^');
//                    string[] commonChanceLevelsString = row[Common + Recipe].Split('^');
//                    string[] uncommonChanceLevelsString = row[UnCommon + Recipe].Split('^');
//                    string[] rareChanceLevelsString = row[Rare + Recipe].Split('^');
//                    string[] epicChanceLevelsString = row[Epic + Recipe].Split('^');
//                    string[] legendreiLevelsString = row[Legendrei + Recipe].Split('^');

//                    for (int i = 0; i < dropChanceString.Length; i++)
//                    {
//                        var dropChances = SplitData(dropChanceString[i]);
//                        var commonChanceLevels = SplitData(commonChanceLevelsString[i]);
//                        var uncommonChanceLevels = SplitData(uncommonChanceLevelsString[i]);
//                        var rareChanceLevels = SplitData(rareChanceLevelsString[i]);
//                        var epicChanceLevels = SplitData(epicChanceLevelsString[i]);
//                        var legendreiLevels = SplitData(legendreiLevelsString[i]);

//                        _battleDropChacnes.Add(new BattleDropChance((ActsEnum)(i + 1), dropChances, commonChanceLevels, uncommonChanceLevels, rareChanceLevels, epicChanceLevels, legendreiLevels));
//                    }
//                    _recipesChances = _battleDropChacnes.ToArray();
//                }
//            }

//            return true;

//            byte[] SplitData(string tab)
//            {
//                string[] data = tab.Split('&');
//                byte[] array = new byte[data.Length];

//                for (int i = 0; i < data.Length; i++)
//                {
//                    if (byte.TryParse(data[i], out byte amount))
//                        array[i] = amount;
//                    else
//                        throw new System.Exception(" Could not be converted to number (byte)!\n" + data[i]); ;
//                }
//                return array;
//            }
//        }
//#endif
//    }


//}

