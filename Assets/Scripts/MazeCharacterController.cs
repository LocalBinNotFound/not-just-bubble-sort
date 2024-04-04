using UnityEngine;
using ProceduralToolkit.Samples;
using ProceduralToolkit;

public class MazeCharacterController : MonoBehaviour
{
    private MazeGeneratorConfigurator mazeConfigurator;
    private Vector3 currentGridPosition;
    private const int roomSize = 2;
    private const int wallSize = 1;
    private float trueRoomSize;
    private float trueWallSize;

    void Start() {
        mazeConfigurator = FindObjectOfType<MazeGeneratorConfigurator>();

        RectTransform mazeRectTransform = mazeConfigurator.mazeImage.rectTransform;
        Vector2 top_left_corner = new Vector2(mazeRectTransform.rect.xMin, mazeRectTransform.rect.yMax);

        Vector2 worldPoint = mazeRectTransform.TransformPoint(top_left_corner);
        float scaleFactorX = 20;
        trueRoomSize = roomSize * scaleFactorX;
        trueWallSize = wallSize * scaleFactorX;

        // Adjust the starting position
        transform.position = new Vector3(worldPoint.x + trueWallSize + 20, worldPoint.y - trueWallSize - 20, 0);
        currentGridPosition = transform.position;
    }

    public void Move(Directions direction) {
        switch (direction) {
            case Directions.Up:
                currentGridPosition.y += trueRoomSize+trueWallSize;
                break;
            case Directions.Down:
                currentGridPosition.y -= trueRoomSize+trueWallSize;
                break;
            case Directions.Left:
                currentGridPosition.x -= trueRoomSize+trueWallSize;
                break;
            case Directions.Right:
                currentGridPosition.x += trueRoomSize+trueWallSize;
                break;
        }

        UpdateCharacterPosition();
    }

    private void UpdateCharacterPosition() {
        transform.position = new Vector3(currentGridPosition.x, currentGridPosition.y, 0);
    }

    private bool IsWalkable(Vector2Int gridPosition)
    {

        return true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Move(Directions.Up);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Directions.Down);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Directions.Left);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Directions.Right);
        }
    }
}

