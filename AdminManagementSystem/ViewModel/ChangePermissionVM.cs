namespace AdminManagementSystem.ViewModel
{
	public class ChangePermissionVM
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string CurrentRole { get; set; }
		public string NewRoleId { get; set; }
		public List<ShowRoleVM> Roles { get; set; } 
	}
}
