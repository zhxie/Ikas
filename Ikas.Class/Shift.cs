using System;
using System.Collections.Generic;
using System.Text;

namespace Ikas.Class
{
    public class Shift : Base
    {
        public List<ShiftStage> Stages { get; }

        public bool IsOpen
        {
            get
            {
                if (Stages.Count > 0)
                {
                    DateTime dateTime = DateTime.Now;
                    if (dateTime >= Stages[0].StartTime.ToLocalTime() && dateTime <= Stages[0].EndTime.ToLocalTime())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

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
