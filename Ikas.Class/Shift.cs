using System;
using System.Collections.Generic;
using System.Text;

namespace Ikas.Class
{
    public class Shift : Base
    {
        public List<ShiftStage> Stages { get; }

        public Shift(List<ShiftStage> stages)
        {
            Stages = stages;
        }
        public Shift()
        {
            Stages = new List<ShiftStage>();
        }
        public Shift(ErrorType error) : base(error)
        {
            Stages = new List<ShiftStage>();
        }
    }
}
