using System;
using System.Collections.Generic;
using System.Text;

namespace Ikas.Class
{
    public class Salmoniod
    {
        public enum Key
        {
            goldie = 3,
            steelhead = 6,
            flyfish = 9,
            scrapper = 12,
            steelEel = 13,
            stinger = 14,
            maws = 15,
            griller = 16,
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
