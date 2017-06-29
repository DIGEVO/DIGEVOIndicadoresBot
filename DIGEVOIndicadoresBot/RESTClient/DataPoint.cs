using System;

namespace DIGEVOIndicadoresBot.RESTClient
{
    public class DataPoint
    {
        public DataPoint(DateTime? Date = null, float Value = float.MaxValue)
        {
            this.Date = Date ?? DateTime.MaxValue; this.Value = Value;
        }

        public DateTime Date { get; set; }
        public float Value { get; set; }

        public override string ToString() { return $"{Date} {Value}"; }
    }
}