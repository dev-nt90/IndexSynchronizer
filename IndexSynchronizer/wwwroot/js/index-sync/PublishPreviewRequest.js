// see signalr-hub-init.js
// Publish preview request for source server
function publishPreviewRequestSource(requestData) {
    previewConnection.invoke("PreviewRequestSource", requestData).catch((err) => {
        console.error("Error sending preview request for Source:", err.toString());
    });
}

// Publish preview request for target server
function publishPreviewRequestTarget(requestData) {
    previewConnection.invoke("PreviewRequestTarget", requestData).catch((err) => {
        console.error("Error sending preview request for Target:", err.toString());
    });
}

const previewButtonSource = document.getElementById('previewButtonSource');
const previewButtonTarget = document.getElementById('previewButtonTarget');

previewButtonSource.addEventListener('click', () => {
    var usernameSource = document.getElementById('userInputSource');
    var passwordSource = document.getElementById('passwordInputSource');
    var serverSource = document.getElementById('serverInputSource');
    var databaseSource = document.getElementById('databaseInputSource');
    var tablenameSource = document.getElementById('tableInputSource');

    var sourceConnectionDetails = new ConnectionDetails(usernameSource, passwordSource, serverSource, databaseSource, tablenameSource, true);
    publishPreviewRequestSource(sourceConnectionDetails);
});

previewButtonTarget.addEventListener('click', () => {
    var usernameTarget = document.getElementById('userInputTarget');
    var passwordTarget = document.getElementById('passwordInputTarget');
    var serverTarget = document.getElementById('serverInputTarget');
    var databaseTarget = document.getElementById('databaseInputTarget');
    var tablenameTarget = document.getElementById('tableInputTarget');

    var targetConnectionDetails = new ConnectionDetails(usernameTarget, passwordTarget, serverTarget, databaseTarget, tablenameTarget, false);
    publishPreviewRequestTarget(targetConnectionDetails);
});