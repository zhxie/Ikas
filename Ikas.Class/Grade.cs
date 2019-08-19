using System;
using System.Collections.Generic;
using System.Text;

namespace Ikas.Class
{
    public class Grade
    {
        public enum Key
        {
            grade_unknown = -1,
            intern,
            apprentice,
            part_timer,
            go_getter,
            overachiever,
            profeshional
        }

        public Key Id { get; }
        public string Name { get; }

        public Grade(Key id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
