import { Component, Vue, Watch } from 'vue-property-decorator';
import { Route } from 'vue-router';
import AuthFlowMixin from './authFlow';
import {
	jwtAuthenticationService, authorizationService, localizationService, apiService, dateTimeService,
	authorizationVuexNamespace, langVuexNamespace, authenticationJWTVuexNamespace, IUserToken, numberService
} from '@/container';

@Component
export default class JwtAuthFlowMixin extends AuthFlowMixin {

	@authorizationVuexNamespace.State
	currentUser!: any;
	@authorizationVuexNamespace.State
	loadingUserFailure!: any;

	@langVuexNamespace.Getter
	isGlobalizationDefined!: boolean;
	@langVuexNamespace.State
	loadingLocaleFailure!: boolean;

	@authenticationJWTVuexNamespace.State
	loggedIn!: boolean;
	@authenticationJWTVuexNamespace.State
	userToken!: IUserToken;

	created() {
		Vue.prototype.$authFlow = { supportLogout: true };

		apiService.options.handleUnauthorizedError = axiosError => {
			this.executeLogout();
		};

		_.subscribeMessage('logout', () => {
			this.executeLogout();
		});
	}

	get authCompleted() {
		return this.loggedIn && _.hasValue(this.currentUser) && this.isGlobalizationDefined;
	}

	executeLogout() {
		this.appLoading = false;
		this.appLoadingError = false;
		jwtAuthenticationService.logout();
		authorizationService.reset();
		this.$router.push({ name: 'login' });
		delete apiService.axios.defaults.headers.authorization;
	}

	@Watch('currentRoute')
	private async routeChangeForJwtAuthFlow(route: Route) {
		if (!this.authCompleted) {
			if (!this.loggedIn) {
				this.appLoading = false;
				this.$router.push({ name: 'login' })
					.catch(err => { /* I need this empty catch in order to avoid a built-in warning of vue-router */ }
					);
			} else {
				this.appLoading = true;
				apiService.axios.defaults.headers.authorization = `bearer ${this.userToken.accessToken}`;
				await authorizationService.loadUserInformation();
			}
		} else {
			if (!this.redirectToForbiddenIfNeeded(route)) {
				this.appLoading = false;
				if (route.name === 'login')
					this.$router.push({ name: 'login' });
			}
		}
	}

	@Watch('currentUser')
	private async userChanged(user: any) {
		await localizationService.loadLocale(user.languageCode, user.cultureCode);
		dateTimeService.changeLocale(user.cultureCode, user.languageCode);
		numberService.changeCulture(user.cultureCode);
	}

	@Watch('loadingUserFailure')
	onLoadingUserFailure(loadingUserFailure: any) {
		if (loadingUserFailure) {
			const response = (loadingUserFailure.response && loadingUserFailure.response.status) ? loadingUserFailure.response.status : undefined;
			if (!response || (response && response !== 401 && response !== 403))
				this.appLoadingError = true;
		}
	}

	@Watch('isGlobalizationDefined')
	private onIsGlobalizationDefinedChanged(isGlobalizationDefined: boolean) {
		if (isGlobalizationDefined) {
			if (!this.redirectToForbiddenIfNeeded(this.currentRoute)) {
				this.appLoading = false;
				if (this.currentRoute.name === 'login')
					this.$router.push('/');
			}
		}
	}

	@Watch('loadingLocaleFailure')
	onLoadingLocaleFailure(loadingLocaleFailure: any) {
		if (loadingLocaleFailure) {
			this.appLoadingError = true;
		}
	}

}
