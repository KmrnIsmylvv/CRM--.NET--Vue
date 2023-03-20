<template>
	<r-table
		:action="action"
		:controller="controller"
		:headers="headers"
		:paging="paging"
		:reload.sync="reloadTable"
		:state-key="stateKey"
		:toolbar="toolbar"
		>

		<template #toolbar-add>
				<v-btn
					@click="onUserCreate"
					icon
					>
					<v-icon>fa-plus</v-icon>
				</v-btn>
		</template>

		<template #row-actions="{ item }">
			<r-action-buttons
				:actions="tableActions"
				:context="item"
				:debounce="300"
				text>
				<template #toggle>
					<v-switch
						:input-value="item.isEnabled"
						hide-details
					/>
				</template>
			</r-action-buttons>
		</template>

	</r-table>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'vue-property-decorator';

import {
	appBarVuexNamespace, langVuexNamespace,
	apiService, dialogService,
	IAppBarTitleModel,
	IButtonAction,
	ITableFilter, ITableHeader, ITablePagingOptions, ITableToolbarItem,
	Filter, FilterOperator, RuleOperator,
	RActionButtons,	RTable
} from '@/container';

import Create from './user-create.vue';
import Update from './user-update.vue';
import { UserUpdateModel } from './types';

@Component({
	components: {
		RActionButtons,
		RTable
	}
})
export default class UserListComponent extends Vue {
	controller = 'system';
	action = 'users';
	stateKey = 'usersTable';

	headers: ITableHeader[] = [
		{
			visible: false,
			visibilityChangeable: false,
			field: 'idUser'
		},
		{
			field: 'username',
			localeKey: 'systemManagement.users.username'
		},
		{
			field: 'firstName',
			localeKey: 'systemManagement.users.firstName'
		},
		{
			field: 'lastName',
			localeKey: 'systemManagement.users.lastName'
		},
		{
			field: 'email',
			localeKey: 'systemManagement.users.eMail'
		},
		{
			field: 'languageCode',
			localeKey: 'systemManagement.users.languageCode'
		},
		{
			field: 'cultureCode',
			localeKey: 'systemManagement.users.cultureCode'
		},
		{
			field: 'groupNames',
			localeKey: 'systemManagement.users.groupNames',
			visible: false
		},
		{
			field: 'permissions',
			localeKey: 'systemManagement.users.permissions',
			visible: false
		},
		{
			format: 'row-actions',
			sortable: false,
			visibilityChangeable: false,
			settingsLocaleKey: 'app.actions',
			width: '60px'
		}
	];

	paging: ITablePagingOptions = {
		hasNavigationOutside: true,
		hideNavigationIfOnePageOnly: true
	};

	textFilterStructure: ITableFilter = {
		filters: [
			Filter.withRule('userName', RuleOperator.IsLike),
			Filter.withRule('firstName', RuleOperator.IsLike),
			Filter.withRule('lastName', RuleOperator.IsLike),
			Filter.withRule('email', RuleOperator.IsLike),
			Filter.withRule('languageCode', RuleOperator.IsLike),
			Filter.withRule('cultureCode', RuleOperator.IsLike),
			Filter.withRule('groupNames', RuleOperator.IsLike),
			Filter.withRule('permissions', RuleOperator.IsLike)
		],
		operator: FilterOperator.Or
	};

	toolbar: ITableToolbarItem[] = [
		{
			format: 'search',
			searchProps: {
				filter: this.textFilterStructure,
				minLength: 2
			}
		},
		{ format: 'spacer' },
		{ format: 'toolbar-add' },
		{ format: 'export' },
		{ format: 'settings'}
	];

	tableActions = context => {
		return [
			{
				icon: 'fa-edit',
				onAction: this.onUserUpdate,
				disabled: !context.isEnabled
			},
			{
				format: 'toggle',
				onAction: this.onUserToggle
			}
		] as IButtonAction[];
	}

	async reloadTable() { return; }

	@appBarVuexNamespace.Mutation
	setTitle!: (toolbarTitle: IAppBarTitleModel | null) => void;

	@langVuexNamespace.State
	loadingLocaleSuccess!: boolean;

	mounted() {
		this.updateTitle();
	}

	@Watch('loadingLocaleSuccess')
	updateTitle() {
		if (this.loadingLocaleSuccess)
			this.setTitle({
				pageContext: this.$t('systemManagement.users').toString(),
				additionalInfo: this.$t('systemManagement').toString()
			});
	}

	async onUserCreate(options) {
		const saved = await dialogService.popup({
			component: Create,
			containerProps: { width: '50%' }
		}).result;

		if (saved)
			await this.reloadTable();
	}

	async onUserUpdate(action: IButtonAction, { idUser, isEnabled }): Promise<void> {
		action.loading = true;
		const model = await apiService.get<UserUpdateModel>(this.controller, 'user', { idUser });
		action.loading = false;

		const saved = await dialogService.popup({
			component: Update,
			componentProps: { model },
			containerProps: { width: '50%' }
		}).result;

		if (saved)
			location.reload();
	}

	async onUserToggle(action: IButtonAction, { idUser, isEnabled }): Promise<void> {
		await apiService.post(this.controller,
			isEnabled
				? 'user/disable'
				: 'user/enable',
			{ idUser },
			{ inQuery: true });

		location.reload();
	}
}
</script>
