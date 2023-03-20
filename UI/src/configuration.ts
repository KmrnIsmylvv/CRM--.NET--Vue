
export const configuration = {
	backendBaseUrl: String(process.env.VUE_APP_BACKEND_BASE_URL),
	hangfireDashboardUrl: String(process.env.VUE_APP_HANGFIRE_DASHBOARD_URL),
	apiTimeout: Number(process.env.VUE_APP_API_TIMEOUT),
	defaultLocale: String(process.env.VUE_APP_DEFAULT_LOCALE),
	defaultCulture: String(process.env.VUE_APP_DEFAULT_CULTURE),
	supportedLanguages: String(process.env.VUE_APP_SUPPORTED_LANGUAGES),
	supportedCultures: String(process.env.VUE_APP_SUPPORTED_CULTURES),
	defaultLayout: 'default',
	authFlowType: String(process.env.VUE_APP_AUTH_TYPE),
	appId: String(process.env.VUE_APP_ID)
};



