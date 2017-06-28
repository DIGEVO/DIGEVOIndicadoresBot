using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DIGEVOIndicadoresBot.Dialogs
{
    [Serializable]
    public class IndicadoresLuisDialog : LuisDialog<object>
    {
        public IndicadoresLuisDialog() : base(
            new LuisService(new LuisModelAttribute(ConfigurationManager.AppSettings["LuisAppId"], ConfigurationManager.AppSettings["LuisAPIKey"])))
        {
            //todo buscar la forma de realizar log.
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            //context.Activity
            //todo guardar cuando ya lo salude!
            var meridian = context.Activity.LocalTimestamp.Value.ToString("tt", CultureInfo.InvariantCulture).ToLower();
            var hour = context.Activity.LocalTimestamp.Value.Hour;
            var greetings = meridian == "am" ? "buenos días" : hour >= 19 ? "buenas noches" : "buenas tardes";
            await context.PostAsync($"Hola {context.Activity.From.Name}, {greetings}, disculpa ¿puedes repetir tu pregunta?: {result.Query}");
            context.Wait(MessageReceived);
        }

        [LuisIntent("conocer")]
        public async Task KnowIntent(IDialogContext context, LuisResult result)
        {
            var str = String.Join(", ", Enumerable.Select(result.Entities, e => $"{e.Entity.ToString()} {e.Type.ToString()} {e.EndIndex.ToString()}").ToList());
            var str1 = String.Join(", ", Enumerable.SelectMany(result.CompositeEntities, ce => ce.Children.Select(cc => cc.Value)));
            await context.PostAsync($"Hola {context.Activity.From.Name}, entiendo que deseas \"conocer\" acerca de: {result.Query}, {str} {str1}");
            context.Wait(MessageReceived);
        }
    }
}