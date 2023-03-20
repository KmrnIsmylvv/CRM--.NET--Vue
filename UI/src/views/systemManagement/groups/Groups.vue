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
			<v-btn icon @click="onGroupCreate">
				<v-icon>fa-plus</v-icon>
			</v-btn>
		</template>

		<template #row-actions="{ item }">
			<r-action-buttons
				:actions="tableActions"
				:context="item"
				:debounce="300"
				text
				>
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

import Create from './group-create.vue';
import Update from './group-update.vue';
import { GroupUpdateModel } from './types';

@Component({
	components: {
		RActionButtons,
		RTable
	}
})
export default class Groups extends Vue {
	controller = 'system';
	action = 'groups';
	stateKey = 'groupsTable';

	headers: ITableHeader[] = [
		{
			visible: false,
			visibilityChangeable: false,
			field: 'idGroup'
		},
		{
			field: 'name',
			localeKey: 'systemManagement.groups.name'
		},
		{
			field: 'userFullNames',
			localeKey: 'systemManagement.groups.userFullNames',
			visible: false
		},
		{
			field: 'permissions',
			localeKey: 'systemManagement.groups.permissions',
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
			Filter.withRule('name', RuleOperator.IsLike),
			Filter.withRule('userFullNames', RuleOperator.IsLike),
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
				onAction: this.onGroupUpdate,
				disabled: !context.isEnabled
			},
			{
				format: 'toggle',
				onAction: this.onGroupToggle
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
				pageContext: this.$t('systemManagement.groups').toString(),
				additionalInfo: this.$t('systemManagement').toString()
			});
	}

	async onGroupCreate() {
		const saved = await dialogService.popup({
			component: Create,
			containerProps: { width: '50%' }
		}).result;

		if (saved)
			await this.reloadTable();
	}

	async onGroupUpdate(action: IButtonAction, { idGroup, isEnabled }): Promise<void> {
		action.loading = true;
		const model = await apiService.get<GroupUpdateModel>(this.controller, 'group', { idGroup });
		action.loading = false;

		const saved = await dialogService.popup({
			component: Update,
			componentProps: { model },
			containerProps: { width: '50%' }
		}).result;

		if (saved)
			location.reload();
	}

	async onGroupToggle(action: IButtonAction, { idGroup, isEnabled }): Promise<void> {
		await apiService.post(this.controller,
			isEnabled
				? 'group/disable'
				: 'group/enable',
			{ idGroup },
			{ inQuery: true });

		location.reload();
	}

}
</script>
