<template>
	<v-snackbar v-model="impersonationActivated" bottom :timeout="0" color="green">
		<v-icon small class="mr-4" color="white">fa-user</v-icon>
		{{impersontationText}}
		<v-btn small
					 @click="stopImpersonate"
					 class="body-2 font-weight-bold ml-4"
					 :loading="impersonationLoading"
					 color="error">{{ $t('app.stopImpersonate') }}</v-btn>
	</v-snackbar>
</template>

<script lang="ts">

	import { Vue, Component, Prop } from 'vue-property-decorator';
	import { authorizationVuexNamespace, authorizationService } from '@/container';

	@Component
	export default class ImpersonateIndicator extends Vue {

		@authorizationVuexNamespace.State private impersonationActivated!: boolean;
		@authorizationVuexNamespace.State private currentUser!: any;
		@authorizationVuexNamespace.Getter private currentUserCompleteName!: string;
		@authorizationVuexNamespace.State private impersonationLoading!: boolean;

		@Prop({ type: String, default: '' }) public goToRouteNameOnStop!: string;

		isVisible = true;

		get impersontationText() {
			return this.$t('app.impersonatingUser', { userImpersonated: this.currentUser.username });
		}

		async stopImpersonate() {
			await authorizationService.stopImpersonation();
			if (!_.isNullOrEmpty(this.goToRouteNameOnStop))
				this.$router.replace({ name: this.goToRouteNameOnStop });
		}

	}

</script>
