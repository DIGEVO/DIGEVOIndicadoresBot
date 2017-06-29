using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace DIGEVOIndicadoresBot.RESTClient
{
    public class IndicadoresRESTClient
    {
        public static readonly Dictionary<Indicators, int> IndicatorStartYear = new Dictionary<Indicators, int>() {
            { Indicators.uf, 1977},
            { Indicators.ivp, 1990},
            { Indicators.dolar, 1984},
            { Indicators.dolar_intercambio, 1988},
            { Indicators.euro, 1999},
            { Indicators.ipc, 1928},
            { Indicators.utm, 1990},
            { Indicators.imacec, 2004},
            { Indicators.tpm, 2001},
            { Indicators.libra_cobre, 2012},
            { Indicators.tasa_desempleo, 2009},
        };

        public static async Task<DataPoint> GetFinancialData(Indicators indicator, DateTime date)
        {
            var client = new HttpClient();
            var url = $@"http://www.mindicador.cl/api/{indicator}/{date.ToString("dd-MM-yyyy")}";
            var stringTask = client.GetStringAsync(url);
            var stringData = await stringTask;
            var token = JToken.Parse(stringData);
            var jarraydata = token["serie"] ?? new JArray();

            var result = new DataPoint();

            if (jarraydata.HasValues)
            {
                var data = jarraydata.ToArray()[0];
                result.Date = data["fecha"].Value<DateTime>();
                result.Value = data["valor"].Value<float>();
            }

            return result;
        }

        public static IEnumerable<DataPoint> GetFinancialData(Indicators indicator, DateTime startDate, DateTime endDate)
        {
            var client = new HttpClient();

            return Enumerable
                .Range(Math.Max(startDate.Year, IndicatorStartYear[indicator]), endDate.Year - startDate.Year + 1)
                .Select(async y =>
                {
                    string url = $"http://www.mindicador.cl/api/{indicator}/{y}";
                    var stringTask = client.GetStringAsync(url);
                    var stringData = await stringTask;
                    var token = JToken.Parse(stringData);
                    var jarraydata = token["serie"] ?? new JArray();
                    return jarraydata
                            .AsParallel()
                            .Select(ob => new DataPoint(ob["fecha"].Value<DateTime>(), ob["valor"].Value<float>()))
                            .Where(ob => ob.Date >= startDate && ob.Date <= endDate)
                            .OrderBy(ob => ob.Date);
                })
                .SelectMany(t => t.Result);
        }
    }
}