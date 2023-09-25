// see signalr-hub-init.js

// Publish synchronization request
function publishSyncRequest(sourceData, targetData) {
    syncConnection
        .invoke("SyncRequest", sourceData, targetData)
        .catch((err) => {
            console.error("Error sending synchronization request:", err.toString());
            updateStatusIndicator(false);
        });
}

const syncButton = document.getElementById('synchronizeButton');

syncButton.addEventListener('click', () => {
    var usernameSource = document.getElementById('userInputSource');
    var passwordSource = document.getElementById('passwordInputSource');
    var serverSource = document.getElementById('serverInputSource');
    var databaseSource = document.getElementById('databaseInputSource');
    var tablenameSource = document.getElementById('tableInputSource');

    var sourceConnectionDetails = new ConnectionDetails(usernameSource, passwordSource, serverSource, databaseSource, tablenameSource, true);

    var usernameTarget = document.getElementById('userInputTarget');
    var passwordTarget = document.getElementById('passwordInputTarget');
    var serverTarget = document.getElementById('serverInputTarget');
    var databaseTarget = document.getElementById('databaseInputTarget');
    var tablenameTarget = document.getElementById('tableInputTarget');

    var targetConnectionDetails = new ConnectionDetails(usernameTarget, passwordTarget, serverTarget, databaseTarget, tablenameTarget, false);

    publishSyncRequest(sourceConnectionDetails, targetConnectionDetails);
})