using System;
using System.Collections.Generic;
using System.Text;

namespace IkasLib
{
    public class Schedule
    {
        public DateTime EndTime { get; }
        public List<ScheduledStage> Stages { get; }
        public List<ScheduledStage> NextStages { get; }

        public Schedule(DateTime endTime, List<ScheduledStage> stages, List<ScheduledStage> nextStages)
        {
            EndTime = endTime;
            Stages = stages;
            NextStages = nextStages;
        }
        public Schedule(int error)
        {
            EndTime = new DateTime(error);
            Stages = new List<ScheduledStage>();
            NextStages = new List<ScheduledStage>();
        }

        public List<ScheduledStage> GetStages(Mode.Key mode)
        {
            List<ScheduledStage> stages = new List<ScheduledStage>();
            foreach (ScheduledStage stage in Stages)
            {
                if (stage.Mode == mode)
                {
                    stages.Add(stage);
                }
            }
            return stages;
        }
        public List<ScheduledStage> GetNextStages(Mode.Key mode)
        {
            List<ScheduledStage> stages = new List<ScheduledStage>();
            foreach (ScheduledStage stage in NextStages)
            {
                if (stage.Mode == mode)
                {
                    stages.Add(stage);
                }
            }
            return stages;
        }
    }
}
