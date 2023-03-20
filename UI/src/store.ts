import Vue from 'vue';
import Vuex from 'vuex';
Vue.use(Vuex);

const state = {
	currentRoute: null
};

const mutations = {
	setCurrentRoute(currentState, route) {
		currentState.currentRoute = route;
	}
};

const store = new Vuex.Store({
	state,
	mutations
});

export default store;
