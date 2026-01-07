using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{   
    private Rigidbody2D m_rigidbody;

    // These variables are to hold the Action references
    InputAction moveAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the references to "Move" 
        moveAction = InputSystem.actions.FindAction("Move");

        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Read the "Move" action value, which is a 2D vector

        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        m_rigidbody.MovePosition(m_rigidbody.position + moveValue * Time.deltaTime * 10f);
    }
}
