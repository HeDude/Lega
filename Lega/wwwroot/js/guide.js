document.addEventListener("DOMContentLoaded", function () {
    loadGuides();
});

function loadGuides() {
    // Load the guides from the JSON file
    fetch('./model/guide_en.json')
        .then(response => response.json())
        .then(guides => {
            const guidesList = document.querySelector('#guideContainer');
            Object.entries(guides).forEach(([id, guide]) => {
                let name = guide.name || ucfirst(id);
                let birth = guide.birth || "?";
                let death = guide.death || "?";
                let life = `<span>( ${birth} - ${death} )</span>`;
                let filePath = `/${ucfirst(id)}`; // Adjusted path for .cshtml files
                let wikiLink = guide.wikipedia ? `<a href="${guide.wikipedia}" target="_blank"><img src="/img/wikipedia.svg" alt="Wikipedia link" style="width:20px; vertical-align:middle;"></a>` : '';

                // Check if the corresponding .cshtml file (with the first character in uppercase) exists
                fetch(filePath, { method: 'HEAD' })
                    .then(res => {
                        const listItem = document.createElement('li');
                        if (res.ok) {
                            // If the file exists, make the name a clickable link
                            listItem.innerHTML = `<a href="${filePath}">${name}</a> ${life} ${wikiLink}`;
                        } else {
                            // If the file doesn't exist, just display the name
                            listItem.innerHTML = name + " " + life + " " + wikiLink;
                        }
                        guidesList.appendChild(listItem);
                    });
            });
        });
}

// Utility function to capitalize the first letter of a string
function ucfirst(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}
