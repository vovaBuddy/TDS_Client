using UnityEngine;
using System.Collections.Generic;

namespace Main.Inventory
{
    public class LootItemHolder
    {
        public int Amount;
        public int SlotAmount;
        public int MaxInSlot;

        public LootItemHolder(int amount, int slotsAmount, int maxInSlot)
        {
            Amount = amount;
            SlotAmount = slotsAmount;
            MaxInSlot = maxInSlot;
        }
    }
}