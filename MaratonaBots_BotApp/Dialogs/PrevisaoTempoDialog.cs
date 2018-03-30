using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;

namespace MaratonaBots_BotApp.Dialogs
{
    [Serializable]
    public class PrevisaoTempoDialog : LuisDialog<object>
    {
        public PrevisaoTempoDialog(ILuisService service) : base(service) { }
        

    }
}