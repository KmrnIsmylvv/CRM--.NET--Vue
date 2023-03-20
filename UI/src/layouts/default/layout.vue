<template>
	<v-app>
		<r-navigation-drawer
			:routes="availableRoutes"
			:itemColor="drawerItemColor"
			:activeItemColor="drawerActiveItemColor">

			<template slot="drawer-top">
				<img :src="require('@/assets/projectLogo.png')" />
			</template>

		</r-navigation-drawer>

		<r-app-bar dense color="primary" dark app>
			<template v-slot:appBarAppendTitle>
				<user avatarColor="secondary" />
			</template>
		</r-app-bar>

		<v-content>
			<v-container fluid>
				<slot />
			</v-container>
		</v-content>

		<r-dialogs />
		<impersonate-indicator/>

	</v-app>
</template>

<script lang="ts">

import { Vue, Component, Watch } from 'vue-property-decorator';
import { RAppBar } from '@round/app-bar';
import { RDialogs, RNavigationDrawer, routerService } from '@/container';
import User from './components/user.vue';
import ImpersonateIndicator from './components/impersonate-indicator.vue';

@Component({
	components: { RAppBar, RNavigationDrawer, RDialogs, ImpersonateIndicator, User }
})
export default class DefaultLayout extends Vue {

	drawerItemColor = (this.$vuetify.theme as any).currentTheme.secondary;
	drawerActiveItemColor = (this.$vuetify.theme as any).currentTheme.primary;

	get availableRoutes() {
		return routerService.authorizedRoutes;
	}

}

</script>

<style lang="scss">

	html {
		overflow-y: hidden;
	}

</style>
