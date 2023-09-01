window.addEventListener("load", function () {
    const canvas = document.getElementById("multiverseCanvas");
    const ctx = canvas.getContext("2d");
    let universes = [];

    fetch('./model/multiverse.json')
    .then(response => response.json())
    .then(data => {
        // Update universes based on the JSON data
        universes = data.map(universe => {
            return {
                x: universe.x,
                y: universe.y,
                radius: universe.radius,
                name: universe.name,
                character: universe.name.toLowerCase() + '.png'
            };
        });

        // Draw universes
        for (const universe of universes) {
            ctx.beginPath();
            ctx.arc(universe.x, universe.y, universe.radius, 0, Math.PI * 2);

            // Create radial gradient for "bubble" effect
            const gradient = ctx.createRadialGradient(universe.x, universe.y, 0, universe.x, universe.y, universe.radius);
            gradient.addColorStop(0, 'rgba(0, 0, 0, 1)');
            gradient.addColorStop(0.4, 'rgba(0, 0, 0, 0.8)');
            gradient.addColorStop(0.85, 'rgba(0, 0, 0, 0.2)');
            gradient.addColorStop(0.90, 'rgba(255, 255, 255, 0.2)');
            gradient.addColorStop(1, 'rgba(255, 255, 255, 0.8)');

            ctx.fillStyle = gradient;
            ctx.fill();

            // Add light border
            ctx.strokeStyle = 'rgba(255, 255, 255, 0.8)';
            ctx.lineWidth = 2;
            ctx.stroke();

            ctx.closePath();

            // Load the universe background image
            const universeImg = new Image();
            universeImg.src = './img/background/universe.png';
            universeImg.onload = function () {
                ctx.drawImage(universeImg, universe.x - universe.radius, universe.y - universe.radius, universe.radius * 2, universe.radius * 2);
            };

            // Load the corresponding character image
            const img = new Image();
            img.src = `./img/character/${universe.character}`;
            img.onload = function () {
                ctx.drawImage(img, universe.x - universe.radius / 2, universe.y - universe.radius / 2, universe.radius, universe.radius);
            };
        }
    })
    .catch(error => console.error('Error fetching JSON:', error));

    // Listen for clicks on the canvas
    canvas.addEventListener("click", function (event) {
        const rect = canvas.getBoundingClientRect();
        const x = event.clientX - rect.left;
        const y = event.clientY - rect.top;

        for (const universe of universes) {
            const distance = Math.sqrt((x - universe.x) ** 2 + (y - universe.y) ** 2);
            if (distance < universe.radius) {
                alert(`You've selected the ${universe.name} universe!`);
                break;
            }
        }
    });
});
