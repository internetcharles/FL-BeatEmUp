using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	
	public enum CONTROLLER { KEYBOARD, JOYSTICK, TOUCHSCREEN }
	[Header("Current Controller")]
	public CONTROLLER controller = CONTROLLER.KEYBOARD;
	private GameObject TSC; // touch screen controls
	private bool levelInProgress;

	[Header("Keyboard keys")]
	public KeyCode Left = KeyCode.LeftArrow;
	public KeyCode Right = KeyCode.RightArrow;
	public KeyCode Up = KeyCode.UpArrow;
	public KeyCode Down = KeyCode.DownArrow;
	public KeyCode PunchKey = KeyCode.Z;
	public KeyCode KickKey = KeyCode.X;
	public KeyCode DefendKey = KeyCode.C;
	public KeyCode JumpKey = KeyCode.Space;
	public KeyCode BeamKey = KeyCode.None;
	public KeyCode DashKey = KeyCode.None;
	public KeyCode CardFireKey = KeyCode.None;

	private KeyCode[] keycodes = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };

	[Header("Joypad keys")]
	public KeyCode JoypadPunch = KeyCode.JoystickButton2;
	public KeyCode JoypadKick = KeyCode.JoystickButton3;
	public KeyCode JoypadDefend = KeyCode.JoystickButton1;
	public KeyCode JoypadJump = KeyCode.JoystickButton0;

	//delegates
	public delegate void InputEventHandler(Vector2 dir);
	public static event InputEventHandler onInputEvent;
	public delegate void CombatInputEventHandler(string action);
	public static event CombatInputEventHandler onCombatInputEvent;
	private GameSettings settings;

	void OnEnable(){
		EnemyWaveSystem.onLevelStart += OnLevelStart;
		EnemyWaveSystem.onLevelComplete += OnLevelEnd;
	}

	void OnDisable(){
		EnemyWaveSystem.onLevelStart -= OnLevelStart;
		EnemyWaveSystem.onLevelComplete -= OnLevelEnd;
	}

	public static void InputEvent(Vector2 dir){
		if( onInputEvent != null) onInputEvent(dir);
	}

	public static void CombatInputEvent(string action){
		if( onCombatInputEvent != null) onCombatInputEvent(action);
	}

	void Update(){

		//Use keyboard
		if(controller == CONTROLLER.KEYBOARD) KeyboardControls();

		//use joypad
		if(controller == CONTROLLER.JOYSTICK) JoyPadControls();

		//use touchscreen
		if(controller == CONTROLLER.TOUCHSCREEN && !TSC) CreateTouchScreenControls();
		if(TSC) TSC.SetActive((controller == CONTROLLER.TOUCHSCREEN));
		if(TSC && !levelInProgress) TSC.SetActive(false);
	}

	void KeyboardControls(){
		
		//movement
		float x = 0f;
	 	float y = 0f;

		if(Input.GetKey(Left)) x = -1f;
		if(Input.GetKey(Right)) x = 1f;
		if(Input.GetKey(Up)) y = 1f;
		if(Input.GetKey(Down)) y = -1f;

		Vector2 dir = new Vector2(x,y);
		InputEvent(dir);


		//Combat input
		if(Input.GetKeyDown(PunchKey))
		{
			CombatInputEvent("Punch");
		}

		if(Input.GetKeyDown(KickKey))
		{
			CombatInputEvent("Kick");
		}

		if(Input.GetKey(DefendKey))
		{
			CombatInputEvent("Defend");
		}

		if(Input.GetKeyDown(JumpKey))
		{
			CombatInputEvent("Jump");
		}

        if(Input.GetKeyDown(BeamKey))
        {
			CombatInputEvent("Beam");
        }

		if(Input.GetKeyDown(DashKey))
        {
			CombatInputEvent("Dash");
        }
	}

	void JoyPadControls(){
	 	float x = Input.GetAxis("Horizontal");
	 	float y = Input.GetAxis("Vertical");
		Vector2 dir = new Vector2(x,y);
		InputEvent(dir.normalized);

		if(Input.GetKeyDown(JoypadPunch)){
			CombatInputEvent("Punch");
		}

		if(Input.GetKeyDown(JoypadKick)){
			CombatInputEvent("Kick");
		}

		if(Input.GetKey(JoypadDefend)){
			CombatInputEvent("Defend");
		}

		if(Input.GetKey(JoypadJump)){
			CombatInputEvent("Jump");
		}
	}

	void CreateTouchScreenControls(){
		GameObject canvas = FindObjectOfType<Canvas>().gameObject;
		if(canvas != null) {
			TSC = Instantiate(Resources.Load("UI_TouchScreenControls")) as GameObject;
			TSC.transform.SetParent(canvas.transform, false);
		}
	}

	void AssignAbilities()
    {
		var weapons = FindObjectsOfType<Weapon>();
		var abilities = PlayerInfo.instance.playerAbilities;
		for (int i = 0; i < PlayerInfo.instance.playerAbilities.Count; i++)
        {
			if (abilities[i].name == "Beam")
            {
				BeamKey = keycodes[i];
            }
			if (abilities[i].name == "Dash")
			{
				DashKey = keycodes[i];
			}
			if (abilities[i].name == "Card")
            {
				CardFireKey = keycodes[i];
				foreach (Weapon weapon in weapons)
                {
					if (weapon.weaponStats.itemName == "card")
                    {
						weapon.fireKey = CardFireKey;
                    }
                }
            }
		}
    }

	void OnLevelStart(){
		levelInProgress = true;
		AssignAbilities();
	}

	void OnLevelEnd(){
		levelInProgress = false;
	}
}

public enum INPUTTYPE {	
	KEYBOARD = 0,	
	JOYPAD = 2,	
	TOUCHSCREEN = 4, 
}
