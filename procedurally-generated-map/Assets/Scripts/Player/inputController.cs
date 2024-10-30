using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class inputController : MonoBehaviour
{
    [Header("Input Actions Asset")]
    [SerializeField] private InputActionAsset playerActions;

    [Header("Action Map Name References")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string brake = "Brake";

    private InputAction moveAction;
    private InputAction brakeAction;

    public Vector2 MoveInput { get; private set; }
    public bool BrakeInput { get; private set; }

    public static inputController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        moveAction = playerActions.FindActionMap(actionMapName).FindAction(move);
        brakeAction = playerActions.FindActionMap(actionMapName).FindAction(brake);
        RegisterInputActions();
    }

    void RegisterInputActions()
    {
        moveAction.performed += Context => MoveInput = Context.ReadValue<Vector2>();
        moveAction.canceled += Context => MoveInput = Vector2.zero;

        brakeAction.performed += Context => BrakeInput = true;
        brakeAction.canceled += Context => BrakeInput = false;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        brakeAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        brakeAction.Disable();
    }

}
