class ConnectionDetails {
    constructor(username, password, server, database, tablename, isSource) {
        this.username = username;
        this.password = password;
        this.server = server;
        this.database = database;
        this.tablename = tablename;
        this.isSource = isSource;
    }
}