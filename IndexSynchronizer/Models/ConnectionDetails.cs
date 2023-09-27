namespace IndexSynchronizer.Models
{
	public class ConnectionDetails
	{
		public ConnectionDetails() { }
		public String Username { get; set; }
		public String Password { get; set; }
		public String ServerName { get; set; }
		public String DatabaseName { get; set; }
		public String TableName { get; set; }
		public Boolean IsSourceDatabase { get; set; }
	}
}
