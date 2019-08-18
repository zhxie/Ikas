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
    }
}
