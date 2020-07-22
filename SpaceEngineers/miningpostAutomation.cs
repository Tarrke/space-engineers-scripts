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
namespace SpaceEngineers.UWBlockPrograms.MiningAutomation {
    public sealed class Program : MyGridProgram {
    // Your code goes between the next #endregion and #region
#endregion

public Program()
{
    // Run evert 100 ticks
    Runtime.UpdateFrequency = UpdateFrequency.Update100;
}

public void Save()
{
    // Called when the program needs to save its state. Use
    // this method to save your state to the Storage field
    // or some other means. 
}

// Some Global Vars
bool shouldRun = true;

public void Main(string argument, UpdateType updateSource)
{
    // The main entry point of the script, invoked every time
    // one of the programmable block's Run actions are invoked,
    // or the script updates itself. The updateSource argument
    // describes where the update came from.

    // ~~~~~~ Configuration ~~~~~~ //
    string pistonUpName = "Pistons UP";
    string pistonDwName = "Pistons Down";
    string pistonLtName = "Pistons Lateral";
    string drillsName   = "Drills";
    // ~~~~~~ Configuration ~~~~~~ //

    Echo ("Mining Operations");
    // List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
    // GridTerminalSystem.GetBlocks(blocks);

    float pistonUpPosition = 0;
    float pistonUpMaxPosition = 0;
    float pistonUpMinPosition = 0;
    float pistonDwPosition = 0;
    float pistonDwMaxPosition = 0;
    float pistonDwMinPosition = 0;
    float pistonLtPosition = 0;
    float pistonLtMaxPosition = 0;
    float pistonLtMinPosition = 0;

    IMyBlockGroup pistonUp;
    IMyBlockGroup pistonDw;
    IMyBlockGroup pistonLt;
    IMyBlockGroup drills;
    pistonUp = GridTerminalSystem.GetBlockGroupWithName(pistonUpName);
    pistonDw = GridTerminalSystem.GetBlockGroupWithName(pistonDwName);
    pistonLt = GridTerminalSystem.GetBlockGroupWithName(pistonLtName);
    drills   = GridTerminalSystem.GetBlockGroupWithName(drillsName);

    // Echo($"{pistonUp.Name}:");
    List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
    pistonUp.GetBlocks(blocks);
    foreach (var block in blocks)
    {
        // Echo($"- {block.CustomName}");
        var piston = block as IMyPistonBase;
        pistonUpPosition += piston.CurrentPosition;
        pistonUpMaxPosition += piston.MaxLimit;
        pistonUpMinPosition += piston.LowestPosition;
    }

    pistonDw.GetBlocks(blocks);
    foreach (var block in blocks)
    {
        // Echo($"- {block.CustomName}");
        var piston = block as IMyPistonBase;
        pistonDwPosition += piston.CurrentPosition;
        pistonDwMaxPosition += piston.MaxLimit;
        pistonDwMinPosition += piston.LowestPosition;
    }

    pistonLt.GetBlocks(blocks);
    foreach (var block in blocks)
    {
        // Echo($"- {block.CustomName}");
        var piston = block as IMyPistonBase;
        pistonLtPosition += piston.CurrentPosition;
        pistonLtMaxPosition += piston.MaxLimit;
        pistonLtMinPosition += piston.LowestPosition;
    }

    // Fully out of the ground
    if( pistonUpPosition == pistonUpMaxPosition && pistonDwPosition == pistonDwMinPosition ) {
        if( shouldRun == false ) {
            Echo("Job Done");
            return;
        }
        // We shoudl down the upPiston
        pistonUp.GetBlocks(blocks);
        foreach( var block in blocks ) {
            var piston = block as IMyPistonBase;
            piston.Reverse();
        }
    }

    // Up piston in the ground, Dw pistons not in the ground
    if( pistonUpPosition == pistonUpMinPosition && pistonDwPosition == pistonDwMinPosition ) {
        // We should down the Dw Pistons
        pistonDw.GetBlocks(blocks);
        foreach( var block in blocks ) {
            var piston = block as IMyPistonBase;
            piston.Reverse();
        }
    }

    // Fully in the ground
    if( pistonUpPosition == pistonUpMinPosition && pistonDwPosition == pistonDwMaxPosition ) {
        shouldRun = false;
        // We should reverse all pistons
        pistonDw.GetBlocks(blocks);
        foreach( var block in blocks ) {
            var piston = block as IMyPistonBase;
            piston.Reverse();
        }
        pistonUp.GetBlocks(blocks);
        foreach( var block in blocks ) {
            var piston = block as IMyPistonBase;
            piston.Reverse();
        }
        drills.GetBlocks(blocks);
        foreach( var block in blocks ) {
            var drill = block as IMyShipDrill;
            drill.Enabled = false;
        }
    }
}


#region PreludeFooter
    }
}
#endregion