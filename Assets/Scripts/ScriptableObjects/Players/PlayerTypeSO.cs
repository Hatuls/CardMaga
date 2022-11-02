using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.Battle.Players
{
    [CreateAssetMenu(fileName = "New Player Type SO", menuName = "ScriptableObjects/Players/New Player Type")]
    public class PlayerTagSO : ScriptableObject
    {

    }



    public static class PlayerHelper
    {
        public static bool ContainTag(this IPlayer player, PlayerTagSO playerTagSO)
        {
            bool isContain = false;
            IReadOnlyList<PlayerTagSO> playerTags = player.PlayerTags;
            int length = playerTags.Count;
            for (int i = 0; i < length; i++)
            {
                isContain |= (playerTags[i] == playerTagSO);
                if (isContain)
                    break;
            }
            return isContain;
        }

        public static bool ContainAllTags(this IPlayer player, PlayerTagSO[] playerTagSO)
        {
            bool isContainAll = true;
            for (int i = 0; i < playerTagSO.Length; i++)
            {
                isContainAll &= player.ContainTag(playerTagSO[i]);
                if (!isContainAll)
                    break;
            }
            return isContainAll;
        }

        public static bool ContainOneOrMoreTags(this IPlayer player, PlayerTagSO[] playerTagSO)
        {
            bool isContainAll = false;
            for (int i = 0; i < playerTagSO.Length; i++)
            {
                isContainAll |= player.ContainTag(playerTagSO[i]);
                if (isContainAll)
                    break;
            }
            return isContainAll;
        }

        public static bool ContainOneOrMoreTags(this IPlayer player, PlayerTagSO[] playerTagSO, out IReadOnlyList<PlayerTagSO> playerTagFound)
        {
            List<PlayerTagSO> playerTags = new List<PlayerTagSO>(playerTagSO.Length);
            bool isContainAll = false;

            for (int i = 0; i < playerTagSO.Length; i++)
            {
                bool containCurrent = player.ContainTag(playerTagSO[i]);
                if (containCurrent)
                    playerTags.Add(playerTags[i]);

                isContainAll |= containCurrent;
            }
            playerTagFound = playerTags;
            return isContainAll;
        }
    }
}