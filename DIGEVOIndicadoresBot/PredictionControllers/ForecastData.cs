using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DIGEVOIndicadoresBot.PredictionControllers
{
    public class ForecastData
    {
        public int Instance { get; set; }
        public bool Holdout { get; set; }
        public double Value { get; set; }
        public double Forecast { get; set; }

        public ForecastData() { }

        public ForecastData(int Instance, bool Holdout, double Value, double Forecast)
        {
            this.Instance = Instance;
            this.Holdout = Holdout;
            this.Value = Value;
            this.Forecast = Forecast;
        }

        //E(t) = D(t) - F(t)
        public double Error()
        {
            return Forecast - Value;
        }

        //Absolute Error = | E(t) |
        public double AbsoluteError()
        {
            return Math.Abs(Error());
        }

        //Percent Error = E(t) / D(t)
        public double PercentError()
        {
            return Value != 0 ? Error() / Value : double.NaN;
        }

        //Absolute Percent Error = |E(t)| / D(t)
        public double AbsolutePercentError()
        {
            return Value != 0 ? AbsoluteError() / Value : double.NaN;
        }

        public override string ToString()
        {
            var v = Value == double.MaxValue ? " " : Value.ToString();
            return $"{Instance} {v} {Forecast} {Holdout} {Error()} {AbsoluteError()} {PercentError()} {AbsolutePercentError()}";
        }
    }
}