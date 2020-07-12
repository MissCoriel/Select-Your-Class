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
            helper.Events.GameLoop.Saving += this.OnSaving;
        }
        public void OnSaving(object sender, SavingEventArgs e)
        {
            if (classCheck != null)
            {
                this.Helper.Data.WriteSaveData("")
            }
        }
        public void SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            if (Context.IsMainPlayer)
            {   
                try
                {
                    var checkClass = this.Helper.Data.ReadSaveData<SaveModel>("ClassChosen");
                    if(checkClass == null || checkClass.chosenClass == null)
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





               /* if (this.chosenClass == null)
                {
                    classCheck = ClassMenu.classCheck;
                    Monitor.Log($"Checking Class: {classCheck}", LogLevel.Debug);
                    if (classCheck == null)
                    {
                        if (Game1.activeClickableMenu == null)
                        {
                            Game1.activeClickableMenu = new ClassMenu(this.Helper.Data, this.Helper.DirectoryPath);

                        }

                    }

                }*/
            }
                
            else
            {
                this.IsFetchingJobFromHost = true;
                this.Helper.Multiplayer.SendMessage(new RequestClassMessage { PlayerId = (int)Game1.player.uniqueMultiplayerID }, nameof(RequestClassMessage), playerIDs: new[] { Game1.MasterPlayer.UniqueMultiplayerID });
            }
            
            

                /*if (Game1.activeClickableMenu == null)
                {
                    Game1.activeClickableMenu = new ClassMenu(this.Helper.Data, this.Helper.DirectoryPath);
                    choseClass = true;
                }*/
            
        }
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            classCheck = ClassMenu.classCheck;
            if (classCheck == null)
            {
                classCheck = "Not Selected";
            }
            if (Context.IsMainPlayer && classCheck == "Not Selected" && Game1.activeClickableMenu == null)
            {
                Game1.activeClickableMenu = new ClassMenu(this.Helper.Data, this.Helper.DirectoryPath);
            }

            /*if (Context.IsPlayerFree && this.chosenClass == null && !this.IsFetchingJobFromHost)
            {
                Game1.activeClickableMenu = new ClassMenu(this.Helper.Data, this.Helper.DirectoryPath);
                // TODO: save selected class to this.Job when the menu closes
            }*/
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
                            var message = e.ReadAs<RequestClassMessage>();
                            var job = this.Helper.Data.ReadSaveData<SaveModel>($"job.{message.PlayerId}");
                            this.Helper.Multiplayer.SendMessage(new ClassInfoMessage { PlayerId = message.PlayerId, chosenClass = job }, nameof(ClassInfoMessage), playerIDs: new[] { e.FromPlayerID });
                        }
                        break;
                

             // farmhand: receive job data for the current player
                    case nameof(ClassInfoMessage):
                        if (!Context.IsMainPlayer)
                        {
                            var message = e.ReadAs<ClassInfoMessage>();
                            classSelect = message.chosenClass;
                            this.IsFetchingJobFromHost = false;
                        }
                        break;
                }

            }
        }
    }
}

