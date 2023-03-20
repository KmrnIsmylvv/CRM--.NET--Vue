<template>
	<iframe v-if="authenticated"
			frameborder="0"
			:src="dashboardUrl"
			style="width: 100%; height: calc(100vh - 100px)" />
</template>

<script lang="ts">
	import { Vue, Component } from 'vue-property-decorator';
	import { configuration, apiService  } from '@/container';

	@Component({
		// for some reason defining this hook inside of body of component does not work
		beforeDestroy: async () => {
			if (configuration.authFlowType !== 'Windows')
				await apiService.post('system', 'reset-auth-cookie');
		}
	})
	export default class Hangfire extends Vue {

		authenticated = false;

		async created() {

			if (configuration.authFlowType !== 'Windows')
				await apiService.post('system', 'set-auth-cookie');

			this.authenticated = true;
		}

		get dashboardUrl() {
			return configuration.hangfireDashboardUrl;
		}
	}
</script>

<route>
	{
	"visibleTo": [ 'HangfireDashboard_Reader' ],
	"includeInDrawer": false
	}
</route>
