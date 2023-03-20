namespace Elfo.Contoso.LearningRoundKamran.Domain
{
	public enum ValidationErrorCode
	{
		EmptySystemGroupName,
		InvalidSystemGroupNameLength,
		EmptyIdUser,
		EmptyUsername,
		EmptyPassword,
		EmptyLanguageCode,
		EmptyCultureCode,
		EmptyFirstName,
		EmptyLastName,
		EmptyIdGroup,
		UserGroupAssociationAlreadyExists,
		UsernameAlreadyExists,
		UserDoesNotExist,
		GroupDoesNotExist,
        GroupAlreadyExistsWithName,
        EmptyPermission,
		UserPermissionAssociationAlreadyExists,
		GroupPermissionAssociationAlreadyExists,
		InvalidUserEmail,
		InvalidUsernameLength,
		InvalidLanguageCodeLength,
		InvalidCultureCodeLength,
		GroupShouldHaveAtLeastOnePermission,
		
		EmptyCourseName,
        EmptyIdTeacher,
        InvalidCourseNameLength,
		InvalidCourse,
		InvalidTeacher,
		InvalidCourseDate,
		CourseNameAlreadyExists,

		EmptyExamName,
		EmptyIdCourse,
		InvalidExamNameLength,
		InvalidExamStartTime,
		ExamNameAlreadyExists,

	}
}
