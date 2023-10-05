class ConnectionDetails {
    constructor(username, password, server, database, tablename, isSource) {
        this.Username = username.toString();
        this.Password = password.toString();
        this.ServerName = server.toString();
        this.DatabaseName = database.toString();
        this.TableName = tablename.toString();
    }
}