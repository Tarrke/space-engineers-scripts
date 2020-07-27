#region Prelude
using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using VRageMath;
using VRage.Game;
using VRage.Collections;
using Sandbox.ModAPI.Ingame;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Game.EntityComponents;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;

// Change this namespace for each script you create.
namespace SpaceEngineers.UWBlockPrograms.tipsAndTricks {
    public sealed class Program : MyGridProgram {
    // Your code goes between the next #endregion and #region
#endregion

public Program() {
    // Run evert 100 ticks
    Runtime.UpdateFrequency = UpdateFrequency.Update100;
}

public findStorageCapacity() {
    string cargoGroupName = "Base Cargo";
    string LCDName = "[MCD] Cargo";
    IMyBlockGroup cargos = GridTerminalSystem.GetBlockGroupWithName(cargoGroupName);
    List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
    cargos.GetBlocks(blocks);
    float capacity = 0;
    float usage = 0;
    foreach( var block in blocks ) {
        IMyCargoContainer cargo = block as IMyCargoContainer;
        capacity += (float) cargo.GetInventory(0).MaxVolume;
        usage    += (float) cargo.GetInventory(0).CurrentVolume;
    }
    float pctUsed = 100.0f * usage / capacity;
    IMyTextSurface lcd = GridTerminalSystem.GetBlockWithName(LCDName) as IMyTextSurface;

    // If we found that LCD, let's write the graph to it.
    if (lcd != null) {
        // Start the display text with a title and our space used in %.
        string displayText = String.Format("Capacity\nOverall: {0}%\n", (int)pctUsed);
        // Build the graph from the top down.
        for (int x = 0; x <= 10; x++) {
            if (pctUsed >= 100 - x * 10) {
                displayText += "    |    -----    |\n";
            } else {
                displayText += "    |              |\n";
            }
        }
        // Show the result on the LCD.
        //lcd.ShowTextureOnScreen();
        lcd.ContentType = ContentType.TEXT_AND_IMAGE;
        lcd.FontSize = 2;
        lcd.WriteText(displayText, false);
        //lcd.ShowPublicTextOnScreen();
    }

    /*
    // Now let's find the warning sign.
    lcd = GridTerminalSystem.GetBlockWithName(warningName) as IMyTextPanel;

    // If we found the warning sign, decide whether to have it turned on or off.
    if (lcd != null) {
        // If we've used more than 80% of our cargo, have it turn on. Else, have it turn off.
        if (pctUsed > 80) {
            lcd.ApplyAction("OnOff_On");
        } else {
            lcd.ApplyAction("OnOff_Off");
        }
    }
    */
}

public void Main(string args) {
    // Get a LCD and write to it
    IMyTerminalBlock block = GridTerminalSystem.GetBlockWithName("[LCD]");
    IMyTextPanel myPanel = block as IMyTextPanel;
    IMyTextSurface myTextSurface = myPanel as IMyTextSurface;
    myTextSurface.ContentType = ContentType.TEXT_AND_IMAGE;
    myTextSurface.FontSize = 2;
    string message = "";
    myTextSurface.WriteText(message);
}

#region PreludeFooter
    }
}
#endregion