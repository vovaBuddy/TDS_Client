using System.Collections.Generic;

namespace Main.Characters.State.States
{
    public class RiffleState : ControlState 
    {
        public RiffleState(ref Dictionary<string, ControlState> endStateTable)
        {
            TransitionTable = new Dictionary<string, ControlState>();
            MsgTable = new Dictionary<string, string>();
            StateName = "Riffle State";
            
            MsgTable.Add(InputSystem.API.Messages.UPDATE_AXIS, API.Messages.MOVE);
            MsgTable.Add(InputSystem.API.Messages.UPDATE_CURSOR_POS, AimTarget.API.Messages.UPDATE_CURSORE_POSITION);
            MsgTable.Add(InputSystem.API.Messages.LMOUSE_PRESSED, Weapons.API.Messages.SHOOTING);
            MsgTable.Add(InputSystem.API.Messages.LMOUSE_PRESSED_DOWN, Weapons.API.Messages.SHOOT);
            MsgTable.Add(InputSystem.API.Messages.LMOUSE_PRESSED_UP, Weapons.API.Messages.END_SHOOT);
            MsgTable.Add(InputSystem.API.Messages.R_PRESSED, Weapons.API.Messages.START_RELOAD);
            MsgTable.Add(InputSystem.API.Messages.RMOUSE_DOWN, API.Messages.START_AIMING);
            MsgTable.Add(InputSystem.API.Messages.RMOUSE_UP, API.Messages.STOP_AIMING);
            MsgTable.Add(InputSystem.API.Messages.F_PRESSED, Inventory.API.Messages.PICK_UP_NEAR);
            MsgTable.Add(InputSystem.API.Messages.H_PRESSED, API.Messages.START_HEALING);
            MsgTable.Add(InputSystem.API.Messages.B_PRESSED, Weapons.API.Messages.CHANGE_MOD);
            MsgTable.Add(InputSystem.API.Messages.TAB_PRESSED, Inventory.API.Messages.WANT_OPEN_LOOT_MENU);
            MsgTable.Add(InputSystem.API.Messages.MOUSE_WHELL_FORWARD, Inventory.API.Messages.GET_NEXT_WEAPON);
            MsgTable.Add(InputSystem.API.Messages.MOUSE_WHELL_BACK, Inventory.API.Messages.GET_NEXT_WEAPON);
        }

        public void InitTransitionTable(StateAdapter adapter)
        {
            TransitionTable.Add(Inventory.API.Messages.WANT_OPEN_LOOT_MENU, adapter._lootMenuState);
        }
    }
}