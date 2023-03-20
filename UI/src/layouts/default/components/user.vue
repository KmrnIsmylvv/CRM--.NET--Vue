<template>
	<div>
		<v-icon v-if="impersonationActivated" color="red" dark class="mr-2">fa-user-friends</v-icon>
		<v-icon v-if="profilerStarted" color="red" dark class="mr-2">fa-database</v-icon>
		<v-menu v-model="isMenuOpened"
						offset-y
						origin="center center"
						:close-on-content-click="false"
						:nudge-bottom="10">
			<template v-slot:activator="{ on }">
				<v-btn icon
							 large
							 text
							 :loading="avatarLoading"
							 class="mr-2"
							 v-on="on">
					<r-avatar :user="displayedUser"
										:size="avatarSize"
										:color="avatarColor" />
				</v-btn>
			</template>
			<v-card>

				<v-toolbar flat>
					<v-toolbar-title v-if="displayedUserCompleteName">
						<v-list-item-title>{{displayedUserCompleteName}}</v-list-item-title>
						<v-list-item-subtitle>{{displayedUser.username}}</v-list-item-subtitle>
					</v-toolbar-title>
					<v-toolbar-title v-else>
						<v-list-item-title>{{displayedUser.username}}</v-list-item-title>
					</v-toolbar-title>
				</v-toolbar>

				<v-divider />

				<!-- <v-list> -->
				<v-list-item>
					<v-list-item-icon class="mr-4">
						<v-icon>fa-cog</v-icon>
					</v-list-item-icon>
					<v-list-item-content>
						<v-list-item-title>{{ $t('app.settings') }}</v-list-item-title>
					</v-list-item-content>
				</v-list-item>
				<v-list-item v-if="supportsProfiler" @click="openQueryInterceptor">
					<v-list-item-icon class="mr-4">
						<v-icon>fa-database</v-icon>
					</v-list-item-icon>
					<v-list-item-content>
						<v-list-item-title>{{ $t('app.queryViewer') }}</v-list-item-title>
					</v-list-item-content>
				</v-list-item>
				<div v-if="supportsImpersonation">
					<v-subheader>{{ $t('app.impersonation') }}</v-subheader>
					<v-list-item>
						<v-list-item-content class="pa-0">
							<r-combobox :selected.sync="userToImpersonateSelected"
													controller="system"
													action="userFullNamesToImpersonate"
													item-key="idUser"
													item-value="fullName"
													clearable
													hide-details
													outlined
													dense
													class="usersAvailables" />
						</v-list-item-content>
					</v-list-item>
				</div>
				<v-list-item v-if="canAccessHangfireDashboard"
										 @click="openHangfireDashboard">
					<v-list-item-icon class="mr-4">
						<v-icon>fa-cogs</v-icon>
					</v-list-item-icon>
					<v-list-item-content>
						<v-list-item-title>{{ $t('app.hangfire-dashboard') }}</v-list-item-title>
					</v-list-item-content>
				</v-list-item>
				<v-list-item v-if="$authFlow.supportLogout"
										 @click="logout">
					<v-list-item-icon class="mr-4">
						<v-icon>fa-sign-out-alt</v-icon>
					</v-list-item-icon>
					<v-list-item-content>
						<v-list-item-title>{{ $t('app.logout') }}</v-list-item-title>
					</v-list-item-content>
				</v-list-item>
				<!-- </v-list> -->
			</v-card>
		</v-menu>
	</div>
</template>
<script lang="ts">
	import { Vue, Component, Watch, Prop } from 'vue-property-decorator';
	import { RAvatar, authorizationVuexNamespace, RCombobox, apiService, authorizationService, profilerVuexNamespace } from '@/container';

	@Component({ components: { RAvatar, RCombobox } })
	export default class User extends Vue {
		@Prop({ type: String, default: 'blue accent-2' }) public avatarColor!: string;
		@Prop({ type: String }) public avatarSize?: string;

		@authorizationVuexNamespace.State private currentUser!: any;
		@authorizationVuexNamespace.Getter private currentUserCompleteName!: string;
		@authorizationVuexNamespace.State private impersonatedBy!: any | null;
		@authorizationVuexNamespace.Getter private impersonatedByCompleteName!: string;
		@authorizationVuexNamespace.State private impersonationActivated!: boolean;
		@authorizationVuexNamespace.State private impersonationLoading!: boolean;
		@profilerVuexNamespace.State private profilerStarted!: boolean;

		private selectionLoading = false;
		private isMenuOpened = false;
		private userToImpersonateSelected: any = null;
		private ignoreImpersonationVuexEvents = false;

		private mounted() {
			if (this.impersonationActivated) {
				this.userToImpersonateSelected = this.currentUser;
				this.userToImpersonateSelected.fullName = this.currentUserCompleteName;
			}
		}

		private get avatarLoading() {
			return this.selectionLoading || this.impersonationLoading;
		}

		private get displayedUser() {
			if (this.impersonationActivated)
				return this.impersonatedBy;
			else
				return this.currentUser;
		}

		private get displayedUserCompleteName() {
			if (this.impersonationActivated)
				return this.impersonatedByCompleteName;
			else
				return this.currentUserCompleteName;
		}

		private get supportsImpersonation() {
			return this.displayedUser.permissions.includes(this.$p.roundCanImpersonate);
		}

		@Watch('userToImpersonateSelected')
		private async onImpersonatedChanged() {
			try {
				this.isMenuOpened = false;
				this.selectionLoading = true;
				if (!_.isNil(this.userToImpersonateSelected)) {
					if (!this.impersonationActivated)
						await authorizationService.startImpersonation(this.userToImpersonateSelected.username);
					else if (this.userToImpersonateSelected.username !== this.currentUser.username) {
						try {
							this.ignoreImpersonationVuexEvents = true;
							await authorizationService.stopImpersonation();
							await authorizationService.startImpersonation(this.userToImpersonateSelected.username);
						} finally {
							this.ignoreImpersonationVuexEvents = false;
						}
					}
					_.sendMessage('checkCurrentRoutePermission');
				} else if (this.impersonationActivated)
					await authorizationService.stopImpersonation();
			} catch {
				this.userToImpersonateSelected = undefined;
			} finally {
				this.selectionLoading = false;
			}
		}

		@Watch('impersonationActivated')
		private onImpersonationActivatedChanged() {
			if (!this.impersonationActivated && !this.ignoreImpersonationVuexEvents)
				this.userToImpersonateSelected = null;

			_.sendMessage('reloadApp');
		}

		private get supportsProfiler() {
			return this.displayedUser.permissions.includes(this.$p.roundCanProfile);
		}

		private openQueryInterceptor() {
			this.isMenuOpened = false;

			const qiPage = this.$router.resolve({ name: 'queryViewer' });
			const style = 'top=10, left=10, width=800, height=600, status=no, menubar=no, toolbar=no, scrollbars=no, location=no';
			const win = window.open(qiPage.href, '', style);
		}

		private get canAccessHangfireDashboard() {
			return this.$ps.has(this.$p.hangfireDashboard_Reader);
		}

		private openHangfireDashboard() {
			this.isMenuOpened = false;
			this.$router.push({ path: '/hangfire' });
		}

		private logout() {
			_.sendMessage('logout');
		}

	}
</script>

<style lang="scss">
	@import '~vuetify/src/styles/styles.sass';

	.usersAvailables {
		.r-combobox

	{
		padding-top: 0px;
		margin-top: 0px;
	}
	}
</style>
