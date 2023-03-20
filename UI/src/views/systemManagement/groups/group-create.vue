<template>
	<popup-form
		:actions="actions"
		:title="$t('systemManagement.groupCreate')"
		>
		<v-container>
			<v-row>
				<v-col cols=12>
					<v-text-field
						v-model="model.name"
						:error="hasError('name')"
						:error-messages="errorMessages('name')"
						:hide-details="!hasError('name')"
						:label="required($t('systemManagement.groups.name'))"
						placeholder=" "
						dense outlined
					/>
				</v-col>
				<v-col cols=12>
					<r-combobox
						v-model="model.idUserList"
						:controller="controller"
						:error="hasError('idUserList')"
						:error-messages="errorMessages('idUserList')"
						:hide-details="!hasError('idUserList')"
						:label="$t('systemManagement.groups.idUserList')"
						action="userFullNames"
						item-key="idUser"
						item-value="fullName"
						placeholder=" "
						dense deletable-chips multiselect
						outlined small-chips
					/>
				</v-col>
				<v-col cols=12>
					<r-combobox
						:selected.sync="model.permissions"
						:error="hasError('permissions')"
						:error-messages="errorMessages('permissions')"
						:hide-details="!hasError('permissions')"
						:items="permissions"
						:label="required($t('systemManagement.groups.permissions'))"
						placeholder=" "
						dense deletable-chips multiselect
						outlined small-chips
					/>
				</v-col>
				<v-col class="py-0"><small>{{$t('app.mandatoryMarkerExplanation')}}</small></v-col>
			</v-row>
		</v-container>
	</popup-form>
</template>

<script lang="ts">

import { Vue, Component, Watch, Model, Mixins } from 'vue-property-decorator';

import {
	configuration,
	apiService, dialogService,
	IButtonAction,
	RCombobox,
	DialogMixin, ValidationMixin
} from '@/container';

import { PopupForm } from '@/layouts/default';
import { GroupCreateModel } from './types';

@Component({
	components: {
		PopupForm,
		RCombobox
	}
})
export default class GroupCreate extends Mixins(DialogMixin, ValidationMixin) {
	controller = 'system';
	action = 'group/create';
	model = new GroupCreateModel();

	languages?: any[] = configuration.supportedLanguages.split(',');

	cultures?: any[] = configuration.supportedCultures.split(',');

	permissions: string[] = [];

	get actions(): IButtonAction[] {
		return [{
			localeKey: 'app.save',
			onAction: action => this.onSave(action),
			disabled: this.hasErrors
		}];
	}

	async created(): Promise<void> {
		this.permissions = await apiService.get<string[]>(this.controller, 'permissions');
	}

	async onSave(action: IButtonAction): Promise<void> {
		this.validateName();
		this.validatePermissions();

		if (this.hasErrors)
			return;

		action.loading = true;

		await this.catchValidationErrorsAsync(() =>
			apiService.post(this.controller, this.action, this.model));

		if (!this.hasErrors)
			this.close(true);
	}

	validateName() {
		this.removeErrors('name');
		this.require('name', this.model.name);
		this.maxLength('name', this.model.name, this.model.nameMaxLength);
	}

	validatePermissions() {
		this.removeErrors('permissions');

		if (this.model.permissions.length === 0)
			this.addError('permissions', this.$t('round.error.isRequired').toString());
	}

	@Watch('model.name')
	onNameChanged() {
		this.validateName();
	}

	@Watch('model.permissions')
	onPermissionsChanged() {
		this.validatePermissions();
	}
}

</script>
