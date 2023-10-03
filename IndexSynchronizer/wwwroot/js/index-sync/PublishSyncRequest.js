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
    var usernameSource = document.getElementById('userInputSource').value;
    var passwordSource = document.getElementById('passwordInputSource').value;
    var serverSource = document.getElementById('serverInputSource').value;
    var databaseSource = document.getElementById('databaseInputSource').value;
    var tablenameSource = document.getElementById('tableInputSource').value;

    var sourceConnectionDetails = new ConnectionDetails(usernameSource, passwordSource, serverSource, databaseSource, tablenameSource, true);

    var usernameTarget = document.getElementById('userInputTarget').value;
    var passwordTarget = document.getElementById('passwordInputTarget').value;
    var serverTarget = document.getElementById('serverInputTarget').value;
    var databaseTarget = document.getElementById('databaseInputTarget').value;
    var tablenameTarget = document.getElementById('tableInputTarget').value;

    var targetConnectionDetails = new ConnectionDetails(usernameTarget, passwordTarget, serverTarget, databaseTarget, tablenameTarget, false);

    publishSyncRequest(sourceConnectionDetails, targetConnectionDetails);
});