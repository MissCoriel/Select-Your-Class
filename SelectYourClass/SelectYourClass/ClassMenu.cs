using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using StardewModdingAPI.Utilities;
using System;
using System.Collections.Generic;
using StardewValley.Events;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using Newtonsoft.Json;
using System.IO;
using StardewValley.BellsAndWhistles;

namespace SelectYourClass
{
    class ClassMenu : IClickableMenu
    {
        public ClickableTextureComponent chooseClass1;
        public ClickableTextureComponent chooseClass2;
        public ClickableTextureComponent chooseClass3;
        public ClickableTextureComponent chooseClass4;
        public ClickableTextureComponent chooseNone;
        public static string modDirectory;
        public SettingsModel model;
        public static string classCheck;
        


        internal class SettingsModel
        {
            public JobModel Class1 { get; set; }
            public JobModel Class2 { get; set; }
            public JobModel Class3 { get; set; }
            public JobModel Class4 { get; set; }
        }
        internal class JobModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int ModHP { get; set; }
            public int ModStam { get; set; }
            public int FarmingSkill { get; set; }
            public int ForageSkill { get; set; }
            public int FishingSkill { get; set; }
            public int MiningSkill { get; set; }
            public int CombatSkill { get; set; }
            public int AddFarmingLevel { get; set; }
            public int AddForagingLevel { get; set; }
            public int AddFishingLevel { get; set; }
            public int AddMiningLevel { get; set; }
            public int AddCombatLevel { get; set; }
            
        }
        public ClassMenu(IDataHelper helper, string modBaseDirectory) : base((int)Utility.getTopLeftPositionForCenteringOnScreen(Game1.viewport.Width, Game1.viewport.Height).X, (int)Utility.getTopLeftPositionForCenteringOnScreen(Game1.viewport.Width, Game1.viewport.Height).X, Game1.viewport.Width, Game1.viewport.Height, true)
        {

            modDirectory = modBaseDirectory;
            model = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(Path.Combine(modDirectory, "JobClass.json")));
            int menuX = (Game1.viewport.Width - (width - 1200)) / 2;
            int menuY = (Game1.viewport.Height - (height - 592)) / 2;

            this.chooseClass1 = new ClickableTextureComponent(new Rectangle(menuX + 512, menuY + 16, 64, 32), Game1.mouseCursors, new Rectangle(294, 428, 21, 11), 4f, false)
            {
                myID = 101,
                rightNeighborID = 102
            };
            this.chooseClass2 = new ClickableTextureComponent(new Rectangle(menuX + 512, menuY + 64, 64, 32), Game1.mouseCursors, new Rectangle(294, 428, 21, 11), 4f, false)
            {
                myID = 102,
                rightNeighborID = 103
            };
            this.chooseClass3 = new ClickableTextureComponent(new Rectangle(menuX + 512, menuY + 112, 64, 32), Game1.mouseCursors, new Rectangle(294, 428, 21, 11), 4f, false)
            {
                myID = 103,
                rightNeighborID = 104
            };
            this.chooseClass4 = new ClickableTextureComponent(new Rectangle(menuX + 512, menuY + 160, 64, 32), Game1.mouseCursors, new Rectangle(294, 428, 21, 11), 4f, false)
            {
                myID = 104,
                rightNeighborID = 105
            };
            this.chooseNone = new ClickableTextureComponent(new Rectangle(menuX + 512, menuY + 208, 64, 32), Game1.mouseCursors, new Rectangle(294, 428, 21, 11), 4f, false)
            {
                myID = 105,
                rightNeighborID = 101
            };

        }
        /* This was found to work with CJB Cheats,  I felt that it would be the best way to go about leveling.  Does not work in my context!!
        private int GetExperiencePoints(int level)
        {
            if (level < 0 || level > 9)
                return 0;

            int[] exp = { 100, 280, 390, 530, 850, 1150, 1500, 2100, 3100, 5000 };

            return exp[level];
        }*/

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (this.chooseClass1.containsPoint(x, y))
            {
               
                if (Context.IsMainPlayer)
                {
                    string selectedClass = model.Class1.Name;
                    SelectYourClass.Mod.TempMonitor.Log($"Chosen Class: {selectedClass}", LogLevel.Debug);
                    ClassMenu.classCheck = selectedClass;
                    Mod.Saving.Data.WriteSaveData("ClassChosen", new SaveModel { chosenClass = selectedClass });
                    SelectYourClass.Mod.TempMonitor.Log($"Save as {selectedClass}", LogLevel.Debug);
                }
                Game1.player.maxHealth = Game1.player.maxHealth + model.Class1.ModHP;
                if (model.Class1.ModHP > 0)
                {
                    Game1.player.health = Game1.player.maxHealth + model.Class1.ModHP;
                }
                Game1.player.maxStamina.Value = Game1.player.maxStamina.Value + model.Class1.ModStam;
                if (model.Class1.ModStam > 0)
                {
                    Game1.player.stamina = Game1.player.maxStamina.Value + model.Class1.ModStam;
                }
                SelectYourClass.Mod.TempMonitor.Log($"Modifying HP {model.Class1.ModHP} HP!  Modifying Stamina {model.Class1.ModStam} Stamina!", LogLevel.Debug);
                int readfarmlevelups = model.Class1.AddFarmingLevel;
                int readforagelevelups = model.Class1.AddForagingLevel;
                int readfishinglevelups = model.Class1.AddFishingLevel;
                int readmininglevelups = model.Class1.AddMiningLevel;
                int readcombatlevelups = model.Class1.AddCombatLevel;
                int farmlvlup = 0;
                int foragelvlup = 0;
                int fishinglvlup = 0;
                int mininglvlup = 0;
                int combatlvlup = 0;
                // Farming Level Check ~~
                if (readfarmlevelups == 1)
                {
                    farmlvlup = 100;
                }
                else if (readfarmlevelups == 2)
                {
                    farmlvlup = 380;
                }
                else if (readfarmlevelups == 3)
                {
                    farmlvlup = 770;
                }
                else if (readfarmlevelups == 4)
                {
                    farmlvlup = 1300;
                }
                else if (readfarmlevelups == 5)
                {
                    farmlvlup = 2150;
                }
                else if (readfarmlevelups == 6)
                {
                    farmlvlup = 3300;
                }
                else if (readfarmlevelups == 7)
                {
                    farmlvlup = 4800;
                }
                else if (readfarmlevelups == 8)
                {
                    farmlvlup = 6900;
                }
                else if (readfarmlevelups == 9)
                {
                    farmlvlup = 10000;
                }
                else if (readfarmlevelups == 10)
                {
                    farmlvlup = 15000;
                }
                else if (readfarmlevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Farm Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }
                // Foraging Level Check ~~
                if (readforagelevelups == 1)
                {
                    foragelvlup = 100;
                }
                else if (readforagelevelups == 2)
                {
                    foragelvlup = 380;
                }
                else if (readforagelevelups == 3)
                {
                    foragelvlup = 770;
                }
                else if (readforagelevelups == 4)
                {
                    foragelvlup = 1300;
                }
                else if (readforagelevelups == 5)
                {
                    foragelvlup = 2150;
                }
                else if (readforagelevelups == 6)
                {
                    foragelvlup = 3300;
                }
                else if (readforagelevelups == 7)
                {
                    foragelvlup = 4800;
                }
                else if (readforagelevelups == 8)
                {
                    foragelvlup = 6900;
                }
                else if (readforagelevelups == 9)
                {
                    foragelvlup = 10000;
                }
                else if (readforagelevelups == 10)
                {
                    foragelvlup = 15000;
                }
                else if (readforagelevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Forage Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }
                // Fishing Level Check ~~
                if (readfishinglevelups == 1)
                {
                    fishinglvlup = 100;
                }
                else if (readfishinglevelups == 2)
                {
                    fishinglvlup = 380;
                }
                else if (readfishinglevelups == 3)
                {
                    fishinglvlup = 770;
                }
                else if (readfishinglevelups == 4)
                {
                    fishinglvlup = 1300;
                }
                else if (readfishinglevelups == 5)
                {
                    fishinglvlup = 2150;
                }
                else if (readfishinglevelups == 6)
                {
                    fishinglvlup = 3300;
                }
                else if (readfishinglevelups == 7)
                {
                    fishinglvlup = 4800;
                }
                else if (readfishinglevelups == 8)
                {
                    fishinglvlup = 6900;
                }
                else if (readfishinglevelups == 9)
                {
                    fishinglvlup = 10000;
                }
                else if (readfishinglevelups == 10)
                {
                    fishinglvlup = 15000;
                }
                else if (readfishinglevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Fishing Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }
                // Mining Level Check ~~
                if (readmininglevelups == 1)
                {
                    mininglvlup = 100;
                }
                else if (readmininglevelups == 2)
                {
                    mininglvlup = 380;
                }
                else if (readmininglevelups == 3)
                {
                    mininglvlup = 770;
                }
                else if (readmininglevelups == 4)
                {
                    mininglvlup = 1300;
                }
                else if (readmininglevelups == 5)
                {
                    mininglvlup = 2150;
                }
                else if (readmininglevelups == 6)
                {
                    mininglvlup = 3300;
                }
                else if (readmininglevelups == 7)
                {
                    mininglvlup = 4800;
                }
                else if (readmininglevelups == 8)
                {
                    mininglvlup = 6900;
                }
                else if (readmininglevelups == 9)
                {
                    mininglvlup = 10000;
                }
                else if (readmininglevelups == 10)
                {
                    mininglvlup = 15000;
                }
                else if (readmininglevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Mining Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }
                // Combat Level Check ~~
                if (readcombatlevelups == 1)
                {
                    combatlvlup = 100;
                }
                else if (readcombatlevelups == 2)
                {
                    combatlvlup = 380;
                }
                else if (readcombatlevelups == 3)
                {
                    combatlvlup = 770;
                }
                else if (readcombatlevelups == 4)
                {
                    combatlvlup = 1300;
                }
                else if (readcombatlevelups == 5)
                {
                    combatlvlup = 2150;
                }
                else if (readcombatlevelups == 6)
                {
                    combatlvlup = 3300;
                }
                else if (readcombatlevelups == 7)
                {
                    combatlvlup = 4800;
                }
                else if (readcombatlevelups == 8)
                {
                    combatlvlup = 6900;
                }
                else if (readcombatlevelups == 9)
                {
                    combatlvlup = 10000;
                }
                else if (readcombatlevelups == 10)
                {
                    combatlvlup = 15000;
                }
                else if (readcombatlevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Combat Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }

                IList<Point> newfarmLevels = Game1.player.newLevels;
                int wasNewfarmLevels = newfarmLevels.Count;
                Game1.player.gainExperience(0, farmlvlup);
                if (newfarmLevels.Count > wasNewfarmLevels)
                   newfarmLevels.RemoveAt(newfarmLevels.Count - 1);

                IList<Point> newforageLevels = Game1.player.newLevels;
                int wasNewforageLevels = newforageLevels.Count;
                Game1.player.gainExperience(2, foragelvlup);
                SelectYourClass.Mod.TempMonitor.Log($"Adding {foragelvlup} foraging XP", LogLevel.Debug);
                if (newforageLevels.Count > wasNewforageLevels)
                    newforageLevels.RemoveAt(newforageLevels.Count - 1);

                IList<Point> newfishingLevels = Game1.player.newLevels;
                int wasNewfishingLevels = newfishingLevels.Count;
                Game1.player.gainExperience(1, fishinglvlup);
                if (newfishingLevels.Count > wasNewfishingLevels)
                    newfishingLevels.RemoveAt(newfishingLevels.Count - 1);

                IList<Point> newminingLevels = Game1.player.newLevels;
                int wasNewminingLevels = newminingLevels.Count;
                Game1.player.gainExperience(3, mininglvlup);
                if (newminingLevels.Count > wasNewminingLevels)
                    newminingLevels.RemoveAt(newminingLevels.Count - 1);

                IList<Point> newcombatLevels = Game1.player.newLevels;
                int wasNewcombatLevels = newcombatLevels.Count;
                Game1.player.gainExperience(4, combatlvlup);
                if (newcombatLevels.Count > wasNewcombatLevels)
                    newcombatLevels.RemoveAt(newcombatLevels.Count - 1);


                Game1.addHUDMessage(new HUDMessage($"{Game1.player} has selected {model.Class1.Name}"));
                
                Game1.exitActiveMenu();
            }
            if (this.chooseClass2.containsPoint(x, y))
            {


                Game1.player.maxHealth = Game1.player.maxHealth + model.Class2.ModHP;
                if (model.Class2.ModHP > 0)
                {
                    Game1.player.health = Game1.player.maxHealth + model.Class2.ModHP;
                }
                Game1.player.maxStamina.Value = Game1.player.maxStamina.Value + model.Class2.ModStam;
                if (model.Class2.ModStam > 0)
                {
                    Game1.player.stamina = Game1.player.maxStamina.Value + model.Class2.ModStam;
                }
                SelectYourClass.Mod.TempMonitor.Log($"Modifying HP {model.Class2.ModHP} HP!  Modifying Stamina {model.Class2.ModStam} Stamina!", LogLevel.Debug);
                int readfarmlevelups = model.Class2.AddFarmingLevel;
                int readforagelevelups = model.Class2.AddForagingLevel;
                int readfishinglevelups = model.Class2.AddFishingLevel;
                int readmininglevelups = model.Class2.AddMiningLevel;
                int readcombatlevelups = model.Class2.AddCombatLevel;
                int farmlvlup = 0;
                int foragelvlup = 0;
                int fishinglvlup = 0;
                int mininglvlup = 0;
                int combatlvlup = 0;
                // Farming Level Check ~~
                if (readfarmlevelups == 1)
                {
                    farmlvlup = 100;
                }
                else if (readfarmlevelups == 2)
                {
                    farmlvlup = 380;
                }
                else if (readfarmlevelups == 3)
                {
                    farmlvlup = 770;
                }
                else if (readfarmlevelups == 4)
                {
                    farmlvlup = 1300;
                }
                else if (readfarmlevelups == 5)
                {
                    farmlvlup = 2150;
                }
                else if (readfarmlevelups == 6)
                {
                    farmlvlup = 3300;
                }
                else if (readfarmlevelups == 7)
                {
                    farmlvlup = 4800;
                }
                else if (readfarmlevelups == 8)
                {
                    farmlvlup = 6900;
                }
                else if (readfarmlevelups == 9)
                {
                    farmlvlup = 10000;
                }
                else if (readfarmlevelups == 10)
                {
                    farmlvlup = 15000;
                }
                else if (readfarmlevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Farm Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }
                // Foraging Level Check ~~
                if (readforagelevelups == 1)
                {
                    foragelvlup = 100;
                }
                else if (readforagelevelups == 2)
                {
                    foragelvlup = 380;
                }
                else if (readforagelevelups == 3)
                {
                    foragelvlup = 770;
                }
                else if (readforagelevelups == 4)
                {
                    foragelvlup = 1300;
                }
                else if (readforagelevelups == 5)
                {
                    foragelvlup = 2150;
                }
                else if (readforagelevelups == 6)
                {
                    foragelvlup = 3300;
                }
                else if (readforagelevelups == 7)
                {
                    foragelvlup = 4800;
                }
                else if (readforagelevelups == 8)
                {
                    foragelvlup = 6900;
                }
                else if (readforagelevelups == 9)
                {
                    foragelvlup = 10000;
                }
                else if (readforagelevelups == 10)
                {
                    foragelvlup = 15000;
                }
                else if (readforagelevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Forage Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }
                // Fishing Level Check ~~
                if (readfishinglevelups == 1)
                {
                    fishinglvlup = 100;
                }
                else if (readfishinglevelups == 2)
                {
                    fishinglvlup = 380;
                }
                else if (readfishinglevelups == 3)
                {
                    fishinglvlup = 770;
                }
                else if (readfishinglevelups == 4)
                {
                    fishinglvlup = 1300;
                }
                else if (readfishinglevelups == 5)
                {
                    fishinglvlup = 2150;
                }
                else if (readfishinglevelups == 6)
                {
                    fishinglvlup = 3300;
                }
                else if (readfishinglevelups == 7)
                {
                    fishinglvlup = 4800;
                }
                else if (readfishinglevelups == 8)
                {
                    fishinglvlup = 6900;
                }
                else if (readfishinglevelups == 9)
                {
                    fishinglvlup = 10000;
                }
                else if (readfishinglevelups == 10)
                {
                    fishinglvlup = 15000;
                }
                else if (readfishinglevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Fishing Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }
                // Mining Level Check ~~
                if (readmininglevelups == 1)
                {
                    mininglvlup = 100;
                }
                else if (readmininglevelups == 2)
                {
                    mininglvlup = 380;
                }
                else if (readmininglevelups == 3)
                {
                    mininglvlup = 770;
                }
                else if (readmininglevelups == 4)
                {
                    mininglvlup = 1300;
                }
                else if (readmininglevelups == 5)
                {
                    mininglvlup = 2150;
                }
                else if (readmininglevelups == 6)
                {
                    mininglvlup = 3300;
                }
                else if (readmininglevelups == 7)
                {
                    mininglvlup = 4800;
                }
                else if (readmininglevelups == 8)
                {
                    mininglvlup = 6900;
                }
                else if (readmininglevelups == 9)
                {
                    mininglvlup = 10000;
                }
                else if (readmininglevelups == 10)
                {
                    mininglvlup = 15000;
                }
                else if (readmininglevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Mining Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }
                // Combat Level Check ~~
                if (readcombatlevelups == 1)
                {
                    combatlvlup = 100;
                }
                else if (readcombatlevelups == 2)
                {
                    combatlvlup = 380;
                }
                else if (readcombatlevelups == 3)
                {
                    combatlvlup = 770;
                }
                else if (readcombatlevelups == 4)
                {
                    combatlvlup = 1300;
                }
                else if (readcombatlevelups == 5)
                {
                    combatlvlup = 2150;
                }
                else if (readcombatlevelups == 6)
                {
                    combatlvlup = 3300;
                }
                else if (readcombatlevelups == 7)
                {
                    combatlvlup = 4800;
                }
                else if (readcombatlevelups == 8)
                {
                    combatlvlup = 6900;
                }
                else if (readcombatlevelups == 9)
                {
                    combatlvlup = 10000;
                }
                else if (readcombatlevelups == 10)
                {
                    combatlvlup = 15000;
                }
                else if (readcombatlevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Combat Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }

                IList<Point> newfarmLevels = Game1.player.newLevels;
                int wasNewfarmLevels = newfarmLevels.Count;
                Game1.player.gainExperience(0, farmlvlup);
                if (newfarmLevels.Count > wasNewfarmLevels)
                    newfarmLevels.RemoveAt(newfarmLevels.Count - 1);

                IList<Point> newforageLevels = Game1.player.newLevels;
                int wasNewforageLevels = newforageLevels.Count;
                Game1.player.gainExperience(2, foragelvlup);
                SelectYourClass.Mod.TempMonitor.Log($"Adding {foragelvlup} foraging XP", LogLevel.Debug);
                if (newforageLevels.Count > wasNewforageLevels)
                    newforageLevels.RemoveAt(newforageLevels.Count - 1);

                IList<Point> newfishingLevels = Game1.player.newLevels;
                int wasNewfishingLevels = newfishingLevels.Count;
                Game1.player.gainExperience(1, fishinglvlup);
                if (newfishingLevels.Count > wasNewfishingLevels)
                    newfishingLevels.RemoveAt(newfishingLevels.Count - 1);

                IList<Point> newminingLevels = Game1.player.newLevels;
                int wasNewminingLevels = newminingLevels.Count;
                Game1.player.gainExperience(3, mininglvlup);
                if (newminingLevels.Count > wasNewminingLevels)
                    newminingLevels.RemoveAt(newminingLevels.Count - 1);

                IList<Point> newcombatLevels = Game1.player.newLevels;
                int wasNewcombatLevels = newcombatLevels.Count;
                Game1.player.gainExperience(4, combatlvlup);
                if (newcombatLevels.Count > wasNewcombatLevels)
                    newcombatLevels.RemoveAt(newcombatLevels.Count - 1);


                Game1.addHUDMessage(new HUDMessage($"{Game1.player} has selected {model.Class2.Name}"));
                Game1.exitActiveMenu();
            }
            if (this.chooseClass3.containsPoint(x, y))
            {


                Game1.player.maxHealth = Game1.player.maxHealth + model.Class3.ModHP;
                if (model.Class3.ModHP > 0)
                {
                    Game1.player.health = Game1.player.maxHealth + model.Class3.ModHP;
                }
                Game1.player.maxStamina.Value = Game1.player.maxStamina.Value + model.Class3.ModStam;
                if (model.Class3.ModStam > 0)
                {
                    Game1.player.stamina = Game1.player.maxStamina.Value + model.Class3.ModStam;
                }
                SelectYourClass.Mod.TempMonitor.Log($"Modifying HP {model.Class3.ModHP} HP!  Modifying Stamina {model.Class3.ModStam} Stamina!", LogLevel.Debug);
                int readfarmlevelups = model.Class3.AddFarmingLevel;
                int readforagelevelups = model.Class3.AddForagingLevel;
                int readfishinglevelups = model.Class3.AddFishingLevel;
                int readmininglevelups = model.Class3.AddMiningLevel;
                int readcombatlevelups = model.Class3.AddCombatLevel;
                int farmlvlup = 0;
                int foragelvlup = 0;
                int fishinglvlup = 0;
                int mininglvlup = 0;
                int combatlvlup = 0;
                // Farming Level Check ~~
                if (readfarmlevelups == 1)
                {
                    farmlvlup = 100;
                }
                else if (readfarmlevelups == 2)
                {
                    farmlvlup = 380;
                }
                else if (readfarmlevelups == 3)
                {
                    farmlvlup = 770;
                }
                else if (readfarmlevelups == 4)
                {
                    farmlvlup = 1300;
                }
                else if (readfarmlevelups == 5)
                {
                    farmlvlup = 2150;
                }
                else if (readfarmlevelups == 6)
                {
                    farmlvlup = 3300;
                }
                else if (readfarmlevelups == 7)
                {
                    farmlvlup = 4800;
                }
                else if (readfarmlevelups == 8)
                {
                    farmlvlup = 6900;
                }
                else if (readfarmlevelups == 9)
                {
                    farmlvlup = 10000;
                }
                else if (readfarmlevelups == 10)
                {
                    farmlvlup = 15000;
                }
                else if (readfarmlevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Farm Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }
                // Foraging Level Check ~~
                if (readforagelevelups == 1)
                {
                    foragelvlup = 100;
                }
                else if (readforagelevelups == 2)
                {
                    foragelvlup = 380;
                }
                else if (readforagelevelups == 3)
                {
                    foragelvlup = 770;
                }
                else if (readforagelevelups == 4)
                {
                    foragelvlup = 1300;
                }
                else if (readforagelevelups == 5)
                {
                    foragelvlup = 2150;
                }
                else if (readforagelevelups == 6)
                {
                    foragelvlup = 3300;
                }
                else if (readforagelevelups == 7)
                {
                    foragelvlup = 4800;
                }
                else if (readforagelevelups == 8)
                {
                    foragelvlup = 6900;
                }
                else if (readforagelevelups == 9)
                {
                    foragelvlup = 10000;
                }
                else if (readforagelevelups == 10)
                {
                    foragelvlup = 15000;
                }
                else if (readforagelevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Forage Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }
                // Fishing Level Check ~~
                if (readfishinglevelups == 1)
                {
                    fishinglvlup = 100;
                }
                else if (readfishinglevelups == 2)
                {
                    fishinglvlup = 380;
                }
                else if (readfishinglevelups == 3)
                {
                    fishinglvlup = 770;
                }
                else if (readfishinglevelups == 4)
                {
                    fishinglvlup = 1300;
                }
                else if (readfishinglevelups == 5)
                {
                    fishinglvlup = 2150;
                }
                else if (readfishinglevelups == 6)
                {
                    fishinglvlup = 3300;
                }
                else if (readfishinglevelups == 7)
                {
                    fishinglvlup = 4800;
                }
                else if (readfishinglevelups == 8)
                {
                    fishinglvlup = 6900;
                }
                else if (readfishinglevelups == 9)
                {
                    fishinglvlup = 10000;
                }
                else if (readfishinglevelups == 10)
                {
                    fishinglvlup = 15000;
                }
                else if (readfishinglevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Fishing Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }
                // Mining Level Check ~~
                if (readmininglevelups == 1)
                {
                    mininglvlup = 100;
                }
                else if (readmininglevelups == 2)
                {
                    mininglvlup = 380;
                }
                else if (readmininglevelups == 3)
                {
                    mininglvlup = 770;
                }
                else if (readmininglevelups == 4)
                {
                    mininglvlup = 1300;
                }
                else if (readmininglevelups == 5)
                {
                    mininglvlup = 2150;
                }
                else if (readmininglevelups == 6)
                {
                    mininglvlup = 3300;
                }
                else if (readmininglevelups == 7)
                {
                    mininglvlup = 4800;
                }
                else if (readmininglevelups == 8)
                {
                    mininglvlup = 6900;
                }
                else if (readmininglevelups == 9)
                {
                    mininglvlup = 10000;
                }
                else if (readmininglevelups == 10)
                {
                    mininglvlup = 15000;
                }
                else if (readmininglevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Mining Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }
                // Combat Level Check ~~
                if (readcombatlevelups == 1)
                {
                    combatlvlup = 100;
                }
                else if (readcombatlevelups == 2)
                {
                    combatlvlup = 380;
                }
                else if (readcombatlevelups == 3)
                {
                    combatlvlup = 770;
                }
                else if (readcombatlevelups == 4)
                {
                    combatlvlup = 1300;
                }
                else if (readcombatlevelups == 5)
                {
                    combatlvlup = 2150;
                }
                else if (readcombatlevelups == 6)
                {
                    combatlvlup = 3300;
                }
                else if (readcombatlevelups == 7)
                {
                    combatlvlup = 4800;
                }
                else if (readcombatlevelups == 8)
                {
                    combatlvlup = 6900;
                }
                else if (readcombatlevelups == 9)
                {
                    combatlvlup = 10000;
                }
                else if (readcombatlevelups == 10)
                {
                    combatlvlup = 15000;
                }
                else if (readcombatlevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Combat Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }

                IList<Point> newfarmLevels = Game1.player.newLevels;
                int wasNewfarmLevels = newfarmLevels.Count;
                Game1.player.gainExperience(0, farmlvlup);
                if (newfarmLevels.Count > wasNewfarmLevels)
                    newfarmLevels.RemoveAt(newfarmLevels.Count - 1);

                IList<Point> newforageLevels = Game1.player.newLevels;
                int wasNewforageLevels = newforageLevels.Count;
                Game1.player.gainExperience(2, foragelvlup);
                SelectYourClass.Mod.TempMonitor.Log($"Adding {foragelvlup} foraging XP", LogLevel.Debug);
                if (newforageLevels.Count > wasNewforageLevels)
                    newforageLevels.RemoveAt(newforageLevels.Count - 1);

                IList<Point> newfishingLevels = Game1.player.newLevels;
                int wasNewfishingLevels = newfishingLevels.Count;
                Game1.player.gainExperience(1, fishinglvlup);
                if (newfishingLevels.Count > wasNewfishingLevels)
                    newfishingLevels.RemoveAt(newfishingLevels.Count - 1);

                IList<Point> newminingLevels = Game1.player.newLevels;
                int wasNewminingLevels = newminingLevels.Count;
                Game1.player.gainExperience(3, mininglvlup);
                if (newminingLevels.Count > wasNewminingLevels)
                    newminingLevels.RemoveAt(newminingLevels.Count - 1);

                IList<Point> newcombatLevels = Game1.player.newLevels;
                int wasNewcombatLevels = newcombatLevels.Count;
                Game1.player.gainExperience(4, combatlvlup);
                if (newcombatLevels.Count > wasNewcombatLevels)
                    newcombatLevels.RemoveAt(newcombatLevels.Count - 1);


                Game1.addHUDMessage(new HUDMessage($"{Game1.player} has selected {model.Class3.Name}"));
                Game1.exitActiveMenu();
            }
            if (this.chooseClass4.containsPoint(x, y))
            {


                Game1.player.maxHealth = Game1.player.maxHealth + model.Class4.ModHP;
                if (model.Class4.ModHP > 0)
                {
                    Game1.player.health = Game1.player.maxHealth + model.Class4.ModHP;
                }
                Game1.player.maxStamina.Value = Game1.player.maxStamina.Value + model.Class4.ModStam;
                if (model.Class4.ModStam > 0)
                {
                    Game1.player.stamina = Game1.player.maxStamina.Value + model.Class4.ModStam;
                }
                SelectYourClass.Mod.TempMonitor.Log($"Modifying HP {model.Class4.ModHP} HP!  Modifying Stamina {model.Class4.ModStam} Stamina!", LogLevel.Debug);
                int readfarmlevelups = model.Class4.AddFarmingLevel;
                int readforagelevelups = model.Class4.AddForagingLevel;
                int readfishinglevelups = model.Class4.AddFishingLevel;
                int readmininglevelups = model.Class4.AddMiningLevel;
                int readcombatlevelups = model.Class4.AddCombatLevel;
                int farmlvlup = 0;
                int foragelvlup = 0;
                int fishinglvlup = 0;
                int mininglvlup = 0;
                int combatlvlup = 0;
                // Farming Level Check ~~
                if (readfarmlevelups == 1)
                {
                    farmlvlup = 100;
                }
                else if (readfarmlevelups == 2)
                {
                    farmlvlup = 380;
                }
                else if (readfarmlevelups == 3)
                {
                    farmlvlup = 770;
                }
                else if (readfarmlevelups == 4)
                {
                    farmlvlup = 1300;
                }
                else if (readfarmlevelups == 5)
                {
                    farmlvlup = 2150;
                }
                else if (readfarmlevelups == 6)
                {
                    farmlvlup = 3300;
                }
                else if (readfarmlevelups == 7)
                {
                    farmlvlup = 4800;
                }
                else if (readfarmlevelups == 8)
                {
                    farmlvlup = 6900;
                }
                else if (readfarmlevelups == 9)
                {
                    farmlvlup = 10000;
                }
                else if (readfarmlevelups == 10)
                {
                    farmlvlup = 15000;
                }
                else if (readfarmlevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Farm Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }
                // Foraging Level Check ~~
                if (readforagelevelups == 1)
                {
                    foragelvlup = 100;
                }
                else if (readforagelevelups == 2)
                {
                    foragelvlup = 380;
                }
                else if (readforagelevelups == 3)
                {
                    foragelvlup = 770;
                }
                else if (readforagelevelups == 4)
                {
                    foragelvlup = 1300;
                }
                else if (readforagelevelups == 5)
                {
                    foragelvlup = 2150;
                }
                else if (readforagelevelups == 6)
                {
                    foragelvlup = 3300;
                }
                else if (readforagelevelups == 7)
                {
                    foragelvlup = 4800;
                }
                else if (readforagelevelups == 8)
                {
                    foragelvlup = 6900;
                }
                else if (readforagelevelups == 9)
                {
                    foragelvlup = 10000;
                }
                else if (readforagelevelups == 10)
                {
                    foragelvlup = 15000;
                }
                else if (readforagelevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Forage Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }
                // Fishing Level Check ~~
                if (readfishinglevelups == 1)
                {
                    fishinglvlup = 100;
                }
                else if (readfishinglevelups == 2)
                {
                    fishinglvlup = 380;
                }
                else if (readfishinglevelups == 3)
                {
                    fishinglvlup = 770;
                }
                else if (readfishinglevelups == 4)
                {
                    fishinglvlup = 1300;
                }
                else if (readfishinglevelups == 5)
                {
                    fishinglvlup = 2150;
                }
                else if (readfishinglevelups == 6)
                {
                    fishinglvlup = 3300;
                }
                else if (readfishinglevelups == 7)
                {
                    fishinglvlup = 4800;
                }
                else if (readfishinglevelups == 8)
                {
                    fishinglvlup = 6900;
                }
                else if (readfishinglevelups == 9)
                {
                    fishinglvlup = 10000;
                }
                else if (readfishinglevelups == 10)
                {
                    fishinglvlup = 15000;
                }
                else if (readfishinglevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Fishing Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }
                // Mining Level Check ~~
                if (readmininglevelups == 1)
                {
                    mininglvlup = 100;
                }
                else if (readmininglevelups == 2)
                {
                    mininglvlup = 380;
                }
                else if (readmininglevelups == 3)
                {
                    mininglvlup = 770;
                }
                else if (readmininglevelups == 4)
                {
                    mininglvlup = 1300;
                }
                else if (readmininglevelups == 5)
                {
                    mininglvlup = 2150;
                }
                else if (readmininglevelups == 6)
                {
                    mininglvlup = 3300;
                }
                else if (readmininglevelups == 7)
                {
                    mininglvlup = 4800;
                }
                else if (readmininglevelups == 8)
                {
                    mininglvlup = 6900;
                }
                else if (readmininglevelups == 9)
                {
                    mininglvlup = 10000;
                }
                else if (readmininglevelups == 10)
                {
                    mininglvlup = 15000;
                }
                else if (readmininglevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Mining Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }
                // Combat Level Check ~~
                if (readcombatlevelups == 1)
                {
                    combatlvlup = 100;
                }
                else if (readcombatlevelups == 2)
                {
                    combatlvlup = 380;
                }
                else if (readcombatlevelups == 3)
                {
                    combatlvlup = 770;
                }
                else if (readcombatlevelups == 4)
                {
                    combatlvlup = 1300;
                }
                else if (readcombatlevelups == 5)
                {
                    combatlvlup = 2150;
                }
                else if (readcombatlevelups == 6)
                {
                    combatlvlup = 3300;
                }
                else if (readcombatlevelups == 7)
                {
                    combatlvlup = 4800;
                }
                else if (readcombatlevelups == 8)
                {
                    combatlvlup = 6900;
                }
                else if (readcombatlevelups == 9)
                {
                    combatlvlup = 10000;
                }
                else if (readcombatlevelups == 10)
                {
                    combatlvlup = 15000;
                }
                else if (readcombatlevelups == 0)
                {
                    SelectYourClass.Mod.TempMonitor.Log("Class gains no Combat Levels!", LogLevel.Debug);
                }
                else
                {
                    SelectYourClass.Mod.TempMonitor.Log("Invalid level count added!  Valid entries are 0 through 10", LogLevel.Error);
                }

                IList<Point> newfarmLevels = Game1.player.newLevels;
                int wasNewfarmLevels = newfarmLevels.Count;
                Game1.player.gainExperience(0, farmlvlup);
                if (newfarmLevels.Count > wasNewfarmLevels)
                    newfarmLevels.RemoveAt(newfarmLevels.Count - 1);

                IList<Point> newforageLevels = Game1.player.newLevels;
                int wasNewforageLevels = newforageLevels.Count;
                Game1.player.gainExperience(2, foragelvlup);                
                if (newforageLevels.Count > wasNewforageLevels)
                    newforageLevels.RemoveAt(newforageLevels.Count - 1);

                IList<Point> newfishingLevels = Game1.player.newLevels;
                int wasNewfishingLevels = newfishingLevels.Count;
                Game1.player.gainExperience(1, fishinglvlup);
                if (newfishingLevels.Count > wasNewfishingLevels)
                    newfishingLevels.RemoveAt(newfishingLevels.Count - 1);

                IList<Point> newminingLevels = Game1.player.newLevels;
                int wasNewminingLevels = newminingLevels.Count;
                Game1.player.gainExperience(3, mininglvlup);
                if (newminingLevels.Count > wasNewminingLevels)
                    newminingLevels.RemoveAt(newminingLevels.Count - 1);

                IList<Point> newcombatLevels = Game1.player.newLevels;
                int wasNewcombatLevels = newcombatLevels.Count;
                Game1.player.gainExperience(4, combatlvlup);
                if (newcombatLevels.Count > wasNewcombatLevels)
                    newcombatLevels.RemoveAt(newcombatLevels.Count - 1);


                Game1.addHUDMessage(new HUDMessage($"{Game1.player} has selected {model.Class4.Name}"));
                Game1.exitActiveMenu();
            }
            if (this.chooseNone.containsPoint(x, y))
            {
                Game1.addHUDMessage(new HUDMessage($"{Game1.player} chose not to select a Class!"));
                Game1.exitActiveMenu();
            }
        }
        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            this.xPositionOnScreen = (int)Utility.getTopLeftPositionForCenteringOnScreen(1280, 720, 0, 0).X;
            this.yPositionOnScreen = (int)Utility.getTopLeftPositionForCenteringOnScreen(1280, 720, 0, 0).Y;

        }
        public override void draw(SpriteBatch b)
        {
            int menuX = (Game1.viewport.Width - (width - 1200)) / 2;
            int menuY = (Game1.viewport.Height - (height - 592)) / 2;

            //SelectYourClass.Mod.TempMonitor.Log($"Class1 Name:" + model.Class1.Name, LogLevel.Debug);

            b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.White * 0.4f);
            Vector2 titleSize = Game1.dialogueFont.MeasureString("Please Choose a Class");
            drawTextureBox(b, menuX, menuY - 80, (int)titleSize.X + 32, (int)titleSize.Y + 32, Color.White);
            Utility.drawTextWithShadow(b, "Please Choose a Class", Game1.dialogueFont, new Vector2(menuX + 16, menuY - 64), Game1.textColor);
            drawTextureBox(b, menuX, menuY, width - 1200, height - 592, Color.White);
            Utility.drawTextWithShadow(b, model.Class1.Name, Game1.dialogueFont, new Vector2(menuX + 16, menuY + 16), Game1.textColor);
            Utility.drawTextWithShadow(b, model.Class2.Name, Game1.dialogueFont, new Vector2(menuX + 16, menuY + 64), Game1.textColor);
            Utility.drawTextWithShadow(b, model.Class3.Name, Game1.dialogueFont, new Vector2(menuX + 16, menuY + 112), Game1.textColor);
            Utility.drawTextWithShadow(b, model.Class4.Name, Game1.dialogueFont, new Vector2(menuX + 16, menuY + 160), Game1.textColor);
            Utility.drawTextWithShadow(b, "No Class (Vanilla Stats)", Game1.dialogueFont, new Vector2(menuX + 16, menuY + 208), Game1.textColor);

            chooseClass1.draw(b);
            chooseClass2.draw(b);
            chooseClass3.draw(b);
            chooseClass4.draw(b);
            chooseNone.draw(b);
            if (this.chooseClass1.containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()))
            {
                IClickableMenu.drawHoverText(b, model.Class1.Description, Game1.smallFont);
            }
            if (this.chooseClass2.containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()))
            {
                IClickableMenu.drawHoverText(b, model.Class2.Description, Game1.smallFont);
            }
            if (this.chooseClass3.containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()))
            {
                IClickableMenu.drawHoverText(b, model.Class3.Description, Game1.smallFont);
            }
            if (this.chooseClass4.containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()))
            {
                IClickableMenu.drawHoverText(b, model.Class4.Description, Game1.smallFont);
            }
            if (this.chooseNone.containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()))
            {
                IClickableMenu.drawHoverText(b, "Select no Class and play as a regular player.", Game1.smallFont);
            }
            drawMouse(b);

        }

    }
}
