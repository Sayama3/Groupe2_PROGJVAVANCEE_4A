using UnityEngine;

public class HumanPlayerController : PlayerController
{
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaceBomb();
        }
        
        if(horizontalInput < 0f)
        {
            MoveLeft();
        }
        else if(horizontalInput > 0f)
        {
            MoveRight();
        }
        else if(verticalInput < 0f)
        {
            MoveDown();
        }
        else if(verticalInput > 0f)
        {
            MoveUp();
        }
    }
}