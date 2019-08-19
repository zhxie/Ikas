using System;
using System.Collections.Generic;
using System.Text;

namespace Ikas.Class
{
    public class Salmoniod
    {
        public enum Key
        {
            salmoniod_unknown = -1,
            goldie = 3,
            steelhead = 6,
            flyfish = 9,
            scrapper = 12,
            steel_eel = 13,
            stinger = 14,
            maws = 15,
            griller2 = 16,  // griller was used by event of salmon run, use griller2 instead
            drizzler = 21
        }

        public Key Id { get; }
        public string Name { get; }

        public Salmoniod(Key id, string name)
        {
            Id = id;
            Name = name;
        }

        public static Key ParseKey(string s)
        {
            switch (s)
            {
                case "sakelien-golden":
                    return Key.goldie;
                case "sakelien-bomber":
                    return Key.steelhead;
                case "sakelien-cup-twins":
                    return Key.flyfish;
                case "sakelien-shield":
                    return Key.scrapper;
                case "sakelien-snake":
                    return Key.steel_eel;
                case "sakelien-tower":
                    return Key.stinger;
                case "sakediver":
                    return Key.maws;
                case "sakedozer":
                    return Key.griller2;
                case "sakerocket":
                    return Key.drizzler;
                default:
                    throw new FormatException();
            }
        }
    }

    public class SalmoniodCount
    {
        public Salmoniod Salmoniod;
        public int Count;

        public SalmoniodCount(Salmoniod salmoniod, int count)
        {
            Salmoniod = salmoniod;
            Count = count;
        }
    }
}
