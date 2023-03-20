<template>
	<v-app>	
		<v-content>
			<v-container fill-height>

				<v-row v-if="isInError">
					<v-col cols=12>
						<v-row align="center" justify="center">
							<v-icon color="error" size="150px">fa-times</v-icon>
						</v-row>
					</v-col>
					<v-col cols=12 class="text-center error--text title">
						There was an error during loading. Please, retry in a few seconds or contact the support team.
					</v-col>
					<v-col cols=12 class="text-center">
						<v-btn color="error" @click.native="onRetryClick">
							<v-icon small flat left>fa-sync</v-icon>
							Retry
						</v-btn>
					</v-col>
				</v-row>

				<v-row v-else>
					<v-col cols=12>
						<v-row align="center" justify="center">
							<img class="loading-logo" :src="require('@/assets/projectLogoBig.png')"/>
						</v-row>
					</v-col>
					<v-col cols=12>
						<v-row align="center" justify="center">
							<v-progress-circular indeterminate
																	 size="60"
																	color="primary"/>
						</v-row>
					</v-col>
				</v-row>

			</v-container>
		</v-content>
	</v-app>
</template>

<script lang="ts">

import { Vue, Component, Prop } from 'vue-property-decorator';

@Component
export default class AppLoading extends Vue {
	@Prop() public isInError?: boolean;

	private onRetryClick(): void {
		location.reload();
	}
}

</script>

<style lang="scss">

.loading-logo {
	height: 80px;
}

</style>
