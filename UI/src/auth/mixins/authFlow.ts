import { Component, Vue } from 'vue-property-decorator';
import { State } from 'vuex-class';
import { Route } from 'vue-router';
import { authorizationService, apiService, configuration, localizationService } from '@/container';

@Component
export default class AuthFlowMixin extends Vue {
	appLoading = false;
	appLoadingError = false;

	@State
	currentRoute!: Route;

	created() {
		_.subscribeMessage('checkCurrentRoutePermission', () => {
			this.redirectToForbiddenIfNeeded(this.$router.currentRoute);
		});

		apiService.options.handleForbiddenError = axiosError => {
			this.appLoading = false;
			if (localizationService.loadedLanguages.length === 0)
				// This happens when the user hasn't any permission associated: so the getUserInformation goes in error 403,
				// the flow enter here and no locales are yet loaded.
				// We can't put this loadLocale inside the watcher of loadingUserFailure because the flow enter before here,
				// due to the fact that this exceptions occurs at row level inside axios.
				localizationService.loadLocale(configuration.defaultLocale);
			this.$router.replace({ name: 'forbidden' });
		};

	}

	redirectToForbiddenIfNeeded(route: Route): boolean {
		if (!this.isCurrentRouteVisibleToCurrentUser(route)) {
			this.$router.replace({ name: 'forbidden' }, () => this.appLoading = false);
			return true;
		} else
			return false;
	}

	isCurrentRouteVisibleToCurrentUser(route: Route): boolean {
		if (_.hasValue(route.meta) && _.isArray(route.meta.visibleTo))
			return authorizationService.permissionsService.hasAny(route.meta.visibleTo);

		return true;
	}

}
