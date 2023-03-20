export class UserCreateModel {
	public username: string | null = null;
	public firstName: string | null = null;
	public lastName: string | null = null;
	public email: string | null = null;
	public languageCode: string | null = null;
	public cultureCode: string | null = null;
	public telephoneNumber: string | null = null;
	public idGroupList: number[] = [];
	public permissions: string[] = [];
}

export class GroupModel {
	public idGroupList: number[] = [];
}

export class UserUpdateModel {
	public idUser: number | null = null;
	public username: string | null = null;
	public firstName: string | null = null;
	public lastName: string | null = null;
	public email: string | null = null;
	public languageCode: string | null = null;
	public cultureCode: string | null = null;
	public telephoneNumber: string | null = null;
	public idGroupList: number[] = [];
	public permissions: string[] = [];

}

export class Validation {
	public static usernameMaxLength = 50;
	public static firstNameMaxLength = 50;
	public static lastNameMaxLength = 50;
	public static emailMaxLength = 50;
	public static telephoneNumberMaxLength = 20;
}
