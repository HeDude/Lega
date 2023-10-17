
// JavaScript
document.addEventListener('DOMContentLoaded', async function() {
    // Fetch the JSON data for behavioral terms in categories
    let response = await fetch('/model/behavioral_style_terms.json');
    let categories = await response.json();

    let html = '';
    Object.values(categories).slice(0, 10).forEach(terms => {
        // Randomly decide if this row/category is "found"
        const isFound = Math.random() > 0.5;
        let rowPoints = isFound ? 20 : 0;

        terms.forEach(term => {
            // Allocate points to this cell if the row is found
            let cellPoints = 0;
            if (isFound) {
                cellPoints = Math.min(rowPoints, Math.floor(Math.random() * 11));
                rowPoints -= cellPoints;
            }

            // Calculate the fill percentage
            const fillPercentage = (cellPoints / 10) * 100;

            // Create the cell with a linear gradient background
            const cellClass = isFound ? 'cell row-found' : 'cell';
            const cellStyle = `background: linear-gradient(to top, green ${fillPercentage}%, transparent ${fillPercentage}%);`;
            html += `<div class="${cellClass}" data-points="${cellPoints}" style="${cellStyle}">${term}</div>`;
        });
    });

    document.getElementById('maverickGrid').innerHTML = html;
});
