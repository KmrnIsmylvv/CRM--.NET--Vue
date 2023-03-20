using NHibernate.Type;

namespace Elfo.Contoso.LearningRoundKamran.Api
{
	public sealed class Permissions
	{
        #region Round

        public const string RoundCanProfile = "Round_CanProfile";
        public const string RoundCanImpersonate = "Round_CanImpersonate";

        #endregion

        #region SystemManagement

        public const string CanReadSystemUser = "CanReadSystemUser";
		public const string CanWriteSystemUser = "CanWriteSystemUser";

		public const string CanReadSystemGroup = "CanReadSystemGroup";
		public const string CanWriteSystemGroup = "CanWriteSystemGroup";

		public const string CanReadSystemUserPermissionAssociation = "CanReadSystemUserPermissionAssociation";
		public const string CanWriteSystemUserPermissionAssociation = "CanWriteSystemUserPermissionAssociation";

		public const string CanReadSystemGroupPermissionAssociation = "CanReadSystemGroupPermissionAssociation";
		public const string CanWriteSystemGroupPermissionAssociation = "CanWriteSystemGroupPermissionAssociation";

		public const string CanReadSystemUserGroupAssociation = "CanReadSystemUserGroupAssociation";
		public const string CanWriteSystemUserGroupAssociation = "CanWriteSystemUserGroupAssociation";

		public const string CanReadPermission = "CanReadPermission";

		#endregion

		#region Course Management
		public const string CanReadCourse = "CanReadCourse";
		public const string CanWriteCourse = "CanWriteCourse";

        public const string CanReadExam = "CanReadExam";
        public const string CanWriteExam = "CanWriteExam";

        #endregion

        public const string HangfireDashboard_Reader= "HangfireDashboard_Reader";
	}
}
