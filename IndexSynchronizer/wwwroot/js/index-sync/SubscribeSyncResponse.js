
const syncConnection = new signalR.HubConnectionBuilder()
    .withUrl("/synchub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

syncConnection.on("SyncResponse", (response) => {
    updateStatusIndicator(true);

    // TODO: update stats; here or lower?
});

syncConnection.start().catch((err) => {
    updateStatusIndicator(false);

    // TODO: update stats
});