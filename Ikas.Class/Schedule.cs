using System;
using System.Collections.Generic;
using System.Text;

namespace Ikas.Class
{
    public class Schedule : Base
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
        public Schedule(ErrorType error) : base(error)
        {
            EndTime = new DateTime(0);
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

    public class SalmonRunSchedule : Base
    {
        public SalmonRunStage Stage { get; }
        public SalmonRunStage NextStage { get; }

        public SalmonRunSchedule(SalmonRunStage stage, SalmonRunStage nextStage)
        {
            Stage = stage;
            NextStage = nextStage;
        }
        public SalmonRunSchedule(ErrorType error) : base(error)
        {

        }
    }
}
