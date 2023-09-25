const syncConnection = new signalR.HubConnectionBuilder()
    .withUrl("/synchub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

// Publish synchronization (indexing) request
function publishSyncRequest(requestData) {
    syncConnection.invoke("SyncRequest", requestData).catch((err) => {
        console.error("Error sending synchronization request:", err.toString());
        updateStatusIndicator(false);
    });
}