using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections.Generic;

public class BOTS : MonoBehaviour
{
    // Prefab references
    public GameObject robotPrefab;
    public GameObject boxPrefab;
    public GameObject containerPrefab;

    // Parent object for grouping
    public GameObject parentObject;

    [System.Serializable]
    public class Robot { public int[] position; }

    [System.Serializable]
    public class Box { public int[] position; }

    [System.Serializable]
    public class Container { public int[] position; public int capacity; }

    [System.Serializable]
    public class RobotContainer
    {
        public List<Robot> robots;
        public List<Container> containers;
        public List<Box> boxes;
    }

    // Global variables
    public int m = 10;
    public int n = 10;
    public int steps = 100;
    public int currentStep = 0;
    public bool requestInProgress = false;

    public int scale = 10;

    public (int x, int y)[] robots = new (int x, int y)[]
    {
        (1, 2), (3, 4), (5, 6), (2, 2), (8, 7)
    };

    public (int x, int y, int c)[] containers = new (int x, int y, int c)[]
    {
    (0, 5, 5),(9, 0, 5),(9, 9, 5)
    };

    public (int x, int y)[] boxes = new (int x, int y)[]
    {
            (6, 2), (4, 7), (8, 3), (5, 9), (9, 1), (7, 8), (3, 3), (1, 9),
    (2, 6), (8, 5), (0, 4), (9, 7), (3, 8), (6, 0), (7, 4)
    };

    public int begin = 1;

    void Start()
    {
        // Initialize the parent object
        if (parentObject == null)
        {
            parentObject = new GameObject("ObjectsParent");
        }

        // Start the first request
        StartCoroutine(SendAPIRequest());
    }

    void Update()
    {
        // No need to handle requestInProgress here
    }

    // Method to clear all children of the parent object
    void ClearParentObject()
    {
        Debug.Log("Clearing parent object...");
        foreach (Transform child in parentObject.transform)
        {
            Debug.Log("Destroying child: " + child.gameObject.name);
            Destroy(child.gameObject);
        }
    }

    // Method to instantiate robots
    void InstantiateRobots()
    {
        if (robotPrefab == null)
        {
            Debug.LogError("Robot prefab is not assigned.");
            return;
        }

        foreach (var robot in robots)
        {
            GameObject robotInstance = Instantiate(robotPrefab, new Vector3(robot.y * scale, 0, robot.x * scale), Quaternion.identity, parentObject.transform);
            robotInstance.tag = "Robot";
            Debug.Log("Instantiated robot at: " + robot.y + ", " + robot.x);
        }
    }

    // Method to instantiate boxes
    void InstantiateBoxes()
    {
        if (boxPrefab == null)
        {
            Debug.LogError("Box prefab is not assigned.");
            return;
        }

        foreach (var box in boxes)
        {
            GameObject boxInstance = Instantiate(boxPrefab, new Vector3(box.y * scale, 0, box.x * scale), Quaternion.identity, parentObject.transform);
            boxInstance.tag = "Box";
            Debug.Log("Instantiated box at: " + box.y + ", " + box.x);
        }
    }

    // Method to instantiate containers
    void InstantiateContainers()
    {
        if (containerPrefab == null)
        {
            Debug.LogError("Container prefab is not assigned.");
            return;
        }

        foreach (var container in containers)
        {
            GameObject containerInstance = Instantiate(containerPrefab, new Vector3(container.y * scale, 0, container.x * scale), Quaternion.identity, parentObject.transform);
            containerInstance.tag = "Container";
            Debug.Log("Instantiated container at: " + container.y + ", " + container.x);
        }
    }

    IEnumerator SendAPIRequest()
    {
        string url = "http://127.0.0.1:8000/step";

        // Manually construct the JSON string
        string robotsJson = "[";
        for (int i = 0; i < robots.Length; i++)
        {
            robotsJson += $"[{robots[i].x}, {robots[i].y}]";
            if (i < robots.Length - 1) robotsJson += ",";
        }
        robotsJson += "]";

        string containersJson = "[";
        for (int i = 0; i < containers.Length; i++)
        {
            containersJson += $"[{containers[i].x}, {containers[i].y}]";
            if (i < containers.Length - 1) containersJson += ",";
        }
        containersJson += "]";

        string boxesJson = "[";
        for (int i = 0; i < boxes.Length; i++)
        {
            boxesJson += $"[{boxes[i].x}, {boxes[i].y}]";
            if (i < boxes.Length - 1) boxesJson += ",";
        }
        boxesJson += "]";

        // Combine all the parts into a final JSON string
        string jsonData = "{" +
            $"\"m\": {m}, " +
            $"\"n\": {n}, " +
            $"\"steps\": {steps}, " +
            $"\"robots\": {robotsJson}, " +
            $"\"boxes\": {boxesJson}, " +
            $"\"containers\": {containersJson}, " +
            $"\"begin\": {begin}" + // This just works with 0 and 1 (hardcoded)
            "}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            // Process the response 
            Debug.Log("Response: " + request.downloadHandler.text);

            // Deserialize JSON to a C# object
            RobotContainer robotContainer = null;
            try
            {
                robotContainer = JsonUtility.FromJson<RobotContainer>(request.downloadHandler.text);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Deserialization failed: " + ex.Message);
                yield break;
            }

            if (robotContainer == null)
            {
                Debug.LogError("Deserialized RobotContainer is null.");
                yield break;
            }

            // Clear the parent object before instantiating new prefabs
            ClearParentObject();

            // Update robots
            if (robotContainer.robots != null)
            {
                for (int i = 0; i < robotContainer.robots.Count; i++)
                {
                    robots[i].x = robotContainer.robots[i].position[1];
                    robots[i].y = robotContainer.robots[i].position[0];
                }
                InstantiateRobots();
            }
            else
            {
                Debug.LogError("RobotContainer.robots is null.");
            }

            // Update containers
            if (robotContainer.containers != null)
            {
                int i = 0;
                foreach (var container in robotContainer.containers)
                {
                    if (container.position[1] == containers[i].x && container.position[0] == containers[i].y)
                    {
                        containers[i].c = container.capacity;
                    }
                    i++;
                }
                InstantiateContainers();
            }
            else
            {
                Debug.LogError("RobotContainer.containers is null.");
            }

            // Update boxes
            if (robotContainer.boxes != null)
            {
                var newBoxes = new (int x, int y)[robotContainer.boxes.Count];
                for (int i = 0; i < robotContainer.boxes.Count; i++)
                {
                    newBoxes[i].x = robotContainer.boxes[i].position[1];
                    newBoxes[i].y = robotContainer.boxes[i].position[0];
                }
                boxes = newBoxes;
                InstantiateBoxes();
            }
            else
            {
                Debug.LogError("RobotContainer.boxes is null.");
            }

            begin = 0;
            currentStep++;
            yield return new WaitForSeconds(1.0f);
            // Send another request for the next step
            if (currentStep < steps)
            {

                StartCoroutine(SendAPIRequest());
            }
            else
            {
                Debug.Log("Completed all steps.");
            }
        }
    }
}