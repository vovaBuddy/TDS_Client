using UnityEngine;
using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;

namespace Main.InputSystem
{
    public class InputSystem : MonoBehaviour
    {
        private static InputSystem _instance;
        private Vector3 _inputVector;
        
        public static InputSystem Instance
        {
            get {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<InputSystem>();
             
                    if (_instance == null)
                    {
                        var container = new GameObject("__INPUT_SYSTEM");
                        _instance = container.AddComponent<InputSystem>();
                    }
                }
     
                return _instance;
            }
        }
        
        void Awake()
        {
            Cursor.visible = false;
            _inputVector = Vector3.zero;
        }

        void Update()
        {
            MessageBus.SendMessage(CommonMessage.Get(API.Messages.UPDATE_CURSOR_POS, 
                Vector3Data.GetVector3Data(Input.mousePosition)));

            _inputVector.x = Input.GetAxis("Horizontal");
            _inputVector.z = Input.GetAxis("Vertical");

            MessageBus.SendMessage(CommonMessage.Get(API.Messages.UPDATE_AXIS, 
                Vector3Data.GetVector3Data(_inputVector)));

            if(Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                MessageBus.SendMessage(CommonMessage.Get(API.Messages.MOUSE_WHELL_FORWARD));
            }
            else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                MessageBus.SendMessage(CommonMessage.Get(API.Messages.MOUSE_WHELL_BACK));
            }

            if (Input.GetMouseButton(0))
            {
                MessageBus.SendMessage(CommonMessage.Get(API.Messages.LMOUSE_PRESSED));
            }

            if(Input.GetMouseButtonDown(0))
            {
                MessageBus.SendMessage(CommonMessage.Get(API.Messages.LMOUSE_PRESSED_DOWN));
            }

            if (Input.GetMouseButtonUp(0))
            {
                MessageBus.SendMessage(CommonMessage.Get(API.Messages.LMOUSE_PRESSED_UP));
            }

            //            if (Input.GetMouseButtonDown(1))
            //            {
            //                MessageBus.SendMessage(ObjectData.GetMessage(API.Messages.RMOUSE_DOWN));
            //            }
            //            else if (Input.GetMouseButtonUp(1))
            //            {
            //                MessageBus.SendMessage(ObjectData.GetMessage(API.Messages.RMOUSE_UP));
            //            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                MessageBus.SendMessage(CommonMessage.Get(API.Messages.R_PRESSED));
            }
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                MessageBus.SendMessage(CommonMessage.Get(API.Messages.F_PRESSED));
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                MessageBus.SendMessage(CommonMessage.Get(API.Messages.B_PRESSED));
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                MessageBus.SendMessage(CommonMessage.Get(API.Messages.H_PRESSED));
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            { 
                MessageBus.SendMessage(CommonMessage.Get(API.Messages.TAB_PRESSED));
            }
        }
    }
}