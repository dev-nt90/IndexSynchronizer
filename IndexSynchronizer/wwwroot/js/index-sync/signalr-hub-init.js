// TODO: dom-constants.js
const syncConnection = new signalR.HubConnectionBuilder()
    .withUrl("/IndexSyncHub")
    .configureLogging(signalR.LogLevel.Information)
    .withAutomaticReconnect()
    .build();
const previewConnection = new signalR.HubConnectionBuilder()
    .withUrl("/IndexPreviewHub")
    .configureLogging(signalR.LogLevel.Information)
    .withAutomaticReconnect()
    .build();

syncConnection.start();
previewConnection.start();