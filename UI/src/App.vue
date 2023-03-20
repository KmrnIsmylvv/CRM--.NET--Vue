<template>
	<app-loading v-if="appLoading || appLoadingError" :isInError="appLoadingError" :key="reloadAppKey"></app-loading>
	<component v-else-if="(!appLoading && !appLoadingError) && hasLayout" :is="currentLayout" :key="reloadAppKey">
		<router-view />
	</component>
	<router-view v-else-if="(!appLoading && !appLoadingError) && !hasLayout" :key="reloadAppKey" />
</template>

<script lang="ts">
	import { Vue, Component, Watch } from 'vue-property-decorator';
	import { Route } from 'vue-router';
	import { configuration } from '@/configuration';
	import { authorizationService, langVuexNamespace } from '@/container';
	import { AuthFlow } from '@/auth';
	import AppLoading from '@/layouts/default/components/app-loading.vue';

	@Component({
		components: { AppLoading }
	})
	export default class App extends AuthFlow {

		currentLayoutName: string = 'none';
		reloadAppKey = 0;

		created() {
			_.subscribeMessage('reloadApp', () => {
				this.reloadAppKey += 1;
			});
		}

		get currentLayout() {
			if (!this.appLoading && this.currentLayoutName)
				return () => import(`@/layouts/${this.currentLayoutName}/layout.vue`);
		}

		get hasLayout() {
			return !this.appLoading && !(this.currentLayoutName === 'none');
		}

		@Watch('currentRoute')
		onCurrentRouteChanged(route: Route) {
			this.currentLayoutName = _.hasValue(route.meta) && _.hasValue(route.meta.layout)
				? route.meta.layout
				: configuration.defaultLayout;
		}
	}
</script>
