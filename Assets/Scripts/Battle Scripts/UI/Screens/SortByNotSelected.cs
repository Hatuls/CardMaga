using Battles.UI;
using Cards;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rei.Utilities
{
    public class SortByNotSelected : SortAbst<Card>
    {
        [SerializeField]

        MetaCardUIFilterScreen filterHandler;

        public ushort? ID { get; set; }


        public override IEnumerable<Card> Sort()
        {
            var collection = filterHandler.Collection.Where(x=>x.gameObject.activeSelf);
           var cardCollectionInstance = collection.Select(x => x.CardUI.RecieveCardReference());
            if (ID == null)
                return cardCollectionInstance;




            return cardCollectionInstance.Where(x => x.CardInstanceID != ID);
        }

        public override void SortRequest()
        {
            filterHandler.SortBy(this);
        }
    }
}