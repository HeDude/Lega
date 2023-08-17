// Global questions variable
let questions;

// Function to fetch the JSON data
function fetchJSONData() {
    fetch('./model/socrates_en.json')
        .then(response => response.json())
        .then(data => {
            questions = data;
            // Initialize the UI with the first question after fetching the JSON
            updateUI("about_what_may_I_ask_you_something");
        })
        .catch(error => {
            console.error('Error fetching the JSON:', error);
        });
}

// Function to update the UI based on a question key
function updateUI(questionKey) {
    const questionData = questions[questionKey];
    if (questionData.choice && questionData.choice.length === 0) {
        // Display the victory message when no answer choices are present
        const questionTitleElement = document.getElementById('questionTitle');
        if (questionTitleElement) {
            questionTitleElement.innerText = "You have silenced Socrates! Congratulations!";
            for (let i = 1; i <= 4; i++) {
                const btn = document.getElementById(`btn${i}`);
                btn.style.display = 'none';  // Hide all answer buttons
            }
        } else {
            console.error("Element with id 'questionTitle' not found.");
        }
        return;
    }

    // Update the main question
    const questionTitleElement = document.getElementById('questionTitle');
    if (questionTitleElement) {
        questionTitleElement.innerText = questionData.title;
    } else {
        console.error("Element with id 'questionTitle' not found.");
    }

    // Update buttons
    for (let i = 0; i < 4; i++) {
        const btn = document.getElementById(`btn${i + 1}`);
        if (i < questionData.choice.length) {
            btn.innerText = questionData.choice[i].answer;
            btn.style.display = 'block';
            btn.onclick = function () {
                goToNextQuestion(questionData.choice[i].response);
            };
        } else {
            btn.style.display = 'none';
        }
    }
}

// Function to choose the next question based on weights
function goToNextQuestion(response) {
    const totalWeight = Object.values(response).reduce((a, b) => a + b, 0);
    const randomNum = Math.random() * totalWeight;
    let weightSum = 0;

    for (const [key, weight] of Object.entries(response)) {
        weightSum += weight;
        if (randomNum <= weightSum) {
            updateUI(key);
            break;
        }
    }
}

// Fetch the JSON data when the document is loaded
document.addEventListener('DOMContentLoaded', fetchJSONData);
