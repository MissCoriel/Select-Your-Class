using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using StardewModdingAPI.Utilities;
using System;
using System.Collections.Generic;
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
        public string className1;
        public string classDesc1;
        public string className2;
        public string classDesc2;
        public string className3;
        public string classDesc3;
        public string className4;
        public string classDesc4;


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
        
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (this.chooseClass1.containsPoint(x, y))
            {
                Game1.player.maxHealth = model.Class1.ModHP;
                Game1.player.health = model.Class1.ModHP;
                Game1.player.maxStamina.Value = model.Class1.ModStam;
                Game1.player.stamina = model.Class1.ModStam;
                
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
