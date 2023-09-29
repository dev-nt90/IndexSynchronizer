// see signalr-hub-init.js
syncConnection.on("SyncResponse", (response) => {
    updateStatusIndicator(true);

    // TODO: update stats; here or lower?
});