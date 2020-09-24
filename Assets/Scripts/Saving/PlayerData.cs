﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    [Serializable]
    public class PlayerData
    {
        public Dictionary<int, string[]> playerDecks;
        public Dictionary<string, bool> cardsUpgrade;

        public int money;
        public Dictionary<int, string[]> playerEquipments;

        public PlayerData()
        {
            var cards = Resources.LoadAll<Card>("Cards");
            playerDecks = new Dictionary<int, string[]>();
            cardsUpgrade = new Dictionary<string, bool>();
            foreach (Card c in cards)
            {
                cardsUpgrade[c.cardName] = false;
            }

            money = 0;
            playerEquipments = new Dictionary<int, string[]>();
        }
    }
}