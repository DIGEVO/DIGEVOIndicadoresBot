using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DIGEVOIndicadoresBot.PredictionControllers
{
    public class TimeSerialForecasting
    {
        //
        //simple moving average
        //
        //            ( dt + d(t-1) + d(t-2) + ... + d(t-n+1) )
        //  f(t+1) =  -----------------------------------------
        //                              n
        public static List<ForecastData> SimpleMovingAverage(double[] values, int extension, int periods, int holdout)
        {
            var forecasts = new List<ForecastData>();

            foreach (var i in Enumerable.Range(0, values.Length + extension))
            {
                var Instance = i;
                var Holdout = (i > values.Length - holdout) && (i < values.Length);
                var Value = i < values.Length ? values[i] : 0;
                var Forecast = i == 0 ?
                                values[i] :
                                forecasts
                                    .Where(fd1 => fd1.Instance >= (i - periods) && fd1.Instance < i)
                                    .Average(fd1 =>
                                    {
                                        if ((i <= values.Length - holdout)) return fd1.Value;
                                        else return fd1.Instance < values.Length ? fd1.Value : fd1.Forecast;
                                    });

                var fd = new ForecastData(Instance, Holdout, Value, Forecast);

                forecasts.Add(fd);
            }

            return forecasts;
        }
    }
}