using System.Collections.Generic;
using UnityEngine;
using HeDude;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace Maze
{
    /// <summary>
    /// The EscapeRoom class handles all behavior of the EscapeRooms in the Maze.
    /// The EscapeRooms are structured in a 3x3 matrix so can have 9 differents positions.
    /// </summary>
     public class Escaperoom : MonoBehaviour
    {
        /// <summary>
        /// The component Position is an enum of all the positions (Center, Top, TopRight to BottomLeft to ToLeft etcetera).
        /// This forces a dropdown of valid positions in the Inspector.
        /// </summary>
        public Matrix3by3 Position;

        /// <summary>
        /// The Quiz class contains the question and 4 type of answers
        /// </summary>
        public Quiz Quiz { get { Quizes.TryGetValue(Position.ToString(), out Quiz result); return result; } }

        /// <summary>
        /// This dictionary resambles the JSON configuration file for the quizes
        /// </summary>
        private static Dictionary<string, Quiz> Quizes;

        /// <summary>
        /// The Start class is always loaded after the constructor. In this case it reads a config file for the Escaperoom quizes
        /// </summary>
        void Start()
        {
 //           string questions_json = File.ReadAllText(Application.dataPath + "/Config/Quizes.json");
//            string questions_json = File.ReadAllText("Assets/Config/Quizes.json");
            string questions_json = (new WebClient()).DownloadString("https://game.entreprenasium.nl/maze/Config/Quizes.json");
            Quizes = JsonConvert.DeserializeObject<Dictionary<string, Quiz>>(questions_json);
        }
    } 
}
