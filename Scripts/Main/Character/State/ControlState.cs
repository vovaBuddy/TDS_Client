using System.Collections.Generic;

namespace Main.Characters.State
{
    public abstract class ControlState
    {
        public string StateName;
        public Dictionary<string, string> MsgTable;
        public Dictionary<string, ControlState> TransitionTable;
    }
}