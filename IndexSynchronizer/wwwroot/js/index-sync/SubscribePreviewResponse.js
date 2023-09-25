﻿// see signalr-hub-init.js
// Subscribe to preview responses for Source server
previewConnection.on("PreviewResponseSource", (response) => {
    // Handle the preview response for Source
    console.log("Received preview response for Source:", response);
    var indexAreaSource = document.getElementById('indexAreaSource');
    indexAreaSource.textContent = response;
});

// Subscribe to preview responses for Target
previewConnection.on("PreviewResponseB", (response) => {
    // Handle the preview response for Target
    console.log("Received preview response for Target:", response);
    var indexAreaTarget = document.getElementById('indexAreaTarget');
    indexAreaTarget.textContent = response;
});
