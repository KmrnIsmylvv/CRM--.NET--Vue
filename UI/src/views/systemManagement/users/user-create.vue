<template>
	<popup-form
		:actions="actions"
		:title="$t('systemManagement.userCreate')"
		>
		<v-container>
			<v-row>
				<v-col cols=12>
					<v-text-field
						v-model="model.username"
						:error="hasError('username')"
						:error-messages="errorMessages('username')"
						:hide-details="!hasError('username')"
						:label="required($t('systemManagement.users.username'))"
						placeholder=" "
						dense outlined
					/>
				</v-col>
				<v-col cols=6>
					<v-text-field
						v-model="model.firstName"
						:error="hasError('firstName')"
						:error-messages="errorMessages('firstName')"
						:hide-details="!hasError('firstName')"
						:label="required($t('systemManagement.users.firstName'))"
						placeholder=" "
						dense outlined
					/>
				</v-col>
				<v-col cols=6>
					<v-text-field
						v-model="model.lastName"
						:error="hasError('lastName')"
						:error-messages="errorMessages('lastName')"
						:hide-details="!hasError('lastName')"
						:label="required($t('systemManagement.users.lastName'))"
						placeholder=" "
						dense outlined
					/>
				</v-col>
				<v-col cols=12>
					<v-text-field
						v-model="model.email"
						:error="hasError('email')"
						:error-messages="errorMessages('email')"
						:hide-details="!hasError('email')"
						:label="required($t('systemManagement.users.eMail'))"
						placeholder=" "
						dense outlined
					/>
				</v-col>
				<v-col cols=6>
					<r-combobox
						v-model="model.languageCode"
						:error="hasError('languageCode')"
						:error-messages="errorMessages('languageCode')"
						:hide-details="!hasError('languageCode')"
						:items="languages"
						:label="required($t('systemManagement.users.languageCode'))"
						placeholder=" "
						dense outlined
					/>
				</v-col>
				<v-col cols=6>
					<r-combobox
						v-model="model.cultureCode"
						:error="hasError('cultureCode')"
						:error-messages="errorMessages('cultureCode')"
						:hide-details="!hasError('cultureCode')"
						:items="cultures"
						:label="required($t('systemManagement.users.cultureCode'))"
						placeholder=" "
						dense outlined
					/>
				</v-col>
				<v-col cols=12>
					<v-text-field
						v-model="model.telephoneNumber"
						:error="hasError('telephoneNumber')"
						:error-messages="errorMessages('telephoneNumber')"
						:hide-details="!hasError('telephoneNumber')"
						:label="$t('systemManagement.users.telephoneNumber')"
						placeholder=" "
						dense outlined
					/>
				</v-col>
				<v-col cols=12>
					<r-combobox
						v-model="model.idGroupList"
						:controller="controller"
						:error="hasError('idGroupList')"
						:error-messages="errorMessages('idGroupList')"
						:hide-details="!hasError('idGroupList')"
						:label="$t('systemManagement.users.idGroupList')"
						action="groupNames"
						item-key="idGroup"
						item-value="name"
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
						:label="$t('systemManagement.users.permissions')"
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
import { PopupForm } from '@/layouts/default';

import {
	configuration,
	apiService, dialogService,
	IButtonAction,
	RCombobox,
	DialogMixin, ValidationMixin
} from '@/container';

import {
	GroupModel,
	UserCreateModel,
	Validation
} from './types';

@Component({
	components: { PopupForm, RCombobox }
})
export default class UserCreate extends Mixins(DialogMixin, ValidationMixin) {
	controller = 'system';
	action = 'user/create';
	model = new UserCreateModel();

	usernameMaxLength = Validation.usernameMaxLength;
	firstNameMaxLength = Validation.firstNameMaxLength;
	lastNameMaxLength = Validation.lastNameMaxLength;
	emailMaxLength = Validation.emailMaxLength;
	telephoneNumberMaxLength = Validation.telephoneNumberMaxLength;

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
		this.permissions = await apiService.get(this.controller, 'permissions');
	}

	async onSave(action: IButtonAction): Promise<void> {
		this.validateUsername();
		this.validateFirstName();
		this.validateLastName();
		this.validateEmail();
		this.validateLanguageCode();
		this.validateCultureCode();

		if (this.hasErrors)
			return;

		action.loading = true;

		await this.catchValidationErrorsAsync(() =>
			apiService.post(this.controller, this.action, this.model));

		if (!this.hasErrors)
			this.close(true);
	}

	validateUsername() {
		this.removeErrors('username');
		this.require('username', this.model.username);
		this.maxLength('username', this.model.username, this.usernameMaxLength);
	}

	validateFirstName() {
		this.removeErrors('firstName');
		this.require('firstName', this.model.firstName);
		this.maxLength('firstName', this.model.firstName, this.firstNameMaxLength);
	}

	validateLastName() {
		this.removeErrors('lastName');
		this.require('lastName', this.model.lastName);
		this.maxLength('lastName', this.model.lastName, this.lastNameMaxLength);
	}

	validateEmail() {
		this.removeErrors('email');
		this.require('email', this.model.email);
		this.maxLength('email', this.model.email, this.emailMaxLength);
	}

	validateLanguageCode() {
		this.removeErrors('languageCode');
		this.require('languageCode', this.model.languageCode);
	}

	validateCultureCode() {
		this.removeErrors('cultureCode');
		this.require('cultureCode', this.model.cultureCode);
	}

	validateTelephoneNumber() {
		this.removeErrors('telephoneNumber');
		this.maxLength('telephoneNumber', this.model.telephoneNumber, this.telephoneNumberMaxLength);
	}

	@Watch('model.username')
	onUsernameChanged() {
		this.validateUsername();
	}

	@Watch('model.firstName')
	onFirstNameChanged() {
		this.validateFirstName();
	}

	@Watch('model.lastName')
	onLastNameChanged() {
		this.validateLastName();
	}

	@Watch('model.email')
	onEmailChanged() {
		this.validateEmail();
	}

	@Watch('model.languageCode')
	onLanguageChanged() {
		this.validateLanguageCode();
	}

	@Watch('model.cultureCode')
	onCultureCodeChanged() {
		this.validateCultureCode();
	}

	@Watch('model.telephoneNumber')
	onTelephoneNumberChanged() {
		this.validateTelephoneNumber();
	}
}

</script>
