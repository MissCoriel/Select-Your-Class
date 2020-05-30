using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;
using Newtonsoft.Json;
using StardewModdingAPI.Events;

namespace SelectYourClass
{
    class Mod : StardewModdingAPI.Mod
    {
        bool choseClass = false;
        internal static IMonitor TempMonitor;

        public override void Entry(IModHelper helper)
        {
            TempMonitor = this.Monitor;

            helper.Events.GameLoop.SaveLoaded += this.SaveLoaded;
        }
        public void SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            if (choseClass == false)
            {
                if (Game1.activeClickableMenu == null)
                {
                    Game1.activeClickableMenu = new ClassMenu(this.Helper.Data, this.Helper.DirectoryPath);
                }
            }
        }
    }
}
