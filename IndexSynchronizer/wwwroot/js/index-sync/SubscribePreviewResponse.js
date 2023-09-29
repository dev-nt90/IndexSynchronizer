// see signalr-hub-init.js
// Subscribe to preview responses for Source server
previewConnection.on("PreviewResponseSource", (response) => {
    // Handle the preview response for Source
    console.debug("Received preview response for Source:", response);
    var indexAreaSource = document.getElementById('indexAreaSource');

    response.forEach(function (definition) {
        var newElement = document.createElement("div");
        newElement.textContent = definition;
        newElement.style.border = "1px solid black";
        newElement.style.display = "flex";
        indexAreaSource.appendChild(newElement);
    })
});

// Subscribe to preview responses for Target
previewConnection.on("PreviewResponseTarget", (response) => {
    // Handle the preview response for Target
    console.debug("Received preview response for Target:", response);
    var indexAreaTarget = document.getElementById('indexAreaTarget');

    response.forEach(function (definition) {
        var newElement = document.createElement("div");
        newElement.textContent = definition;
        newElement.style.border = "1px solid black";
        newElement.style.display = "flex";
        indexAreaTarget.appendChild(newElement);
    })
});
