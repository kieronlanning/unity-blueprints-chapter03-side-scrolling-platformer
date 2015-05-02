using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    readonly int[][] _level =
    {
        new[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        new[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        new[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        new[] {1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 0, 0, 0, 4, 0, 1},
        new[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1},
        new[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1},
        new[] {1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        new[] {1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        new[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        new[] {1, 0, 0, 0, 0, 0, 3, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        new[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        new[] {1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        new[] {1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1},
        new[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 3, 1},
        new[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        new[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
        new[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1},
        new[] {1, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1},
        new[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1},
        new[] {1, 0, 0, 0, 3, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1},
        new[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
        new[] {1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
        new[] {1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
        new[] {1, 0, 2, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
        new[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    };

    static public GameController Instance;

    public Transform wall;

    public Transform player;

    public Transform orb;

    public GUIText scoreText;

    int _orbsCollected;

    int _orbsTotal;

    void BuildLevel()
    {
        // Get the DynamicObjects object that we created in the scene
        // so we can make it our newly created objects' parent.

        var dynamicParent = GameObject.Find("DynamicObjects");

        // Go through each element inside out level variable.
        for (var yPos = 0; yPos < _level.Length; yPos++)
        {
            for (var xPos = 0; xPos < _level[yPos].Length; xPos++)
            {
                Transform toCreate = null;

                var levelItemId = _level[yPos][xPos];
                switch (levelItemId)
                {
                    case 0:
                        // Do nothing because we don't want anything there.
                        break;
                    case 1:
                        toCreate = wall;
                        break;
                    case 2:
                        toCreate = player;
                        break;
                    case 3:
                        toCreate = orb;
                        break;
                    default:
                        print("Invalid number: " + levelItemId);
                        break;
                }

                if (toCreate != null)
                {
                    var newObject =
                        (Transform)
                            Instantiate(toCreate, new Vector3(xPos, (_level.Length - yPos), 0), Quaternion.identity);

                    // Set the object's parent to DynamicObjects so it doesn't clutter
                    // the hierarchy.
                    newObject.parent = dynamicParent.transform;
                }
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        BuildLevel();

        var orbs = GameObject.FindGameObjectsWithTag("Orb");

        _orbsCollected = 0;
        _orbsTotal = orbs.Length;

        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Orbs: " + _orbsCollected + "/" + _orbsTotal;
    }

    void Awake()
    {
        Instance = this;
    }

    public void CollectedOrb()
    {
        _orbsCollected++;
        UpdateScoreText();
    }
}