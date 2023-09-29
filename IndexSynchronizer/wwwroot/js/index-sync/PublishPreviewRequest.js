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
    var usernameSource = document.getElementById('userInputSource').value;
    var passwordSource = document.getElementById('passwordInputSource').value;
    var serverSource = document.getElementById('serverInputSource').value;
    var databaseSource = document.getElementById('databaseInputSource').value;
    var tablenameSource = document.getElementById('tableInputSource').value;

    var sourceConnectionDetails = new ConnectionDetails(usernameSource, passwordSource, serverSource, databaseSource, tablenameSource, true);
    publishPreviewRequestSource(sourceConnectionDetails);
});

previewButtonTarget.addEventListener('click', () => {
    var usernameTarget = document.getElementById('userInputTarget').value;
    var passwordTarget = document.getElementById('passwordInputTarget').value;
    var serverTarget = document.getElementById('serverInputTarget').value;
    var databaseTarget = document.getElementById('databaseInputTarget').value;
    var tablenameTarget = document.getElementById('tableInputTarget').value;

    var targetConnectionDetails = new ConnectionDetails(usernameTarget, passwordTarget, serverTarget, databaseTarget, tablenameTarget, false);
    publishPreviewRequestTarget(targetConnectionDetails);
});