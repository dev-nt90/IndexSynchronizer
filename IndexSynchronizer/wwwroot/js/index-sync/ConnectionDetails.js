class ConnectionDetails {
    constructor(username, password, server, database, schemaname, tablename, isSource) {
        this.Username = username.toString();
        this.Password = password.toString();
        this.ServerName = server.toString();
        this.DatabaseName = database.toString();
        this.SchemaName = schemaname.toString();
        this.TableName = tablename.toString();
    }
}