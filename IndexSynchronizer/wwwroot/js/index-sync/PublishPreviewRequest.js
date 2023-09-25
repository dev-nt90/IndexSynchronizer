// Connect to the "preview" hub
const previewConnection = new signalR.HubConnectionBuilder()
    .withUrl("/previewhub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

// Publish preview request for Function A
function publishPreviewRequestA(requestData) {
    previewConnection.invoke("PreviewRequestA", requestData).catch((err) => {
        console.error("Error sending preview request for Function A:", err.toString());
    });
}

// Publish preview request for Function B
function publishPreviewRequestB(requestData) {
    previewConnection.invoke("PreviewRequestB", requestData).catch((err) => {
        console.error("Error sending preview request for Function B:", err.toString());
    });
}