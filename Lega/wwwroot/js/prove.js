// Handle the drag
function drag(event) {
  event.dataTransfer.setData("text", event.target.id);
}

// Allow the drag
function allowDrop(event) {
  event.preventDefault();
}

// Handle the drop
function drop(event) {
  event.preventDefault();
  var data = event.dataTransfer.getData("text");
  var nodeCopy = document.getElementById(data).cloneNode(true);
  nodeCopy.id = "newId"; // Change the id or remove it; this is optional
  event.target.appendChild(nodeCopy);

  // Add click event to remove the item when clicked
  nodeCopy.addEventListener('click', function() {
    event.target.removeChild(nodeCopy);
  });
}

// Add click event to existing items in the droppable area to remove them when clicked
document.getElementById('evidence').addEventListener('click', function(event) {
  if (event.target.classList.contains('magistrate-collectible-item')) {
    event.target.remove();
  }
});