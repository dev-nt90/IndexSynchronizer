// Connect to the "preview" hub
const previewConnection = new signalR.HubConnectionBuilder()
    .withUrl("/previewhub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

// Subscribe to preview responses for Function A
previewConnection.on("PreviewResponseA", (response) => {
    // Handle the preview response for Function A
    console.log("Received preview response for Function A:", response);
    // Update the UI for Function A
});

// Subscribe to preview responses for Function B
previewConnection.on("PreviewResponseB", (response) => {
    // Handle the preview response for Function B
    console.log("Received preview response for Function B:", response);
    // Update the UI for Function B
});
