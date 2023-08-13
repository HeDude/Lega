using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Maze
{
    public class ReadJsonFile : MonoBehaviour
    {
        GameObject[] questionCanvasses;
        GameObject[] questions;
        GameObject[] reproductiveAnswers;
        GameObject[] applicationAnswers;
        GameObject[] productiveAnswers;
        GameObject[] meaningAnswers;

        private void Start()
        {
            questionCanvasses = GameObject.FindGameObjectsWithTag("QuestionCanvas");
            questions = GameObject.FindGameObjectsWithTag("Question");
            reproductiveAnswers = GameObject.FindGameObjectsWithTag("AnswerReproductive");
            applicationAnswers = GameObject.FindGameObjectsWithTag("AnswerApplication");
            productiveAnswers = GameObject.FindGameObjectsWithTag("AnswerProductive");
            meaningAnswers = GameObject.FindGameObjectsWithTag("AnswerMeaning");
        }

        //Update is called every frame
        private void Update()
        {
            //All question/answer canvasses face the player
            foreach (GameObject canvas in questionCanvasses)
            {
                canvas.transform.forward = new Vector3(Camera.main.transform.forward.x, canvas.transform.forward.y, Camera.main.transform.forward.z);
            }
        }

        //Checks for collision for a trigger collider
        private void OnTriggerEnter(Collider other)
        {
            //Checks whether the tag of the collided object is Escape Room
            if (other.tag == "EscapeRoom")
            {
                //Tries to find the RoomInfo component
                Escaperoom escaperoom = other.gameObject.GetComponent<Escaperoom>();

                //If the collided object has the RoomInfo component
                if (escaperoom != null)
                {
                    //Changes the question canvas to the question of the current escape room
                    foreach (GameObject question in questions)
                        question.GetComponent<Text>().text = escaperoom.Quiz.Question;

                    //Changes the reproductive answer canvas to the reproductive answer of the current escape room
                    foreach (GameObject answerReproductive in reproductiveAnswers)
                        answerReproductive.GetComponent<Text>().text = escaperoom.Quiz.Reproductive;

                    //Changes the application answer canvas to the application answer of the current escape room
                    foreach (GameObject answerApplication in applicationAnswers)
                        answerApplication.GetComponent<Text>().text = escaperoom.Quiz.Application;

                    //Changes the meaning answer canvas to the meaning answer of the current escape room
                    foreach (GameObject answerMeaning in meaningAnswers)
                        answerMeaning.GetComponent<Text>().text = escaperoom.Quiz.Meaning;

                    //Changes the productive answer canvas to the productive answer of the current escape room
                    foreach (GameObject answerProductive in productiveAnswers)
                        answerProductive.GetComponent<Text>().text = escaperoom.Quiz.Productive;
                }
            }
        }

        //Checks whether the player has exitted a triggercollider
        private void OnTriggerExit(Collider other)
        {
            //Checks whether the tag of the trigger collider is Escape Room
            if (other.tag == "EscapeRoom")
            {
                //Clears the text of the questions and answers
                foreach (GameObject question in questions)
                    question.GetComponent<Text>().text = "";

                //Changes the reproductive answer canvas to the reproductive answer of the current escape room
                foreach (GameObject answerReproductive in GameObject.FindGameObjectsWithTag("AnswerReproductive"))
                    answerReproductive.GetComponent<Text>().text = "";

                //Changes the application answer canvas to the application answer of the current escape room
                foreach (GameObject answerApplication in GameObject.FindGameObjectsWithTag("AnswerApplication"))
                    answerApplication.GetComponent<Text>().text = "";

                //Changes the meaning answer canvas to the meaning answer of the current escape room
                foreach (GameObject answerMeaning in GameObject.FindGameObjectsWithTag("AnswerMeaning"))
                    answerMeaning.GetComponent<Text>().text = "";

                //Changes the productive answer canvas to the productive answer of the current escape room
                foreach (GameObject answerProductive in GameObject.FindGameObjectsWithTag("AnswerProductive"))
                    answerProductive.GetComponent<Text>().text = "";
            }
        }
    }
}
