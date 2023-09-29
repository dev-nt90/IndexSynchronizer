namespace IndexSynchronizer.Models
{
	public interface IConnectionDetails
	{
		public String Username { get; set; }
		public String Password { get; set; }
		public String ServerName { get; set; }
		public String DatabaseName { get; set; }
		public String TableName { get; set; }
	}
}
