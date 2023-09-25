using UnityEngine;

public class RandomPlayerController : PlayerController
{
    private void Update()
    {
        if (GameManager.tick)
        {
            int randomAction = Random.Range(0, 5);

            switch (randomAction)
            {
                case 0:
                    PlaceBomb();
                    break;
                case 1:
                    MoveLeft();
                    break;
                case 2:
                    MoveRight();
                    break;
                case 3:
                    MoveDown();
                    break;
                case 4:
                    MoveUp();
                    break;
            }
        }
    }
}