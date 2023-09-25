// TODO: dom-constants.js
const syncConnection = new signalR.HubConnectionBuilder()
    .withUrl("/synchub")
    .configureLogging(signalR.LogLevel.Information)
    .withAutomaticReconnect()
    .build();
const previewConnection = new signalR.HubConnectionBuilder()
    .withUrl("/previewhub")
    .configureLogging(signalR.LogLevel.Information)
    .withAutomaticReconnect()
    .build();

syncConnection.start();
previewConnection.start();