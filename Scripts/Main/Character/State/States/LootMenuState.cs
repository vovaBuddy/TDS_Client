using System.Collections.Generic;

namespace Main.Characters.State.States
{
    public class LootMenuState : ControlState
    {
        public LootMenuState(ref Dictionary<string, ControlState> endStateTable)
        {
            TransitionTable = new Dictionary<string, ControlState>();
            MsgTable = new Dictionary<string, string>();
            StateName = "Loot Menu State";

            MsgTable.Add(InputSystem.API.Messages.UPDATE_AXIS, API.Messages.MOVE);
            MsgTable.Add(InputSystem.API.Messages.TAB_PRESSED, Inventory.API.Messages.CLOSE_LOOT_MENU);
        }

        public void InitTransitionTable(StateAdapter adapter)
        {
            TransitionTable.Add(Inventory.API.Messages.CLOSE_LOOT_MENU, adapter._riffleState);
        }
    }
}