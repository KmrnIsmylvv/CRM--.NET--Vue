import Vue from 'vue';
import _ from 'lodash';
import { Store } from 'vuex';
import { namespace } from 'vuex-class';
import { RouteConfig } from 'vue-router';
import { configuration } from '@/configuration';
import localizationService, { localizationModuleKey } from '@round/localization';
import utilsService from '@round/utils';
import apiService from '@round/api';
import appBarService, { appBarPrototypeKey } from '@round/app-bar';
import routerService from '@round/router';
import stateService, { StateStorage } from '@round/state';
import tableService from '@round/table';
import comboboxService from '@round/combobox';
import dialogService from '@round/dialog';
import dateTimeService from '@round/datetime';
import numberService from '@round/number';
import searchService from '@round/search';
import authorizationService, { authorizationModuleKey } from '@round/authorization';
import jwtAuthenticationService, { authenticationJWTModuleKey } from '@round/authentication-jwt';
import profilerService, { profilerModuleKey } from '@round/profiler';
import { AxiosResponse, AxiosError } from 'axios';
import { autonumericFormats, dateFormats } from '@/locales';

const langVuexNamespace = namespace(localizationModuleKey);
const authorizationVuexNamespace = namespace(authorizationModuleKey);
const authenticationJWTVuexNamespace = namespace(authenticationJWTModuleKey);
const profilerVuexNamespace = namespace(profilerModuleKey);
const appBarVuexNamespace = namespace(appBarPrototypeKey);

class Container {
	public init(store: Store<any>, routes: RouteConfig[]) {
		appBarService.init({ store });
		utilsService.init();
		this.configureApi();
		authorizationService.init({
			api: apiService,
			initialStore: store
		});
		jwtAuthenticationService.init({
			api: apiService,
			initialStore: store
		});
		localizationService.init({
			initialStore: store,
			defaultLocale: configuration.defaultLocale,
			downloadLocale: async locale => await import(/* webpackChunkName: "lang-[request]" */ `@/locales/${locale}`)
		});
		dateTimeService.init({
			culture: configuration.defaultCulture,
			language: configuration.defaultLocale,
			dateFormats
		});
		numberService.init({
			numberFormats: autonumericFormats,
			culture: configuration.defaultCulture,
			format: 'integer'
		});
		profilerService.init({
			api: apiService,
			initialStore: store
		});
		this.configureRoutes(routes, store);
		stateService.init({
			api: apiService,
			initialStore: store
		});
		this.configureComponents(store);
		searchService.init({
			stateService,
			stateStorage: StateStorage.VuexStore,
		});
	}

	private configureApi() {
		const parseSuccess = (response: AxiosResponse<any>) =>
			_.hasValue(response.data.data)
				? response.data.data
				: response.data;

		const handleError = (error: AxiosError) => {
			if (!_.hasValue(error.response))
				throw error;

			if (error.response.status === 401)
				throw error;

			if (_.hasValue(error.response.data) && _.hasValue(error.response.data.error)) {
				if (_.hasValue(error.response.data.error.isValidation) && error.response.data.error.isValidation)
					// Validation exception
					throw error.response.data.error;
				else
					// Domain exception
					throw error.response.data.error;
			}

			if (_.hasValue(error.response.data))
				throw error.response.data;

			throw error;
		};

		apiService.init(
			{
				baseURL: configuration.backendBaseUrl,
				timeout: configuration.apiTimeout,
				headers: { 'Cache-Control': 'no-cache' },
				withCredentials: true // Check cross-site Access-Control
			},
			{
				parseSuccess,
				handleError
			});
	}

	private configureRoutes(routes: RouteConfig[], store: Store<any>) {
		routerService.init({
			routes,
			permissionsService: authorizationService.permissionsService
		});

		routerService.router.beforeEach((to, from, next) => {
			if (!_.hasValue((routerService.getRouteByPath(to.path)))) {
				next({ replace: true, name: 'notFound' });
				return;
			}
			next();
		});

		routerService.router.afterEach(to => {
			store.commit('setCurrentRoute', to);
		});
	}

	private configureComponents(store: Store<any>) {
		dialogService.init({ store });

		tableService.init({
			apiService,
			stateService,
			settingsStorage: StateStorage.UserSettings,
			stateStorage: StateStorage.VuexStore
		});

		comboboxService.init({
			apiService
		});
	}
}


export { utilsService, searchService, apiService, routerService, stateService, dialogService, tableService, comboboxService, dateTimeService, numberService };
export { localizationService, langVuexNamespace };
export { authorizationService, authorizationVuexNamespace };
export { jwtAuthenticationService, authenticationJWTVuexNamespace };
export { appBarService, appBarVuexNamespace };
export { profilerVuexNamespace };
export { configuration };

export default new Container();

export * from '@round/search';
export * from '@round/api';
export * from '@round/app-bar';
export * from '@round/authentication-jwt';
export * from '@round/router';
export * from '@round/validation';
export * from '@round/avatar';
export * from '@round/actions';
export * from '@round/dialog';
export * from '@round/combobox';
export * from '@round/table';
export * from '@round/profiler';
export * from '@round/navigation-drawer';
export * from '@round/state';
export * from '@round/datetime';
