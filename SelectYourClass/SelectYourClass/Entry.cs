using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;
using Newtonsoft.Json;
using StardewModdingAPI.Events;
using System.Security.AccessControl;
using System.Net;
using System.Runtime.Remoting.Channels;

namespace SelectYourClass
{
    class Mod : StardewModdingAPI.Mod
    {
        public static bool choseClass = false;
        internal static IMonitor TempMonitor;
        private SaveModel classSelect = null;
        public static string classCheck;
        private bool IsFetchingJobFromHost = false;
        internal static IModHelper Saving;
        public static string FarmHandClass;
        internal class RequestClassMessage
        {
            public int PlayerId { get; set; }
        }
        /// <summary>A host-to-farmhand message containing the requested player's class (which may be null if it wasn't saved yet).</summary>
        internal class ClassInfoMessage
        {
            public int PlayerId { get; set; }
            public SaveModel chosenClass { get; set; }
        }

        public override void Entry(IModHelper helper)
        {
            TempMonitor = this.Monitor;
            Saving = this.Helper;
            
            helper.Events.GameLoop.SaveLoaded += this.SaveLoaded;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.Multiplayer.ModMessageReceived += this.OnModMessageReceived;
        }
        public void SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            if (Context.IsMainPlayer)
            {   
                try
                {
                    var checkClass = this.Helper.Data.ReadSaveData<SaveModel>("ClassChosen");
                    if (checkClass == null)
                    {
                        
                        classCheck = "Not Selected";
                        Monitor.Log("No data found!", LogLevel.Debug);
                    }
                    else
                    {
                        classCheck = checkClass.chosenClass;
                        Monitor.Log($"Data found! Your class: {classCheck}", LogLevel.Debug);
                    }


                }
                catch (Exception) { }
            }
                
            else
            {
                this.IsFetchingJobFromHost = true;
                this.Helper.Multiplayer.SendMessage(new RequestClassMessage { PlayerId = (int)Game1.player.uniqueMultiplayerID }, nameof(RequestClassMessage), playerIDs: new[] { Game1.MasterPlayer.UniqueMultiplayerID });
            }
        }
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsMainPlayer) return;
            if (ClassMenu.classCheck != null)
            {
                classCheck = ClassMenu.classCheck;
            }

            if (classCheck == "Not Selected" && Game1.activeClickableMenu == null)
            {
                Game1.activeClickableMenu = new ClassMenu(this.Helper.Data, this.Helper.DirectoryPath);
            }

            if (!Context.IsMainPlayer && FarmHandClass == "Not Selected" && !this.IsFetchingJobFromHost)
            {
                Game1.activeClickableMenu = new ClassMenu(this.Helper.Data, this.Helper.DirectoryPath);
                Monitor.Log("This happened for farmhand", LogLevel.Debug);
                // TODO: save selected class to this.Job when the menu closes
            }
            if (!Context.IsMainPlayer && FarmHandClass == null && !this.IsFetchingJobFromHost)
            {
                Game1.activeClickableMenu = new ClassMenu(this.Helper.Data, this.Helper.DirectoryPath);
                Monitor.Log("This happened for farmhand", LogLevel.Debug);
                // TODO: save selected class to this.Job when the menu closes
            }

        }

        private void OnModMessageReceived(object sender, ModMessageReceivedEventArgs e)
        {
            if (e.FromModID == this.ModManifest.UniqueID)
            {
                switch (e.Type)
                {
                    // host player: receive request for job data from a farmhand
                    case nameof(RequestClassMessage):
                        if (Context.IsMainPlayer)
                        {
                            try
                            {
                                var message = e.ReadAs<RequestClassMessage>();
                                var job = this.Helper.Data.ReadSaveData<SaveModel>($"Class.{message.PlayerId}");
                                this.Helper.Multiplayer.SendMessage(new ClassInfoMessage { PlayerId = message.PlayerId, chosenClass = job }, nameof(ClassInfoMessage), playerIDs: new[] { e.FromPlayerID });

                                FarmHandClass = job.chosenClass;
                                if (FarmHandClass == null && ClassMenu.FarmHandClass != null)
                                {
                                    FarmHandClass = ClassMenu.FarmHandClass;
                                    this.Helper.Data.WriteSaveData($"Class.{message.PlayerId}", new SaveModel { chosenClass = FarmHandClass });
                                }
                                else if (FarmHandClass == null && ClassMenu.FarmHandClass == null)
                                {
                                    FarmHandClass = "Not Selected";
                                    Monitor.Log("No Data Found for a Farmhand!", LogLevel.Debug);
                                }


                            }
                            catch (Exception) { }
                        }
                        break;
                

                    // farmhand: receive job data for the current player
                    case nameof(ClassInfoMessage):
                        if (!Context.IsMainPlayer)
                        {
                            var message = e.ReadAs<ClassInfoMessage>();
                            try
                            {
                                classSelect = message.chosenClass;
                                FarmHandClass = classSelect.chosenClass;
                                if (FarmHandClass == null && ClassMenu.FarmHandClass != null)
                                {
                                    FarmHandClass = ClassMenu.FarmHandClass;
                                }
                                else if (FarmHandClass == null && ClassMenu.FarmHandClass == null)
                                {
                                    FarmHandClass = "Not Selected";
                                    Monitor.Log("No Data Found for a Farmhand!", LogLevel.Debug);
                                }

                            }
                            catch (Exception) { }
                            this.IsFetchingJobFromHost = false;
                        }
                        break;
                }

            }
        }
    }
}

