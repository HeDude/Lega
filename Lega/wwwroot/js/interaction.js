// Global jsonData variable
let jsonData;
let guideName;

// Function to fetch the JSON data from the given file path
const BASE_PATH = './model/';

// Function to fetch the JSON data from the given file path starting with the 
function loadGuideInteraction(guide, startKey, languageCode) {
    guideName = guide;
    fetch(`${BASE_PATH}${guideName.toLowerCase()}_${languageCode.toLowerCase()}.json`)
        .then(response => response.json())
        .then(data => {
            jsonData = data;
            displayInteraction(startKey,);
        })
        .catch(error => {
            console.error('Error fetching the JSON:', error);
        });
}

// Function to update the UI based on a interaction key
function displayInteraction(interactionKey) {
    const interactionData = jsonData[interactionKey];
    if (interactionData.choice && interactionData.choice.length === 0) {
        // Display the victory message when no answer choices are present
        const interactionTitleElement = document.getElementById('interactionTitle');
        if (interactionTitleElement) {
            interactionTitleElement.innerText = `You have silenced ${guideName}! Congratulations!`;
            for (let i = 1; i <= 4; i++) {
                const btn = document.getElementById(`btn${i}`);
                btn.style.display = 'none';  // Hide all answer buttons
            }
        } else {
            console.error("Element with id 'interactionTitle' not found.");
        }
        return;
    }

    // Update the main interaction
    const interactionTitleElement = document.getElementById('interactionTitle');
    if (interactionTitleElement) {
        interactionTitleElement.innerText = interactionData.title;
    } else {
        console.error("Element with id 'interactionTitle' not found.");
    }

    // Handle the optional description property
    const interactionDescriptionElement = document.getElementById("interactionDescription");
    if (interactionDescriptionElement && interactionData.description) {
        interactionDescriptionElement.innerText = interactionData.description;
    } else if (interactionDescriptionElement) {
        interactionDescriptionElement.innerText = "";  // Clear any previous content if the description isn't available
    }


    // Update buttons
    for (let i = 0; i < 4; i++) {
        const btn = document.getElementById(`btn${i + 1}`);
        if (i < interactionData.choice.length) {
            btn.innerText = interactionData.choice[i].option;
            btn.style.display = 'block';
            btn.onclick = function () {
                goToNextInteraction(interactionData.choice[i].response);
            };
        } else {
            btn.style.display = 'none';
        }
    }
}

// Function to choose the next interaction based on weights
function goToNextInteraction(response) {
    const totalWeight = Object.values(response).reduce((a, b) => a + b, 0);
    const randomNum = Math.random() * totalWeight;
    let weightSum = 0;

    for (const [key, weight] of Object.entries(response)) {
        weightSum += weight;
        if (randomNum <= weightSum) {
            displayInteraction(key);
            break;
        }
    }
}