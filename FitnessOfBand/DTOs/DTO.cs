using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessOfBand.DTO
{
    public class Data
    {
        public IntInfo HeartRate { get; set; }
        public DoubleInfo Speed { get; set; }
        public DoubleInfo Pace { get; set; }
        public LongInfo Distance { get; set; }
        public LongInfo Step { get; set; }
        public LongInfo Calories { get; set; }
        public DateInfo Date { get; set; }
    }

    public class DateInfo
    {
        public DateTime Initial { get; set; }
        public DateTime Finished { get; set; }
    }

    public class IntInfo
    {
        public int Initial { get; set; }
        public int Final { get; set; }
    }

    public class DoubleInfo
    {
        public double Initial { get; set; }
        public double Final { get; set; }
    }

    public class LongInfo
    {
        public long Initial { get; set; }
        public long Final { get; set; }
    }

    public class Information
    {
        public int Id { get; set; }
        public int Wearable_Id { get; set; }
        public DateTime InitialDateTime { get; set; }
        public DateTime FinishedDateTime { get; set; }
        public int InitialHeartRate { get; set; }
        public int FinalHeartRate { get; set; }
        public long InitialDistance { get; set; }
        public long FinalDistance { get; set; }
        public long InitialSteps { get; set; }
        public long FinalSteps { get; set; }
        public long InitialCalories { get; set; }
        public long FinalCalories { get; set; }
    }
}
