<template>
  <v-app id="403">
    <v-container fluid fill-height>
      <v-row align="center" justify="center">
        <div class="mr-4 hidden-sm-and-down">
          <img :src="require('@/assets/Errors/403.svg')" alt="">
        </div>
        <div class="text-md-center">
          <h1>403</h1>
          <h2 class="my-4 headline ">{{$t('app.forbidden')}}</h2>
          <v-row justify="center">
            <v-btn color="primary"
                   @click="goHome"
                   :loading="isLoading">{{$t('app.goHome')}}</v-btn>
            <span class="ma-2" />
            <v-btn color="primary" @click="logout" v-if="$authFlow.supportLogout">{{ $t('app.logout') }}</v-btn>
          </v-row>
        </div>
      </v-row>
    </v-container>
    <impersonate-indicator go-to-route-name-on-stop="home" />
  </v-app>
</template>

<script lang="ts">
	import { Vue, Component } from 'vue-property-decorator';
	import { authorizationService } from '@/container';
	import ImpersonateIndicator from '@/layouts/default/components/impersonate-indicator.vue';

	@Component({
		components: { ImpersonateIndicator }
	})
	export default class Forbidden extends Vue {

		isLoading: boolean = false;

		async goHome() {
			try {
				this.isLoading = true;
				await authorizationService.loadUserInformation();
				this.$router.push({ path: '/' });
			} finally {
				this.isLoading = false;
			}
		}

		logout() {
			_.sendMessage('logout');
		}
	}
</script>

<style scoped lang="css">
  h1 {
    font-size: 150px;
    line-height: 150px;
    font-weight: 700;
    color: #252932;
    text-shadow: rgba(61, 61, 61, 0.3) 1px 1px, rgba(61, 61, 61, 0.2) 2px 2px, rgba(61, 61, 61, 0.3) 3px 3px;
  }
</style>

<route>
  {
  "layout": "none",
  "includeInDrawer": false,
  "path": "/403"
  }
</route>
