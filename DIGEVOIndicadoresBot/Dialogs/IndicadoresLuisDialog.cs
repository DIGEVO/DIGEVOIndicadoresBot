using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace DIGEVOIndicadoresBot.Dialogs
{
    [Serializable]
    public class IndicadoresLuisDialog : LuisDialog<object>
    {
        public IndicadoresLuisDialog() : base(
            new LuisService(new LuisModelAttribute(ConfigurationManager.AppSettings["LuisAppId"],ConfigurationManager.AppSettings["LuisAPIKey"])))
        {
            //todo buscar la forma de realizar log.
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            //context.Activity
            await context.PostAsync($"{context.Activity.From.Name}, You have reached the none intent. You said: {result.Query}"); //
            context.Wait(MessageReceived);
        }

        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "MyIntent" with the name of your newly created intent in the following handler
        [LuisIntent("conocer")]
        public async Task Conocer(IDialogContext context, LuisResult result)
        {
            var str = String.Join(", ", Enumerable.Select(result.Entities, e => $"{e.Entity.ToString()} {e.Type.ToString()} {e.EndIndex.ToString()}").ToList());
            await context.PostAsync($"You have reached the \"conocer\" intent. You said: {result.Query}, {str}"); //
            context.Wait(MessageReceived);
        }
    }
}