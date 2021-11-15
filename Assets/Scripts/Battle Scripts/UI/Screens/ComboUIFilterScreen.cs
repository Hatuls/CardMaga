﻿using Battles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Map.UI
{
    public class ComboUIFilterScreen : MonoBehaviour
    {
        [SerializeField] GameObject _comboRecipePrefab;
        [SerializeField] List<ComboRecipeUI> _comboRecipies;

        private void CreateRecipes()
        {
            var combos = BattleData.Player.CharacterData.ComboRecipe;
            while (combos.Length > _comboRecipies.Count)
            {
                var combo = Instantiate(_comboRecipePrefab, this.transform).GetComponent<ComboRecipeUI>();
                _comboRecipies.Add(combo);
            }
        }

        public void SortByTwoCombination()
          => SortByCombination(2);
    
        public void SortByThreeCombination()
            => SortByCombination(3);
        private void SortByCombination(int amount)
        {
            CreateRecipes();
            int length = _comboRecipies.Count;
            var combos = BattleData.Player.CharacterData.ComboRecipe;
            var sortedCombos = combos.Where((x) => x.ComboSO.ComboSequance.Length == amount);

            int sortedCombosLength = sortedCombos.Count();

            for (int i = 0; i < length; i++)
            {
                if (i < sortedCombosLength)
                {
                    if (_comboRecipies[i].gameObject.activeSelf == false)
                        _comboRecipies[i].gameObject.SetActive(true);
                    _comboRecipies[i].InitRecipe(sortedCombos.ElementAt(i));
                }
                else
                {
                    if (_comboRecipies[i].gameObject.activeSelf == true)
                        _comboRecipies[i].gameObject.SetActive(false);
                }
            }
        }
        public void ShowAllCombos()
        {
            CreateRecipes();
            int length = _comboRecipies.Count;
            var combos = BattleData.Player.CharacterData.ComboRecipe;
            for (int i = 0; i < length; i++)
            {
                if (i < combos.Length)
                {
                    if (_comboRecipies[i].gameObject.activeSelf == false)
                        _comboRecipies[i].gameObject.SetActive(true);

                    _comboRecipies[i].InitRecipe(combos[i]);
                }
                else
                {
                    if (_comboRecipies[i].gameObject.activeSelf == true)
                        _comboRecipies[i].gameObject.SetActive(false);
                }
            }
        }

    }
}