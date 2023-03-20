<template>
  <v-app class="login">
    <v-content>
			<v-container fluid fill-height>
				<v-row align="center" justify="center">
					<v-col cols=12 sm=8 md=4 lg=4>
						<v-form ref="form" v-model="model.valid" lazy-validation>
							<v-card class="elevation-1 pa-4">
								<v-card-text>
									<div class="layout column align-center">
										<img :src="require('@/assets/projectIcon.svg')" width="120" height="120">
										<h1 class="flex my-6 primary--text">Login</h1>
									</div>
										<v-text-field append-icon="fa-user" label="Username" type="text"
																	v-model="model.username" :rules="[v => !!v || 'Username is required']"
																	required
																	@keyup.enter="login"/>
										<v-text-field append-icon="fa-lock" label="Password" type="password"
																	v-model="model.password" :rules="[v => !!v || 'Password is required']"
																	required
																	@keyup.enter="login"/>
								</v-card-text>
								<v-card-actions>
									<v-spacer></v-spacer>
									<v-btn block color="primary" @click="login" v-on:keyup.enter="submit" :loading="loading" :disabled="!model.valid">Login</v-btn>
								</v-card-actions>
							</v-card>
						</v-form>
					</v-col>
				</v-row>
			</v-container>
    </v-content>
		<r-dialogs/>
  </v-app>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator';
import { State, namespace } from 'vuex-class';
import { apiService, authenticationJWTVuexNamespace, jwtAuthenticationService } from '@/container';
import { RDialogs, dialogService } from '@/container';

@Component({
	components: { RDialogs }
})
export default class App extends Vue {

	$refs!: {
		form: HTMLFormElement
	};

	@authenticationJWTVuexNamespace.State
	loggingIn!: any;

	model = {
		valid: true,
		username: '',
		password: ''
	};

	get loading() {
		return this.loggingIn;
	}

	async login() {
		try {
			if (this.$refs.form.validate()) {
				await jwtAuthenticationService.login(this.model.username, this.model.password);
				this.$router.push('/');
			}
		} catch (error) {
			this.$refs.form.reset();
			this.$refs.form.resetValidation();
			dialogService.error('Error', 'Username or password wrong');
		}
	}

}

</script>

<style scoped lang="css">
  .login {
    height: 50%;
    width: 100%;
    position: absolute;
    top: 0;
    left: 0;
    content: "";
    z-index: 0;
		background-color: transparent
  }
</style>

<route>
{
	"layout": "none",
	"includeInDrawer": false
}
</route>
