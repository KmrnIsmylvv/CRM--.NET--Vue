export class GroupCreateModel {
	public name: string | null = null;
	public idUserList: number[] = [];
	public permissions: string[] = [];

	public nameMaxLength = 50;
}

export class GroupUpdateModel {
	public name: string | null = null;
	public idUserList: number[] = [];
	public permissions: string[] = [];

	public nameMaxLength = 50;
}
