using System.Collections.Generic;
using Core.MessageBus;
using Main.Characters.State.States;
using UnityEngine;

namespace Main.Characters.State
{
    public class StateAdapter : SubscriberBehaviour
    {
        private ControlState _prevState;
        private ControlState _curState;
       
        private Dictionary<string, ControlState> _endStateTable;

        public RiffleState _riffleState;
        public LootMenuState _lootMenuState;

        protected override void Awake()
        {
            base.Awake();

            _endStateTable = new Dictionary<string, ControlState>();
            
            _riffleState = new RiffleState(ref _endStateTable);
            _lootMenuState = new LootMenuState(ref _endStateTable);

            _riffleState.InitTransitionTable(this);
            _lootMenuState.InitTransitionTable(this);

            _curState = _riffleState;
        }

        [Subscribe(
            InputSystem.API.Messages.UPDATE_AXIS,
            InputSystem.API.Messages.F_PRESSED,
            InputSystem.API.Messages.LSHIFT_UP,
            InputSystem.API.Messages.R_PRESSED,
            InputSystem.API.Messages.B_PRESSED,
            InputSystem.API.Messages.H_PRESSED,
            InputSystem.API.Messages.RMOUSE_UP,
            InputSystem.API.Messages.LSHIFT_DOWN,
            InputSystem.API.Messages.RMOUSE_DOWN,
            InputSystem.API.Messages.SPACE_PRESSED,
            InputSystem.API.Messages.LCNTRL_PRESSED,
            InputSystem.API.Messages.LMOUSE_PRESSED,
            InputSystem.API.Messages.LMOUSE_PRESSED_DOWN,
            InputSystem.API.Messages.LMOUSE_PRESSED_UP,
            InputSystem.API.Messages.LSHIFT_PRESSED,
            InputSystem.API.Messages.RMOUSE_PRESSED,
            InputSystem.API.Messages.UPDATE_CURSOR_POS,
            InputSystem.API.Messages.MOUSE_WHELL_FORWARD,
            InputSystem.API.Messages.MOUSE_WHELL_BACK
        )]
        private void InputProcess(Message msg)
        {
            if(_curState.MsgTable.ContainsKey(msg.Type))
            {
                var outMsg = CommonMessage.Get(_curState.MsgTable[msg.Type], 
                    msg.Data != null ? msg.Data.GetCopy() : null);

                if(_curState.TransitionTable.ContainsKey(_curState.MsgTable[msg.Type]))
                {
                    _prevState = _curState;
                    _curState = _curState.TransitionTable[_curState.MsgTable[msg.Type]];
                }

                MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel], outMsg);
            }

            else if (_curState.TransitionTable.ContainsKey(msg.Type))
            {
                _prevState = _curState;
                _curState = _curState.TransitionTable[msg.Type];
            }

            else
            {
                Debug.Log("No dispatcher for " + msg.Type + " in " + _curState.StateName);
            }
        }

        [Subscribe(
            InputSystem.API.Messages.TAB_PRESSED
        )]
        private void InputProcessBroadcast(Message msg)
        {
            if (_curState.MsgTable.ContainsKey(msg.Type))
            {
                var outMsg = CommonMessage.Get(_curState.MsgTable[msg.Type],
                    msg.Data != null ? msg.Data.GetCopy() : null);

                if (_curState.TransitionTable.ContainsKey(_curState.MsgTable[msg.Type]))
                {
                    _prevState = _curState;
                    _curState = _curState.TransitionTable[_curState.MsgTable[msg.Type]];
                }

                MessageBus.SendMessage(outMsg);
            }

            else if (_curState.TransitionTable.ContainsKey(msg.Type))
            {
                _prevState = _curState;
                _curState = _curState.TransitionTable[msg.Type];
            }

            else
            {
                Debug.Log("No dispatcher for " + msg.Type + " in " + _curState.StateName);
            }
        }
    }
}