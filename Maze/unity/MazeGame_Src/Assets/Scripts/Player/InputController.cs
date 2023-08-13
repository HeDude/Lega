using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HeDude;

namespace Maze
{
    public class InputController : MonoBehaviour
    {
        private Camera playerCamera;
        private CameraController cameraController;
        [SerializeField] private float mouseRange;

        private string reproductive = "Reproductive";
        private string application = "Application";
        private string productive = "Productive";
        private string meaning = "Meaning";

        private Text uiText;

        //Store raycast information
        private RaycastHit hitInfo;

        private GameObject[] doors;
        private GameObject[] puzzleContainers;

        private bool meaningPuzzleIsActive;
        private bool applicationPuzzleIsActive;
        private bool productivePuzzleIsActive;
        private bool reproductivePuzzleIsActive;

        private GameObject mazeMap;
        private GameObject pauseMenu;
        private bool isDragging;

        private Vector3 offset;
        private Vector3 screenPosition;

        //Store current escape room position
        Matrix3by3 currentPosition;

        //Start is called before the first frame update
        private void Start()
        {
            playerCamera = Camera.main;
            cameraController = playerCamera.transform.gameObject.GetComponent<CameraController>();

            mouseRange = 5;

            uiText = GameObject.Find("UIText").GetComponent<Text>();

            uiText.text = "";

            doors = GameObject.FindGameObjectsWithTag("Door");
            puzzleContainers = GameObject.FindGameObjectsWithTag("PuzzleContainer");
            mazeMap = GameObject.Find("Map");
            pauseMenu = GameObject.Find("PauseMenu");

            mazeMap.SetActive(false);
            pauseMenu.SetActive(false);

            foreach (GameObject container in puzzleContainers)
                container.SetActive(false);

            meaningPuzzleIsActive = false;
            applicationPuzzleIsActive = false;
            productivePuzzleIsActive = false;
            reproductivePuzzleIsActive = false;
            isDragging = false;
        }

        //Update is called once per frame
        private void Update()
        {
            //Checks whether clicks on a answer button
            CheckAnswerButtonPress();

            //Checks whether a puzzle is currently is active and if it should stop
            CheckToStopPuzzle();

            //Show map
            CheckToShowMap();

            //Show map
            CheckToShowPauseMenu();

            //Show map
            CheckToTeleport();
        }

        //Checks whether clicks on a answer button
        private void CheckAnswerButtonPress()
        {
            GameObject answer = ReturnRaycastGameobject();
            if (answer != null && answer.tag == "Button")
            {
                uiText.text = "Press 'left mousebutton' to interact";

                if (Input.GetMouseButtonDown(0))
                    StartPuzzle(currentPosition, hitInfo.collider.gameObject.name);
            }
            else
                uiText.text = "";
        }

        //Closes or opens specific door depending on state
        private void CloseDoor(string _type, bool _state)
        {
            foreach (GameObject door in doors)
                if (door.name == _type)
                    door.SetActive(_state);
        }

        //Checks for collision for a trigger collider
        private void OnTriggerEnter(Collider other)
        {
            //Checks whether the tag of the collided object is Escape Room
            if (other.tag == "EscapeRoom")
            {
                foreach (GameObject door in doors)
                    StartCoroutine(OpenAllDoors(door.name, true));

                currentPosition = other.gameObject.GetComponent<Escaperoom>().Position;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            //Checks whether the tag of the collided object is Escape Room
            if (other.tag == "EscapeRoom")
                foreach (GameObject door in doors)
                    CloseDoor(door.name, false);
            
            foreach (GameObject container in puzzleContainers)
                container.SetActive(false);
        }

        private IEnumerator OpenAllDoors(string _type, bool _state)
        {
            yield return new WaitForSeconds(1);
                CloseDoor(_type, _state);
        }

        //Start specific puzzle depending on type
        private void StartPuzzle(Matrix3by3 _position, string _type)
        {
            if (_type == reproductive)
                StartReproductivePuzzle(_position); //tell me how to learn
            if (_type == application)
                StartApplicationPuzzle(_position); //I like to find out how i learn by doing
            if (_type == productive)
                StartProductivePuzzle(_position); //Determine how i learn by myself
            if (_type == meaning)
                StartMeaningPuzzle(_position); //investigate learning
        }

        //Start reproductive puzzle depending on which room you are in
        private void StartReproductivePuzzle(Matrix3by3 _position)
        {
            Debug.Log("REPODRUCTIVE PUZZLE");
            if (_position == Matrix3by3.Center)
            {
                foreach (GameObject puzzleContainer in puzzleContainers)
                    if (puzzleContainer.name == "CenterReproductive")
                        puzzleContainer.SetActive(true);

                reproductivePuzzleIsActive = true;
            }
        }

        //Start meaning puzzle depending on which room you are in
        private void StartMeaningPuzzle(Matrix3by3 _position)
        {
            Debug.Log("MEANING PUZZLE");

            if (_position == Matrix3by3.Center)
            {
                foreach (GameObject puzzleContainer in puzzleContainers)
                    if (puzzleContainer.name == "CenterMeaning")
                        puzzleContainer.SetActive(true);

                meaningPuzzleIsActive = true;
            }
        }

        //Start productive puzzle depending on which room you are in
        private void StartProductivePuzzle(Matrix3by3 _position)
        {
            Debug.Log("PODRUCTIVE PUZZLE");
            if(_position == Matrix3by3.Center)
            {
                foreach (GameObject puzzleContainer in puzzleContainers)
                    if (puzzleContainer.name == "CenterProductive")
                        puzzleContainer.SetActive(true);

                productivePuzzleIsActive = true;
            }
        }

        //Start application puzzle depending on which room you are in
        private void StartApplicationPuzzle(Matrix3by3 _position)
        {
            Debug.Log("APPLICATION PUZZLE");
            if (_position == Matrix3by3.Center)
            {
                foreach (GameObject puzzleContainer in puzzleContainers)
                    if (puzzleContainer.name == "CenterApplication")
                        puzzleContainer.SetActive(true);

                applicationPuzzleIsActive = true;
            }
        }

        //Stops the puzzle
        private void CheckToStopPuzzle()
        {
            //The reproductive puzzle
            if (reproductivePuzzleIsActive)
            {
                GameObject answer = ReturnRaycastGameobject();
                if (answer != null && answer.tag == "PuzzleFinish")
                {
                    uiText.text = "Press 'left mousebutton' to interact";

                    if (Input.GetMouseButtonDown(0))
                        EndPuzzle("Reproductive", "CenterReproductive", ref reproductivePuzzleIsActive);
                }
                else
                    uiText.text = "";
            }

            //The productive puzzle
            if (productivePuzzleIsActive)
            {
                GameObject answer = ReturnRaycastGameobject();
                if (answer != null && answer.tag == "PuzzleFinish")
                {
                    uiText.text = "Press 'left mousebutton' to interact";

                    if (Input.GetMouseButtonDown(0))
                        EndPuzzle("Productive", "CenterProductive", ref productivePuzzleIsActive);
                }
                else
                    uiText.text = "";
            }

            //The meaning puzzle
            if (meaningPuzzleIsActive)
            {
                GameObject answer = ReturnRaycastGameobject();
                if (answer != null && answer.tag == "PuzzleFinish")
                {
                    uiText.text = "Press 'left mousebutton' to interact";

                    if (Input.GetMouseButtonDown(0))
                        EndPuzzle("Meaning", "CenterMeaning", ref meaningPuzzleIsActive);
                }
                else
                    uiText.text = "";
            }

            //The application puzzle
            if (applicationPuzzleIsActive)
            {
                GameObject answer = ReturnRaycastGameobject();

                DragObject(answer);



                //if (Input.GetMouseButtonDown(0))
                    //EndPuzzle("Application", "CenterApplication", ref applicationPuzzleIsActive);
            }
        }

        //Drags object
        private void DragObject(GameObject _draggableObject)
        {
            if (_draggableObject != null && _draggableObject.tag == "PuzzleDraggable")
            {
                //If player clicks the left mouse button
                if (Input.GetMouseButtonDown(0))
                {
                    //Sets isDragging to true
                    isDragging = true;
                    Debug.Log("target position :" + _draggableObject.transform.position);

                    //Convert world position to screen position.
                    screenPosition = Camera.main.WorldToScreenPoint(_draggableObject.transform.position);
                    offset = _draggableObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
                }

                //Stops dragging when the player stops holding left mouse button
                if (Input.GetMouseButtonUp(0))
                {
                    isDragging = false;
                    _draggableObject.GetComponent<Rigidbody>().isKinematic = false;
                }

                uiText.text = "Press 'left mousebutton' to interact";

                //If the player is dragging the mouse
                if (isDragging)
                {
                    //track mouse position.
                    Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);

                    //convert screen position to world position with offset changes.
                    Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;

                    //It will update target gameobject's current postion.
                    _draggableObject.transform.position = currentPosition;

                    _draggableObject.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
            else
            {
                isDragging = false;
                uiText.text = "";
            }
        }

        //Ends the puzzle
        private void EndPuzzle(string _type, string _puzzle, ref bool _currentPuzzle)
        {
            CloseDoor(_type, false);

            foreach (GameObject puzzleContainer in puzzleContainers)
                if (puzzleContainer.name == _puzzle)
                    puzzleContainer.SetActive(false);

            _currentPuzzle = false;
        }

        //Sends a raycast
        private GameObject ReturnRaycastGameobject()
        {
            //Shoots a raycast from the main camera
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo, mouseRange))
                return hitInfo.collider.gameObject;

            return null;
        }

        //Checks whether to show the map
        private void CheckToShowMap()
        {
            if(Input.GetKeyDown(KeyCode.M))
            {
                if (mazeMap.activeSelf)
                {
                    mazeMap.SetActive(false);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    cameraController.EnableSensitivity();
                }
                else
                {
                    mazeMap.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    cameraController.DisableSensitivity();
                }
            }
        }

        //Checks whether to show the map
        private void CheckToShowPauseMenu()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (pauseMenu.activeSelf)
                {
                    pauseMenu.SetActive(false);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    cameraController.EnableSensitivity();
                    Time.timeScale = 1;
                }
                else
                {
                    pauseMenu.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    cameraController.DisableSensitivity();
                    Time.timeScale = 0;
                }
            }
        }

        //Checks whether the player should teleport
        private void CheckToTeleport()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                transform.position = new Vector3(-310, 1, 115);

            if (Input.GetKeyDown(KeyCode.Alpha2))
                transform.position = new Vector3(113, 1, 115);

            if (Input.GetKeyDown(KeyCode.Alpha3))
                transform.position = new Vector3(-120, 1, 350);

            if (Input.GetKeyDown(KeyCode.Alpha4))
                transform.position = new Vector3(-120, 1, -150);
        }
    }
}
